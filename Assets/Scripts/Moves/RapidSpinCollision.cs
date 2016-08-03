using UnityEngine;
using System.Collections;

public class RapidSpinCollision : MonoBehaviour
{
	private Rapid_Spin rapidSpin;
	private PokemonComponents components;
	private Collider col;

	void Awake()
	{
		rapidSpin = transform.root.GetComponent<Rapid_Spin>();
		components = transform.root.GetComponent<PokemonComponents>();
		col = GetComponent<Collider>();
	}
	void OnTriggerEnter(Collider collider)
	{
		if(!components.pokemon.trainer.isServer || !col.enabled)
			return;

		if(collider.gameObject == components.pokemon.enemy)
			Calculations.DealDamage(components.pokemon, collider.transform.root.GetComponent<Pokemon>(), rapidSpin);
	}
}
