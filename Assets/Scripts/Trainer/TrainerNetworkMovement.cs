using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

[NetworkSettings(channel = 1, sendInterval = 0.05f)]
public class TrainerNetworkMovement : NetworkBehaviour
{
	[SyncVar(hook="InControl")] public bool inControl = true;
	[HideInInspector] public bool throwing = false, returning = false;

	public struct Inputs			
	{
		public float forward, sides, vertical, cameraForwardX, cameraForwardZ;
		public bool walk;

		public float timeStamp;
	}
	public struct SyncInputs			
	{
		public sbyte forward, sides, vertical, cameraForwardX, cameraForwardZ;
		public bool walk;

		public float timeStamp;
	}
	public struct Results
	{
		public Quaternion rotation;
		public Vector3 position;
		public bool walking;
		public float timeStamp;
	}
	public struct SyncResults
	{
		public ushort yaw;
		public Vector3 position;
		public bool walking;
		public float timeStamp;
	}

	[SerializeField] private Transform mesh;
	[SerializeField] private float snapDistance = 1.0f, walkSpeed = 1.0f, runMultiplier = 1.0f, jumpHeight = 10.0f;

	[SyncVar(hook="RecieveResults")] private SyncResults syncResults;

	private float dataStep = 0.0f, lastTimeStamp = 0.0f, step = 0.0f, verticalSpeed = 0.0f, speed = 0.0f, movementSpeed = 0.0f;
	private bool playData = false, jumping = false, moving = false;
	private Vector3 startPosition, forward, right, moveDirection, lookDirection, lastPosition;
	private Quaternion startRotation, rotation;
	private Inputs inputs;
	private Results results;
	private List<Inputs> InputsList = new List<Inputs>();
	private List<Results> ResultsList = new List<Results>();
	private TrainerComponents components;

	public void SetStartPosition(Vector3 position)
	{
		results.position = position;
		lastPosition = position;
	}
	public void SetStartRotation(Quaternion rotation)
	{
		results.rotation = rotation;
	}

	public override void OnStartLocalPlayer()
	{
		components.audioL.enabled = true;
		components.input.enabled = true;
		Camera.main.SendMessage("SetTrainerTarget", components.cameraFocus);
	}

