using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Chat : NetworkBehaviour
{
	private Trainer trainer;

	public HUD hud;
	public SyncListString MainChatHistory = new SyncListString();

	void Start()
	{
		trainer = GetComponent<Trainer>();
	}

	[Command]
	public void CmdSendMessage(string msg)
	{
		RpcReceiveMessage(trainer.characterName + ": " + msg);
	}

	[ClientRpc]
	private void RpcReceiveMessage(string msg)
	{
		MainChatHistory.Add(msg);
	}

	public void OnMainChatChange(SyncListString.Operation op, int index)
	{
		Debug.Log ("Message added index = " + index.ToString());
		if(isLocalPlayer && op == SyncListString.Operation.OP_ADD)
			hud.mainChat.text += "\n" + MainChatHistory[index];
	}
}
