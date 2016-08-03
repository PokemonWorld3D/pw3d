using UnityEngine;
using System.Collections;

public class Tackle : Move
{
	[SerializeField] private Collider col;
	[SerializeField] private TrailRenderer trail;
	[SerializeField] private float newRunMultiplier = 2.0f, trailTime = 0.5f;

	private float originalRunMultiplier;

	public void Reset()
	{
		ResetMoveData("Tackle", "A physical attack in which the user charges and slams into the target with its whole body.", false, false, false, false, false,
			PokemonType.NORMAL, MoveCategory.PHYSICAL, 0.0f, 50);
	}

	public void TackleStart()
	{
		ResetMoveValues();

		trail.time = trailTime;
		col.enabled = true;

		originalRunMultiplier = components.pokemon.trainer.components.pokeControl.runMultiplier;
		components.pokemon.trainer.components.pokeControl.runMultiplier = newRunMultiplier;
		components.pokemon.DeductPP();
	}
	public void TackleFinish()
	{
		trail.time = -1.0f;
		col.enabled = false;
		components.pokemon.trainer.components.pokeControl.runMultiplier = originalRunMultiplier;
	}
}
