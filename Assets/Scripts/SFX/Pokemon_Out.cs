using UnityEngine;
using System.Collections;

public class Pokemon_Out : MonoBehaviour
{
	private ParticleSystem system;
	private ParticleSystem.Particle[] particles;
	private int particlesRemaining = 100;
	
	void Start()
	{
		system = GetComponent<ParticleSystem>();
	}
	void Update()
	{
		if(system.isStopped)
		{
			if(particlesRemaining == 0)
			{
				Destroy(gameObject);
			}
			particlesRemaining = system.particleCount;
		}
	}
}

