using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class HUD : MonoBehaviour
{
	public Trainer trainer;
	public GameObject wildPokemonPanel, otherTrainerPanel, battleRequestPanel;
	public Player_Pokemon_Portrait playerPokemonPortrait;
	public Enemy_Pokemon_Portrait enemyPokemonPortrait;
	public Battle_Request battleRequestScript;
	public PokemonRosterPanel pokemonRosterPanel;

	public InputField chatInput;
	public Text mainChat, battleChat;
	
	private GameObject target;

	public void SetTrainer(Trainer trainer)
	{
		this.trainer = trainer;
		this.trainer.hud = this;
		pokemonRosterPanel.Setup();
	}
	public void DisplayWildPokemonPanel(GameObject target)
	{
		wildPokemonPanel.SetActive(true);
		this.target = target;
	}
	public void HideWildPokemonPanel()
	{
		wildPokemonPanel.SetActive(false);
		target = null;
	}
	public void WildPokemonBattle()
	{
		NetworkInstanceId netId = target.GetComponent<NetworkIdentity>().netId;

		trainer.CmdInitWildPokemonBattle(netId);
		HideWildPokemonPanel();
	}
	public void DisplayOtherTrainerPanel(GameObject target)
	{
		otherTrainerPanel.SetActive(true);
		this.target = target;
	}
	public void HideOtherTrainerPanel()
	{
		otherTrainerPanel.SetActive(false);
		target = null;
	}
	public void RequestTrainerBattle()
	{
		Trainer otherTrainer = target.GetComponent<Trainer>();

		if(!trainer.inBattle && trainer.canBattle && !otherTrainer.inBattle && otherTrainer.canBattle)
			trainer.CmdRequestTrainerBattle(otherTrainer.netId);
	}

	public void AddToMainChat(string message)
	{
		mainChat.text += "\n" + message;
	}
	public void AddToBattleChat(string message)
	{
		battleChat.text += "\n" + message;
	}


	public void SendChatMessage()
	{
		if(!string.IsNullOrEmpty(chatInput.text.Trim()))
		{
			Messages.ChatMessage msg = new Messages.ChatMessage();
			msg.message = chatInput.text;
			NetworkManager.singleton.client.Send((short)Messages.MessageTypes.CHAT_MESSAGE, msg);

			chatInput.text = string.Empty;
		}
	}
}
