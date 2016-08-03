using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SFX_Bubble : MonoBehaviour
{
	private Bubble move;
	private ParticleSystem bubble;
	private float lastTime;
	private Debuff speedDown = new Debuff
	{
		type = Debuff.Types.SpdDown,
		percentage = 0.10f,
		duration = 30.0f
	};

	void Awake()
	{
		move = transform.root.GetComponent<Bubble>();
		bubble = GetComponent<ParticleSystem>();
	}
	void OnParticleCollision(GameObject target)
	{
		if(!transform.root.gameObject.GetComponent<NetworkIdentity>().isServer || target != move.components.pokemon.enemy)
			return;

		PokemonComponents components = target.GetComponent<PokemonComponents>();

		Calculations.DealDamage(move.components.pokemon, components.pokemon, move);

		if(Random.Range(0.00f, 1.00f) > 0.90f)		//10% CHANCE
		{
			components.buffsDebuffs.Debuffs.Add(speedDown);
			components.stats.AdjSpeed(speedDown.percentage);

			NetworkServer.SendToAll((short)Messages.MessageTypes.BATTLE_MESSAGE,
				Messages.StatModMessage(components.pokemon, move, StatType.SPEED, false));
		}
	}
	void Update()
	{
		if(!transform.root.gameObject.GetComponent<NetworkIdentity>().isServer)
			return;

		if(bubble.isPlaying)
		{
			if(Time.time - lastTime >= 1.0f)
			{
				move.components.pokemon.DeductPP();
				lastTime = Time.time;
			}
		}
	}
}