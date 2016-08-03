using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Withdraw : Move
{
	public Buff defenseUp = new Buff
	{
		type = Buff.Types.DefUp,
		percentage = 0.10f,
		duration = Mathf.Infinity
	};

	public void Reset()
	{
		ResetMoveData("Withdraw", "The user withdraws its body into its hard shell, raising its defense.", false, false, false, false, false, PokemonType.WATER,
			MoveCategory.STATUS, 0.0f, 0);
	}
	public void StartWithdraw()
	{
		if(!isServer)
			return;

		if(!components.buffsDebuffs.Buffs.Contains(defenseUp))
		{
			components.buffsDebuffs.Buffs.Add(defenseUp);
			components.stats.AdjCurMaxDef((int)((float)components.stats.curMaxDEF * defenseUp.percentage));
			components.stats.AdjCurDef((int)((float)components.stats.curDEF * defenseUp.percentage));

			NetworkServer.SendToAll((short)Messages.MessageTypes.BATTLE_MESSAGE,
				Messages.StatModMessage(components.pokemon, this, StatType.DEFENSE, true));
		}
	}
	public void FinishWithdraw()
	{
		if(!isServer)
			return;

		if(components.buffsDebuffs.Buffs.Contains(defenseUp))
		{
			components.buffsDebuffs.Buffs.Remove(defenseUp);
			components.stats.AdjCurMaxDef(-(int)((float)components.stats.maxDEF * defenseUp.percentage));
			components.stats.AdjCurDef(-(int)((float)components.stats.curMaxDEF * defenseUp.percentage));
		}
	}
}
