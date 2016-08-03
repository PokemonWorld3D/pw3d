using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PokemonHPPP : NetworkBehaviour
{
	[SyncVar(hook="SyncCurHP")] public int curHP;
	[SyncVar(hook="SyncCurPP")] public int curPP;
	[SyncVar(hook="SyncCurMaxHP")] public int curMaxHP;
	[SyncVar(hook="SyncCurMaxPP")] public int curMaxPP;
	[SyncVar(hook="SyncMaxHP")] public int maxHP;
	[SyncVar(hook="SyncMaxPP")] public int maxPP;
	[SyncVar(hook="SyncHPEV")] public int hpEV;
	[SyncVar(hook="SyncPPEV")] public int ppEV;
	[SyncVar(hook="SyncHPIV")] public int hpIV;
	[SyncVar(hook="SyncPPIV")] public int ppIV;

	public GameObject damagePrefab;

	private float regenRate = 3.0f;
	private int ppRegen;
	private PokemonComponents components;
	
	void Awake()
	{
		components = GetComponent<PokemonComponents>();
	}
	void Start()
	{
		if(components.pokemon.trainer && components.pokemon.trainer.isServer)
			InvokeRepeating("Regen", 0.0f, regenRate);
	}
	
	#region HOOKS
	private void SyncCurHP(int curHP)
	{
		int difference = curHP - this.curHP;

		Vector3 here = new Vector3(transform.position.x, components.controller.height + 1.0f, transform.position.z);
		GameObject floatDmg = Instantiate(damagePrefab, here, Quaternion.identity) as GameObject;

		if(difference < 0)
			floatDmg.GetComponent<Floating_Damage>().AssignValues(Color.red, difference.ToString(), false);
		if(difference > 0 && this.curHP > 0)
			floatDmg.GetComponent<Floating_Damage>().AssignValues(Color.green, difference.ToString(), false);

		Destroy(floatDmg, 1.0f);

		this.curHP = curHP;
	}
	private void SyncCurPP(int _newValue)
	{
		curPP = _newValue;
	}
	private void SyncCurMaxHP(int _newValue)
	{
		curMaxHP = _newValue;
	}
	private void SyncCurMaxPP(int _newValue)
	{
		curMaxPP = _newValue;
	}
	private void SyncMaxHP(int _newValue)
	{
		maxHP = _newValue;
	}
	private void SyncMaxPP(int _newValue)
	{
		maxPP = _newValue;
	}
	private void SyncHPEV(int _newValue)
	{
		hpEV = _newValue;
	}
	private void SyncPPEV(int _newValue)
	{
		ppEV = _newValue;
	}
	private void SyncHPIV(int _newValue)
	{
		hpIV = _newValue;
	}
	private void SyncPPIV(int _newValue)
	{
		ppIV = _newValue;
	}
	#endregion
	
	public void SetupFirstTime()
	{
		hpIV = Random.Range(0, 32);
		ppIV = Random.Range(0, 32);
		maxHP = Calculations.CalculateHP(components.pokemon.baseHP, components.pokemon.level, hpIV, hpEV);
		maxPP = Calculations.CalculateHP(components.pokemon.basePP, components.pokemon.level, ppIV, ppEV);
		curMaxHP = maxHP;
		curMaxPP = maxPP;
		curHP = curMaxHP;
		curPP = curMaxPP;
		ppRegen = (int)(curMaxPP * 0.1f);

		if(ppRegen < 1)
			ppRegen = 1;
	}
	public void SetupExisting()
	{
		maxHP = Calculations.CalculateHP(components.pokemon.baseHP, components.pokemon.level, hpIV, hpEV);
		maxPP = Calculations.CalculateHP(components.pokemon.basePP, components.pokemon.level, ppIV, ppEV);
		ppRegen = (int)(curMaxPP * 0.1f);

		if(ppRegen < 1)
			ppRegen = 1;
	}
	public void LevelUp()
	{
		maxHP = Calculations.CalculateHP(components.pokemon.baseHP, components.pokemon.level, hpIV, hpEV);
		maxPP = Calculations.CalculateHP(components.pokemon.basePP, components.pokemon.level, ppIV, ppEV);
		curMaxHP = maxHP;
		curMaxPP = maxPP;
		curHP = curMaxHP;
		curPP = curMaxPP;
		ppRegen = (int)(curMaxPP * 0.1f);
		
		if(ppRegen < 1)
			ppRegen = 1;
	}

	private void Regen()
	{
		if(!components.pokemon.attacking)
			AdjCurPP(ppRegen);
	}

	[ServerCallback] public void AdjCurHP(int _adj, bool _critical)
	{
		if(components.conditions.protecting && _adj < 0)
			return;
		
		if(_adj >= -curHP && components.conditions.bracing)
		{
			_adj = (-curHP + 1);
			components.conditions.bracing = false;
		}

//		Vector3 here = new Vector3(transform.position.x, col.height, transform.position.z);
//		GameObject floatDmg = Instantiate(damagePrefab, here, Quaternion.identity) as GameObject;
//
//		if(_adj < 0)
//		{
//			floatDmg.GetComponent<Floating_Damage>().AssignValues(Color.red, _adj.ToString(), _critical);
////			if(sleeping)
////			{
////				RemoveBuffDebuff(BuffsAndDebuffs.SLEEPING, 0.0f, 0.0f, null);
////			}
//		}
//		if(_adj > 0)
//			floatDmg.GetComponent<Floating_Damage>().AssignValues(Color.green, _adj.ToString(), _critical);
//
//		Destroy(floatDmg, 1.0f);
//
//		NetworkServer.Spawn(floatDmg);

		curHP += _adj;
		
		if(curHP < 0)
			curHP = 0;
		
		if(curHP > curMaxHP)
			curHP = curMaxHP;
		
		if(curHP == 0)
			StartCoroutine(components.pokemon.Faint());
	}
	[ServerCallback] public void AdjCurPP(int _adj)
	{
		curPP += _adj;
		
		if(curPP < 0)
			curPP = 0;
		
		if(curPP > curMaxPP)
			curPP = curMaxPP;
	}
}
