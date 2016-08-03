using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Water_Pulse : Move
{
	[SerializeField] private GameObject prefab;
	[SerializeField] private Transform source;
	[SerializeField] private float speed = 1.0f, lifetime = 5.0f, scaleSpeed = 0.5f;
	[SerializeField] private Vector3 scale;

	private GameObject waterPulse;
	private bool crit;
	private Rigidbody rBody;

	public void Reset()
	{
		ResetMoveData("Water Pulse", "The user attacks the target with a pulsing blast of water. This may also confuse the target.", false, false, false, false, false,
			PokemonType.WATER, MoveCategory.SPECIAL, 0.0f, 60);
	}
	public void StartWaterPulse()
	{
		waterPulse = Instantiate(prefab, source.position, source.rotation) as GameObject;
		waterPulse.transform.parent = source;
		waterPulse.GetComponent<SFX_Water_Pulse>().AssignValues(components.pokemon, scale, scaleSpeed);
	}
	public void FireWaterPulse()
	{
		waterPulse.transform.parent = null;
		rBody = waterPulse.GetComponent<Rigidbody>();
		rBody.velocity = source.forward * speed;
		Destroy(waterPulse, lifetime);
	}

}
