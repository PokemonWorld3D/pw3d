using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SFX_Fire_Spin : NetworkBehaviour
{
	private Fire_Spin move;
	private ParticleSystem fireSpin;
	private float lastTime;
	
	void Awake()
	{
		move = transform.root.GetComponent<Fire_Spin>();
		fireSpin = GetComponent<ParticleSystem>();
	}
	void OnParticleCollision(GameObject target)
	{
		if(!transform.root.gameObject.GetComponent<NetworkIdentity>().isServer || target != move.components.pokemon.enemy)
			return;

		PokemonComponents components = target.GetComponent<PokemonComponents>();

		if(!components.conditions.partiallyTrapped)
		{
			components.conditions.partiallyTrapped = true;
			components.conditions.RpcPartiallyTrapped("Fire Spin");
		}
	}
	void Update()
	{
		if(!transform.root.gameObject.GetComponent<NetworkIdentity>().isServer)
			return;

		if(fireSpin.isPlaying && Time.time - lastTime >= 1.0f)
		{
			move.components.pokemon.DeductPP();
			lastTime = Time.time;
		}
	}
}
