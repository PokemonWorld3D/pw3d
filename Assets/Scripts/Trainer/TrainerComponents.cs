using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TrainerComponents : MonoBehaviour
{
	public TrainerAI ai { get; private set; }
	public Trainer trainer { get; private set; }
	public TrainerInput input { get; private set;}
	public TrainerNetworkMovement netMovement { get; private set; }
	public PokemonInput pokeInput { get; private set; }
	public PokemonControl pokeControl { get; private set; }
	public CharacterController controller { get; private set; }
	public Animator anim { get; private set; }
	public SkinnedMeshRenderer mesh { get; private set; }
	public Camera cam { get; private set; }
	public Transform cameraFocus { get; private set; }
	public AudioListener audioL { get; private set; }
	public AudioSource audioS { get; private set; }

	void Awake()
	{
		ai = GetComponent<TrainerAI>();
		trainer = GetComponent<Trainer>();
		input = GetComponent<TrainerInput>();
		netMovement = GetComponent<TrainerNetworkMovement>();
		pokeInput = GetComponent<PokemonInput>();
		pokeControl = GetComponent<PokemonControl>();
		controller = GetComponent<CharacterController>();
		anim = GetComponent<Animator>();
		mesh = GetComponentInChildren<SkinnedMeshRenderer>();
		cam = GetComponentInChildren<Camera>();
		cameraFocus = transform.FindChild("Camera Focus");
		audioL = GetComponent<AudioListener>();
		audioS = GetComponent<AudioSource>();
	}
}
