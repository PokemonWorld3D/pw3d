using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SFX_Water_Pulse : NetworkBehaviour
{
	private float scaleSpeed = 0.5f;
	private Pokemon owner;
	private Vector3 scale;

	void Update()
	{
		if(scale != Vector3.zero)
		{
			float current = transform.localScale.sqrMagnitude;
			float target = scale.sqrMagnitude;

			if(current < target)
				transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * scaleSpeed);
		}
	}
	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.root.gameObject == owner.gameObject)
			return;

		if(collider.transform.root.gameObject == owner.enemy.gameObject && isServer)
			Calculations.DealDamage(owner, owner.enemy.GetComponent<Pokemon>(), owner.GetComponent<Water_Pulse>());

		//-------------Instantiate the explosion here.---------------------------------//
		Destroy(gameObject);
	}
	public void AssignValues(Pokemon owner, Vector3 scale, float scaleSpeed)
	{
		this.owner = owner;
		this.scale = scale; 
		this.scaleSpeed = scaleSpeed;
	}
}
