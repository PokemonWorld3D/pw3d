using UnityEngine;
using System.Collections;

public class SFX_Inferno : MonoBehaviour
{
	private ParticleSystem fire;
	private ParticleSystem.Particle[] particles;
	
	void Start()
	{
		fire = GetComponent<ParticleSystem>();
	}
	void Update()
	{
		if(fire.isStopped && fire.particleCount == 0)
			Destroy(gameObject);
	}
}
