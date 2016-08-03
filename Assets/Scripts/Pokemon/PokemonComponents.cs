using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PokemonComponents : MonoBehaviour
{
	public PokemonAI ai { get; private set; }
	public Pokemon pokemon { get; private set; }
	public PokemonHPPP hpPP { get; private set; }
	public PokemonStats stats { get; private set; }
	public PokemonConditions conditions { get; private set; }
	public PokemonBuffsDebuffs buffsDebuffs { get; private set; }
	public Aiming headLook { get; private set; }
	public CharacterController controller { get; private set; }
	public SkinnedMeshRenderer mesh { get; private set; }
	public Transform cameraFocus;
	public Transform armatureMesh;
	public Animator anim { get; private set; }
	public AudioListener audioL { get; private set; }
	public AudioSource audioS { get; private set; }

	void Awake()
	{
		ai = GetComponent<PokemonAI>();
		pokemon = GetComponent<Pokemon>();
		hpPP = GetComponent<PokemonHPPP>();
		stats = GetComponent<PokemonStats>();
		conditions = GetComponent<PokemonConditions>();
		buffsDebuffs = GetComponent<PokemonBuffsDebuffs>();
		headLook = GetComponent<Aiming>();
		controller = GetComponent<CharacterController>();
		mesh = GetComponentInChildren<SkinnedMeshRenderer>();
		anim = GetComponentInChildren<Animator>();
		audioL = GetComponent<AudioListener>();
		audioS = GetComponent<AudioSource>();
	}
}
