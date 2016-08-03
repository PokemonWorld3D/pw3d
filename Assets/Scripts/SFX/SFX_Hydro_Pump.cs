using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SFX_Hydro_Pump : MonoBehaviour
{
	private Hydro_Pump move;
	private ParticleSystem hydroPump;
	private float lastTime;

	void Awake()
	{
		move = transform.root.GetComponent<Hydro_Pump>();
		hydroPump = GetComponent<ParticleSystem>();
	}
	void OnParticleCollision(GameObject target)
	{
		if(!transform.root.gameObject.GetComponent<NetworkIdentity>().isServer || target != move.components.pokemon.enemy)
			return;
		
		PokemonComponents components = target.GetComponent<PokemonComponents>();

		Calculations.DealDamage(move.components.pokemon, components.pokemon, move);
	}
}
