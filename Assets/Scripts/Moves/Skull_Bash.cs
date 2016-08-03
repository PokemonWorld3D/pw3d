using UnityEngine;
using System.Collections;

public class Skull_Bash : Move
{
	[SerializeField] private Collider col;
	[SerializeField] private TrailRenderer trail;
	[SerializeField] private float newRunMultiplier = 2.0f, trailTime = 0.5f;

	private float originalRunMultiplier;

	public void Reset()
	{
		ResetMoveData("Skull Bash", "The user tucks in its head, then rams the target.", false, false, false, true, false, PokemonType.NORMAL, MoveCategory.PHYSICAL, 0.0f, 130);
	}

	public void SkullBashStart()
	{
		ResetMoveValues();

		trail.time = trailTime;
		col.enabled = true;

		originalRunMultiplier = components.pokemon.trainer.components.pokeControl.runMultiplier;
		components.pokemon.trainer.components.pokeControl.runMultiplier = newRunMultiplier;
		components.pokemon.DeductPP();
	}
	public void SkullBashFinish()
	{
		trail.time = -1.0f;
		col.enabled = false;
		components.pokemon.trainer.components.pokeControl.runMultiplier = originalRunMultiplier;
	}
}
