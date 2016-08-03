using UnityEngine;
using System.Collections;

public class Bug_Bite : Move
{
	[SerializeField]
	private Collider col;

	public void Reset()
	{
		ResetMoveData("Bug Bite", "The user bites the target. If the target is holding a Berry, the user eats it and gains its effect.", false, false, false, true, false,
			PokemonType.BUG, MoveCategory.PHYSICAL, 0.0f, 60);
	}

	public void BugBiteDamage()
	{
		ResetMoveValues();

		if(!isServer)
			return;

		Collider[] cols = Physics.OverlapSphere(col.transform.position, range);

		for(int i = 0; i < cols.Length; i++)
		{
			if(cols[i].gameObject == components.pokemon.enemy)
				Calculations.DealDamage(components.pokemon, cols[i].transform.root.GetComponent<Pokemon>(), this);
		}
	}
}
