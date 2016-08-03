using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class Growl : Move
{
	public ParticleSystem growl;
	public Debuff attackDown = new Debuff
	{
		type = Debuff.Types.AtkDown,
		percentage = 0.10f,
		duration = 10.0f
	};

	public void Reset()
	{
		ResetMoveData("Growl", "The user growls in an endearing way, making opposing Pokémon less wary. This lowers their Attack stats.", false, false, false, false, false,
			PokemonType.NORMAL, MoveCategory.STATUS, 0.0f, 0);
	}
	public void GrowlStart()
	{
		growl.Play();
		components.audioS.PlayOneShot(soundEffect);

		if(!isServer)
			return;
		
		Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, range);
		
		for(int i = 0; i < cols.Length; i++)
		{
			if(cols[i].gameObject == components.pokemon.enemy)
			{
				PokemonComponents targetComponents = cols[i].GetComponent<PokemonComponents>();

				if(!targetComponents.buffsDebuffs.Debuffs.Contains(attackDown))
				{
					targetComponents.buffsDebuffs.Debuffs.Add(attackDown);
					targetComponents.stats.AdjCurAtk(-(int)((float)targetComponents.stats.curATK * attackDown.percentage));

					NetworkServer.SendToAll((short)Messages.MessageTypes.BATTLE_MESSAGE,
						Messages.StatModMessage(targetComponents.pokemon, this, StatType.ATTACK, false));
				}
			}
		}
	}
}