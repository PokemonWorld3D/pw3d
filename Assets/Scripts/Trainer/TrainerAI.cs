using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TrainerAI : MonoBehaviour
{
	public Vector3 moveDirection;

	[SerializeField] private float minDistance = 3.0f, maxDistance = 5.0f;

	private enum AIStates { IDLE, FOLLOW }
	private AIStates state;
	private Dictionary<AIStates, Action> FSM = new Dictionary<AIStates, Action>();
	private float distance, speed;
	private TrainerComponents components;
	private Transform pokemon;

	void OnEnable()
	{
		pokemon = components.trainer.activePokemon.transform;
	}
	void Awake()
	{
		components = GetComponent<TrainerComponents>();
	}
	void Start()
	{
		FSM.Add(AIStates.IDLE, IdleState);
		FSM.Add(AIStates.FOLLOW, FollowState);
	}
	void Update()
	{
		FSM[state].Invoke();
	}

	private void SetState(AIStates state)
	{
		this.state = state;
	}
	private void DistanceCheck()
	{
		distance = Vector3.Distance(transform.position, pokemon.position);
	}
	private void IdleState()
	{
		DistanceCheck();

		if(distance > maxDistance)
			SetState(AIStates.FOLLOW);
	}
	private void FollowState()
	{
		moveDirection = pokemon.position - transform.position;

		DistanceCheck();

		if(distance <= maxDistance)
		{
			moveDirection = Vector3.zero;
			SetState(AIStates.IDLE);
		}
	}
}
