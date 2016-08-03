using UnityEngine;
using System.Collections;

public class Bubble : Move
{
	public ParticleSystem bubble;

	public void Reset()
	{
		ResetMoveData("Bubble", "A spray of countless bubbles is jetted at the opposing Pokémon. This may also lower their speed.", false, false, false, false, false,
			PokemonType.WATER, MoveCategory.SPECIAL, 0.0f, 40);
	}
	public void StartBubble()
	{
		ResetMoveValues();

		bubble.Play();
	}
	public void FinishBubble()
	{
		bubble.Stop();
	}
}