	void Awake()
	{
		components = GetComponent<TrainerComponents>();
	}
	void Start()
	{
		SetStartPosition(transform.position);
		SetStartRotation(transform.rotation);
	}
	void Update()
	{
		if(isLocalPlayer && inControl)
			GetInputs(ref inputs);
	}
	void FixedUpdate()
	{
		if(isServer)
			MovementAnimationCheck(transform.position);
		
		if(isLocalPlayer && inControl)
		{
			inputs.timeStamp = Time.time;
			//Client side prediction for non-authoritative client or plane movement and rotation for listen server/host
			Vector3 lastPosition = results.position;
			Quaternion lastRotation = results.rotation;
			results.rotation = Rotate(inputs, results);
			results.walking = Walk(inputs, results);
			results.position = Move(inputs, results);
			if(hasAuthority)
			{
				//Listen server/host part
				//Sending results to other clients(state sync)
				if(dataStep >= GetNetworkSendInterval())
				{
					if(Vector3.Distance(results.position, lastPosition) > 0.0f || Quaternion.Angle(results.rotation, lastRotation) > 0.0f)
					{
						results.timeStamp = inputs.timeStamp;
						//Struct need to be fully new to count as dirty 
						//Convering some of the values to get less traffic
						SyncResults tempResults;
						tempResults.yaw = (ushort)(results.rotation.eulerAngles.y * 182);
						tempResults.position = results.position;
						tempResults.walking = results.walking;
						tempResults.timeStamp = results.timeStamp;
						syncResults = tempResults;
					}
					dataStep = 0.0f;
				}
				dataStep += Time.fixedDeltaTime;
			}
			else
			{
				//Owner client. Non-authoritative part
				//Add inputs to the inputs list so they could be used during reconciliation process
				if(Vector3.Distance(results.position,lastPosition) > 0.0f || Quaternion.Angle(results.rotation,lastRotation) > 0.0f)
				{
					InputsList.Add(inputs);
				}
				//Sending inputs to the server
				//Unfortunately there is now method overload for [Command] so I need to write several almost similar functions
				//This one is needed to save on network traffic
				SyncInputs syncInputs;
				syncInputs.forward = (sbyte)(inputs.forward * 127);
				syncInputs.sides = (sbyte)(inputs.sides * 127);
				syncInputs.vertical = (sbyte)(inputs.vertical * 127);
				syncInputs.cameraForwardX = (sbyte)(inputs.cameraForwardX * 127);
				syncInputs.cameraForwardZ = (sbyte)(inputs.cameraForwardZ * 127);
				if(Vector3.Distance(results.position, lastPosition) > 0.0f )
				{
					if(Quaternion.Angle(results.rotation,lastRotation) > 0.0f)
					{
						Cmd_MovementRotationInputs(syncInputs.forward, syncInputs.sides, syncInputs.vertical, syncInputs.cameraForwardX, syncInputs.cameraForwardZ,
							inputs.walk, inputs.timeStamp);
					}
					else
					{
						Cmd_MovementInputs(syncInputs.forward, syncInputs.sides, syncInputs.vertical, syncInputs.cameraForwardX, syncInputs.cameraForwardZ, inputs.walk,
							inputs.timeStamp);
					}
				}
			}
		}
		else
		{
			if(hasAuthority)
			{
				//Server

				//Check if there is atleast one record in inputs list
				if(InputsList.Count == 0)
				{
					return;
				}
				//Move and rotate part. Nothing interesting here
				Inputs inputs = InputsList[0];
				InputsList.RemoveAt(0);
				Vector3 lastPosition = results.position;
				Quaternion lastRotation = results.rotation;
				results.rotation = Rotate(inputs, results);
				results.walking = Walk(inputs, results);
				results.position = Move(inputs, results);

				//Sending results to other clients(state sync)

				if(dataStep >= GetNetworkSendInterval())
				{
					if(Vector3.Distance(results.position,lastPosition) > 0.0f || Quaternion.Angle(results.rotation,lastRotation) > 0.0f)
					{
						//Struct need to be fully new to count as dirty 
						//Convering some of the values to get less traffic
						results.timeStamp = inputs.timeStamp;
						SyncResults tempResults;
						tempResults.yaw = (ushort)(results.rotation.eulerAngles.y * 182);
						tempResults.position = results.position;
						tempResults.walking = results.walking;
						tempResults.timeStamp = results.timeStamp;
						syncResults = tempResults;
					}
					dataStep = 0.0f;
				}
				dataStep += Time.fixedDeltaTime;
			}
			else
			{
				//Non-owner client a.k.a. dummy client
				//there should be at least two records in the results list so it would be possible to interpolate between them in case if there would be some dropped packed or latency spike
				//And yes this stupid structure should be here because it should start playing data when there are at least two records and continue playing even if there is only one record left 
				if(ResultsList.Count == 0)
				{
					playData = false;
				}
				if(ResultsList.Count >= 2)
				{
					playData = true;
				}
				if(playData)
				{
					if(dataStep == 0.0f)
					{
						startPosition = results.position;
						startRotation = results.rotation;
					}
					step = (1.0f / (GetNetworkSendInterval()));
					results.rotation = Quaternion.Slerp(startRotation, ResultsList[0].rotation, dataStep);
					results.position = Vector3.Lerp(startPosition, ResultsList[0].position, dataStep);
					results.walking = ResultsList[0].walking;
					dataStep += step * Time.fixedDeltaTime;
					if(dataStep >= 1.0f)
					{
						dataStep = 0.0f;
						ResultsList.RemoveAt(0);
					}
				}
				UpdateRotation(results.rotation);
				UpdatePosition(results.position);
				UpdateWalking(results.walking);
			}
		}
	}

