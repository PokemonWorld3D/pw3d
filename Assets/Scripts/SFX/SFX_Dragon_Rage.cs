using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SFX_Dragon_Rage : NetworkBehaviour
{
	[SerializeField] private float scaleSpeed = 0.5f;

	private Pokemon owner;
	private Vector3 scale;

	void OnTriggerEnter(Collider collider)
	{
		if(collider.transform.root.gameObject == owner.gameObject)
			return;

		if(collider.gameObject == owner.enemy.gameObject)
			Hit(collider.GetComponent<Pokemon>());

		//-------------Instantiate the explosion here.---------------------------------//
		Destroy(gameObject);
	}
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

	public void AssignValues(Pokemon owner, Vector3 scale)
	{
		this.owner = owner;
		this.scale = scale; 
	}

	[Server] private void Hit(Pokemon target)
	{
		target.components.hpPP.AdjCurHP(-40, false);

		NetworkServer.SendToAll((short)Messages.MessageTypes.BATTLE_MESSAGE,
			Messages.DamageMessage(owner, target, owner.GetComponent<Dragon_Rage>(), 40, false));
	}
}
