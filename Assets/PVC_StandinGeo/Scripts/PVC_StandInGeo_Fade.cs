using UnityEngine;
using System.Collections;

[AddComponentMenu("LOD/PVC_StandInGeo_Fade")]
public class PVC_StandInGeo_Fade : MonoBehaviour {
	public bool Invert= false;
	public float FadeMinDist= 100;
	public float FadeDist= 10;

	void Update () {
		//As the player approaches the stand-in geometry this will fade it on
		if ( Camera.main && GetComponent<Renderer>() && GetComponent<Renderer>().material ) {
			float distSqr= (Camera.main.transform.position - transform.position).sqrMagnitude;
			float maxDist= FadeMinDist + Mathf.Max (0, FadeDist);
			float fade= Mathf.InverseLerp( FadeMinDist * FadeMinDist, maxDist * maxDist, distSqr );
			if ( ! Invert ) fade= 1- fade;
			GetComponent<Renderer>().material.SetFloat("_Fade", fade);
		}
	}
}
