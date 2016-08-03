using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Messages
{
	public enum MessageTypes{ CHAT_MESSAGE = 1000, BATTLE_MESSAGE = 1001 }

	public class ChatMessage : MessageBase
	{
		public string message;
	}
	public class BattleMessage : MessageBase
	{
		public string message;
	}

	public static BattleMessage DamageMessage(Pokemon attacker, Pokemon target, Move moveUsed, int damage, bool critical)
	{
		string attackersName = string.IsNullOrEmpty(attacker.nickName) ? attacker.pokemonName : attacker.nickName;
		string targetsName = string.IsNullOrEmpty(target.nickName) ? target.pokemonName : target.nickName;
		BattleMessage msg = new BattleMessage();


		if(critical)
		{
			msg.message = attackersName + "(" + attacker.trainersName + ") dealt " + damage + " damage to " + targetsName + "(" + target.trainersName +
			") using " + moveUsed.moveName + "!!!";

			return msg;
		}
		else
		{
			msg.message = attackersName + "(" + attacker.trainersName + ") dealt " + damage + " damage to " + targetsName + "(" + target.trainersName +
			") using " + moveUsed.moveName + ".";

			return msg;
		}
	}
	public static BattleMessage BurnMessage(Pokemon target, Move moveUsed)
	{
		string targetsName = string.IsNullOrEmpty(target.nickName) ? target.pokemonName : target.nickName;
		BattleMessage msg = new BattleMessage();

		msg.message = targetsName + "(" + target.trainersName + ") was burned by " + moveUsed.moveName + "!";

		return msg;
	}
	public static BattleMessage FlinchMessage(Pokemon target, Move moveUsed)
	{
		string targetsName = string.IsNullOrEmpty(target.nickName) ? target.pokemonName : target.nickName;
		BattleMessage msg = new BattleMessage();

		msg.message = moveUsed.moveName + " caused " + targetsName + "(" + target.trainersName + ") to flinch!";

		return msg;
	}
	public static BattleMessage PartiallyTrappedMessage(Pokemon target, Move moveUsed)
	{
		string targetsName = string.IsNullOrEmpty(target.nickName) ? target.pokemonName : target.nickName;
		BattleMessage msg = new BattleMessage();

		msg.message = targetsName + "(" + target.trainersName + ") was trapped by " + moveUsed.moveName + "!";

		return msg;
	}
	public static BattleMessage StatModMessage(Pokemon target, Move moveUsed, StatType stat, bool trueForUp)
	{
		string targetsName = string.IsNullOrEmpty(target.nickName) ? target.pokemonName : target.nickName;
		BattleMessage msg = new BattleMessage();
		string statString = string.Empty;

		if(stat == StatType.ATTACK)
			statString = "attack";
		else if(stat == StatType.DEFENSE)
			statString = "defense";
		else if(stat == StatType.SPECIALATTACK)
			statString = "special attack";
		else if(stat == StatType.SPECIALDEFENSE)
			statString = "special defense";
		else if(stat == StatType.SPEED)
			statString = "speed";

		if(trueForUp)
			msg.message = targetsName + "(" + target.trainersName + ") raised its " + statString + " using " + moveUsed.moveName + "!";
		else
			msg.message = targetsName + "(" + target.trainersName + ") had its " + statString + " lowered by " + moveUsed.moveName + "!";

		return msg;
	}
}
