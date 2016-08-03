using UnityEngine;
using System.Collections;

public class Hydro_Pump : Move
{
	[SerializeField] private ParticleSystem[] HydroPump;

	void Reset()
	{
		ResetMoveData("Hydro Pump", "The target is blasted by a huge volume of water launched under great pressure.", false, false, false, false, false, PokemonType.WATER,
			MoveCategory.SPECIAL, 0.0f, 110);
	}

	public void StartHydroPump()
	{
		foreach(ParticleSystem pSys in HydroPump)
			pSys.Play();
	}
	public void FinishHydroPump()
	{
		foreach(ParticleSystem pSys in HydroPump)
			pSys.Stop();
	}
}
