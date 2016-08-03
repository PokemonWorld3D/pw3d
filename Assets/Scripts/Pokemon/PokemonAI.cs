using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;

public class PokemonAI : NetworkBehaviour
{
	public bool attacking;

	[SerializeField] private float maxMoveRange = 1.0f, idleDuration = 1.0f, respawnDuration = 1.0f;

	private PokemonStates state;
	private Dictionary<PokemonStates, Action> FSM = new Dictionary<PokemonStates, Action>();
	private NavMeshAgent navAgent;
	private bool wandering;
	private Vector3 destination;
	private float idleTimer, respawnTimer, speed;
	private PokemonComponents components;

	void OnEnable()
	{
		navAgent.enabled = true;
	}
	void OnDisable()
	{
		navAgent.enabled = false;
	}
	void Awake()
	{
		navAgent = GetComponent<NavMeshAgent>();
		components = GetComponent<PokemonComponents>();
	}
	void Start()
	{
		FSM.Add(PokemonStates.Idle, new Action(IdleState));
		FSM.Add(PokemonStates.Wander, new Action(WanderState));
		FSM.Add(PokemonStates.Captured, new Action(CapturedState));
		FSM.Add(PokemonStates.Respawning, new Action(RespawnState));
		SetState(PokemonStates.Idle);
	}
	void Update()
	{
		if(!isServer)
			return;
		
		FSM[state].Invoke();
	}

	public void SetState(PokemonStates state)
	{
		this.state = state;
	}

	private void IdleState()
	{
		idleTimer -= Time.deltaTime;

		if(idleTimer <= 0.0f)
		{
			idleTimer = idleDuration;
			SetState(PokemonStates.Wander);
		}
	}
	private void WanderState()
	{
		if(!wandering)
		{
			wandering = true;
			destination = FindNavMeshPosition((UnityEngine.Random.insideUnitSphere * maxMoveRange) + transform.position);
			navAgent.SetDestination(destination);
			navAgent.Resume();
		}
		if(wandering)
		{
			if(navAgent.remainingDistance <= float.Epsilon)
			{
				wandering = false;
				navAgent.Stop();
				SetState(PokemonStates.Idle);
			}
		}

		speed = navAgent.desiredVelocity.normalized.magnitude;
//		anim.SetFloat("Speed", speed);
	}
	private void CapturedState()
	{
		if(navAgent.hasPath || navAgent.pathPending)
			navAgent.Stop();
		
		if(speed != 0.0f)
		{
			speed = 0.0f;
//			anim.SetFloat("Speed", speed);
		}
	}
	private void RespawnState()
	{
		respawnTimer += Time.deltaTime;

		if(respawnTimer >= respawnDuration)
		{
			respawnTimer = 0.0f;
			components.pokemon.SetupPokemonFirstTime();
			components.pokemon.RpcEnable();
			SetState(PokemonStates.Idle);
		}
	}

	private Vector3 FindNavMeshPosition(Vector3 destination)
	{
		NavMeshHit hit;
		NavMesh.SamplePosition(destination, out hit, maxMoveRange, NavMesh.AllAreas);

		return hit.position;
	}
}
public enum PokemonStates { Idle, Wander, Captured, Respawning }