using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Pokemon_AI : NetworkBehaviour
{
	#region Wild Pokemon Variables
	public float maxMoveRange = 10.0f, idleTime = 15.0f, deadTime = 10.0f, battleTime = 10.0f;
	public GameObject target;
	public bool usingMove;
	public List<Move> CloseRangeMoves, MediumRangeMoves, LongRangeMoves;
	
	private bool movingToDestination, inBattleState, canAttackTarget, following;
	private float idleTimer, deadTimer, battleTimer, speed;
	private Pokemon pokemon;
	private PokemonHPPP hpPP;
	private Vector3 destination;
	private Animator anim;
	private NavMeshAgent navAgent;
	
	public WorldStates worldState;
	public enum WorldStates{ Idle, Move, Battle, Captured, Dead, Reset }
	public BattleStates battleState;
	public enum BattleStates{ Follow, Attack, Run }
	#endregion

	#region Captured Pokemon Variables
	public float minDistance = 1.0f, maxDistance = 3.0f;
	
	private float distance;
	private Transform trainer;
	#endregion
	
	void Awake()
	{
		pokemon = GetComponent<Pokemon>();
		hpPP = GetComponent<PokemonHPPP>();
		anim = GetComponent<Animator>();
		navAgent = GetComponent<NavMeshAgent>();
		worldState = WorldStates.Idle;
	}
	void OnEnable()
	{
		navAgent.enabled = true;

		if(pokemon.isCaptured)
		{
			trainer = pokemon.trainer.transform;
			navAgent.Resume();
		}
		else
			StartCoroutine(AI());
	}
	void OnDisable()
	{
		navAgent.Stop();
		navAgent.enabled = false;
	}
	void Update()
	{
		if(!pokemon.isCaptured)
			return;

		if(!trainer)
			trainer = pokemon.trainer.transform;

		speed = navAgent.desiredVelocity.normalized.magnitude;
		
		anim.SetFloat("Speed", speed);
		
		distance = Vector3.Distance(transform.position, trainer.position);
		
		if(distance > maxDistance)
			Follow();
		if(distance < minDistance)
			Idle();
	}
	private void Idle()
	{
		navAgent.Stop();
	}
	private void Follow()
	{
		navAgent.destination = trainer.position;
		navAgent.Resume();
	}

	private IEnumerator AI()
	{
		while(true)
		{
			switch(worldState)
			{
			case WorldStates.Idle:
				IdleState();
				break;
			case WorldStates.Move:
				MoveState();
				break;
			case WorldStates.Battle:
				BattleState();
				break;
			case WorldStates.Captured:
				CapturedState();
				break;
			case WorldStates.Dead:
				DeadState();
				break;
			case WorldStates.Reset:
				ResetState();
				break;
			}
			yield return null;
		}
	}
	private IEnumerator Battle()
	{
		while(true)
		{
			switch(battleState)
			{
				case BattleStates.Follow:
					FollowState();
					break;
				case BattleStates.Attack:
//					AttackState();
					break;
				case BattleStates.Run:
					RunState();
					break;
			}
			yield return null;
		}
	}
	
	#region States
	private void ResetState()
	{
		target = null;
		movingToDestination = false;
		usingMove = false;
		canAttackTarget = false;
		idleTimer = 0.0f;
		battleTimer = 0.0f;
		deadTimer = 0.0f;
		speed = 0.0f;
		anim.SetFloat("Speed", speed);
		pokemon.SetupPokemonFirstTime();
		anim.SetBool("Dead", false);
//		pokemon.RpcEnable();
	}
	private void IdleState()
	{
		speed = navAgent.desiredVelocity.normalized.magnitude;
		
		anim.SetFloat("Speed", speed);
		
		idleTimer += Time.deltaTime;
		
		if(idleTimer >= idleTime)
			worldState = WorldStates.Move;
	}
	private void MoveState()
	{
		if(!movingToDestination)
		{
			movingToDestination = true;
			destination = Random.insideUnitSphere * maxMoveRange;
			destination += transform.position;
			Vector3 goTo = FindNavMeshPosition(destination);

			StartCoroutine(MoveToDestination(goTo, 0.01f));
		}
		
		speed = navAgent.desiredVelocity.normalized.magnitude;
		
		anim.SetFloat("Speed", speed);
	}
	private void BattleState()
	{
		if(movingToDestination)
		{
			movingToDestination = false;
			StopCoroutine(MoveToDestination(Vector3.zero, 0.0f));
		}

//		if(!inBattleState)
//		{
//			inBattleState = true;
//			StartCoroutine(Battle());
//		}
	}
	private void FollowState()
	{
		Vector3 targetPosition = target.GetComponent<CapsuleCollider>().ClosestPointOnBounds(transform.position);
		targetPosition.y = target.transform.position.y;
		
		Vector3 myPosition = GetComponent<CapsuleCollider>().ClosestPointOnBounds(target.transform.position);
		myPosition.y = transform.position.y;
		
		float distance = Vector3.Distance(myPosition, targetPosition);

		if(!following && distance >= 1.0f)
		{
			following = true;
			Vector3 destination = FindNavMeshPosition(targetPosition);
			StartCoroutine(Chase(destination, 0.9f));
		}
		else if(!following && distance < 1.0f && distance >= 0.5f)
		{
			following = true;
			Vector3 destination = FindNavMeshPosition(targetPosition);
			StartCoroutine(Chase(destination, 0.4f));
		}
		else if(!following)
			battleState = BattleStates.Attack;
	}
//	private void AttackState()
//	{
//		if(usingMove)
//			return;
//
//		Vector3 targetPosition = target.GetComponent<CapsuleCollider>().ClosestPointOnBounds(transform.position);
//		targetPosition.y = target.transform.position.y;
//		
//		Vector3 myPosition = GetComponent<CapsuleCollider>().ClosestPointOnBounds(target.transform.position);
//		myPosition.y = transform.position.y;
//		
//		float distance = Vector3.Distance(myPosition, targetPosition);
//
//		if(distance > 1.0f)
//		{
//			for(int m = LongRangeMoves.Count - 1; m >= 0; m--)
//			{
//				int result = pokemon.KnownMoves.FindIndex(move => move.moveName == LongRangeMoves[m].moveName);
//				
//				pokemon.activeMoveIndex = result;
//				
//				if(hpPP.curPP >= pokemon.activeMove.ppCost && pokemon.activeMove.aiTimer == 0.0f)
//				{
//					pokemon.activeMoveIndex = m;
//					anim.SetBool(pokemon.activeMove.moveName, true);
//					usingMove = true;
//					pokemon.activeMove.aiTimer = pokemon.activeMove.aiTime;
//					return;
//				}
//			}
//		}
//
//		if(distance < 1.0f && distance > 0.5f)
//		{
//			for(int m = MediumRangeMoves.Count - 1; m >= 0; m--)
//			{
//				int result = pokemon.KnownMoves.FindIndex(move => move.moveName == MediumRangeMoves[m].moveName);
//				
//				pokemon.activeMoveIndex = result;
//				
//				if(hpPP.curPP >= pokemon.activeMove.ppCost && pokemon.activeMove.aiTimer == 0.0f)
//				{
//					Debug.Log (pokemon.activeMove.aiTimer);
//					pokemon.activeMoveIndex = m;
//					anim.SetBool(pokemon.activeMove.moveName, true);
//					usingMove = true;
//					pokemon.activeMove.aiTimer = pokemon.activeMove.aiTime;
//					return;
//				}
//			}
//		}
//
//		if(distance <  0.5f)
//		{
//			for(int m = CloseRangeMoves.Count - 1; m >= 0; m--)
//			{
//				int result = pokemon.KnownMoves.FindIndex(move => move.moveName == CloseRangeMoves[m].moveName);
//
//				pokemon.activeMoveIndex = result;
//
//				if(hpPP.curPP >= pokemon.activeMove.ppCost && pokemon.activeMove.aiTimer == 0.0f)
//				{
//					pokemon.activeMoveIndex = m;
//					anim.SetBool(pokemon.activeMove.moveName, true);
//					usingMove = true;
//					pokemon.activeMove.aiTimer = pokemon.activeMove.aiTime;
//					return;
//				}
//			}
//		}
//
//		battleState = BattleStates.Follow;
//	}
	private void RunState()
	{

	}
//	private void BattleState()
//	{
//		if(!usingMove)
//		{
//			if(pokemon.isInBattle)
//			{
//				if(target == null || target.Equals(null))
//				{
//					battleTimer += Time.deltaTime;
//					if(battleTimer >= battleTime)
//					{
//						pokemon.EndBattle();
//						battleTimer = 0.0f;
//					}
//					return;
//				}
//				if(CheckTarget(target))
//				{
//					canAttackTarget = false;
//					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.transform.position - transform.position),
//					                                      10f * Time.smoothDeltaTime);
//					transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
//					
//					Vector3 targetPosition = target.GetComponent<CapsuleCollider>().ClosestPointOnBounds(transform.position);
//					targetPosition.y = target.transform.position.y;
//					
//					Vector3 myPosition = GetComponent<CapsuleCollider>().ClosestPointOnBounds(target.transform.position);
//					myPosition.y = transform.position.y;
//					
//					float distance = Vector3.Distance(myPosition, targetPosition);
//					
//					for(int m = pokemon.KnownMoves.Count - 1; m >= 0; m--)
//					{
//					int m = Random.Range(0, pokemon.KnownMoves.Count - 1);
//						pokemon.activeMoveIndex = m;
//						if(hpPP.curPP >= pokemon.activeMove.ppCost && pokemon.activeMove.coolDownTimer == 0.0f)
//						{
//							if(distance > 1.0f && pokemon.activeMove.isRanged)
//							{
//								navAgent.Stop();
//								canAttackTarget = true;
//								pokemon.aim = targetPosition;
//								anim.SetBool(pokemon.activeMove.moveName, true);
//								usingMove = true;
//								pokemon.activeMove.coolDownTimer = pokemon.activeMove.coolDownTime;
//								//break;
//							return;
//							}
//							if(!pokemon.activeMove.isRanged && distance < pokemon.activeMove.range)
//							{
//								navAgent.Stop();
//								canAttackTarget = true;
//								pokemon.aim = targetPosition;
//								anim.SetBool(pokemon.activeMove.moveName, true);
//								usingMove = true;
//								pokemon.activeMove.coolDownTimer = pokemon.activeMove.coolDownTime;
//								//break;
//							return;
//							}
//						}
//					else
//						return;
//					}
//					
//					if(!canAttackTarget)
//					{
//						if(distance > 1.0f)
//						{
//							navAgent.destination = target.transform.position;
//							navAgent.Resume();
//						}
//					}
//				}
//				else
//					worldState = WorldStates.Idle;
//			}
//		}
//		
//		speed = navAgent.desiredVelocity.normalized.magnitude * 2.0f;
//		
//		anim.SetFloat("Speed", speed);
//	}
	private void CapturedState()
	{
		if(movingToDestination)
		{
			movingToDestination = false;
			StopCoroutine(MoveToDestination(Vector3.zero, 0.0f));
		}
	}
	private void DeadState()
	{
		deadTimer += Time.deltaTime;
	
		if(deadTimer >= deadTime)
			worldState = WorldStates.Reset;
	}
	#endregion

	public void StopMove()
	{
		Debug.Log ("Stop move called on " + pokemon.activeMove.moveName);
		if(!pokemon.isCaptured)
			pokemon.EndAttack();
	}

	private bool CheckTarget(GameObject _targetPokemon)
	{
		if(target.GetComponent<PokemonHPPP>().curHP == 0)
		{
			target = null;
			pokemon.enemy = null;
			return false;
		}
		else
			return true;
	}
	private Vector3 FindNavMeshPosition(Vector3 destination)
	{
		NavMeshHit hit;
		NavMesh.SamplePosition(destination, out hit, maxMoveRange, NavMesh.AllAreas);
		
		return hit.position;
	}

	private IEnumerator Chase(Vector3 position, float stopDistance)
	{
		navAgent.SetDestination(position);
		
		while(Vector3.Distance(transform.position, position) > stopDistance)
		{
			yield return null;
		}

		following = false;
		battleState = BattleStates.Attack;
	}
	private IEnumerator MoveToDestination(Vector3 position, float stopDistance)
	{
		navAgent.SetDestination(position);
		
		while(Vector3.Distance(transform.position, position) > stopDistance)
		{
			yield return null;
		}
		
		idleTimer = 0.0f;
		movingToDestination = false;
		worldState = WorldStates.Idle;
	}
}
