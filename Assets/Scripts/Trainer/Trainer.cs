using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Trainer : NetworkBehaviour
{
	[HideInInspector][SyncVar(hook="SyncUsername")] public string username;
	[HideInInspector][SyncVar(hook="SyncCharacterName")] public string characterName;
	[HideInInspector][SyncVar(hook="SyncGender")] public int genderInt;
	[HideInInspector][SyncVar(hook="SyncFunds")] public int funds;
	[HideInInspector][SyncVar(hook="SyncLastZone")] public string lastZone;
	[HideInInspector][SyncVar(hook="SyncLastPosition")] public Vector3 lastPosition;
	[HideInInspector][SyncVar(hook="SyncPokemonSlot")] public int pokemonSlot;
	[HideInInspector][SyncVar(hook="SyncCanBattle")] public bool canBattle = true;
	[HideInInspector][SyncVar(hook="SyncInBattle")] public bool inBattle = false;

	[HideInInspector] public SyncListString Inventory = new SyncListString(), MainChatHistory = new SyncListString();
	[HideInInspector] public SyncListPokeData DownloadedData = new SyncListPokeData(), PokemonRoster = new SyncListPokeData(), PokemonInventory = new SyncListPokeData();

	[HideInInspector] public HUD hud;

	public Pokemon activePokemon { get; private set; }
	public Trainer opponent { get; private set; }
	public TrainerComponents components { get; private set; }

	[SerializeField] private Transform grip;
	[SerializeField] private GameObject emptyPokeBallPrefab, pokemonPokeBallPrefab, returnPokeBall;
	[SerializeField] private AudioClip pokeBallGrow, pokemonReturn;

	private Gender gender;
	private bool hasPokemon = false, throwing = false, returning = false;

	private GameObject emptyPokeBall, pokemonPokeBall;
	private Vector3 target;

	public override void OnStartClient()
	{
		PokemonRoster.Callback = OnPokeRosterAdd;
		MainChatHistory.Callback = OnMainChatChange;
	}
	public override void OnStartLocalPlayer()
	{
		GameObject.Find("HUD").SendMessage("SetTrainer", this);
	}

	void Awake()
	{
		components = GetComponent<TrainerComponents>();
	}

	//FOR TESTING ONLY
//	void Start()
//	{
//		if(isServer)
//		{
//			characterName = "Red";
//			PokemonRoster.Add(new Poke_Data("Charmander", "", false, 100, 3, 204, 204, 137, 107, 141, 121, 135, 204, 204, 137, 107, 141, 121, 135, 0, 0, 0, 0, 0, 0, 0, 16, 16, 16, 16, 16, 16,
//				16, 99999, "", 0));
//			PokemonRoster.Add(new Poke_Data("Caterpie", "", false, 100, 3, 216, 216, 89, 91, 61, 61, 99, 216, 216, 89, 91, 61, 61, 99, 0, 0, 0, 0, 0, 0, 0, 16, 16, 16, 16, 16, 16,
//			16, 99999, "", 1));
//			PokemonRoster.Add(new Poke_Data("Squirtle", "", false, 100, 3, 214, 214, 128, 151, 121, 149, 96, 214, 214, 128, 151, 121, 149, 96, 0, 0, 0, 0, 0, 0, 0, 16, 16, 16, 16, 16, 16,
//				16, 99999, "", 2));
//		}
//	}

	#region Commands
	[Command] public void CmdNextSlot()
	{
		pokemonSlot = pokemonSlot == PokemonRoster.Count - 1 ? 0 : pokemonSlot + 1;
	}
	[Command] public void CmdLastSlot()
	{
		pokemonSlot = pokemonSlot > 0 ? pokemonSlot - 1 : PokemonRoster.Count - 1;
	}
	[Command] public void CmdPokemonGo()
	{
		if(throwing || returning || hasPokemon)
			return;

		hasPokemon = true;
		throwing = true;
		components.netMovement.throwing = true;
		components.anim.SetBool("Throw Pokemon Poke Ball", true);
	}
	[Command] public void CmdPokemonCapture()
	{
		if(throwing || returning)
			return;
		
		throwing = true;
		components.netMovement.throwing = true;
		components.anim.SetBool("Throw Empty Poke Ball", true);
	}
	[Command] public void CmdSwap()
	{
		components.ai.enabled = !components.ai.enabled;
		components.netMovement.inControl = !components.netMovement.inControl;
		components.pokeControl.inControl = !components.pokeControl.inControl;
	}
	[Command] public void CmdRecallPokemon()
	{
		if(!activePokemon)
			return;
		
		components.netMovement.returning = true;
		transform.LookAt(activePokemon.gameObject.transform);
		components.anim.SetBool("Start Pokemon Return", true);
	}
	[Command] public void CmdSetPokeBallTarget(Vector3 target)
	{
		this.target = target;
	}
	[Command] public void CmdRequestTrainerBattle(NetworkInstanceId otherTrainerNetId)
	{
		NetworkServer.FindLocalObject(otherTrainerNetId).GetComponent<Trainer>().RpcIncomingBattleRequest(characterName, netId);
	}
	[Command] public void CmdAcceptBattleRequest(NetworkInstanceId otherTrainerNetId)
	{
		NetworkServer.FindLocalObject(otherTrainerNetId).GetComponent<Trainer>().RpcRequestAcceted(characterName);
		NetworkServer.FindLocalObject(otherTrainerNetId).GetComponent<Trainer>().RpcInitTrainerBattle(netId);
		RpcInitTrainerBattle(otherTrainerNetId);
	}
	[Command] public void CmdDeclineBattleRequest(NetworkInstanceId otherTrainerNetId)
	{
		NetworkServer.FindLocalObject(otherTrainerNetId).GetComponent<Trainer>().RpcRequestDenied(characterName);
	}

	[Command] public void CmdAttack(bool value)
	{
//		THE FOLLOWING IS USED FOR ATTACK TESTING & DEBUGGING ONLY
		if(value)
		{
			activePokemon.Attack();
//			components.pokeControl.canMove = false;
		}
		else if(!value)
		{
			activePokemon.EndAttack();
//			components.pokeControl.canMove = true;
		}
//		if(value && activePokemon.isInBattle)
//		{
//			activePokemon.Attack();
//			components.pokeControl.canMove = false;
//		}
//		else if(!value)
//		{
//			activePokemon.EndAttack();
//			components.pokeControl.canMove = true;
//		}
	}
	[Command] public void CmdNextMove()
	{
		activePokemon.activeMoveIndex = activePokemon.activeMoveIndex == activePokemon.KnownMoves.Count - 1 ? 0 : activePokemon.activeMoveIndex + 1;
		activePokemon.activeMove = activePokemon.KnownMoves[activePokemon.activeMoveIndex];
	}
	[Command] public void CmdLastMove()
	{
		activePokemon.activeMoveIndex = activePokemon.activeMoveIndex > 0 ? activePokemon.activeMoveIndex - 1 : activePokemon.KnownMoves.Count - 1;
		activePokemon.activeMove = activePokemon.KnownMoves[activePokemon.activeMoveIndex];
	}
	[Command] public void CmdHeadRotation(Quaternion rotation)
	{
		activePokemon.components.headLook.headRotation = rotation;
	}
	[Command] public void CmdBodyRotation(Quaternion rotation)
	{
		activePokemon.components.headLook.bodyRotation = rotation;
	}
	[Command] public void CmdTarget(Vector3 target)
	{
		activePokemon.components.headLook.target = target;
	}
	[Command] public void CmdInitWildPokemonBattle(NetworkInstanceId _netId)
	{
		GameObject wildPokemon = NetworkServer.FindLocalObject(_netId);

		activePokemon.enemy = NetworkServer.FindLocalObject(_netId);
		activePokemon.isInBattle = true;
		activePokemon.RpcSetEnemy(_netId);

		wildPokemon.GetComponent<Pokemon_AI>().target = activePokemon.gameObject;
		wildPokemon.GetComponent<Pokemon>().isInBattle = true;
		wildPokemon.GetComponent<Pokemon>().enemy = activePokemon.gameObject;
		wildPokemon.GetComponent<Pokemon_AI>().worldState = Pokemon_AI.WorldStates.Battle;
	}
	#endregion

	#region ClientRpc
	[ClientRpc] public void RpcSetActivePokemon(NetworkInstanceId _pokemonNetId)
	{
		GameObject pokemon = ClientScene.FindLocalObject(_pokemonNetId);

		activePokemon = pokemon.GetComponent<Pokemon>();
		activePokemon.trainer = this;
		activePokemon.hud = hud;

		components.pokeControl.pokemon = pokemon;
		components.pokeControl.components = pokemon.GetComponent<PokemonComponents>();
		components.pokeControl.Init();
		components.pokeControl.enabled = true;

		if(isServer && inBattle)
			activePokemon.components.anim.SetBool("In Battle", true);
		
		if(isLocalPlayer)
			hud.playerPokemonPortrait.SetActivePokemon(pokemon);
	}
	[ClientRpc] public void RpcRemoveActivePokemon()
	{
		activePokemon = null;
		hasPokemon = false;

		if(isLocalPlayer)
			hud.playerPokemonPortrait.RemoveActivePokemon();
	}
	[ClientRpc] public void RpcIncomingBattleRequest(string trainerName, NetworkInstanceId otherTrainerNetId)
	{
		if(!isLocalPlayer)
			return;

		hud.battleRequestScript.IncomingBattleRequest(trainerName, otherTrainerNetId);
	}
	[ClientRpc] public void RpcRequestAcceted(string trainersName)
	{
		if(!isLocalPlayer)
			return;

		hud.battleRequestScript.RequestAccepted(trainersName);
	}
	[ClientRpc] public void RpcRequestDenied(string trainersName)
	{
		if(!isLocalPlayer)
			return;

		hud.battleRequestScript.RequestDenied(trainersName);
	}
	[ClientRpc] public void RpcInitTrainerBattle(NetworkInstanceId opponent)
	{
		Trainer otherTrainer = ClientScene.FindLocalObject(opponent).GetComponent<Trainer>();

		inBattle = true;
		this.opponent = otherTrainer;

		if(activePokemon)
		{
			activePokemon.isInBattle = true;

			if(isServer)
				activePokemon.components.anim.SetBool("In Battle", true);

			if(otherTrainer.activePokemon)
			{
				activePokemon.enemy = otherTrainer.activePokemon.gameObject;

				if(isLocalPlayer)
					hud.enemyPokemonPortrait.SetTargetPokemon(otherTrainer.activePokemon.gameObject);
			}
		}
	}
	[ClientRpc] public void RpcSetPokemonEnemy(NetworkInstanceId pokemonNetId)
	{
		GameObject otherPokemon = ClientScene.FindLocalObject(pokemonNetId);

		activePokemon.isInBattle = true;

		if(isServer)
			activePokemon.components.anim.SetBool("In Battle", true);
		
		activePokemon.enemy = otherPokemon;

		if(isLocalPlayer)
			hud.enemyPokemonPortrait.SetTargetPokemon(otherPokemon);
	}
	[ClientRpc] public void RpcEndTrainerBattle(bool victor)
	{
		opponent = null;
		inBattle = false;

		if(activePokemon)
			activePokemon.enemy = null;

		if(isServer)
		{
			components.anim.SetBool("In Battle", false);

			if(activePokemon)
				activePokemon.components.anim.SetBool("In Battle", false);
		}
	}
	#endregion

	#region Hooks
	private void SyncUsername(string username)
	{
		this.username = username;
	}
	private void SyncCharacterName(string characterName)
	{
		this.characterName = characterName;
	}
	private void SyncGender(int genderInt)
	{
		this.genderInt = genderInt;
		this.gender = (Gender)genderInt;
	}
	private void SyncFunds(int funds)
	{
		this.funds = funds;
	}
	private void SyncLastZone(string lastZone)
	{
		this.lastZone = lastZone;
	}
	private void SyncLastPosition(Vector3 lastPosition)
	{
		this.lastPosition = lastPosition;
	}
	private void SyncPokemonSlot(int pokemonSlot)
	{
		this.pokemonSlot = pokemonSlot;

		if(isLocalPlayer)
			hud.pokemonRosterPanel.UpdateSelectedPokemon(this.pokemonSlot);
	}
	private void SyncCanBattle(bool canBattle)
	{
		this.canBattle = canBattle;
	}
	private void SyncInBattle(bool inBattle)
	{
		this.inBattle = inBattle;

		if(isServer)
			components.anim.SetBool("In Battle", inBattle);
	}
	#endregion

	#region Pokemon Balls
	public void CreateEmptyBall()
	{
		components.audioS.PlayOneShot(pokeBallGrow);
		
		if(!isServer)
			return;
		
		emptyPokeBall = Instantiate(emptyPokeBallPrefab, grip.position, grip.rotation) as GameObject;
		emptyPokeBall.transform.SetParent(grip);
		emptyPokeBall.GetComponent<Empty_Poke_Ball>().trainer = this;
		emptyPokeBall.GetComponent<Empty_Poke_Ball>().thisPokeBallType = Poke_Ball_Types.MASTERBALL;
		
		NetworkServer.Spawn(emptyPokeBall);
	}
	public void ThrowEmptyBall()
	{
		if(isServer)
		{
			Vector3 throwSpeed = Physics_Calculations.CalculateBestThrowSpeed(emptyPokeBall.transform.position, target, 1.0f);
			
			emptyPokeBall.transform.parent = null;
			emptyPokeBall.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);
			emptyPokeBall.GetComponent<Rigidbody>().useGravity = true;
			components.anim.SetBool("Throw Empty Poke Ball", false);
		}

		throwing = false;
		components.netMovement.throwing = false;
	}
	public void CreatePokemonBall()
	{
		components.audioS.PlayOneShot(pokeBallGrow);
		
		if(!isServer)
			return;
		
		pokemonPokeBall = Instantiate(pokemonPokeBallPrefab, grip.position, grip.rotation) as GameObject;
		pokemonPokeBall.transform.SetParent(grip);
		pokemonPokeBall.GetComponent<Pokemon_Poke_Ball>().trainer = this;
		pokemonPokeBall.GetComponent<Pokemon_Poke_Ball>().data = this.PokemonRoster[pokemonSlot];
		
		NetworkServer.Spawn(pokemonPokeBall);
	}
	public void ThrowPokemonBall()
	{
		if(isServer)
		{
			Vector3 targetPos = components.mesh.transform.position + (components.mesh.transform.forward * 5.0f);
			Vector3 throwSpeed = Physics_Calculations.CalculateBestThrowSpeed(pokemonPokeBall.transform.position, targetPos, 1.0f);
			
			pokemonPokeBall.transform.parent = null;
			pokemonPokeBall.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);
			pokemonPokeBall.GetComponent<Rigidbody>().useGravity = true;

			throwing = false;
			components.netMovement.throwing = false;
			components.anim.SetBool("Throw Pokemon Poke Ball", false);
		}
	}
	#endregion

	public void SwapToPokemon()
	{
		if(activePokemon == null || activePokemon.GetComponent<PokemonHPPP>().curHP == 0)
			return;

		components.netMovement.Control(false);
		components.pokeControl.Control(true);
	}
	public void SwapToTrainer()
	{
		components.pokeControl.Control(false);
		components.netMovement.Control(true);
	}
	public void ActivateReturnBall()
	{
		returnPokeBall.SetActive(true);

		if(isServer)
			components.anim.SetBool("Start Pokemon Return", false);

		StartCoroutine(PokemonReturn());
	}
	public void DeactivateReturnBall()
	{
		returnPokeBall.SetActive(false);

		if(isServer)
			components.anim.SetBool("Finish Pokemon Return", false);
	}

	private void OnPokeRosterAdd(SyncListPokeData.Operation op, int index)
	{
		if(isLocalPlayer && op == SyncList<Poke_Data>.Operation.OP_ADD)
			hud.pokemonRosterPanel.Setup();
	}
	public void OnMainChatChange(SyncListString.Operation op, int index)
	{
		if(isLocalPlayer && op == SyncListString.Operation.OP_ADD)
			hud.AddToMainChat(MainChatHistory[index]);
	}
	private void GetStatsBack(GameObject pokemon)
	{
		Pokemon temp = pokemon.GetComponent<Pokemon>();
		PokemonHPPP hpPP = pokemon.GetComponent<PokemonHPPP>();
		PokemonStats stats = pokemon.GetComponent<PokemonStats>();

		Poke_Data data = new Poke_Data(temp.pokemonName, temp.nickName, temp.isFromTrade, temp.level, (int)temp.nature, hpPP.curMaxHP, hpPP.curMaxPP, stats.curMaxATK,
		                               stats.curMaxDEF, stats.curMaxSPATK, stats.curMaxSPDEF, stats.curMaxSPD, hpPP.curHP, hpPP.curPP, stats.curATK, stats.curDEF,
		                               stats.curSPATK, stats.curSPDEF, stats.curSPD, hpPP.hpEV, hpPP.ppEV, stats.atkEV, stats.defEV, stats.spatkEV, stats.spdefEV,
		                               stats.spdEV, hpPP.hpIV, hpPP.ppIV, stats.atkIV, stats.defIV, stats.spatkIV, stats.spdefIV, stats.spdIV, temp.currentEXP,
		                               temp.equippedItem, temp.slot);

		PokemonRoster[data.slot] = data;
	}

	private IEnumerator PokemonReturn()
	{
		if(isServer)
			GetStatsBack(activePokemon.gameObject);
			
		components.audioS.PlayOneShot(pokemonReturn);

		yield return new WaitForSeconds(1.0f);
		
		returnPokeBall.GetComponent<Pokemon_Return>().target = activePokemon.gameObject;
		returnPokeBall.GetComponent<Pokemon_Return>().lineRenderer.enabled = true;

		if(isServer)
			activePokemon.GetComponent<Pokemon>().RpcBeingCaptured();

		yield return new WaitForSeconds(1.0f);
		
	//		Vector3 scale = Vector3.zero;
	//
	//		while(Vector3.Distance(trainer.activePokemon.gameObject.transform.localScale, scale) > 0.1f)
	//		{
	//			trainer.activePokemon.gameObject.transform.localScale = Vector3.Lerp(trainer.activePokemon.gameObject.transform.localScale, scale, Time.deltaTime * 5.0f);
	//			yield return null;
	//		}

		if(isServer)
			NetworkServer.Destroy(activePokemon.gameObject);

		returnPokeBall.GetComponent<Pokemon_Return>().target = null;

		yield return new WaitForSeconds(1.5f);

		returnPokeBall.GetComponent<Pokemon_Return>().lineRenderer.enabled = false;

		if(isServer)
			RpcRemoveActivePokemon();

		components.anim.SetBool("Finish Pokemon Return", true);

		components.netMovement.returning = false;

		if(isServer && inBattle && opponent)
		{
			bool allDead = true;

			for(int i = 0; i < PokemonRoster.Count; i++)
			{
				if(PokemonRoster[i].curHP > 0)
					allDead = false;
			}

			if(allDead)
			{
				canBattle = false;
				RpcEndTrainerBattle(false);
				opponent.RpcEndTrainerBattle(true);
			}
		}

		yield return null;
	}
}

