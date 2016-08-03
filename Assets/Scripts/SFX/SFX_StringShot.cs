using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SFX_StringShot : NetworkBehaviour
{
	private String_Shot move;
	private ParticleSystem stringShot;
	private float lastTime;
	private Debuff speedDown = new Debuff
	{
		type = Debuff.Types.SpdDown,
		percentage = 0.50f,
		duration = 10.0f
	};

	void Awake()
	{
		move = transform.root.GetComponent<String_Shot>();
		stringShot = GetComponent<ParticleSystem>();
	}
	void OnParticleCollision(GameObject target)
	{
		if(!transform.root.gameObject.GetComponent<NetworkIdentity>().isServer || target != move.components.pokemon.enemy)
			return;

		PokemonComponents components = target.GetComponent<PokemonComponents>();

		if(!components.buffsDebuffs.Debuffs.Contains(speedDown))
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

		if(stringShot.isPlaying)
		{
			if(Time.time - lastTime >= 1.0f)
			{
				move.components.pokemon.DeductPP();
				lastTime = Time.time;
			}
		}
	}
}
