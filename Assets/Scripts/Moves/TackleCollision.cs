using UnityEngine;
using System.Collections;

public class TackleCollision : MonoBehaviour 
{
	private Tackle tackle;
	private PokemonComponents components;
	private Collider col;

	void Awake()
	{
		tackle = transform.root.GetComponent<Tackle>();
		components = transform.root.GetComponent<PokemonComponents>();
		col = GetComponent<Collider>();
	}
	void OnTriggerEnter(Collider collider)
	{
		if(!components.pokemon.trainer.isServer || !col.enabled)
			return;

		tackle.TackleFinish();

		if(collider.gameObject == components.pokemon.enemy)
			Calculations.DealDamage(components.pokemon, collider.gameObject.GetComponent<Pokemon>(), tackle);
	}
}
