using UnityEngine;
using System.Collections;

public class Water_Gun : Move
{
	public ParticleSystem waterGun;

	public void Reset()
	{
		ResetMoveData("Water Gun", "The target is blasted with a forceful shot of water.", false, false, false, false, false,
			PokemonType.WATER, MoveCategory.SPECIAL, 0.0f, 40);
	}
	public void StartWaterGun()
	{
		ResetMoveValues();

		waterGun.Play();
	}
	public void FinishWaterGun()
	{
		waterGun.Stop();
	}
}