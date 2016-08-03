using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PartiallyTrapped : MonoBehaviour
{
	[SerializeField] private Transform parent;
	[SerializeField] private Vector3 defaultPos;
	[SerializeField] private float duration;
	[SerializeField] private bool causeDamage;
	[SerializeField] private int damageOverTime = 1;
	[SerializeField] private PokemonComponents components;

	private float timer;

	void OnCollisionStay(Collision col)
	{
		if(!parent.root.GetComponent<NetworkIdentity>().isServer)
			return;

		if(!causeDamage)
			return;

		if(col.gameObject.CompareTag("Pokemon"))
		{
			PokemonComponents components = col.gameObject.GetComponent<PokemonComponents>();

			if(components.pokemon.isInBattle)
				components.hpPP.AdjCurHP(-damageOverTime, false);
		}
	}
	void Update()
	{
		timer += Time.deltaTime;

		if(timer >= duration)
		{
			if(parent.root.GetComponent<NetworkIdentity>().isServer)
				components.conditions.partiallyTrapped = false;
			
			transform.root.SetParent(parent);
			transform.root.localPosition = defaultPos;
			timer = 0.0f;
			transform.parent.gameObject.SetActive(false);
		}
	}
}
