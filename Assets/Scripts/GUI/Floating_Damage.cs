using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Floating_Damage : NetworkBehaviour
{
	public Text theText;
	public float scroll = 0.5f, duration = 0.5f, alpha = 1.0f;

	[HideInInspector]
	public Color color;
	
	void Update()
	{
//		if(alpha > 0)
//		{
			Vector3 curPos = transform.position;

			curPos.y += scroll * Time.deltaTime;
			transform.position = curPos;
//			alpha -= Time.deltaTime / duration;
//
//			Color curColor = theText.color;
//
//			curColor.a = alpha;
//			theText.color = color = curColor;
//		}
//		else
//			Destroy(gameObject);
	}

	public void AssignValues(Color color, string amount, bool crit)
	{
		theText.color = color;
		theText.text = amount;
		if(crit)
		{
			theText.fontStyle = FontStyle.BoldAndItalic;
			theText.fontSize = 18;
		}
	}
}
