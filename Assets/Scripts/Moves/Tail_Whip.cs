using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Tail_Whip : Move 
{
	public Debuff defenseDown = new Debuff
	{
		type = Debuff.Types.AtkDown,
		percentage = 0.10f,
		duration = 10.0f
	};

	public void Reset()
	{
		ResetMoveData("Tail Whip", "The user wags its tail cutely, making opposing Pokémon less wary and lowering their defense temporarily.", false, false, false, false, false,
			PokemonType.NORMAL, MoveCategory.STATUS, 0.0f, 0);
	}
	public void TailWhipStart()
	{
		if(!isServer)
			return;

		Collider[] cols = Physics.OverlapSphere(gameObject.transform.position, range);

		for(int i = 0; i < cols.Length; i++)
		{
			if(cols[i].gameObject == components.pokemon.enemy)
			{
				PokemonComponents targetComponents = cols[i].GetComponent<PokemonComponents>();
				if(!targetComponents.buffsDebuffs.Debuffs.Contains(defenseDown))
				{
					targetComponents.buffsDebuffs.Debuffs.Add(defenseDown);
					targetComponents.stats.AdjCurDef(-(int)((float)targetComponents.stats.curDEF * defenseDown.percentage));

					NetworkServer.SendToAll((short)Messages.MessageTypes.BATTLE_MESSAGE,
						Messages.StatModMessage(targetComponents.pokemon, this, StatType.DEFENSE, false));
				}
			}
		}

		components.pokemon.DeductPP();
	}
}
