using UnityEngine;
using System.Collections;

public class Rapid_Spin : Move 
{
	[SerializeField] private Collider col1, col2;
	[SerializeField] private TrailRenderer trail1, trail2;
	[SerializeField] private float trailTime = 0.5f;

	public void Reset()
	{
		ResetMoveData("Rapid Spin", "A spin attack that can also eliminate such moves as Bind, Wrap, Leech Seed, and Spikes.", false, false, false, true, false, 
			PokemonType.NORMAL, MoveCategory.PHYSICAL, 0.0f, 20);
	}
	public void RapidSpinStart()
	{
		ResetMoveValues();

		trail1.time = trailTime;
		trail2.time = trailTime;
		col1.enabled = true;
		col2.enabled = true;
	}
	public void RapidSpinFinish()
	{	
		trail1.time = -1.0f;
		trail2.time = -1.0f;
		col1.enabled = false;
		col2.enabled = false;
	}
}
