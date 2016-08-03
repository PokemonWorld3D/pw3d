using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SFX_Smokescreen : MonoBehaviour
{
	private Smokescreen move;
	private ParticleSystem smokescreen;
	private float lastTime;

	void Awake()
	{
		move = transform.root.GetComponent<Smokescreen>();
		smokescreen = GetComponent<ParticleSystem>();
	}
	void Update()
	{
		if(smokescreen.isPlaying)
		{
			if(Time.time - lastTime >= 1.0f)
			{
				move.components.pokemon.DeductPP();
				lastTime = Time.time;
			}
		}
	}
}
