using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;
using System.Collections.Generic;

public class Network_Manager : NetworkManager
{
	public string username, characterName;
	public Trainer trainer;
	public bool thisIsTheServer;

	[SerializeField]
	private string serverName;	//ALSO KNOWN AS THE singleton.matchName

	void Start()
	{
		singleton.StartMatchMaker();
		singleton.matchMaker.SetProgramAppID((AppID)404501);
	}

	public override void OnClientConnect(NetworkConnection con)
	{
		PlayerSetupMessage message = new PlayerSetupMessage();
		message.username = username;
		message.characterName = characterName;

		ClientScene.AddPlayer(con, 0, message);
	}
	public override void OnClientSceneChanged(NetworkConnection con)
	{
		
	}
	public override void OnStartClient(NetworkClient mClient)
	{
		base.OnStartClient(mClient);

		mClient.RegisterHandler((short)Messages.MessageTypes.CHAT_MESSAGE, OnClientChatMessage);
	}
	public override void OnServerAddPlayer(NetworkConnection con, short playerControllerId, NetworkReader reader)
	{
		PlayerSetupMessage message = reader.ReadMessage<PlayerSetupMessage>();
		StartCoroutine(SetupPlayer(con, playerControllerId, message.username, message.characterName));
	}
	public override void OnServerDisconnect(NetworkConnection con)
	{
		StartCoroutine(SavePlayer(con));
	}
	public override void OnStartServer()
	{
		base.OnStartServer();

		NetworkServer.RegisterHandler((short)Messages.MessageTypes.CHAT_MESSAGE, OnServerChatMessage);
	}
	public override void OnMatchList(ListMatchResponse matchList)
	{
		matches = matchList.matches;
	}

	public void InitServer()
	{
		singleton.matchMaker.CreateMatch(serverName, 4, true, "", singleton.OnMatchCreate);
	}
	private void OnServerChatMessage(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<Messages.ChatMessage>();

		//USE THIS TO CHECK FOR MESSAGES THAT WE DON'T WANT SENT
		if (msg.message == "???")
			return;
		else
		{
			Messages.ChatMessage chat = new Messages.ChatMessage ();
			chat.message = netMsg.conn.playerControllers [0].gameObject.GetComponent<Trainer> ().characterName + ": " + msg.message;
			NetworkServer.SendToAll ((short)Messages.MessageTypes.CHAT_MESSAGE, chat);
		}
	}
	private void OnClientChatMessage(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<Messages.ChatMessage>();
		if (!thisIsTheServer)
			Debug.Log ("Client message received on network ID " + singleton.client.connection.connectionId.ToString());
		singleton.client.connection.playerControllers[0].gameObject.GetComponent<Trainer>().MainChatHistory.Add(msg.message);
	}

	private Poke_Data DecodeData(string[] Pokemon)
	{
		Poke_Data data = new Poke_Data();

		data.pokemonName = Pokemon[0];
		data.nickName = Pokemon[1];
		data.isFromTrade = Pokemon[2] == "1";
		data.level = int.Parse(Pokemon[3]);
		data.nature = int.Parse(Pokemon[4]);
		data.curMaxHP = int.Parse(Pokemon[5]);
		data.curMaxPP = int.Parse(Pokemon[6]);
		data.curMaxATK = int.Parse(Pokemon[7]);
		data.curMaxDEF = int.Parse(Pokemon[8]);
		data.curMaxSPATK = int.Parse(Pokemon[9]);
		data.curMaxSPDEF = int.Parse(Pokemon[10]);
		data.curMaxSPD = int.Parse(Pokemon[11]);
		data.curHP = int.Parse(Pokemon[12]);
		data.curPP = int.Parse(Pokemon[13]);
		data.curATK = int.Parse(Pokemon[14]);
		data.curDEF = int.Parse(Pokemon[15]);
		data.curSPATK = int.Parse(Pokemon[16]);
		data.curSPDEF = int.Parse(Pokemon[17]);
		data.curSPD = int.Parse(Pokemon[18]);
		data.hpEV = int.Parse(Pokemon[19]);
		data.ppEV = int.Parse(Pokemon[20]);
		data.atkEV = int.Parse(Pokemon[21]);
		data.defEV = int.Parse(Pokemon[22]);
		data.spatkEV = int.Parse(Pokemon[23]);
		data.spdefEV = int.Parse(Pokemon[24]);
		data.spdEV = int.Parse(Pokemon[25]);
		data.hpIV = int.Parse(Pokemon[26]);
		data.ppIV = int.Parse(Pokemon[27]);
		data.atkIV = int.Parse(Pokemon[28]);
		data.defIV = int.Parse(Pokemon[29]);
		data.spatkIV = int.Parse(Pokemon[30]);
		data.spdefIV = int.Parse(Pokemon[31]);
		data.spdIV = int.Parse(Pokemon[32]);
		data.currentEXP = int.Parse(Pokemon[33]);
		data.equippedItem = Pokemon[34];
		data.slot = int.Parse(Pokemon[35]);

		return data;
	}

