using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PokemonInput : NetworkBehaviour
{
	public PokeInput currentInput { get; private set; }

	private float horizontal, vertical;
	private bool walkInput, jumpInput, nextMoveInput, lastMoveInput, attackInput, attacking = false, swapToInput, targetInput;
	private TrainerComponents components;

	void Awake()
	{
		components = GetComponent<TrainerComponents>();
	}
	void Start()
	{
		currentInput = new PokeInput();
	}
	void Update()
	{
		if(!isLocalPlayer)
			return;
		
		if(components.pokeControl.inControl)
		{
			horizontal = Input.GetAxisRaw("Horizontal");
			vertical = Input.GetAxisRaw("Vertical");

			walkInput = Input.GetButton("Walk");
			jumpInput = Input.GetButton("Jump");
			nextMoveInput = Input.GetButtonDown("Next Move");
			lastMoveInput = Input.GetButtonDown("Last Move");
			attackInput = Input.GetMouseButton(1);
			swapToInput = Input.GetButtonDown("Swap To");
			targetInput = Input.GetMouseButtonDown(0);

			currentInput = new PokeInput()
			{
				horizontal = this.horizontal,
				vertical = this.vertical,
				walkInput = this.walkInput,
				jumpInput = this.jumpInput,
				nextMoveInput = this.nextMoveInput,
				lastMoveInput = this.lastMoveInput,
				attackInput = this.attackInput,
				swapToInput = this.swapToInput,
				targetInput = this.targetInput
			};
		}

		if(components.pokeControl.inControl)
		{
			if(currentInput.nextMoveInput)
				components.trainer.CmdNextMove();

			if(currentInput.lastMoveInput)
				components.trainer.CmdLastMove();

			if(currentInput.attackInput && !attacking)
			{
				components.trainer.CmdAttack(true);
				attacking = true;
			}
			else if(!currentInput.attackInput && attacking)
			{
				components.trainer.CmdAttack(false);
				attacking = false;
			}

			if(currentInput.swapToInput)
				components.trainer.SwapToTrainer();

			if(currentInput.targetInput)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
	
				if(Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
				{
					if(hit.transform.gameObject.CompareTag("Pokemon") && hit.transform.gameObject != gameObject)
					{
						if(!hit.transform.gameObject.GetComponent<Pokemon>().isCaptured)
						{
							components.trainer.hud.DisplayWildPokemonPanel(hit.transform.gameObject);
							components.trainer.hud.wildPokemonPanel.transform.position = Input.mousePosition;
						}
					}
				}
			}
		}
	}
}
public struct PokeInput
{
	public float vertical, horizontal;
	public bool walkInput, jumpInput, nextMoveInput, lastMoveInput, attackInput, swapToInput, targetInput;
}