public enum Gender { MALE, FEMALE }

[System.Serializable] public struct Poke_Data
{
	public string pokemonName, nickName, equippedItem;
	public int nature, level, curMaxHP, curMaxPP, curMaxATK, curMaxDEF, curMaxSPATK, curMaxSPDEF, curMaxSPD, curHP, curPP, curATK, curDEF, curSPATK,
	curSPDEF, curSPD, hpEV, ppEV, atkEV, defEV, spatkEV, spdefEV, spdEV, hpIV, ppIV, atkIV, defIV, spatkIV, spdefIV, spdIV, currentEXP, slot;
	public bool isFromTrade;
	
	public Poke_Data(string _pokemonName, string _nickName, bool _isFromTrade, int _level, int _nature, int _curMaxHP, int _curMaxPP, int _curMaxATK, int _curMaxDEF,
	                    int _curMaxSPATK, int _curMaxSPDEF, int _curMaxSPD, int _curHP, int _curPP, int _curATK, int _curDEF, int _curSPATK, int _curSPDEF, int _curSPD, int _hpEV,
	                    int _ppEV, int _atkEV, int _defEV, int _spatkEV, int _spdefEV, int _spdEV, int _hpIV, int _ppIV, int _atkIV, int _defIV, int _spatkIV, int _spdefIV, int _spdIV,
	                    int _currentEXP, string _equippedItem, int _slot)
	{
		pokemonName = _pokemonName;
		nickName = _nickName;
		isFromTrade = _isFromTrade;
		level = _level;
		nature = _nature;
		curMaxHP = _curMaxHP;
		curMaxPP = _curMaxPP;
		curMaxATK = _curMaxATK;
		curMaxDEF = _curMaxDEF;
		curMaxSPATK = _curMaxSPATK;
		curMaxSPDEF = _curMaxSPDEF;
		curMaxSPD = _curMaxSPD;
		curHP = _curHP;
		curPP = _curPP;
		curATK = _curATK;
		curDEF = _curDEF;
		curSPATK = _curSPATK;
		curSPDEF = _curSPDEF;
		curSPD = _curSPD;
		hpEV = _hpEV;
		ppEV = _ppEV;
		atkEV = _atkEV;
		defEV = _defEV;
		spatkEV = _spatkEV;
		spdefEV = _spdefEV;
		spdEV = _spdEV;
		hpIV = _hpIV;
		ppIV = _ppIV;
		atkIV = _atkIV;
		defIV = _defIV;
		spatkIV = _spatkIV;
		spdefIV = _spdefIV;
		spdIV = _spdIV;
		currentEXP = _currentEXP;
		equippedItem = _equippedItem;
		slot = _slot;
	}
}

public class SyncListPokeData : SyncListStruct<Poke_Data>
{

}