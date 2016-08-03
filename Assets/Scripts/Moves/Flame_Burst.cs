using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Flame_Burst : Move
{
	public GameObject prefab;
	public Transform mouth;
	public float speed = 5.0f, lifetime = 5.0f;

	private Vector3 target;

	public void Reset()
	{
		ResetMoveData("Flame Burst", "The user attacks the target with a bursting flame. The bursting flame damages Pokémon near the target as well.", false, false, false, false,
			false, PokemonType.FIRE, MoveCategory.SPECIAL, 0.0f, 70);
	}
	public void FlameBurstStart()
	{
		ResetMoveValues();

		Ray ray = new Ray(mouth.position, mouth.forward);
		RaycastHit[] Hits;

		Hits = Physics.RaycastAll(ray, Mathf.Infinity);
		for(int i = 0; i < Hits.Length; i++)
		{
			if(Hits[i].transform.gameObject != gameObject)
			{
				target = Hits[i].point;
				break;
			}
		}

		GameObject flameBurst = Instantiate(prefab, mouth.position, mouth.rotation) as GameObject;
		flameBurst.GetComponent<SFX_Flame_Burst>().owner = components.pokemon;
		Rigidbody rBody = flameBurst.GetComponent<Rigidbody>();
		Vector3 dir = target - mouth.position;
		rBody.velocity = dir * speed;
		Destroy(flameBurst, lifetime);
	}
}