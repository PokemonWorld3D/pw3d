using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SFX_Flame_Burst : NetworkBehaviour
{
	public Pokemon owner;

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.root.gameObject == owner.gameObject)
			return;

		if(collider.transform.root.gameObject == owner.enemy.gameObject)
			Hit(collider.GetComponent<Pokemon>());

		//-------------Instantiate the explosion here.---------------------------------//
		Destroy(gameObject);
	}

	[Server] private void Hit(Pokemon target)
	{
		Calculations.DealDamage(owner, owner.enemy.GetComponent<Pokemon>(), owner.GetComponent<Flame_Burst>());
	}
}