using UnityEngine;
using System.Collections;

public class Fire_Spin : Move
{
	public ParticleSystem fireSpin;

	public void Reset()
	{
		ResetMoveData("Fire Spin", "The target becomes trapped within a fierce vortex of fire that rages for five seconds.", false, false, false, false, false, PokemonType.FIRE,
			MoveCategory.SPECIAL, 0.0f, 35);
	}
	public void StartFireSpin()
	{
		ResetMoveValues();
		
		fireSpin.Play();
	}
	public void FinishFireSpin()
	{
		fireSpin.Stop();
	}
}