	public IEnumerator JoinServer()
	{
		if(singleton.matches == null)
			singleton.matchMaker.ListMatches(0, 20, "", singleton.OnMatchList);

		while(singleton.matches == null)
			yield return null;
		
		for(int i = 0; i < singleton.matches.Count; i++)
			if(singleton.matches[i].name == serverName)
			{
				singleton.matchName = singleton.matches[i].name;
				singleton.matchSize = (uint)singleton.matches[i].currentSize;
				singleton.matchMaker.JoinMatch(singleton.matches[i].networkId, "", singleton.OnMatchJoined);
				break;
			}

		yield return null;
	}

	private IEnumerator SetupPlayer(NetworkConnection con, short playerControllerId, string username, string characterName)
	{
		WWWForm trainerForm = new WWWForm();

		trainerForm.AddField("username", username);
		trainerForm.AddField("characterName", characterName);

		WWW w = new WWW("http://pokemonworld3d.dx.am/download_character.php", trainerForm);

		yield return w;

		string[] CharacterData = w.text.Split(',');

		Vector3 pos = new Vector3(float.Parse(CharacterData[5]), float.Parse(CharacterData[6]), float.Parse(CharacterData[7]));
		GameObject player = Instantiate(singleton.playerPrefab, pos, Quaternion.identity) as GameObject;
		Trainer trainer = player.GetComponent<Trainer>();

		trainer.username = CharacterData[0];
		trainer.characterName = CharacterData[1];
		trainer.genderInt = int.Parse(CharacterData[2]);
		trainer.funds = int.Parse(CharacterData[3]);

		WWWForm pokemonForm = new WWWForm();

		pokemonForm.AddField("charname", characterName);

		WWW ww = new WWW("http://pokemonworld3d.dx.am/download_pokemon.php", pokemonForm);

		yield return ww;

		List<string> Strings = new List<string>();
		int position = 0;
		int start = 0;
		char[] TrimChars = {'!'};

		do
		{
			position = ww.text.IndexOf("!", start);
			if(position >= 0)
			{
				Strings.Add(ww.text.Substring(start, position - start + 1).Trim(TrimChars));
				start = position + 1;
			}
		} while (position > 0);

		NetworkServer.AddPlayerForConnection(con, player, playerControllerId);

		int numberOfPokemon = 0;

		for(int i = 0; i < Strings.Count; i++)
		{
			string[] Pokemon = Strings[i].Split(","[0]);
			Poke_Data data = DecodeData(Pokemon);

			if(numberOfPokemon < 5)
				trainer.PokemonRoster.Add(data);
			else
				trainer.PokemonInventory.Add(data);

			numberOfPokemon++;
		}
	}
	private IEnumerator SavePlayer(NetworkConnection con)
	{
		Trainer trainer = con.playerControllers[0].gameObject.GetComponent<Trainer>();
		WWWForm playerForm = new WWWForm();

		playerForm.AddField("username", trainer.username);
		playerForm.AddField("characterName", trainer.characterName);
		playerForm.AddField("gender", trainer.genderInt.ToString());
		playerForm.AddField("funds", trainer.funds.ToString());
		playerForm.AddField("x", trainer.gameObject.transform.position.x.ToString());
		playerForm.AddField("y", trainer.gameObject.transform.position.y.ToString());
		playerForm.AddField("z", trainer.gameObject.transform.position.z.ToString());

		WWW w = new WWW("http://pokemonworld3d.dx.am/upload_player.php", playerForm);

		yield return w;

		for(int i = 0; i < trainer.PokemonRoster.Count; i++)
		{
			WWWForm pokemonForm = new WWWForm();
			int boolean = 0;

			if(trainer.PokemonRoster[i].isFromTrade)
				boolean = 1;
			
			pokemonForm.AddField("trainerName", trainer.characterName);
			pokemonForm.AddField("pokemonName", trainer.PokemonRoster[i].pokemonName);
			pokemonForm.AddField("nickName", trainer.PokemonRoster[i].nickName);
			pokemonForm.AddField("isFromTrade", boolean.ToString());
			pokemonForm.AddField("level", trainer.PokemonRoster[i].level.ToString());
			pokemonForm.AddField("nature", ((int)trainer.PokemonRoster[i].nature).ToString());
			pokemonForm.AddField("curMaxHP", trainer.PokemonRoster[i].curMaxHP.ToString());
			pokemonForm.AddField("curMaxPP", trainer.PokemonRoster[i].curMaxPP.ToString());
			pokemonForm.AddField("curMaxATK", trainer.PokemonRoster[i].curMaxATK.ToString());
			pokemonForm.AddField("curMaxDEF", trainer.PokemonRoster[i].curMaxDEF.ToString());
			pokemonForm.AddField("curMaxSPATK", trainer.PokemonRoster[i].curMaxSPATK.ToString());
			pokemonForm.AddField("curMaxSPDEF", trainer.PokemonRoster[i].curMaxSPDEF.ToString());
			pokemonForm.AddField("curMaxSPD", trainer.PokemonRoster[i].curMaxSPD.ToString());
			pokemonForm.AddField("curHP", trainer.PokemonRoster[i].curHP.ToString());
			pokemonForm.AddField("curPP", trainer.PokemonRoster[i].curPP.ToString());
			pokemonForm.AddField("curATK", trainer.PokemonRoster[i].curATK.ToString());
			pokemonForm.AddField("curDEF", trainer.PokemonRoster[i].curDEF.ToString());
			pokemonForm.AddField("curSPATK", trainer.PokemonRoster[i].curSPATK.ToString());
			pokemonForm.AddField("curSPDEF", trainer.PokemonRoster[i].curSPDEF.ToString());
			pokemonForm.AddField("curSPD", trainer.PokemonRoster[i].curSPD.ToString());
			pokemonForm.AddField("hpEV", trainer.PokemonRoster[i].hpEV.ToString());
			pokemonForm.AddField("ppEV", trainer.PokemonRoster[i].ppEV.ToString());
			pokemonForm.AddField("atkEV", trainer.PokemonRoster[i].atkEV.ToString());
			pokemonForm.AddField("defEV", trainer.PokemonRoster[i].defEV.ToString());
			pokemonForm.AddField("spatkEV", trainer.PokemonRoster[i].spatkEV.ToString());
			pokemonForm.AddField("spdefEV", trainer.PokemonRoster[i].spdefEV.ToString());
			pokemonForm.AddField("spdEV", trainer.PokemonRoster[i].spdEV.ToString());
			pokemonForm.AddField("hpIV", trainer.PokemonRoster[i].hpIV.ToString());
			pokemonForm.AddField("ppIV", trainer.PokemonRoster[i].ppIV.ToString());
			pokemonForm.AddField("atkIV", trainer.PokemonRoster[i].atkIV.ToString());
			pokemonForm.AddField("defIV", trainer.PokemonRoster[i].defIV.ToString());
			pokemonForm.AddField("spatkIV", trainer.PokemonRoster[i].spatkIV.ToString());
			pokemonForm.AddField("spdefIV", trainer.PokemonRoster[i].spdefIV.ToString());
			pokemonForm.AddField("spdIV", trainer.PokemonRoster[i].spdIV.ToString());
			pokemonForm.AddField("currentEXP", trainer.PokemonRoster[i].currentEXP.ToString());
			pokemonForm.AddField("equippedItem", trainer.PokemonRoster[i].equippedItem);
			pokemonForm.AddField("slot", trainer.PokemonRoster[i].slot.ToString());

			WWW ww = new WWW("http://pokemonworld3d.dx.am/upload_pokemon.php", pokemonForm);

			yield return ww;
		}

		for(int i = 0; i < trainer.PokemonInventory.Count; i++)
		{
			WWWForm pokemonForm = new WWWForm();
			int boolean = 0;

			if(trainer.PokemonInventory[i].isFromTrade)
				boolean = 1;
			
			pokemonForm.AddField("trainerName", trainer.characterName);
			pokemonForm.AddField("pokemonName", trainer.PokemonInventory[i].pokemonName);
			pokemonForm.AddField("nickName", trainer.PokemonInventory[i].nickName);
			pokemonForm.AddField("isFromTrade", boolean.ToString());
			pokemonForm.AddField("level", trainer.PokemonInventory[i].level.ToString());
			pokemonForm.AddField("nature", ((int)trainer.PokemonInventory[i].nature).ToString());
			pokemonForm.AddField("curMaxHP", trainer.PokemonInventory[i].curMaxHP.ToString());
			pokemonForm.AddField("curMaxPP", trainer.PokemonInventory[i].curMaxPP.ToString());
			pokemonForm.AddField("curMaxATK", trainer.PokemonInventory[i].curMaxATK.ToString());
			pokemonForm.AddField("curMaxDEF", trainer.PokemonInventory[i].curMaxDEF.ToString());
			pokemonForm.AddField("curMaxSPATK", trainer.PokemonInventory[i].curMaxSPATK.ToString());
			pokemonForm.AddField("curMaxSPDEF", trainer.PokemonInventory[i].curMaxSPDEF.ToString());
			pokemonForm.AddField("curMaxSPD", trainer.PokemonInventory[i].curMaxSPD.ToString());
			pokemonForm.AddField("curHP", trainer.PokemonInventory[i].curHP.ToString());
			pokemonForm.AddField("curPP", trainer.PokemonInventory[i].curPP.ToString());
			pokemonForm.AddField("curATK", trainer.PokemonInventory[i].curATK.ToString());
			pokemonForm.AddField("curDEF", trainer.PokemonInventory[i].curDEF.ToString());
			pokemonForm.AddField("curSPATK", trainer.PokemonInventory[i].curSPATK.ToString());
			pokemonForm.AddField("curSPDEF", trainer.PokemonInventory[i].curSPDEF.ToString());
			pokemonForm.AddField("curSPD", trainer.PokemonInventory[i].curSPD.ToString());
			pokemonForm.AddField("hpEV", trainer.PokemonInventory[i].hpEV.ToString());
			pokemonForm.AddField("ppEV", trainer.PokemonInventory[i].ppEV.ToString());
			pokemonForm.AddField("atkEV", trainer.PokemonInventory[i].atkEV.ToString());
			pokemonForm.AddField("defEV", trainer.PokemonInventory[i].defEV.ToString());
			pokemonForm.AddField("spatkEV", trainer.PokemonInventory[i].spatkEV.ToString());
			pokemonForm.AddField("spdefEV", trainer.PokemonInventory[i].spdefEV.ToString());
			pokemonForm.AddField("spdEV", trainer.PokemonInventory[i].spdEV.ToString());
			pokemonForm.AddField("hpIV", trainer.PokemonInventory[i].hpIV.ToString());
			pokemonForm.AddField("ppIV", trainer.PokemonInventory[i].ppIV.ToString());
			pokemonForm.AddField("atkIV", trainer.PokemonInventory[i].atkIV.ToString());
			pokemonForm.AddField("defIV", trainer.PokemonInventory[i].defIV.ToString());
			pokemonForm.AddField("spatkIV", trainer.PokemonInventory[i].spatkIV.ToString());
			pokemonForm.AddField("spdefIV", trainer.PokemonInventory[i].spdefIV.ToString());
			pokemonForm.AddField("spdIV", trainer.PokemonInventory[i].spdIV.ToString());
			pokemonForm.AddField("currentEXP", trainer.PokemonInventory[i].currentEXP.ToString());
			pokemonForm.AddField("equippedItem", trainer.PokemonInventory[i].equippedItem);
			pokemonForm.AddField("slot", trainer.PokemonInventory[i].slot.ToString());

			WWW ww = new WWW("http://pokemonworld3d.dx.am/upload_pokemon.php", pokemonForm);

			yield return ww;
		}

		base.OnServerDisconnect(con);
	}
}
public class PlayerSetupMessage : MessageBase
{
	public string username, characterName;
}