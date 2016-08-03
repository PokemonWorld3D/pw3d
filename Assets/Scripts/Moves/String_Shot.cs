using UnityEngine;
using System.Collections;

public class String_Shot : Move
{
	public ParticleSystem stringShot;

	public void Reset()
	{
		ResetMoveData("String Shot", "The opposing Pokémon are bound with silk blown from the user's mouth that harshly lowers their Speed.", false, false, false, false, false,
			PokemonType.BUG, MoveCategory.STATUS, 0.0f, 0);
	}
	public void StringShotStart()
	{
		ResetMoveValues();

		stringShot.Play();
	}
	public void StringShotFinish()
	{
		stringShot.Stop();
	}
}
