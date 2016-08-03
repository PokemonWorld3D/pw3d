using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour
{
	public float maxZ = 8.0f;
	public int noOfSegments = 12;
	public float posRange = 0.15f;
	public float radius = 1.0f;
	
	private Vector2 midPoint;
	private Color color = Color.white;
	private LineRenderer lineRenderer;
	
	//	void Start()
	//	{
	//		lineRenderer = GetComponent<LineRenderer>();
	//		lineRenderer.SetVertexCount(noOfSegments);
	//		for(int i = 1; i < noOfSegments - 1; i++)
	//		{
	//			float z = ((float)i * (maxZ) / (float)(noOfSegments - 1));
	//			lineRenderer.SetPosition(i, new Vector3(Random.Range(-posRange, posRange), Random.Range(-posRange, posRange), z));
	//		}
	//		lineRenderer.SetPosition(0, new Vector3(0.0f, 0.0f, 0.0f));
	//		lineRenderer.SetPosition(noOfSegments - 1, new Vector3(0.0f, 0.0f, maxZ));
	//	}
	void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.SetVertexCount(noOfSegments);
		midPoint = new Vector2(Random.Range(-radius, radius), Random.Range(-radius, radius));
		for(int i = 1; i < noOfSegments; i++)
		{
			float z = ((float)i * (maxZ) / (float)(noOfSegments - 1));
			float x = -midPoint.x * z * z / 16.0f + z * midPoint.x / 2.0f;
			float y = -midPoint.y * z * z / 16.0f + z * midPoint.y / 2.0f;
			lineRenderer.SetPosition(i, new Vector3(x + Random.Range(-posRange, posRange), y + Random.Range(-posRange, posRange), z));
		}
		lineRenderer.SetPosition(0, new Vector3(0.0f, 0.0f, 0.0f));
		lineRenderer.SetPosition(noOfSegments - 1, new Vector3(0.0f, 0.0f, maxZ));
	}
	void Update()
	{
		color.a -= 5.0f * Time.deltaTime;
		lineRenderer.SetColors(color, color);
		if(color.a <= 0.0f)
			Destroy(gameObject);
	}
}
