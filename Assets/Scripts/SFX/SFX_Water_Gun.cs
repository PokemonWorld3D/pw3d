using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SFX_Water_Gun : MonoBehaviour
{
	private Water_Gun move;
	private ParticleSystem waterGun;
	private float lastTime;

	void Awake()
	{
		move = transform.root.GetComponent<Water_Gun>();
		waterGun = GetComponent<ParticleSystem>();
	}
	void OnParticleCollision(GameObject target)
	{
		if(!transform.root.gameObject.GetComponent<NetworkIdentity>().isServer || target != move.components.pokemon.enemy)
			return;

		PokemonComponents components = target.GetComponent<PokemonComponents>();

		Calculations.DealDamage(move.components.pokemon, components.pokemon, move);
	}
	void Update()
	{
		if(!transform.root.gameObject.GetComponent<NetworkIdentity>().isServer)
			return;

		if(waterGun.isPlaying)
		{
			if(Time.time - lastTime >= 1.0f)
			{
				move.components.pokemon.DeductPP();
				lastTime = Time.time;
			}
		}
	}
}