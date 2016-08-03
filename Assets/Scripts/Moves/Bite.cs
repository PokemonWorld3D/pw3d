using UnityEngine;
using System.Collections;

public class Bite : Move
{
	public Collider col;

	public void Reset()
	{
		ResetMoveData("Bite", "The target is bitten with viciously sharp fangs. This may also make the target flinch.", false, false, true, true, false,
			PokemonType.DARK, MoveCategory.PHYSICAL, 0.0f, 60);
	}
	public void BiteDamage()
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