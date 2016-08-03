using UnityEngine;
using System.Collections;

public class Smokescreen : Move
{
	[SerializeField]
	private ParticleSystem smokescreen;

	public void Reset()
	{
		ResetMoveData("Smokescreen", "The user releases an obscuring cloud of smoke or ink.", false, false, false, false, false, PokemonType.NORMAL, MoveCategory.STATUS,
			0.0f, 0);
	}
	public void SmokescreenStart()
	{
		smokescreen.Play();
	}
	public void SmokescreenFinish()
	{
		smokescreen.Stop();
	}
}