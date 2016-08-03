using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SFX_Flamethrower : MonoBehaviour
{
	private Flamethrower move;
	private ParticleSystem flamethrower;
	private float lastTime;

	void Awake()
	{
		move = transform.root.GetComponent<Flamethrower>();
		flamethrower = GetComponent<ParticleSystem>();
	}
	void OnParticleCollision(GameObject target)
	{
		if(!transform.root.gameObject.GetComponent<NetworkIdentity>().isServer || target != move.components.pokemon.enemy)
			return;

		PokemonComponents components = target.GetComponent<PokemonComponents>();

		Calculations.DealDamage(move.components.pokemon, components.pokemon, move);

		if(Random.Range(0.00f, 1.00f) > 0.90f)	//10% chance
		{
			components.conditions.burned = true;

			NetworkServer.SendToAll((short)Messages.MessageTypes.BATTLE_MESSAGE, Messages.BurnMessage(components.pokemon, move));
		}
	}
	void Update()
	{
		if(!transform.root.gameObject.GetComponent<NetworkIdentity>().isServer)
			return;
		
		if(flamethrower.isPlaying)
		{
			if(Time.time - lastTime >= 1.0f)
			{
				move.components.pokemon.DeductPP();
				lastTime = Time.time;
			}
		}
	}
}
