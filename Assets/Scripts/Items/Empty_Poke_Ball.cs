using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Empty_Poke_Ball : NetworkBehaviour
{
	public AudioClip[] AudioClips;			//capturing = 0, attemptingCapture = 1, captureSuccess = 2, captureFail = 3;
	public Poke_Ball_Types thisPokeBallType;
	public Trainer trainer;
	public GameObject captureOrb;
	public Material captureMat;
	public Transform inside;
	public SphereCollider col;

	public bool grounded, empty = true;
	private Rigidbody rigidBody;
	private Animator anim;
	private AudioSource audioS;

	void Awake()
	{
		rigidBody = GetComponent<Rigidbody>();
		anim = GetComponent<Animator>();
		audioS = GetComponent<AudioSource>();
	}
	void OnCollisionEnter(Collision col)
	{
		if(!isServer)
			return;

		if(col.gameObject.tag == "Pokemon" && !col.gameObject.GetComponent<Pokemon>().isCaptured && empty)
		{
			RpcPlayAudio(0);
			col.gameObject.GetComponent<Pokemon>().RpcBeingCaptured();
			StartCoroutine(Capture(col.collider));
		}

		if(col.gameObject.tag == "Terrain")
			grounded = true;
	}

	[ClientRpc]
	public void RpcPlayAudio(int clipNumber)
	{
		audioS.PlayOneShot(AudioClips[clipNumber]);
	}
		
	private IEnumerator Capture(Collider col)
	{
		empty = false;
		rigidBody.useGravity = false;

		yield return StartCoroutine(MovePokeBall(col));

		rigidBody.WakeUp();
		rigidBody.useGravity = true;

		while(!grounded)
			yield return null;

		rigidBody.isKinematic = true;
		col.enabled = false;

		yield return StartCoroutine(TryToCatch(col));
	}
	private IEnumerator MovePokeBall(Collider col)
	{
		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
		rigidBody.Sleep();

		float offset = 1.5f;
		Vector3 moveTo = new Vector3(transform.position.x - offset, col.bounds.extents.y + offset, transform.position.z - offset);

		while(Vector3.Distance(transform.position, moveTo) > 0.01f)
		{
			transform.LookAt(col.transform.position);
			transform.position = Vector3.Lerp(transform.position, moveTo, 5.0f * Time.deltaTime);
			yield return null;
		}

		anim.SetBool("Open", true);

		GameObject orb = Instantiate(captureOrb, col.gameObject.GetComponentInChildren<Renderer>().GetComponent<Renderer>().bounds.center, Quaternion.identity) as GameObject;
		NetworkServer.Spawn(orb);

		// !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		// Need to include some code here that makes the Pokemon shrink and until it's smaller than the capture orb and then disappear.
		col.gameObject.GetComponent<Pokemon>().RpcDisable();

		while(Vector3.Distance(inside.position, orb.transform.position) > 0.01f)
		{
			orb.transform.position = Vector3.Lerp(orb.transform.position, inside.position, 2.7f * Time.deltaTime);
			yield return null;
		}

		NetworkServer.Destroy(orb);

		anim.SetBool("Open", false);

		Vector3 flatFwd = new Vector3(transform.forward.x, 0.0f, transform.forward.z);
		Quaternion fwdRotation = Quaternion.LookRotation(flatFwd, Vector3.up);
		float angle = Quaternion.Angle(transform.rotation, fwdRotation);

		while(angle > 1.0f)
		{
			transform.rotation = Quaternion.Slerp(transform.rotation, fwdRotation, Time.deltaTime * 5.0f);
			angle = Quaternion.Angle(transform.rotation, fwdRotation);
			yield return null;
		}

		yield return null;
	}
	private IEnumerator TryToCatch(Collider col)
	{
		Pokemon thisPokemon = col.gameObject.GetComponent<Pokemon>();
		PokemonConditions conditions = col.gameObject.GetComponent<PokemonConditions>();

		RpcPlayAudio(1);
		anim.SetBool("Capturing", true);

		yield return new WaitForSeconds(AudioClips[1].length+1);

		bool tryToCapture = Calculations.AttemptCapture(thisPokemon, conditions, thisPokeBallType);

		if(tryToCapture)
		{
			anim.SetBool("Capturing", false);
			thisPokemon.isCaptured = true;
			thisPokemon.trainersName = trainer.characterName;

			Pokemon temp = thisPokemon;
			PokemonHPPP hp = thisPokemon.gameObject.GetComponent<PokemonHPPP>();
			PokemonStats stats = thisPokemon.gameObject.GetComponent<PokemonStats>();
			Poke_Data dataHolderPokemon = new Poke_Data(temp.pokemonName, temp.nickName, temp.isFromTrade, temp.level, (int)temp.nature, hp.curMaxHP, hp.curMaxPP, stats.curMaxATK,
		                                                	  stats.curMaxDEF, stats.curMaxSPATK, stats.curMaxSPDEF, stats.curMaxSPD, hp.curHP, hp.curPP, stats.curATK, stats.curDEF,
			                                                  stats.curSPATK, stats.curSPDEF, stats.curSPD, hp.hpEV, hp.ppEV, stats.atkEV, stats.defEV, stats.spatkEV, stats.spdefEV,
			                                                  stats.spdEV, hp.hpIV, hp.ppIV, stats.atkIV, stats.defIV, stats.spatkIV, stats.spdefIV, stats.spdIV, temp.currentEXP,
			                                                  temp.equippedItem, temp.slot);

			if(trainer.PokemonRoster.Count < 6)
				trainer.PokemonRoster.Add(dataHolderPokemon);
			else
				trainer.PokemonInventory.Add(dataHolderPokemon);

			thisPokemon.isCaptured = false;
			col.gameObject.GetComponent<PokemonAI>().SetState(PokemonStates.Respawning);
			RpcPlayAudio(2);

			yield return new WaitForSeconds(AudioClips[2].length);
		}
		else
		{
			anim.SetBool("Open_Top", true);
			RpcPlayAudio(3);

			yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

			col.gameObject.GetComponent<Pokemon>().RpcEnable();
			col.gameObject.GetComponent<PokemonAI>().SetState(PokemonStates.Idle);
			anim.SetBool("Open_Top", false);

			yield return new WaitForSeconds(AudioClips[3].length);
		}

		NetworkServer.Destroy(gameObject);

		yield return null;
	}
}
