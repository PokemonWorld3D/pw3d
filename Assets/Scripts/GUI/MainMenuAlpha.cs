using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuAlpha : MonoBehaviour
{
	[SerializeField] private NetworkManagerAlpha netManager;
	[SerializeField] private Text messageText, charNameText;	
	[SerializeField] private GameObject characterCreatePanel, starterSelectPanel, bulbasaurPrefab, charmanderPrefab, squirtlePrefab;
	[SerializeField] private Color transparentColor, defaultColor;

	private GameObject starter;

	public void CreateBulbasaurStarter()
	{
//		starter = Instantiate(bulbasaurPrefab, transform.position, transform.rotation) as GameObject;
//		Pokemon bulbasaur = starter.GetComponent<Pokemon>();
//		bulbasaur.level = 5;
//		bulbasaur.nickName = string.IsNullOrEmpty(nickNameText.text) ? string.Empty : nickNameText.text;
//		bulbasaur.SetupPokemonFirstTime();
//		bulbasaur.equippedItem = string.Empty;
//		bulbasaur.slot = 0;
//		Poke_Data starterData = new Poke_Data(bulbasaur.pokemonName, bulbasaur.nickName, false, bulbasaur.level, (int)bulbasaur.nature,
//			                        bulbasaur.components.hpPP.curMaxHP, bulbasaur.components.hpPP.curMaxPP, bulbasaur.components.stats.curMaxATK, bulbasaur.components.stats.curMaxDEF,
//			                        bulbasaur.components.stats.curMaxSPATK, bulbasaur.components.stats.curMaxSPDEF, bulbasaur.components.stats.curMaxSPD, bulbasaur.components.hpPP.curHP,
//			                        bulbasaur.components.hpPP.curPP, bulbasaur.components.stats.curATK, bulbasaur.components.stats.curDEF, bulbasaur.components.stats.curSPATK,
//			                        bulbasaur.components.stats.curSPDEF, bulbasaur.components.stats.curSPD, bulbasaur.components.hpPP.hpEV, bulbasaur.components.hpPP.ppEV,
//			                        bulbasaur.components.stats.atkEV, bulbasaur.components.stats.defEV, bulbasaur.components.stats.spatkEV, bulbasaur.components.stats.spdefEV,
//			                        bulbasaur.components.stats.spdEV, bulbasaur.components.hpPP.hpIV, bulbasaur.components.hpPP.ppIV, bulbasaur.components.stats.atkIV,
//			                        bulbasaur.components.stats.defIV, bulbasaur.components.stats.spatkIV, bulbasaur.components.stats.spdefIV, bulbasaur.components.stats.spdIV,
//			                        bulbasaur.currentEXP, bulbasaur.equippedItem, 0);
//		netManager.starterData = starterData;
//		LogIn();
		messageText.text = "We're sorry, but Bulbasaur is currently unavailable for selection.";
	}
	public void CreateCharmanderStarter()
	{
		starter = Instantiate(charmanderPrefab, transform.position, transform.rotation) as GameObject;
		Pokemon charmander = starter.GetComponent<Pokemon>();
		charmander.level = 100;
		//charmander.nickName = string.IsNullOrEmpty(nickNameText.text) ? string.Empty : nickNameText.text;
		charmander.SetupPokemonFirstTime();
		charmander.equippedItem = string.Empty;
		charmander.slot = 0;
		Poke_Data starterData = new Poke_Data(charmander.pokemonName, charmander.nickName, false, charmander.level, (int)charmander.nature,
			                        charmander.components.hpPP.curMaxHP, charmander.components.hpPP.curMaxPP, charmander.components.stats.curMaxATK, charmander.components.stats.curMaxDEF,
			                        charmander.components.stats.curMaxSPATK, charmander.components.stats.curMaxSPDEF, charmander.components.stats.curMaxSPD, charmander.components.hpPP.curHP,
			                        charmander.components.hpPP.curPP, charmander.components.stats.curATK, charmander.components.stats.curDEF, charmander.components.stats.curSPATK,
			                        charmander.components.stats.curSPDEF, charmander.components.stats.curSPD, charmander.components.hpPP.hpEV, charmander.components.hpPP.ppEV,
			                        charmander.components.stats.atkEV, charmander.components.stats.defEV, charmander.components.stats.spatkEV, charmander.components.stats.spdefEV,
			                        charmander.components.stats.spdEV, charmander.components.hpPP.hpIV, charmander.components.hpPP.ppIV, charmander.components.stats.atkIV,
			                        charmander.components.stats.defIV, charmander.components.stats.spatkIV, charmander.components.stats.spdefIV, charmander.components.stats.spdIV,
			                        charmander.currentEXP, charmander.equippedItem, 0);
		netManager.starterData = starterData;
		LogIn();
	}
	public void CreateSquirtleStarter()
	{
		starter = Instantiate(squirtlePrefab, transform.position, transform.rotation) as GameObject;
		Pokemon squirtle = starter.GetComponent<Pokemon>();
		squirtle.level = 5;
		//squirtle.nickName = string.IsNullOrEmpty(nickNameText.text) ? string.Empty : nickNameText.text;
		squirtle.SetupPokemonFirstTime();
		squirtle.equippedItem = string.Empty;
		squirtle.slot = 0;
		Poke_Data starterData = new Poke_Data(squirtle.pokemonName, squirtle.nickName, false, squirtle.level, (int)squirtle.nature,
			                        squirtle.components.hpPP.curMaxHP, squirtle.components.hpPP.curMaxPP, squirtle.components.stats.curMaxATK, squirtle.components.stats.curMaxDEF,
			                        squirtle.components.stats.curMaxSPATK, squirtle.components.stats.curMaxSPDEF, squirtle.components.stats.curMaxSPD, squirtle.components.hpPP.curHP,
			                        squirtle.components.hpPP.curPP, squirtle.components.stats.curATK, squirtle.components.stats.curDEF, squirtle.components.stats.curSPATK,
			                        squirtle.components.stats.curSPDEF, squirtle.components.stats.curSPD, squirtle.components.hpPP.hpEV, squirtle.components.hpPP.ppEV,
			                        squirtle.components.stats.atkEV, squirtle.components.stats.defEV, squirtle.components.stats.spatkEV, squirtle.components.stats.spdefEV,
			                        squirtle.components.stats.spdEV, squirtle.components.hpPP.hpIV, squirtle.components.hpPP.ppIV, squirtle.components.stats.atkIV,
			                        squirtle.components.stats.defIV, squirtle.components.stats.spatkIV, squirtle.components.stats.spdefIV, squirtle.components.stats.spdIV,
			                        squirtle.currentEXP, squirtle.equippedItem, 0);
		netManager.starterData = starterData;
		LogIn();
	}
	public void CreateCharacter()
	{
		if(string.IsNullOrEmpty(charNameText.text))
		{
			messageText.text = "Please enter a valid name for yourself.";
			return;
		}
		else
		{
			netManager.characterName = charNameText.text;
			characterCreatePanel.SetActive(false);
			starterSelectPanel.SetActive(true);
		}
	}
	public void LogIn()
	{
		if(netManager.thisIsTheServer)
			netManager.InitServer();
		else
			StartCoroutine(netManager.JoinServer());
	}

	public void MouseEnterPokeBall(Image pokeBall)
	{
		pokeBall.color = transparentColor;
	}
	public void MouseExitPokeBall(Image pokeBall)
	{
		pokeBall.color = defaultColor;
	}
}
