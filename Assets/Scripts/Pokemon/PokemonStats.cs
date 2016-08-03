using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PokemonStats : NetworkBehaviour
{
	[SyncVar(hook="SyncCurATK")] public int curATK;
	[SyncVar(hook="SyncCurDEF")] public int curDEF;
	[SyncVar(hook="SyncCurSPATK")] public int curSPATK;
	[SyncVar(hook="SyncCurSPDEF")] public int curSPDEF;
	[SyncVar(hook="SyncCurSPD")] public int curSPD;
	[SyncVar(hook="SyncCurMaxATK")] public int curMaxATK;
	[SyncVar(hook="SyncCurMaxDEF")] public int curMaxDEF;
	[SyncVar(hook="SyncCurMaxSPATK")] public int curMaxSPATK;
	[SyncVar(hook="SyncCurMaxSPDEF")] public int curMaxSPDEF;
	[SyncVar(hook="SyncCurMaxSPD")] public int curMaxSPD;
	[SyncVar(hook="SyncMaxATK")] public int maxATK;
	[SyncVar(hook="SyncMaxDEF")] public int maxDEF;
	[SyncVar(hook="SyncMaxSPATK")] public int maxSPATK;
	[SyncVar(hook="SyncMaxSPDEF")] public int maxSPDEF;
	[SyncVar(hook="SyncMaxSPD")] public int maxSPD;
	[SyncVar(hook="SyncATKIV")] public int atkIV;
	[SyncVar(hook="SyncDEFIV")] public int defIV;
	[SyncVar(hook="SyncSPATKIV")] public int spatkIV;
	[SyncVar(hook="SyncSPDEFIV")] public int spdefIV;
	[SyncVar(hook="SyncSPDIV")] public int spdIV;
	[SyncVar(hook="SyncATKEV")] public int atkEV;
	[SyncVar(hook="SyncDEFEV")] public int defEV;
	[SyncVar(hook="SyncSPATKEV")] public int spatkEV;
	[SyncVar(hook="SyncSPDEFEV")] public int spdefEV;
	[SyncVar(hook="SyncSPDEV")] public int spdEV;
	
	private PokemonComponents components;
	
	void Awake()
	{
		components = GetComponent<PokemonComponents>();
	}

	[ClientRpc] public void RpcAdjSpeed(float adj)
	{
		AdjSpeed(adj);
	}

	public void AdjSpeed(float adj)
	{
		components.pokemon.trainer.components.pokeControl.curWalkSpeed = components.pokemon.trainer.components.pokeControl.MaxWalkSpeed * adj;
		components.anim.speed = adj;

		if(components.pokemon.trainer.components.pokeControl.curWalkSpeed > components.pokemon.trainer.components.pokeControl.MaxWalkSpeed)
		{
			components.pokemon.trainer.components.pokeControl.curWalkSpeed = components.pokemon.trainer.components.pokeControl.MaxWalkSpeed;
			components.anim.speed = 1.0f;
		}

		if(components.pokemon.trainer.components.pokeControl.curWalkSpeed < 0.0f)
		{
			components.pokemon.trainer.components.pokeControl.curWalkSpeed = 0.0f;
			components.anim.speed = 0.0f;
		}
	}

	#region HOOKS
	private void SyncCurATK(int curATK)
	{
		this.curATK = curATK;
	}
	private void SyncCurDEF(int _newValue)
	{
		curDEF = _newValue;
	}
	private void SyncCurSPATK(int _newValue)
	{
		curSPATK = _newValue;
	}
	private void SyncCurSPDEF(int _newValue)
	{
		curSPDEF = _newValue;
	}
	private void SyncCurSPD(int _newValue)
	{
		curSPD = _newValue;
	}
	private void SyncCurMaxATK(int _newValue)
	{
		curMaxATK = _newValue;
	}
	private void SyncCurMaxDEF(int _newValue)
	{
		curMaxDEF = _newValue;
	}
	private void SyncCurMaxSPATK(int _newValue)
	{
		curMaxSPATK = _newValue;
	}
	private void SyncCurMaxSPDEF(int _newValue)
	{
		curMaxSPDEF = _newValue;
	}
	private void SyncCurMaxSPD(int _newValue)
	{
		curMaxSPD = _newValue;
	}
	private void SyncMaxATK(int _newValue)
	{
		maxATK = _newValue;
	}
	private void SyncMaxDEF(int _newValue)
	{
		maxDEF = _newValue;
	}
	private void SyncMaxSPATK(int _newValue)
	{
		maxSPATK = _newValue;
	}
	private void SyncMaxSPDEF(int _newValue)
	{
		maxSPDEF = _newValue;
	}
	private void SyncMaxSPD(int _newValue)
	{
		maxSPD = _newValue;
	}
	private void SyncATKEV(int _newValue)
	{
		atkEV = _newValue;
	}
	private void SyncDEFEV(int _newValue)
	{
		defEV = _newValue;
	}
	private void SyncSPATKEV(int _newValue)
	{
		spatkEV = _newValue;
	}
	private void SyncSPDEFEV(int _newValue)
	{
		spdefEV = _newValue;
	}
	private void SyncSPDEV(int _newValue)
	{
		spdEV = _newValue;
	}
	private void SyncATKIV(int _newValue)
	{
		atkIV = _newValue;
	}
	private void SyncDEFIV(int _newValue)
	{
		defIV = _newValue;
	}
	private void SyncSPATKIV(int _newValue)
	{
		spatkIV = _newValue;
	}
	private void SyncSPDEFIV(int _newValue)
	{
		spdefIV = _newValue;
	}
	private void SyncSPDIV(int _newValue)
	{
		spdIV = _newValue;
	}
	#endregion
	
	public void SetupFirstTime()
	{
		atkIV = Random.Range(0, 32);
		defIV = Random.Range(0, 32);
		spatkIV = Random.Range(0, 32);
		spdefIV = Random.Range(0, 32);
		spdIV = Random.Range(0, 32);
		maxATK = Calculations.CalculateStat(components.pokemon.baseATK, components.pokemon.level, atkIV, atkEV, components.pokemon.nature, StatType.ATTACK);
		maxDEF = Calculations.CalculateStat(components.pokemon.baseDEF, components.pokemon.level, defIV, defEV, components.pokemon.nature, StatType.DEFENSE);
		maxSPATK = Calculations.CalculateStat(components.pokemon.baseSPATK, components.pokemon.level, spatkIV, spatkEV, components.pokemon.nature, StatType.SPECIALATTACK);
		maxSPDEF = Calculations.CalculateStat(components.pokemon.baseSPDEF, components.pokemon.level, spdefIV, spdefEV, components.pokemon.nature, StatType.SPECIALDEFENSE);
		maxSPD = Calculations.CalculateStat(components.pokemon.baseSPD, components.pokemon.level, spdIV, spdEV, components.pokemon.nature, StatType.SPEED);
		curMaxATK = maxATK;
		curMaxDEF = maxDEF;
		curMaxSPATK = maxSPATK;
		curMaxSPDEF = maxSPDEF;
		curMaxSPD = maxSPD;
		curATK = maxATK;
		curDEF = maxDEF;
		curSPATK = maxSPATK;
		curSPDEF = maxSPDEF;
		curSPD = maxSPD;
	}
	public void SetupExisting()
	{
		maxATK = Calculations.CalculateStat(components.pokemon.baseATK, components.pokemon.level, atkIV, atkEV, components.pokemon.nature, StatType.ATTACK);
		maxDEF = Calculations.CalculateStat(components.pokemon.baseDEF, components.pokemon.level, defIV, defEV, components.pokemon.nature, StatType.DEFENSE);
		maxSPATK = Calculations.CalculateStat(components.pokemon.baseSPATK, components.pokemon.level, spatkIV, spatkEV, components.pokemon.nature, StatType.SPECIALATTACK);
		maxSPDEF = Calculations.CalculateStat(components.pokemon.baseSPDEF, components.pokemon.level, spdefIV, spdefEV, components.pokemon.nature, StatType.SPECIALDEFENSE);
		maxSPD = Calculations.CalculateStat(components.pokemon.baseSPD, components.pokemon.level, spdIV, spdEV, components.pokemon.nature, StatType.SPEED);
	}
	public void LevelUp()
	{
		maxATK = Calculations.CalculateStat(components.pokemon.baseATK, components.pokemon.level, atkIV, atkEV, components.pokemon.nature, StatType.ATTACK);
		maxDEF = Calculations.CalculateStat(components.pokemon.baseDEF, components.pokemon.level, defIV, defEV, components.pokemon.nature, StatType.DEFENSE);
		maxSPATK = Calculations.CalculateStat(components.pokemon.baseSPATK, components.pokemon.level, spatkIV, spatkEV, components.pokemon.nature, StatType.SPECIALATTACK);
		maxSPDEF = Calculations.CalculateStat(components.pokemon.baseSPDEF, components.pokemon.level, spdefIV, spdefEV, components.pokemon.nature, StatType.SPECIALDEFENSE);
		maxSPD = Calculations.CalculateStat(components.pokemon.baseSPD, components.pokemon.level, spdIV, spdEV, components.pokemon.nature, StatType.SPEED);
		curMaxATK = maxATK;
		curMaxDEF = maxDEF;
		curMaxSPATK = maxSPATK;
		curMaxSPDEF = maxSPDEF;
		curMaxSPD = maxSPD;
		curATK = maxATK;
		curDEF = maxDEF;
		curSPATK = maxSPATK;
		curSPDEF = maxSPDEF;
		curSPD = maxSPD;
	}

	public void AdjCurAtk(int adj)
	{
		curATK += adj;

		if(curATK > curMaxATK)
			curATK = curMaxATK;

		if(curATK < 0)
			curATK = 0;
	}
	public void AdjCurDef(int adj)
	{
		curDEF += adj;

		if(curDEF > curMaxDEF)
			curDEF = curMaxDEF;

		if(curDEF < 0)
			curDEF = 0;
	}

	public void AdjCurMaxDef(int adj)
	{
		curMaxDEF += adj;

		if(curDEF < 0)
			curDEF = 0;
	}
}