	[Command(channel = 0)] private void Cmd_MovementRotationInputs(sbyte forward, sbyte sides, sbyte vertical, sbyte cameraForwardX, sbyte cameraForwardZ, bool walk, float timeStamp)
	{
		if(hasAuthority && !isLocalPlayer)
		{
			Inputs inputs;
			inputs.forward = Mathf.Clamp((float)forward / 127, -1.0f, 1.0f);
			inputs.sides = Mathf.Clamp((float)sides / 127, -1.0f, 1.0f);
			inputs.vertical = Mathf.Clamp((float)vertical / 127, -1.0f, 1.0f);
			inputs.cameraForwardX = Mathf.Clamp((float)cameraForwardX / 127, -1.0f, 1.0f);
			inputs.cameraForwardZ = Mathf.Clamp((float)cameraForwardZ / 127, -1.0f, 1.0f);
			inputs.walk = walk;
			inputs.timeStamp = timeStamp;
			InputsList.Add(inputs);
		}
	}
	[Command(channel = 0)] private void Cmd_MovementInputs(sbyte forward, sbyte sides, sbyte vertical, sbyte cameraForwardX, sbyte cameraForwardZ, bool walk, float timeStamp)
	{
		if(hasAuthority && !isLocalPlayer)
		{
			Inputs inputs;
			inputs.forward = Mathf.Clamp((float)forward / 127, -1.0f, 1.0f);
			inputs.sides = Mathf.Clamp((float)sides / 127, -1.0f, 1.0f);
			inputs.vertical = Mathf.Clamp((float)vertical / 127, -1.0f, 1.0f);
			inputs.cameraForwardX = Mathf.Clamp((float)cameraForwardX / 127, -1.0f, 1.0f);
			inputs.cameraForwardZ = Mathf.Clamp((float)cameraForwardZ / 127, -1.0f, 1.0f);
			inputs.walk = walk;
			inputs.timeStamp = timeStamp;
			InputsList.Add(inputs);
		}
	}

	public void Control(bool value)
	{
		if(value)
		{
			components.trainer.CmdSwap();
			components.audioL.enabled = true;
			components.pokeInput.enabled = false;
			components.input.enabled = true;
			Camera.main.SendMessage("SetTrainerTarget", components.cameraFocus);
		}
		else
		{
			components.trainer.CmdSwap();
			components.audioL.enabled = false;
			components.pokeInput.enabled = true;
			components.input.enabled = false;
		}
	}

	public virtual void GetInputs(ref Inputs inputs)
	{
		inputs.sides = components.input.currentInput.horizontal;
		inputs.forward = components.input.currentInput.vertical;

		inputs.cameraForwardX = Camera.main.transform.TransformDirection(Vector3.forward).x;
		inputs.cameraForwardZ = Camera.main.transform.TransformDirection(Vector3.forward).z;
		inputs.walk = components.input.currentInput.walkInput;

		if(components.controller.isGrounded && components.input.currentInput.jumpInput && inputs.vertical <= -0.9f)
		{
			jumping = true;
			components.anim.SetBool("Jump", true);
		}

		float verticalTarget = -1.0f;

		if(jumping)
		{
			verticalTarget = 1.0f;

			if(inputs.vertical >= 0.9f)
			{
				jumping = false;
				components.anim.SetBool("Jump", false);
			}
		}

		inputs.vertical = Mathf.Lerp(inputs.vertical, verticalTarget, 20.0f * Time.deltaTime);
	}

	public virtual void UpdatePosition(Vector3 newPosition)
	{
		if(Vector3.Distance(newPosition, transform.position) > snapDistance)
			transform.position = newPosition;
		else
			components.controller.Move(newPosition - transform.position);

		lastPosition = newPosition;
	}
	public virtual void UpdateRotation(Quaternion newRotation)
	{
		mesh.rotation = Quaternion.Euler(0.0f, newRotation.eulerAngles.y, 0.0f);
		rotation = newRotation;
	}
	public virtual void UpdateWalking(bool walking)
	{

	}

