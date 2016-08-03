using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Pokemon : NetworkBehaviour
{
	public PokemonComponents components { get; private set; }

	public string pokemonName, description, abilityOne, abilityTwo, evolvesInto;
	public int pokemonNumber, evolveLevel, baseHP, basePP, baseATK, baseDEF, baseSPATK, baseSPDEF, baseSPD, baseEXP, hpEVYield,
	ppEVYield, atkEVYield, defEVYield, spatkEVYield, spdefEVYield, spdEVYield, baseFriendship, genderRatio, captureRate;
	public PokemonType typeOne, typeTwo;
	public PokemonLevelRate levelRate;
	public Material[] OriginalMats, ShinyMats, CaptureMats, EvolveMats;
	public Sprite avatar;
	public GameObject[] Disable;
	public LensFlare evolveFlare;
	public float maxWalkSpeed = 1.0f, maxSwimSpeed = 1.0f, runMultiplier = 1.0f, jumpHeight = 1.0f, flareGrowDelay = 0.05f, flareDieDelay = 0.075f, evolutionFlareSize = 1.0f;
	public bool canSwim = false, aiming = false;

	[HideInInspector][SyncVar(hook="SyncLevel")] public int level;
	[HideInInspector][SyncVar(hook="SyncTrainerName")] public string trainersName;
	[HideInInspector][SyncVar(hook="SyncNickName")] public string nickName;
	[HideInInspector][SyncVar(hook="SyncIsCaptured")] public bool isCaptured;
	[HideInInspector][SyncVar(hook="SyncGender")] public PokemonGender gender;
	[HideInInspector][SyncVar(hook="SyncNature")] public PokemonNature nature;
	[HideInInspector][SyncVar(hook="SyncLastEXP")] public int lastReqEXP;
	[HideInInspector][SyncVar(hook="SyncCurEXP")] public int currentEXP;
	[HideInInspector][SyncVar(hook="SyncNextEXP")] public int nextReqEXP;
	public bool isInBattle;
	[HideInInspector][SyncVar(hook="SyncMove")] public int activeMoveIndex;

	[HideInInspector] public string equippedItem;
	[HideInInspector] public bool isSetup, isShiny, isFromTrade, attacking, fainting;
	[HideInInspector] public int slot;
	[HideInInspector] public GameObject enemy;
	[HideInInspector] public Trainer trainer;
	[HideInInspector] public HUD hud;
	[HideInInspector] public Move activeMove;
	[HideInInspector] public List<Move> LearnedMoves, KnownMoves;

	private Collider col;

	void Awake()
	{
		col = GetComponent<Collider>();
		components = GetComponent<PokemonComponents>();
		GetComponents<Move>(LearnedMoves);
	}

	#region Setup Pokemon
	public void SetupPokemonFirstTime()
	{
		System.Array natures = System.Enum.GetValues(typeof(PokemonNature));
		nature = (PokemonNature)natures.GetValue(UnityEngine.Random.Range(0, 24));
		
		components.hpPP.SetupFirstTime();
		components.stats.SetupFirstTime();
		
		if(components.stats.defIV == 10 && components.stats.spdIV == 10 && components.stats.spatkIV == 10)
		{
			if(components.stats.atkIV == 2 || components.stats.atkIV == 3 || components.stats.atkIV == 6 || components.stats.atkIV == 7 || components.stats.atkIV == 10 ||
				components.stats.atkIV == 11 || components.stats.atkIV == 14 || components.stats.atkIV == 15)
			{
				isShiny = true;
				GetComponentInChildren<SkinnedMeshRenderer>().materials = ShinyMats;
			}
		}
		
		if(components.stats.atkIV > genderRatio)
			gender = PokemonGender.MALE;
		else if(components.stats.atkIV <= genderRatio)
			gender = PokemonGender.FEMALE;
		
		if(genderRatio == 0)
			gender = PokemonGender.NONE;
		
		lastReqEXP = Calculations.CalculateCurrentXP(level - 1, levelRate);
		currentEXP = Calculations.CalculateCurrentXP(level, levelRate);
		nextReqEXP = Calculations.CalculateRequiredXP(level, levelRate);
		
		RpcSetupMoves();

		isSetup = true;
	}
	public void SetupExistingPokemon()
	{
		trainersName = trainer.characterName;
		components.hpPP.SetupExisting();
		components.stats.SetupExisting();
		
		if(components.stats.atkIV > genderRatio)
			gender = PokemonGender.MALE;
		else if(components.stats.atkIV <= genderRatio)
			gender = PokemonGender.FEMALE;
		
		if(genderRatio == 0)
			gender = PokemonGender.NONE;
		
		lastReqEXP = Calculations.CalculateCurrentXP(level - 1, levelRate);
		nextReqEXP = Calculations.CalculateRequiredXP(level, levelRate);
		
		RpcSetupMoves();
		
		isCaptured = true;
		isSetup = true;
		
		hud = trainer.hud;
	}
	#endregion

	#region Hooks
	public void SyncLevel(int level)
	{
		this.level = level;
	}
	public void SyncIsCaptured(bool isCaptured)
	{
		this.isCaptured = isCaptured;
	}
	public void SyncTrainerName(string trainersName)
	{
		this.trainersName = trainersName;
	}
	public void SyncNickName(string nickName)
	{
		this.nickName = nickName;
	}
	public void SyncGender(PokemonGender gender)
	{
		this.gender = gender;
	}
	public void SyncNature(PokemonNature nature)
	{
		this.nature = nature;
	}
	public void SyncLastEXP(int lastReqEXP)
	{
		this.lastReqEXP = lastReqEXP;
	}
	public void SyncCurEXP(int currentEXP)
	{
		this.currentEXP = currentEXP;
	}
	public void SyncNextEXP(int nextReqEXP)
	{
		this.nextReqEXP = nextReqEXP;
	}
	public void SyncMove(int activeMoveIndex)
	{
		if(activeMove.coolDownTimer == 0.0f)
			activeMove.enabled = false;

		activeMove.isActiveMove = false;
		this.activeMoveIndex = activeMoveIndex;
		activeMove = KnownMoves[activeMoveIndex];
		activeMove.isActiveMove = true;
		activeMove.enabled = true;

		if(trainer && trainer.isLocalPlayer)
			hud.playerPokemonPortrait.movePanelScript.UpdateActiveMove(this.activeMoveIndex);
	}
	#endregion

	#region ClientRPCs
	[ClientRpc] public void RpcSetupMoves()
	{
		KnownMoves = new List<Move>();

		for(int i = 0; i < LearnedMoves.Count; i++)
		{
			if(level >= LearnedMoves[i].levelLearned)
				KnownMoves.Add(LearnedMoves[i]);
		}

		if(KnownMoves.Count > 0)
		{
			activeMove = KnownMoves[0];
			activeMoveIndex = 0;
			activeMove.isActiveMove = true;
			activeMove.enabled = true;
		}
	}
	[ClientRpc] public void RpcBeingCaptured()
	{
		components.ai.SetState(PokemonStates.Captured);
		col.enabled = false;
		components.mesh.materials = CaptureMats;
	}
	[ClientRpc] public void RpcDisable()
	{
		components.anim.speed = 0.0f;
		components.mesh.enabled = false;
		col.enabled = false;

		foreach(GameObject g in Disable)
			g.SetActive(false);
	}
	[ClientRpc] public void RpcEnable()
	{
		components.anim.speed = 1.0f;
		components.mesh.materials = OriginalMats;
		components.mesh.enabled = true;
		col.enabled = true;
//		ai.worldState = Pokemon_AI.WorldStates.Idle;
		foreach(GameObject g in Disable)
			g.SetActive(true);
	}
	[ClientRpc] public void RpcShrink()
	{

	}
	[ClientRpc] public void RpcSetEnemy(NetworkInstanceId _netId)
	{
		enemy = ClientScene.FindLocalObject(_netId);

		if(isLocalPlayer)
			hud.enemyPokemonPortrait.SetTargetPokemon(enemy);

		isInBattle = true;
		components.anim.SetBool("In_Battle", true);
	}
	[ClientRpc] public void RpcEndBattle()
	{
		enemy = null;

		if(isLocalPlayer)
			hud.enemyPokemonPortrait.RemoveTargetPokemon();


		isInBattle = false;
		components.anim.SetBool("In_Battle", false);
	}
	#endregion

	public void CannotMove()
	{
		trainer.components.pokeControl.canMove = false;
	}
	public void CanMove()
	{
		trainer.components.pokeControl.canMove = true;
	}
	public void Attack()
	{
		if(components.hpPP.curPP >= activeMove.ppCost && activeMove.coolDownTimer == 0.0f)
		{
			components.anim.SetBool(activeMove.moveName, true);

			if(activeMove.coolDownTime != 0.0f)
				activeMove.coolDownTimer = activeMove.coolDownTime;

			attacking = true;
		}

	}
	public void DeductPP()
	{
		components.hpPP.AdjCurPP(-activeMove.ppCost);
	}
	public void EndAttack()
	{
		components.anim.SetBool(activeMove.moveName, false);
		attacking = false;
	}
	public void EnterWater()
	{
		
	}

	#region Coroutines
	public IEnumerator Faint()
	{
		trainer.components.pokeControl.canMove = false;
		isInBattle = false;
		components.conditions.CancelInvoke();

		if(!isCaptured)
			GetComponent<Pokemon_AI>().worldState = Pokemon_AI.WorldStates.Dead;

		components.anim.SetBool("Dead", true);

		if(enemy.GetComponent<Pokemon>().isCaptured)
		{
			Pokemon otherPokemon = enemy.GetComponent<Pokemon>();
			bool luckyEgg = false;

			if(otherPokemon.equippedItem == "Lucky Egg")
				luckyEgg = true;

			int increase = Calculations.AddExperience(isCaptured, otherPokemon.isFromTrade, baseEXP, luckyEgg, level, otherPokemon.level, otherPokemon.evolveLevel);

			yield return StartCoroutine(otherPokemon.IncreaseEXP(increase));

			if(!isCaptured)
				enemy.GetComponent<Pokemon>().RpcEndBattle();
		}

		yield return null;
	}
	public IEnumerator IncreaseEXP(int increase)
	{
		int target = currentEXP + increase;
		float end = (float)target;
		float exp = (float)currentEXP;

		while(currentEXP != target)
		{
			exp = Mathf.MoveTowards(exp, end, 1.0f);
			currentEXP = (int)exp;

			if(currentEXP >= nextReqEXP)
			{
				level += 1;
				lastReqEXP = nextReqEXP;
				nextReqEXP = Calculations.CalculateRequiredXP(level, levelRate);
				components.hpPP.LevelUp();
				components.stats.LevelUp();
				RpcSetupMoves();
			}

			yield return null;
		}

//		if(level >= evolveLevel)
//		{
//			//THIS NEEDS TO BE AN RPC INSTEAD
//			StartCoroutine(EvolveStart());
//		}
	}

	private IEnumerator EvolveStart()
	{
//		components.networking.evolving = true;
		components.anim.SetBool("Default", true);

//		while(animation.isPlaying)
//			yield return null;
		
		components.mesh.materials = EvolveMats;

		yield return new WaitForSeconds(1);

		components.anim.SetBool("Default", false);
		components.anim.SetBool("Evolve", true);

//		while(animation.isPlaying)
//			yield return null;

		float increase = flareGrowDelay + Time.deltaTime;

		while(evolveFlare.brightness < evolutionFlareSize)
		{
			evolveFlare.brightness = evolveFlare.brightness + increase;
			yield return null;
		}


		float decrease = flareDieDelay + Time.deltaTime;

		while(evolveFlare.brightness > 0.0f)
		{
			evolveFlare.brightness = evolveFlare.brightness - decrease;
			yield return null;
		}

		while(evolveFlare.brightness < evolutionFlareSize - 1f)
			yield return null;

		if(isServer)
		{
			GameObject evolvedForm = Instantiate(Resources.Load("Prefabs/Pokemon/" + evolvesInto), transform.position, transform.rotation) as GameObject;

			HandOverStats(evolvedForm, components);
			evolvedForm.GetComponent<Pokemon>().SetupExistingPokemon();

			NetworkServer.Spawn(evolvedForm);

			foreach(GameObject g in Disable)
				g.SetActive(false);

			trainer.RpcSetActivePokemon(evolvedForm.GetComponent<NetworkIdentity>().netId);

			if(trainer.inBattle && trainer.opponent.activePokemon)
			{
				trainer.RpcSetPokemonEnemy(trainer.opponent.activePokemon.netId);
				trainer.opponent.RpcSetPokemonEnemy(evolvedForm.GetComponent<NetworkIdentity>().netId);
			}

			//CALL AN RPC HERE ON THE EVOLVED FORM TO FINISH THE EVOLUTION
			NetworkServer.Destroy(gameObject);
		}
	}
	private IEnumerator EvolveEnd()
	{
		components.mesh.enabled = false;

		components.mesh.materials = EvolveMats;

		components.mesh.enabled = true;

		while(evolveFlare.brightness > 0f)
			yield return null;

		components.mesh.materials = OriginalMats;

		yield return new WaitForSeconds(1);

		//ENABLE LOCAL PLAYER COMPONENTS
	}
	#endregion

	private void HandOverStats(GameObject newPokemon, PokemonComponents oldPokemonComponents)
	{
		PokemonComponents components = newPokemon.GetComponent<PokemonComponents>();

		components.pokemon.trainer = oldPokemonComponents.pokemon.trainer;
		components.pokemon.nickName = oldPokemonComponents.pokemon.nickName;
		components.pokemon.isFromTrade = oldPokemonComponents.pokemon.isFromTrade;
		components.pokemon.level = oldPokemonComponents.pokemon.level;
		components.pokemon.nature = (PokemonNature)oldPokemonComponents.pokemon.nature;
		components.hpPP.curHP = oldPokemonComponents.hpPP.curHP;
		components.hpPP.curPP = oldPokemonComponents.hpPP.curPP;
		components.hpPP.hpEV = oldPokemonComponents.hpPP.hpEV;
		components.hpPP.ppEV = oldPokemonComponents.hpPP.ppEV;
		components.stats.atkEV = oldPokemonComponents.stats.atkEV;
		components.stats.defEV = oldPokemonComponents.stats.defEV;
		components.stats.spatkEV = oldPokemonComponents.stats.spatkEV;
		components.stats.spdefEV = oldPokemonComponents.stats.spdefEV;
		components.stats.spdEV = oldPokemonComponents.stats.spdEV;
		components.hpPP.hpIV = oldPokemonComponents.hpPP.hpIV;
		components.hpPP.ppIV = oldPokemonComponents.hpPP.ppIV;
		components.stats.atkIV = oldPokemonComponents.stats.atkIV;
		components.stats.defIV = oldPokemonComponents.stats.defIV;
		components.stats.spatkIV = oldPokemonComponents.stats.spatkIV;
		components.stats.spdefIV = oldPokemonComponents.stats.spdefIV;
		components.stats.spdIV = oldPokemonComponents.stats.spdIV;
		components.pokemon.currentEXP = oldPokemonComponents.pokemon.currentEXP;
		components.pokemon.equippedItem = oldPokemonComponents.pokemon.equippedItem;
		components.pokemon.slot = oldPokemonComponents.pokemon.slot;
	}
}

#region Pokemon Enums
public enum PokemonType
{
	NONE, BUG, DARK, DRAGON, ELECTRIC, FAIRY, FIGHTING, FIRE, FLYING, GHOST, GRASS, GROUND, ICE, NORMAL, POISON, PSYCHIC, ROCK, STEEL, WATER
}
public enum PokemonGender
{
	NONE, FEMALE, MALE
}
public enum PokemonNature
{
	ADAMANT, BASHFUL, BOLD, BRAVE, CALM, CAREFUL, DOCILE, GENTLE, HARDY, HASTY, IMPISH, JOLLY, LAX, LONELY, MILD, MODEST, NAIVE, NAUGHTY,
	QUIET, QUIRKY, RASH, RELAXED, SASSY, SERIOUS, TIMID
}
public enum PokemonLevelRate
{
	ERRATIC, FAST, FLUCTUATING, MEDIUM_FAST, MEDIUM_SLOW, SLOW
}
#endregion