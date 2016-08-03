using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class PokemonBuffsDebuffs : NetworkBehaviour
{
	public SyncListBuffs Buffs { get; private set; }
	public SyncListDebuffs Debuffs { get; private set; }

	private PokemonComponents components;

	public override void OnStartClient()
	{
		Debuffs.Callback = OnDebuffAdd;
	}

	void Awake()
	{
		components = GetComponent<PokemonComponents>();
		Buffs = new SyncListBuffs();
		Debuffs = new SyncListDebuffs();
	}
	void Start()
	{
		if(!isServer)
			return;

		InvokeRepeating("BuffTicker", 0.0f, 1.0f);
		InvokeRepeating("DebuffTicker", 0.0f, 1.0f);
	}

	private void BuffTicker()
	{
		for(int i = 0; i < Buffs.Count; i++)
		{
			Buff modded = new Buff
			{
				type = Buffs[i].type,
				percentage = Buffs[i].percentage,
				duration = Buffs[i].duration - 1.0f
			};

			if(modded.duration <= 0.0f)
			{
				Buffs.RemoveAt(i);
			}
			else
				Buffs[i] = modded;
		}
	}
	private void DebuffTicker()
	{
		for(int i = 0; i < Debuffs.Count; i++)
		{
			Debuff modded = new Debuff
			{
				type = Debuffs[i].type,
				percentage = Debuffs[i].percentage,
				duration = Debuffs[i].duration - 1.0f
			};

			if(modded.duration <= 0.0f)
				RemoveDebuff(i);
			else
				Debuffs[i] = modded;
		}
	}
	private void RemoveDebuff(int index)
	{
		if(Debuffs[index].type == Debuff.Types.AtkDown)
			components.stats.AdjCurAtk((int)((float)components.stats.curMaxATK * Debuffs[index].percentage));

		if(Debuffs[index].type == Debuff.Types.DefDown)
			components.stats.AdjCurDef((int)((float)components.stats.curMaxDEF * Debuffs[index].percentage));

		if(Debuffs[index].type == Debuff.Types.SpdDown)
			components.stats.RpcAdjSpeed((1.0f + Debuffs[index].percentage));

		Debuffs.RemoveAt(index);
	}
	private void OnBuffAdd(SyncListDebuffs.Operation op, int index)
	{
//		if(pokemon.trainer.isLocalPlayer && op == SyncList<Buff>.Operation.OP_ADD)
//			pokemon.hud.playerPokemonPortrait.SpawnBuffIcon(Buffs[index]);
	}
	private void OnDebuffAdd(SyncListDebuffs.Operation op, int index)
	{
		if(components.pokemon.trainer.isLocalPlayer && op == SyncList<Debuff>.Operation.OP_ADD)
			components.pokemon.hud.playerPokemonPortrait.SpawnDebuffIcon(Debuffs[index]);
	}
}
public struct Buff
{
	public enum Types { AtkUp = 0, DefUp = 1, SpATKUp = 2, SpDEFUp = 3, SpdUp = 4 }
	public Types type;
	public float percentage, duration;
}
public struct Debuff
{
	public enum Types { AtkDown = 0, DefDown = 1, SpATKDown = 2, SpDEFDown = 3, SpdDown = 4 }
	public Types type;
	public float percentage, duration;
}
public class SyncListBuffs : SyncListStruct<Buff>
{
	
}
public class SyncListDebuffs : SyncListStruct<Debuff>
{
	
}
