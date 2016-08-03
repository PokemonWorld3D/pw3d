using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using System.Collections;
using System.Collections.Generic;

public class NetworkManagerAlpha : NetworkManager
{
	public string characterName;
	public Trainer trainer;
	public bool thisIsTheServer;
	public Poke_Data starterData;

	[SerializeField] private string serverName;	//ALSO KNOWN AS THE singleton.matchName
	[SerializeField] private Vector3 startPos;

	void Start()
	{
		singleton.StartMatchMaker();
		singleton.matchMaker.SetProgramAppID((AppID)404501);
	}

	public override void OnClientConnect(NetworkConnection con)
	{
		PlayerSetupMessageAlpha message = new PlayerSetupMessageAlpha();
		message.characterName = characterName;
		message.starterData = starterData;

		ClientScene.AddPlayer(con, 0, message);
	}
	public override void OnClientSceneChanged(NetworkConnection con)
	{

	}
	public override void OnStartClient(NetworkClient mClient)
	{
		base.OnStartClient(mClient);

		mClient.RegisterHandler((short)Messages.MessageTypes.CHAT_MESSAGE, OnClientChatMessage);
		mClient.RegisterHandler((short)Messages.MessageTypes.BATTLE_MESSAGE, OnClientBattleMessage);
	}
	public override void OnServerAddPlayer(NetworkConnection con, short playerControllerId, NetworkReader reader)
	{
		PlayerSetupMessageAlpha message = reader.ReadMessage<PlayerSetupMessageAlpha>();
		StartCoroutine(SetupPlayer(con, playerControllerId, message.characterName, message.starterData));
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
			NetworkServer.SendToAll((short)Messages.MessageTypes.CHAT_MESSAGE, chat);
		}
	}
	private void OnClientChatMessage(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<Messages.ChatMessage>();

//		singleton.client.connection.playerControllers[0].gameObject.GetComponent<Trainer>().MainChatHistory.Add(msg.message);
		singleton.client.connection.playerControllers[0].gameObject.GetComponent<Trainer>().hud.AddToMainChat(msg.message);
	}
	private void OnClientBattleMessage(NetworkMessage netMsg)
	{
		var msg = netMsg.ReadMessage<Messages.BattleMessage>();

		singleton.client.connection.playerControllers[0].gameObject.GetComponent<Trainer>().hud.AddToBattleChat(msg.message);
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

	private IEnumerator SetupPlayer(NetworkConnection con, short playerControllerId, string characterName, Poke_Data starterData)
	{
		GameObject player = Instantiate(singleton.playerPrefab, startPos, Quaternion.identity) as GameObject;
		Trainer trainer = player.GetComponent<Trainer>();

		trainer.characterName = characterName;

		NetworkServer.AddPlayerForConnection(con, player, playerControllerId);

		trainer.PokemonRoster.Add(starterData);
		yield return null;
	}
}
public class PlayerSetupMessageAlpha : MessageBase
{
	public string characterName, starter;
	public Poke_Data starterData;
}