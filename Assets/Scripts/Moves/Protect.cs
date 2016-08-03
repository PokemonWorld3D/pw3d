using UnityEngine;
using System.Collections;

public class Protect : Move
{
	[SerializeField] private GameObject protect;

	public void Reset()
	{
		ResetMoveData("Protect", "Completely foils an opponent's attack.", false, false, false, false, false, PokemonType.NORMAL, MoveCategory.STATUS, 0.0f, 0);
	}
	public void StartProtect()
	{
		protect.SetActive(true);

		if(!isServer)
			return;

		components.conditions.Protect(true);
	}
	public void FinishProtect()
	{
		protect.SetActive(false);

		if(!isServer)
			return;

		components.conditions.Protect(false);
	}
}
