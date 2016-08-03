using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Battle_Request : MonoBehaviour
{
	public Text text;
	public GameObject accept, decline, okay;
	public NetworkInstanceId otherTrainerNetId;

	private HUD hud;

	void Awake()
	{
		hud = GetComponentInParent<HUD>();
	}

	public void IncomingBattleRequest(string trainersName, NetworkInstanceId otherTrainerNetId)
	{
		text.text = trainersName + " would like to battle you.";
		gameObject.SetActive(true);
		accept.SetActive(true);
		decline.SetActive(true);
		this.otherTrainerNetId = otherTrainerNetId;
	}
	public void AcceptRequest()
	{
		hud.trainer.CmdAcceptBattleRequest(otherTrainerNetId);
	}
	public void DeclineRequest()
	{
		hud.trainer.CmdDeclineBattleRequest(otherTrainerNetId);
	}
	public void RequestAccepted(string trainersName)
	{
		text.text = trainersName + " has accepted your request for a Pokemon battle.";
		gameObject.SetActive(true);
		okay.SetActive(true);
	}
	public void RequestDenied(string trainersName)
	{
		text.text = trainersName + " has declined your request for a Pokemon battle.";
		gameObject.SetActive(true);
		okay.SetActive(true);
	}
}
