using UnityEngine;
using System.Collections;

public class Panel_Disabler : MonoBehaviour
{
	public float duration;
	private float timer;

	void Update()
	{
		timer += Time.deltaTime;
		if(timer >= duration)
		{
			timer = 0.0f;
			gameObject.SetActive(false);
		}
	}
}
