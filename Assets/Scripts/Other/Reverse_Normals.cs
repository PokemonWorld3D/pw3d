using UnityEngine;
using System.Collections;

//public class Reverse_Normals : MonoBehaviour
//{
//	private MeshCollider meshCollider;
//	private Mesh mesh;
//
//	void Start()
//	{
//		meshCollider = GetComponent<MeshCollider>();
//		mesh = meshCollider.sharedMesh;
//
//		Vector3[] normals = mesh.normals;
//
//		for(int i = 0; i < normals.Length; i++)
//			normals[i] = -normals[i];
//
//		mesh.normals = normals;
//		
//		for(int m = 0; m < mesh.subMeshCount; m++)
//		{
//			int[] triangles = mesh.GetTriangles(m);
//
//			for(int i = 0; i < triangles.Length; i += 3)
//			{
//				int temp = triangles[i + 0];
//				triangles[i + 0] = triangles[i + 1];
//				triangles[i + 1] = temp;
//			}
//
//			mesh.SetTriangles(triangles, m);
//		}
//
//		meshCollider.sharedMesh = mesh;
//	}
//}

//[RequireComponent(typeof(MeshFilter))]
//public class Reverse_Normals : MonoBehaviour
//{
//	private MeshFilter filter;
//	private Mesh mesh;
//
//	void Start()
//	{
//		filter = GetComponent<MeshFilter>();
//		mesh = filter.mesh;
//			
//		var indices = mesh.triangles;
//		var triangleCount = indices.Length / 3;
//		for(var i = 0; i < triangleCount; i++)
//		{
//			var tmp = indices[i*3];
//			indices[i*3] = indices[i*3 + 1];
//			indices[i*3 + 1] = tmp;
//		}
//		mesh.triangles = indices;
//		// additionally flip the vertex normals to get the correct lighting
//		var normals = mesh.normals;
//		for(var n = 0; n < normals.Length; n++)
//		{
//			normals[n] = -normals[n];
//		}
//		mesh.normals = normals;
//	}
//}

[RequireComponent(typeof(MeshFilter))]
public class Reverse_Normals : MonoBehaviour
{
	private MeshFilter filter;
	private Mesh mesh;
	private MeshCollider meshCollider;
	private Vector3[] Normals;

	void Awake()
	{
		filter = GetComponent<MeshFilter>();
		mesh = filter.mesh;
		meshCollider = GetComponent<MeshCollider>(); 

		Normals = mesh.normals;

		for(int i = 0; i < Normals.Length; i++)
			Normals[i] = -Normals[i];

		mesh.normals = Normals;

		for(int m = 0; m < mesh.subMeshCount; m++)
		{
			int[] Triangles = mesh.GetTriangles(m);

			for(int t = 0; t < Triangles.Length; t+=3)
			{
				int temp = Triangles[t+0];
				Triangles[t + 0] = Triangles[t + 1];
				Triangles[t + 1] = temp;
			}

			mesh.SetTriangles(Triangles, m);
		}

		meshCollider.sharedMesh = filter.mesh;
	}
}