	public virtual Vector3 Move(Inputs inputs, Results current)
	{
		if(throwing || returning)
			return transform.position;
		
		transform.position = current.position;

		speed = current.walking ? walkSpeed : walkSpeed * runMultiplier;

		if(inputs.vertical > 0.0f)
			verticalSpeed = inputs.vertical * jumpHeight;
		else
			verticalSpeed = inputs.vertical * Physics.gravity.magnitude;
		
		Vector3 forward = new Vector3(inputs.cameraForwardX, 0.0f, inputs.cameraForwardZ);
		Vector3 right = new Vector3(inputs.cameraForwardZ, 0.0f, -inputs.cameraForwardX);

		moveDirection = (inputs.sides * right + inputs.forward * forward);
		movementSpeed = speed == walkSpeed ? 1.0f : 2.0f;

		components.controller.Move((Vector3.ClampMagnitude(moveDirection, 1.0f) * speed) + new Vector3(0.0f, verticalSpeed, 0.0f) * Time.fixedDeltaTime);

		return transform.position;
	}
	public virtual bool Walk(Inputs inputs, Results current)
	{
		return inputs.walk;
	}
	public virtual Quaternion Rotate(Inputs inputs, Results current)
	{
		if(throwing || returning)
			return mesh.rotation;
		
		mesh.rotation = current.rotation;

		Vector3 forward = new Vector3(inputs.cameraForwardX, 0.0f, inputs.cameraForwardZ);
		Vector3 right = new Vector3(inputs.cameraForwardZ, 0.0f, -inputs.cameraForwardX);

		lookDirection = (inputs.sides * right + inputs.forward * forward);

		if(lookDirection != Vector3.zero)
		{
			mesh.rotation = Quaternion.LookRotation(lookDirection);
			rotation = Quaternion.LookRotation(lookDirection);
		}

		return rotation;
	}

	[ClientCallback] void RecieveResults(SyncResults syncResults)
	{ 
		//Converting values back
		Results incomingResults;
		incomingResults.rotation = Quaternion.Euler(0.0f, (float)syncResults.yaw / 182.0f, 0.0f);
		incomingResults.position = syncResults.position;
		incomingResults.walking = syncResults.walking;
		incomingResults.timeStamp = syncResults.timeStamp;

		//Discard out of order results
		if(incomingResults.timeStamp <= lastTimeStamp)
			return;
		
		lastTimeStamp = incomingResults.timeStamp;

		//Non-owner client
		if(!isLocalPlayer && !hasAuthority)
		{
			//Adding results to the results list so they can be used in interpolation process
			incomingResults.timeStamp = Time.time;
			ResultsList.Add(incomingResults);
		}

		//Owner client
		//Server client reconciliation process should be executed in order to client's rotation and position with server values but do it without jittering
		if(isLocalPlayer && !hasAuthority)
		{
			//Update client's position and rotation with ones from server 
			results.rotation = incomingResults.rotation;
			results.position = incomingResults.position;

			int foundIndex = -1;
			//Search recieved time stamp in client's inputs list
			for(int index = 0; index < InputsList.Count; index++)
			{
				//If time stamp found run through all inputs starting from needed time stamp 
				if(InputsList[index].timeStamp > incomingResults.timeStamp)
				{
					foundIndex = index;
					break;
				}
			}

			if(foundIndex == -1)
			{
				//Clear Inputs list if no needed records found 
				while(InputsList.Count != 0)
				{
					InputsList.RemoveAt(0);
				}
				return;
			}

			//Replay recorded inputs
			for(int subIndex = foundIndex; subIndex < InputsList.Count; subIndex++)
			{
				results.rotation = Rotate(InputsList[subIndex], results);
				results.walking = Walk(InputsList[subIndex], results);

				results.position = Move(InputsList[subIndex], results);
			}

			//Remove all inputs before time stamp
			int targetCount = InputsList.Count - foundIndex;

			while(InputsList.Count > targetCount)
			{
				InputsList.RemoveAt(0);
			}
		}
	}
	[ClientCallback] void InControl(bool inControl)
	{
		this.inControl = inControl;
	}

	private void MovementAnimationCheck(Vector3 curPosition)
	{
		if(curPosition != lastPosition && !moving)
		{
			components.anim.SetFloat("Speed", 1.0f);
			moving = true;
		}
		else if(curPosition == lastPosition && moving)
		{
			components.anim.SetFloat("Speed", 0.0f);
			moving = false;
		}

		lastPosition = curPosition;
	}
}