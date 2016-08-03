using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Buff_Debuff_Icon : MonoBehaviour
{
	public Image icon;
	public Text timer;
	public float duration = 999.0f;

	void Update()
	{
		timer.text = ((int)duration).ToString();
		duration -= Time.deltaTime;

		if(duration <= 0.0f)
			Destroy(gameObject);
	}

	public void SetupIcon(Sprite sprite, float duration)
	{
		icon.sprite = sprite;
		this.duration = duration;
	}
}
