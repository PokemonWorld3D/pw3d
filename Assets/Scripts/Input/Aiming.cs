using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Aiming : NetworkBehaviour
{
	[HideInInspector][SyncVar(hook="SyncHead")] public Quaternion headRotation;
	[HideInInspector][SyncVar(hook="SyncBody")] public Quaternion bodyRotation;
	[HideInInspector][SyncVar(hook="SyncTarget")] public Vector3 target;

	[SerializeField] private float headXMin, headXMax, headYMin, headYMax, bodyXMin, bodyXMax, bodyYMin, bodyYMax, armLXMin, armLXMax, armLYMin, armLYMax, armRXMin, armRXMax,
	armRYMin, armRYMax;
	[SerializeField] private Transform head, body, armL, armR;

	public bool aimHead = false, aimBody = false, aimArmL = false, aimArmR = false, aimBothArms = false;
	private PokemonComponents components;

	void Awake()
	{
		components = GetComponent<PokemonComponents>();
	}
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
			BothArmAimStart();

		if(Input.GetKeyDown(KeyCode.L))
			BothArmAimStop();
	}
	void LateUpdate()
	{
		if(aimArmL || aimArmR || aimBothArms)
		{
			if(components.pokemon.trainer.isLocalPlayer)
			{
				Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
				RaycastHit[] Hits;
				Hits = Physics.RaycastAll(ray, Mathf.Infinity);

				for(int i = 0; i < Hits.Length; i++)
				{
					if(Hits[i].transform.gameObject != gameObject)
					{
						components.pokemon.trainer.CmdTarget(Hits[i].point);
						break;
					}	
				}
			}
		}

		if(aimHead)
		{
			if(components.pokemon.trainer.isLocalPlayer)
				components.pokemon.trainer.CmdHeadRotation(Quaternion.Euler(Camera.main.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0.0f));
				
			head.rotation = headRotation;
		}
		if(aimBody)
		{
			if(components.pokemon.trainer.isLocalPlayer)
				components.pokemon.trainer.CmdBodyRotation(Quaternion.Euler(Camera.main.transform.eulerAngles.x, body.rotation.y, body.rotation.z));

			body.rotation = bodyRotation;
		}
		if(aimArmL)
		{
			armL.LookAt(target);
			armL.Rotate(new Vector3(1.0f, 0.0f, 0.0f), 90);
		}
		if(aimArmR)
		{
			armR.LookAt(target);
			armR.Rotate(new Vector3(1.0f, 0.0f, 0.0f), 90);
		}
		if(aimBothArms)
		{
			armL.LookAt(target);
			armL.Rotate(new Vector3(1.0f, 0.0f, 0.0f), 90);
			armR.LookAt(target);
			armR.Rotate(new Vector3(1.0f, 0.0f, 0.0f), 90);
		}
	}

	public void HeadAimStart()
	{
		enabled = true;
		aimHead = true;
		CameraAim(headXMin, headXMax, headYMin, headYMax);

		if(isServer)
			components.pokemon.trainer.components.pokeControl.canMove = false;
	}
	public void HeadAimStop()
	{
		enabled = false;
		aimHead = false;
		CameraDefault();

		if(isServer)
			components.pokemon.trainer.components.pokeControl.canMove = true;
	}
	public void BodyAimStart()
	{
		enabled = true;
		aimBody = true;
		CameraAim(bodyXMin, bodyXMax, bodyYMin, bodyYMax);

		if(isServer)
			components.pokemon.trainer.components.pokeControl.canMove = false;
	}
	public void BodyAimStop()
	{
		enabled = false;
		aimBody = false;
		CameraDefault();

		if(isServer)
			components.pokemon.trainer.components.pokeControl.canMove = true;
	}
	public void ArmLAimStart()
	{
		enabled = true;
		aimArmL = true;
		CameraAim(armLXMin, armLXMax, armLYMin, armLYMax);

		if(isServer)
			components.pokemon.trainer.components.pokeControl.canMove = false;
	}
	public void ArmLAimStop()
	{
		enabled = false;
		aimArmL = false;
		CameraDefault();

		if(isServer)
			components.pokemon.trainer.components.pokeControl.canMove = true;
	}
	public void ArmRAimStart()
	{
		enabled = true;
		aimArmR = true;
		CameraAim(armRXMin, armRXMax, armRYMin, armRYMax);

		if(isServer)
			components.pokemon.trainer.components.pokeControl.canMove = false;
	}
	public void ArmRAimStop()
	{
		enabled = false;
		aimArmR = false;
		CameraDefault();

		if(isServer)
			components.pokemon.trainer.components.pokeControl.canMove = true;
	}
	public void BothArmAimStart()
	{
		enabled = true;
		aimBothArms = true;
		CameraAim(armLXMin, armLXMax, armLYMin, armLYMax);

		if(isServer)
			components.pokemon.trainer.components.pokeControl.canMove = false;
	}
	public void BothArmAimStop()
	{
//		enabled = false;
		aimBothArms = false;
		CameraDefault();

		if(isServer)
			components.pokemon.trainer.components.pokeControl.canMove = true;
	}

	private void SyncHead(Quaternion rotation)
	{
		this.headRotation = rotation;
	}
	private void SyncBody(Quaternion rotation)
	{
		this.bodyRotation = rotation;
	}
	private void SyncTarget(Vector3 target)
	{
		this.target = target;
	}
	private void CameraAim(float xMinLimit, float xMaxLimit, float yMinLimit, float yMaxLimit)
	{
		if(components.pokemon.trainer.isLocalPlayer)
		{
			CameraController cam = Camera.main.GetComponent<CameraController>();
			cam.curXMin = cam.target.rotation.eulerAngles.y + xMinLimit;
			cam.curXMax = cam.target.rotation.eulerAngles.y + xMaxLimit;
			cam.curYMin = cam.target.rotation.eulerAngles.x + yMinLimit;
			cam.curYMax = cam.target.rotation.eulerAngles.x + yMaxLimit;
		}
	}
	private void CameraDefault()
	{
		if(components.pokemon.trainer.isLocalPlayer)
		{
			CameraController cam = Camera.main.GetComponent<CameraController>();
			cam.curXMin = cam.xMinLimit;
			cam.curXMax = cam.xMaxLimit;
			cam.curYMin = cam.yMinLimit;
			cam.curYMax = cam.yMaxLimit;
		}
	}
}
