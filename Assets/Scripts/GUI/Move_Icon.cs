using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Move_Icon : MonoBehaviour
{
	public Move move;
	public Image border, icon, timer;
	public Text pp;
	public Color[] MoveTypes;

	void Update()
	{
		if(move.coolDownTime > 0.0f)
			timer.fillAmount = move.coolDownTimer / move.coolDownTime;
		else
			timer.fillAmount = 0.0f;
	}

	public void SetupIcon(Move move)
	{
		this.move = move;
		border.color = MoveTypes[(int)move.moveType];
		icon.sprite = move.icon;
		timer.fillAmount = move.coolDownTimer / move.coolDownTime;
		pp.text = move.ppCost.ToString();
	}
}

