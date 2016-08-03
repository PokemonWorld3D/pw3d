using UnityEngine;
using System.Collections;

public class SkullBashCollision : MonoBehaviour 
{
	private Skull_Bash skullBash;
	private PokemonComponents components;
	private Collider col;

	void Awake()
	{
		skullBash = transform.root.GetComponent<Skull_Bash>();
		components = transform.root.GetComponent<PokemonComponents>();
		col = GetComponent<Collider>();
	}
	void OnTriggerEnter(Collider collider)
	{
		if(!components.pokemon.trainer.isServer || !col.enabled)
			return;

		if(collider.gameObject == components.pokemon.enemy)
			Calculations.DealDamage(components.pokemon, collider.transform.root.GetComponent<Pokemon>(), skullBash);
	}
}
