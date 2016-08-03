using UnityEngine;
using System.Collections;

public class Pokemon_Return : MonoBehaviour
{
	public GameObject target = null;
	public LineRenderer lineRenderer;
	
	private Vector3 spot;
	
	void Start()
	{
		lineRenderer = GetComponentInChildren<LineRenderer>();
	}
	void Update()
	{
		if(target != null)
		{
			spot = target.transform.position;
			spot.y = target.GetComponentInChildren<SkinnedMeshRenderer>().GetComponent<Renderer>().bounds.extents.y / 2.0f; 
			lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position);
			lineRenderer.SetPosition(1, spot);
		}
		if(target == null)
		{
			spot = Vector3.Lerp(spot, lineRenderer.gameObject.transform.position, Time.deltaTime * 5.0f);
			lineRenderer.SetPosition(0, lineRenderer.gameObject.transform.position);
			lineRenderer.SetPosition(1, spot);
		}
	}
}
