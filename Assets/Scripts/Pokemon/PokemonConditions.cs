using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PokemonConditions : NetworkBehaviour
{
	[SyncVar(hook="SyncBurned")] public bool burned;
	[SyncVar(hook="SyncFrozen")] public bool frozen;
	[SyncVar(hook="SyncParalyzed")] public bool paralyzed;
	[SyncVar(hook="SyncPoisoned")] public bool poisoned;
	[SyncVar(hook="SyncSleeping")] public bool sleeping;
	[SyncVar(hook="SyncConfused")] public bool confused;
	[SyncVar(hook="SyncCurse")] public bool cursed;
	[SyncVar(hook="SyncEmbargo")] public bool embargoed;
	[SyncVar(hook="SyncEncore")] public bool encored;
	[SyncVar(hook="SyncFlinch")] public bool flinching;
	[SyncVar(hook="SyncHealBlock")] public bool healBlocked;
	[SyncVar(hook="SyncIdentified")] public bool identified;
	[SyncVar(hook="SyncInfatuated")] public bool infatuated;
	[SyncVar(hook="SyncNightmare")] public bool nightmared;
	[SyncVar(hook="SyncPartiallyTrapped")] public bool partiallyTrapped;
	[SyncVar(hook="SyncPerishSong")] public bool perishSonged;
	[SyncVar(hook="SyncSeeded")] public bool seeded;
//	[SyncVar(hook="SyncTaunted")]
//	public bool taunted;
//	[SyncVar(hook="SyncTormented")]
//	public bool tormented;
	[SyncVar(hook="SyncAquaRing")] public bool aquaRinged;
	[SyncVar(hook="SyncBracing")] public bool bracing;
//	[SyncVar(hook="SyncCenterOfAttention")]
//	public bool centerOfAttention;
	[SyncVar(hook="SyncDefenseCurl")] public bool defenseCurling;
	[SyncVar(hook="SyncRooting")] public bool rooting;
	[SyncVar(hook="SyncMagicCoat")] public bool magicallyCoated;
	[SyncVar(hook="SyncMagLevitating")] public bool magneticallyLevitating;
	[SyncVar(hook="SyncMinimized")] public bool minimized;
	[SyncVar(hook="SyncProtecting")] public bool protecting;
//	[SyncVar(hook="SyncSemiInvulnerable")]
//	public bool semiInvulnerable;
//	[SyncVar(hook="SyncSubstitute")]
//	public bool hasASubstitute;
	[SyncVar(hook="SyncTakingAim")] public bool takingAim;
	[SyncVar(hook="SyncTrapped")] public bool trapped;

	private PokemonComponents components;

	[SerializeField] private GameObject partiallyTrappedSFX;

	void Awake()
	{
		components = GetComponent<PokemonComponents>();
	}
	void Start()
	{
		if(!isServer)
			return;

		InvokeRepeating("ConditionTicker", 0.0f, 5.0f);
	}

	public void Burn(bool value)
	{
		if(!protecting)
			burned = value;
	}
	public void Flinch(bool value)
	{
		if(!protecting)
			flinching = value;
	}
	public void Protect(bool value)
	{
		protecting = value;
	}

	private void ConditionTicker()
	{
		if(burned)
		{
			int amount = (int)((float)components.hpPP.curHP * 0.125f);

			if(amount < 1)
				amount = 1;

			components.hpPP.AdjCurHP(-amount, false);
		}

		if(flinching)
			flinching = false;
	}

	#region Hooks
	public void SyncBurned(bool burned)
	{
		this.burned = burned;

		if(components.pokemon.trainer.isLocalPlayer)
			components.pokemon.hud.playerPokemonPortrait.ModifyStatusCondition(Player_Pokemon_Portrait.statusCondition.Burn, this.burned);
	}
	public void SyncFrozen(bool _frozen)
	{
		frozen = _frozen;
	}
	public void SyncParalyzed(bool _paralyzed)
	{
		paralyzed = _paralyzed;
	}
	public void SyncPoisoned(bool _poisoned)
	{
		poisoned = _poisoned;
	}
	public void SyncSleeping(bool _sleeping)
	{
		sleeping = _sleeping;
	}
	public void SyncConfused(bool _confused)
	{
		confused = _confused;
	}
	public void SyncCurse(bool _cursed)
	{
		cursed = _cursed;
	}
	public void SyncEmbargo(bool _embargoed)
	{
		embargoed = _embargoed;
	}
	public void SyncEncore(bool _encored)
	{
		encored = _encored;
	}
	public void SyncFlinch(bool flinching)
	{
		this.flinching = flinching;

		if(flinching)
			components.anim.speed = 0.0f;
		else
			components.anim.speed = 1.0f;
	}
	public void SyncHealBlock(bool _healBlocked)
	{
		healBlocked = _healBlocked;
	}
	public void SyncIdentified(bool _identified)
	{
		identified = _identified;
	}
	public void SyncInfatuated(bool _infatuated)
	{
		infatuated = _infatuated;
	}
	public void SyncNightmare(bool _nightmared)
	{
		nightmared = _nightmared;
	}
	public void SyncPartiallyTrapped(bool partiallyTrapped)
	{
		this.partiallyTrapped = partiallyTrapped;
	}
	public void SyncPerishSong(bool _perishSonged)
	{
		perishSonged = _perishSonged;
	}
	public void SyncSeeded(bool _seeded)
	{
		seeded = _seeded;
	}
//	public void SyncTaunted(bool _taunted)
//	{
//		taunted = _taunted;
//	}
//	public void SyncTormented(bool _tormented)
//	{
//		tormented = _tormented;
//	}
	public void SyncAquaRing(bool _aquaRinged)
	{
		aquaRinged = _aquaRinged;
	}
	public void SyncBracing(bool _bracing)
	{
		bracing = _bracing;
	}
//	public void SyncCenterOfAttention(bool _centerOfAttention)
//	{
//		centerOfAttention = _centerOfAttention;
//	}
	public void SyncDefenseCurl(bool _defenseCurling)
	{
		defenseCurling = _defenseCurling;
	}
	public void SyncRooting(bool _rooting)
	{
		rooting = _rooting;
	}
	public void SyncMagicCoat(bool _magicallyCoated)
	{
		magicallyCoated = _magicallyCoated;
	}
	public void SyncMagLevitating(bool _magneticallyLevitating)
	{
		magneticallyLevitating = _magneticallyLevitating;
	}
	public void SyncMinimized(bool _minimized)
	{
		minimized = _minimized;
	}
	public void SyncProtecting(bool _protecting)
	{
		protecting = _protecting;
	}
//	public void SyncSemiInvulnerable(bool _semiInvulnerable)
//	{
//		semiInvulnerable = _semiInvulnerable;
//	}
//	public void SyncSubstitute(bool _hasASubstitute)
//	{
//		hasASubstitute = _hasASubstitute;
//	}
	public void SyncTakingAim(bool _takingAim)
	{
		takingAim = _takingAim;
	}
	public void SyncTrapped(bool _trapped)
	{
		trapped = _trapped;
	}
	#endregion

	[ClientRpc] public void RpcPartiallyTrapped(string attack)
	{
		if(attack == "Fire Spin")
		{
			GameObject sfx = partiallyTrappedSFX.transform.FindChild("Fire Spin Effect").gameObject;
			sfx.transform.parent = null;
			sfx.SetActive(true);
		}
	}
}