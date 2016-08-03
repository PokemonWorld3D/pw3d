using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TrainerInput : NetworkBehaviour
{
	public TrainInput currentInput { get; private set; }

	public struct TrainInput
	{
		public float horizontal, vertical;
		public bool walkInput, jumpInput, nextSlotInput, lastSlotInput, captureInput, pokemonGoInput, swapToInput, returnInput, targetInput;
	}

	private float horizontal, vertical;
	private bool walkInput, jumpInput, nextSlotInput, lastSlotInput, captureInput, pokemonGoInput, swapToInput, returnInput, targetInput;
	private TrainerComponents components;

	void Awake()
	{
		components = GetComponent<TrainerComponents>();
	}
	void Start()
	{
		currentInput = new TrainInput();
	}
	void Update()
	{
		if(!isLocalPlayer)
			return;
		
		if(components.netMovement.inControl)
		{
			horizontal = Input.GetAxisRaw("Horizontal");
			vertical = Input.GetAxisRaw("Vertical");
			walkInput = Input.GetButton("Walk");
			jumpInput = Input.GetButton("Jump");
			nextSlotInput = Input.GetButtonDown("Next Slot");
			lastSlotInput = Input.GetButtonDown("Last Slot");
			captureInput = Input.GetButtonDown("Capture");
			pokemonGoInput = Input.GetButton("Pokemon Go");
			swapToInput = Input.GetButtonDown("Swap To");
			returnInput = Input.GetButton("Pokemon Return");
			targetInput = Input.GetMouseButtonDown(0);

			currentInput = new TrainInput()
			{
				horizontal = this.horizontal,
				vertical = this.vertical,
				walkInput = this.walkInput,
				jumpInput = this.jumpInput,
				nextSlotInput = this.nextSlotInput,
				lastSlotInput = this.lastSlotInput,
				captureInput = this.captureInput,
				pokemonGoInput = this.pokemonGoInput,
				swapToInput = this.swapToInput,
				returnInput = this.returnInput,
				targetInput = this.targetInput
			};
		}

		if(components.netMovement.inControl)
		{
			if(currentInput.nextSlotInput)
				components.trainer.CmdNextSlot();

			if(currentInput.lastSlotInput)
				components.trainer.CmdLastSlot();

			if(currentInput.pokemonGoInput)
				components.trainer.CmdPokemonGo();

			if(currentInput.swapToInput)
				components.trainer.SwapToPokemon();

			if(currentInput.targetInput)
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;

				if(Physics.Raycast(ray, out hit, Camera.main.farClipPlane))
				{
					if(hit.transform.gameObject.CompareTag("Trainer") && hit.transform.gameObject != gameObject)
					{
						if(!hit.transform.gameObject.GetComponent<Trainer>().inBattle)
						{
							components.trainer.hud.DisplayOtherTrainerPanel(hit.transform.gameObject);
							components.trainer.hud.otherTrainerPanel.transform.position = Input.mousePosition;
						}
					}
				}
			}

			if(currentInput.captureInput)
			{
				Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
				RaycastHit targetHit;

				if(Physics.Raycast(ray, out targetHit, 100.0f))
				{
					components.trainer.CmdSetPokeBallTarget(targetHit.point);
					return;
				}
			}
				
			if(currentInput.returnInput)
			{
				components.trainer.CmdRecallPokemon();
				return;
			}
		}
	}
}