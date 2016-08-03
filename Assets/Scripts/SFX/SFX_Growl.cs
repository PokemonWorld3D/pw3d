using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SFX_Growl : MonoBehaviour
{
	public Image growl;	
	public float time = 0.5f;
	
	void Update()
	{
		growl.CrossFadeAlpha(0.0f, time, false);

		if(growl.color.a <= 0.0f)
			gameObject.SetActive(false);
	}
}
