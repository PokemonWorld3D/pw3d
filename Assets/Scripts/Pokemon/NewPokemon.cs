using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class NewPokemon : NetworkBehaviour
{
	public bool aiming, inBattle;

	[SyncVar(hook="HookLevel")] private int level { public get; set; }
	[HideInInspector][SyncVar(hook="HookNickName")] public string nickName;
	[HideInInspector][SyncVar(hook="HookIsCaptured")] public bool isCaptured;

	[SerializeField] private string pokemonName, description, evolvesInto;
	[SerializeField] private int pokemonNumber, evolveLevel, baseHP, baseATK, baseDEF, baseSPATK, baseSPDEF, baseSPD, baseEXP, hpEVYield, atkEVYield, defEVYield, spatkEVYield,
	spdefEVYield, genderRatio, captureRate;
	[SerializeField] private PokemonType typeOne;
	[SerializeField] private PokemonLevelRate levelRate;
	[SerializeField] private Material[] OriginalMats, ShinyMats, CaptureMats, EvolveMats;
	[SerializeField] private Sprite avatar;
	[SerializeField] private GameObject[] DisbaleThese;
	[SerializeField] private LensFlare evolveFlare;
	[SerializeField] private float maxWalkSpeed, maxSwimSpeed, runMultiplier, jumpHeight, flareGrowDelay, flareDieDelay, evolutionFlareSize;

	private PokemonGender gender;
	private PokemonNature nature;
	private int lastReqEXP, currentEXP, nextReqEXP, activeMoveIndex, pokeRosterSlot;
	private string equippedItem;
	private bool isShiny, isFromTrade, attacking, fainting;
	private GameObject enemy;
	private Trainer trainer;
	private HUD hud;
	private Move activeMove;
	private List<Move> LearnedMoves, KnownMoves;

	private Collider col;

	void Start()
	{
		
	}

	private void OnVarChange(object variable, object newValue)
	{
		
	}

	#region HOOKS
	private void HookLevel(int level)
	{
		this.level = level;
	}
	private void HookTrainerName(string trainersName)
	{
		this.trainersName = trainersName;
	}
	private void HookNickName(string nickName)
	{

	}
	private void HookIsCaptured(bool isCaptured)
	{

	}
	#endregion
}
