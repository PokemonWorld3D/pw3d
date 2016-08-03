using UnityEngine;
using System.Collections;

public class Rain_Dance : Move
{
	[SerializeField] private GameObject rainDanceSFX;

	public void Reset()
	{
		ResetMoveData("Rain Dance", "The user summons a heavy rain that falls for 5 minutes, powering up Water-type moves.", false, false, false, false, false, PokemonType.WATER,
			MoveCategory.STATUS, 0.0f, 0);
	}
	public void StartRainDance()
	{
		Vector3 spawnPos = transform.position;
		spawnPos.y = 10.0f;
		Quaternion spawnRot = Quaternion.Euler(90.0f, 0.0f, 0.0f);

		GameObject rain = Instantiate(rainDanceSFX, spawnPos, spawnRot) as GameObject;
		Destroy(rain, 300.0f);
	}
}
