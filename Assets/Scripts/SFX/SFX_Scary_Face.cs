using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SFX_Scary_Face : MonoBehaviour
{
	public Image scaryFace;	
	public float time = 1.5f;

	private float timer;

	void Update()
	{
		scaryFace.CrossFadeAlpha(0.0f, time, false);
		timer += Time.deltaTime;

		if(timer >= time)
		{
			timer = 0.0f;
			gameObject.SetActive(false);
		}
	}
}
