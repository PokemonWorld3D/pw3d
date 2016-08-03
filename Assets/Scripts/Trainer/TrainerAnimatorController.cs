using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TrainerAnimatorController : NetworkBehaviour
{
	private TrainerComponents components;
	public float speed;
	public Vector3 velocity;

	void Awake()
	{
		components = GetComponent<TrainerComponents>();
	}
	void Update()
	{
		if(!isServer)
			return;

//		Debug.Log(components.controller.velocity);
//		Debug.Log("Velocity Normalized = " + components.controller.velocity.normalized);
//		Debug.Log("Velocity Magnitude = " + components.controller.velocity.magnitude);
//		Debug.Log("Velocity Normalized Magnitude = " + components.controller.velocity.normalized.magnitude);
		Debug.Log("Velocity Normalized SqrMagnitude = " + components.controller.velocity.normalized.sqrMagnitude);

		velocity = Vector3.Lerp(velocity, transform.InverseTransformDirection(components.controller.velocity), 5.0f * Time.deltaTime);
		velocity.y = 0.0f;
		speed = velocity.magnitude;

		if(speed > 0.1f && speed < 1.0f)
			speed = 1.0f;
		else if(speed > 1.0f)
			speed = 2.0f;
		

		components.anim.SetFloat("Speed", speed);
	}
}
