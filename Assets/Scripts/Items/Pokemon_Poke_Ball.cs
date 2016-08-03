using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Pokemon_Poke_Ball : NetworkBehaviour
{
	public AudioClip pokemonOut;
	public Poke_Data data;
	public Trainer trainer;
	public GameObject pokemonOutSFX, prefab;

	private Animator anim;
	private Rigidbody rigidBody;
	private AudioSource audioS;
	private bool spawning = false;
	private Collider col;
	
	void Awake()
	{
		anim = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody>();
		audioS = GetComponent<AudioSource>();
		col = GetComponent<Collider>();
	}
	void OnCollisionEnter(Collision collider)
	{
		if(!isServer)
			return;

		if(collider.gameObject.CompareTag("Terrain") && !spawning)
		{
			rigidBody.useGravity = false;
			rigidBody.Sleep();
			col.enabled = false;
			StartCoroutine(PokemonGo());
			spawning = true;
		}
	}
	
	private void HandOverStats(GameObject pokemon)
	{
		PokemonComponents components = pokemon.GetComponent<PokemonComponents>();

		components.pokemon.trainer = trainer;
		components.pokemon.nickName = data.nickName;
		components.pokemon.isFromTrade = data.isFromTrade;
		components.pokemon.level = data.level;
		components.pokemon.nature = (PokemonNature)data.nature;
		components.hpPP.curMaxHP = data.curMaxHP;
		components.hpPP.curMaxPP = data.curMaxPP;
		components.stats.curMaxATK = data.curMaxATK;
		components.stats.curMaxDEF = data.curMaxDEF;
		components.stats.curMaxSPATK = data.curMaxSPATK;
		components.stats.curMaxSPDEF = data.curMaxSPDEF;
		components.stats.curMaxSPD = data.curMaxSPD;
		components.hpPP.curHP = data.curHP;
		components.hpPP.curPP = data.curPP;
		components.stats.curATK = data.curATK;
		components.stats.curDEF = data.curDEF;
		components.stats.curSPATK = data.curSPATK;
		components.stats.curSPDEF = data.curSPDEF;
		components.stats.curSPD = data.curSPD;
		components.hpPP.hpEV = data.hpEV;
		components.hpPP.ppEV = data.ppEV;
		components.stats.atkEV = data.atkEV;
		components.stats.defEV = data.defEV;
		components.stats.spatkEV = data.spatkEV;
		components.stats.spdefEV = data.spdefEV;
		components.stats.spdEV = data.spdEV;
		components.hpPP.hpIV = data.hpIV;
		components.hpPP.ppIV = data.ppIV;
		components.stats.atkIV = data.atkIV;
		components.stats.defIV = data.defIV;
		components.stats.spatkIV = data.spatkIV;
		components.stats.spdefIV = data.spdefIV;
		components.stats.spdIV = data.spdIV;
		components.pokemon.currentEXP = data.currentEXP;
		components.pokemon.equippedItem = data.equippedItem;
		components.pokemon.slot = data.slot;
	}

	private IEnumerator PokemonGo()
	{
		rigidBody.velocity = Vector3.zero;
		transform.LookAt(transform.forward);
		anim.SetTrigger("Open_Top");
		audioS.PlayOneShot(pokemonOut);

		yield return new WaitForSeconds(1.0f);

		GameObject pokeOut = Instantiate(pokemonOutSFX, transform.position, Quaternion.identity) as GameObject;
		NetworkServer.Spawn(pokeOut);
		GameObject pokemon = Instantiate(Resources.Load("Prefabs/Pokemon/" + data.pokemonName), transform.position, Quaternion.identity) as GameObject;

		HandOverStats(pokemon);
		trainer.GetComponent<PokemonControl>().SetStartPosition(transform.position);
		trainer.GetComponent<PokemonControl>().SetStartRotation(Quaternion.identity);
//		pokemon.GetComponent<Pokemon_AI>().enabled = true;

		NetworkServer.Spawn(pokemon);

		pokemon.GetComponent<Pokemon>().SetupExistingPokemon();

//		pokemon.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
//
//		Material[] materials = pokemon.GetComponentInChildren<SkinnedMeshRenderer>().materials;
//		Color color = new Color();
//		ColorUtility.TryParseHtmlString("#FFFFFF", out color);
//
//		foreach(Material material in materials)
//			material.SetColor("_EmissionColor", color);
//		
//
//		pokemon.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
//
//		foreach(Material material in materials)
//			StartCoroutine(pokemon.GetComponent<Pokemon>().ChangeToColor(material));

		anim.SetBool("Open", false);

		trainer.RpcSetActivePokemon(pokemon.GetComponent<NetworkIdentity>().netId);

		if(trainer.inBattle && trainer.opponent.activePokemon)
		{
			trainer.RpcSetPokemonEnemy(trainer.opponent.activePokemon.netId);
			trainer.opponent.RpcSetPokemonEnemy(pokemon.GetComponent<NetworkIdentity>().netId);
		}

		while(audioS.isPlaying)
			yield return null;

		NetworkServer.Destroy(gameObject);

		yield return null;
	}
}
