using UnityEngine;
using System.Collections;

public class Look_At : MonoBehaviour
{
	public Transform position;
	public Transform target;

	void Update()
	{
		transform.position = position.position;
		transform.LookAt(target);
	}
}
