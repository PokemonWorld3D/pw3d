//There is an inexpensive asset in the Unity Asset Store called Transparency Capture
//which can be used as an alternative image capture tool. You may prefer the way it
//creates an alpha channel. To use it, download it from the store into the project
//directory and then uncomment the line of this script that defines the
//TRANCEPARENCY_CAPTURE value. If you write your own alpha generation and/or color
//adjustment then use this technique of replacing a subroutine call as opposed to
//modifing the code in this file directly. This will allow you to get updates for this
//code without completely wiping out your modifications.

//#define TRANCEPARENCY_CAPTURE

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif 

/// <summary>
/// Stand-in geometry generator component. This must be attached to an object in the scene.
/// Read the PVC_StandInGeo.pdf file for details.
///
/// by Paul "PVC" Van Camp
/// paulvancamp.com
/// </summary>
[AddComponentMenu("Mesh/PVC_Billboards")]
public class PVC_Billboards: MonoBehaviour {

	public InteractionModeTypes InteractionMode;	//Determine mode for interaction with player
	public string AssetsSubdir= "StandInGeo_Prefabs";	//Subdirectory use for storing generated objects
	public string BaseName= "StandInGeo";				//Basename used for created game object and files
	public int VersionNum= 1;							//Version appended to basename with three digit padding
	public bool IncrVersion= false;						//If true then the version is incremented after each operation
	public float FarClippingDist= 0;				//If > 0 then this replaces the camera's far clipping plane
	public float NearClippingDist= 6;				//If > 0 then this replaces the camera's near clipping plane
	public Material GeoMaterial;					//Template material used to identify the desired shader for results
	
	[HideInInspector]
	public String ErrorMsg;
	[HideInInspector]
	public bool MadeItFlag= false;					//Tells UI that the operation is complete
	[HideInInspector]
	public Plane[] ClippingPlanes;					//Saved clipping planes used to determin what objects are blocked

	public enum InteractionModeTypes {INTERACTION_NONE, DIALOG, DIALOG_AND_KEYS};
	
	protected Camera _camera;
	protected String _classNm;
	protected GameObject _geoObj;
	protected MeshFilter _geoMeshFilter;
	protected int _geoType;
	protected String _makeNearClippingStr, _makeFarClippingStr;
	protected Texture2D _imgCapturedTex;
	protected int _triangleCount;	//Number of triangles created in the last make
	protected bool _windowBrief;
	protected Rect _windowRect;
	
	/// <summary>
	/// Changes the near and far clipping values. They must be changed through this
	/// method when using the GUI since the interface uses a text widget.
	/// </summary>
	/// <param name="near">Near Clipping Plane</param>
	/// <param name="far">Far Clipping Plane</param>
	public void ChangeClipping( float near, float far ) {
		NearClippingDist= near;
		FarClippingDist= far;
		_makeNearClippingStr= NearClippingDist.ToString();
		_makeFarClippingStr= FarClippingDist.ToString();
	}
	
	/// <summary>
	/// Returns the last generated geometry object. Test the variable MadeItFlag to see if the creation
	/// process has been completed.
	/// </summary>
	/// <value>The made geo.</value>
	public GameObject MadeGeometry {
		get { return _geoObj; }
	}

	/// <summary>
	/// Gets the number of triangles in the geometry
	/// </summary>
	public int MadeGeometryTriangles {
		get { return _triangleCount; }
	}
	
	/// <summary>
	/// Starts the geometry creation process. Public variable MadeItFlag is set <c>false<c/c>. This will
	/// be changed to <c>true</c> once the geometry is made.
	/// </summary>
	/// <returns><c>true</c>, if geometry making was started, <c>false</c> otherwise.</returns>
	public bool MakeGeometry() {
		if ( GeoMaterial ) {
			ErrorMsg= null;
			MadeItFlag= false;
			StartCoroutine( _MakeGeo() );
			return true;
		}

		Debug.LogError("PVC_"+_classNm+" requires a Material");
		return false;
	}
	
	/// <summary>
	/// Initialize this instance.
	/// </summary>
	protected virtual void Start() {
		if ( GetComponent<Camera>() ) _camera= GetComponent<Camera>();
		else _camera= Camera.main;
		
		_classNm=  "Billboards";
		ChangeClipping( NearClippingDist, FarClippingDist );
		_triangleCount= 0;
		_windowBrief= true;
		_windowRect= new Rect(5, 5, 0, 0);
	}
	
	/// <summary>
	/// Put up the dialog for generating stand-in geometry.
	/// </summary>
	protected void OnGUI () {
		if ( InteractionMode == InteractionModeTypes.DIALOG ||
			InteractionMode == InteractionModeTypes.DIALOG_AND_KEYS ) {
			if ( InteractionMode == InteractionModeTypes.DIALOG_AND_KEYS && _windowBrief ) {
				_windowRect.width= 400;
				_windowRect.height= 70;
				_windowRect= GUI.Window (0, _windowRect, GUI_WindowBrief, "PVC LOD "+_classNm);
			} else {
				_windowRect.width= 200;
				_windowRect.height= 250;
				_windowRect= GUI.Window (0, _windowRect, GUI_Window, "PVC LOD "+_classNm);
			}
		}
	}
	
	/// <summary>
	/// This is the dialog shown when telling the user to move the camera into position.
	/// </summary>
	/// <param name="windowID">Window ID</param>
	public virtual void GUI_WindowBrief( int windowID ) {
		
		GUI.DragWindow ( new Rect (0,0, 10000, 40));  //the window can be dragged
		
		//Make a background box
		int hrz= 5;
		int vrt= 20;
		string enbl= "";
		
		GameObject geo= MadeGeometry;
		if ( geo ) {
			enbl= "Press \"G\" To "+(geo.activeSelf ? "DISABLE" : "ENABLE")+" StandIn Geometry";
		}
		
		GUI.Label ( new Rect(hrz, vrt, 400, 80), 
		           "Press the ESCAPE KEY to switch to \"Create Geometry\" mode.\n"+
		           enbl
		           );
	}
	
	/// <summary>
	/// Put up a dialog with controls for interactively sets clipping value, selecting whether
	/// a grid or a plane is to be created and other parameters.
	/// </summary>
	/// <param name="windowID">Window Identifier passed by GUI.Window.</param>
	public virtual void GUI_Window( int windowID ) {

		GUI.DragWindow ( new Rect (0,0, 10000, 20));  //the window can be dragged
		String makeButtonLabel= "Make Billboard";
			
		//Make a background box
		int hrz= 5;
		int vrt= 20;
		
		GUI.Label ( new Rect(hrz, vrt, 90, 20), "Clipping Dist:" );
		_GUI_NumField( new Rect(hrz+90, vrt, 50, 20), 
		              ref NearClippingDist, ref _makeNearClippingStr );
		_GUI_NumField( new Rect(hrz+140, vrt, 50, 20), 
		              ref FarClippingDist, ref _makeFarClippingStr );
		vrt += 25;
		if( GUI.Button( new Rect(hrz+30, vrt,125, 20), makeButtonLabel) ) {
			if ( MakeGeometry () ) _windowBrief= true;
		}
		vrt += 25;
	}
	
	/// <summary>
	/// Used in the GUI for floating point value inputs.
	/// </summary>
	/// <returns><c>true</c>, if a new valid value was entered.</returns>
	/// <param name="place">Place within the window.</param>
	/// <param name="val">Current float val.</param>
	/// <param name="valStr">Current string representation of the value.</param>
	protected bool _GUI_NumField( Rect place, ref float val, ref String valStr ) {
		string valRet= GUI.TextField( place, valStr);
		if ( valStr != valRet ) {
			valStr= valRet;
			try {
				val= float.Parse ( valRet );
				return true;
			} 
			catch( Exception ) {
			}
		}
		return false;
	}
	
	/// <summary>
	/// Used in the GUI for integer value inputs.
	/// </summary>
	/// <returns><c>true</c>, if a new valid value was entered.</returns>
	/// <param name="place">Place within the window.</param>
	/// <param name="val">Current integer val.</param>
	protected bool _GUI_NumField( Rect place, ref int val ) {
		string valStr= "";
		if ( val >= 0 ) valStr= val.ToString();
		string valRet= GUI.TextField( place, valStr);
		if ( valStr != valRet ) {
			try {
				val= int.Parse ( valRet );
				return true;
			} 
			catch( Exception ) {
				if ( valRet.Trim().Length == 0 ) val= -1;
			}
		}
		return false;
	}
	
	/// <summary>
	/// Check for keyboard input and change the mode or move accordingly.
	/// </summary>
	protected void Update() {
		if ( InteractionMode == InteractionModeTypes.DIALOG_AND_KEYS ) {
			if ( Input.GetKeyDown(KeyCode.G) ) {
				//Toggle the geometry enabled state if it exists
				if ( _geoObj ) _geoObj.SetActive( ! _geoObj.activeSelf );
			}
			if ( Input.GetKeyDown(KeyCode.Escape) ) {
				//Toggle between full dialog and just a brief message
				_windowBrief= ! _windowBrief;
				if ( ! _windowBrief && _geoObj ) _geoObj.SetActive( false );
			}
		}
	}
	
	/// <summary>
	/// Use ReadPixels to capture an image from the main camera.
	/// </summary>
	/// <returns>The captured image.</returns>
	/// <param name="pRect">pRect is the rectange of the camera view which is to be captured.</param>
	Texture2D _captureView(Rect pRect) {
		Color oldBackgrd= _camera.backgroundColor;
		
		_camera.backgroundColor= Color.white;
		_camera.Render();
		Texture2D wOut = new Texture2D((int)pRect.width, (int)pRect.height, TextureFormat.ARGB32, false);
		wOut.ReadPixels(pRect, 0, 0, false);
		
		_camera.backgroundColor= Color.black;
		_camera.Render();
		Texture2D bOut = new Texture2D((int)pRect.width, (int)pRect.height, TextureFormat.ARGB32, false);
		bOut.ReadPixels(pRect, 0, 0, false);
		
		//Generate an alpha to make background transparent
		for(int iy = 0; iy < pRect.height; iy++) {
			for(int ix = 0; ix < pRect.width; ix++) {
				Color clrW= wOut.GetPixel(ix,iy);
				Color clrB = bOut.GetPixel(ix,iy);
				clrB.a= Color.white.r - (clrW.r-clrB.r);
				bOut.SetPixel(ix, iy, clrB);
			}
		}
		Destroy( wOut );
		bOut.Apply ();
		_camera.backgroundColor= oldBackgrd;
		return bOut;
	}
	
	/// <summary>
	/// Make the specified geometry and apply the screen capture image to it. This will
	/// save out to dist the image along with a material that references the image, a mesh
	/// and a prefab for all of the above.
	/// </summary>
	protected IEnumerator _MakeGeo() {
		//Create the stand-in object
		_triangleCount= 0;
		
		if ( _geoObj ) Destroy( _geoObj );
		_geoObj= new GameObject( BaseName );
		_geoObj.AddComponent<MeshFilter>();
		_geoMeshFilter= _geoObj.GetComponent<MeshFilter>();

		_geoObj.AddComponent<MeshRenderer>();
		_geoObj.GetComponent<Renderer>().material= GeoMaterial;
		_geoObj.AddComponent<PVC_StandInGeo_Fade>();
		PVC_StandInGeo_Fade fadeCmpt= _geoObj.GetComponent<PVC_StandInGeo_Fade>();
		fadeCmpt.enabled= false;
		
		if ( ! _camera ) Debug.LogError("PVC_"+_classNm+" has no camera.");
		else if ( ! _geoMeshFilter ) Debug.LogError("PVC_"+_classNm+" failed to create object with mesh filter");
		else {
			//save current clipping values for restoring at end
			float nearClipping= _camera.nearClipPlane;
			float farClipping= _camera.farClipPlane;
			
			if ( NearClippingDist > 0.001f ) _camera.nearClipPlane= NearClippingDist;
			if ( FarClippingDist > NearClippingDist ) _camera.farClipPlane= FarClippingDist;
			
			_geoMeshFilter.mesh.Clear ();

			Vector3 objectPosition= _camera.ScreenToWorldPoint(
					new Vector3( _camera.pixelWidth * 0.5f, _camera.pixelHeight * 0.5f, _camera.nearClipPlane ) );
			Mesh makeMesh = _MakeGeoOfType( objectPosition);
			_geoObj.transform.position= objectPosition;
			
			_geoObj.SetActive( false ); //disable to make sure mesh does not block the shot
			
			if ( makeMesh != null ) {
				_geoMeshFilter.mesh= makeMesh;
				Rect lRect = new Rect(0f,0f,Screen.width,Screen.height);
				if ( _imgCapturedTex )
					Destroy(_imgCapturedTex);
				
				yield return new WaitForEndOfFrame();
				
				//Here is where we grad the image
#if TRANCEPARENCY_CAPTURE
				_imgCapturedTex = zzTransparencyCapture.capture(lRect);
#else
				_imgCapturedTex = _captureView(lRect);
#endif
	
				_geoObj.SetActive( true );
				
#if UNITY_EDITOR
				// Create folders
				string assetSubdir= string.Format("{0}/{1}", "Assets", AssetsSubdir);
				Directory.CreateDirectory( assetSubdir );
				string assetResourcePath= string.Format("{0}/{1}", assetSubdir, "Resources" );
				Directory.CreateDirectory( assetResourcePath );
				
				AssetDatabase.Refresh();
				string assetBaseNm= string.Format ("{0}{1:000}", BaseName, VersionNum).Replace(" ","_");
				if ( IncrVersion ) VersionNum++;
				
				string assetBasePath= string.Format ("{0}/{1}", assetResourcePath, assetBaseNm);
				string assetImgPath= assetBasePath +".png";
				string assetMeshPath= assetBasePath + "Mesh.asset";
				string assetMatPath= assetBasePath + "Mat.mat";
				
				AssetDatabase.DeleteAsset( assetMeshPath );
				AssetDatabase.CreateAsset(_geoMeshFilter.mesh, assetMeshPath); //save mesh to disk
				
				AssetDatabase.DeleteAsset( assetMatPath );
				AssetDatabase.DeleteAsset( assetImgPath );			
	
				//Save out the image to disk
				using (var lFile = new FileStream(assetImgPath, FileMode.Create))
				{
					BinaryWriter lWriter = new BinaryWriter(lFile);
					lWriter.Write(_imgCapturedTex.EncodeToPNG());
				}
				AssetDatabase.Refresh();
				
				//Save a material to disk which uses the generated image
				_geoObj.GetComponent<Renderer>().material.SetTexture ("_MainTex", (Texture2D)Resources.Load(assetBaseNm, typeof(Texture2D)));
				AssetDatabase.CreateAsset( _geoObj.GetComponent<Renderer>().material, assetMatPath);
				AssetDatabase.Refresh();
				_geoObj.GetComponent<Renderer>().material= (Material)Resources.Load(assetBaseNm+"Mat", typeof(Material));
						
				AssetDatabase.SaveAssets();
				
				//Save a prefab to disk which ties together all of the above
				string prefabPath= string.Format ("{0}/{1}_Prefab.prefab", assetSubdir, assetBaseNm);
				
				var prefab = PrefabUtility.CreateEmptyPrefab( prefabPath );
				PrefabUtility.ReplacePrefab( _geoObj, prefab );
				
				AssetDatabase.Refresh();
#endif

				_geoObj.GetComponent<Renderer>().material.SetTexture ("_MainTex", _imgCapturedTex);
	
				//Store clipping planes that can be used to determin what geometry was blocked
				//by the standin geometry.
				_camera.farClipPlane= 10000;
				ClippingPlanes= GeometryUtility.CalculateFrustumPlanes( _camera );
				
			}
			
			//Restore the camera's clipping planes to their prior value
			_camera.nearClipPlane= nearClipping;
			_camera.farClipPlane= farClipping;
			
			//Mark process as completed even if it failed
			MadeItFlag= true;
		}
	}
	
	/// <summary>
	/// Create geometery of the type specified by class variable _geoType;
	/// </summary>
	/// <returns>The generated mesh.</returns>
	protected virtual Mesh _MakeGeoOfType( Vector3 objectPosition ) {
		return _MakeGeoPlane( objectPosition );
	}
	
	/// <summary>
	/// Create a simple quad mesh composed of two triangles that cover the camera's
	/// near clipping plane. This is used for billboarding frame captured images.
	/// </summary>
	/// <returns>The generated mesh.</returns>
	protected Mesh _MakeGeoPlane( Vector3 objectPosition ) {
		
		var mesh = new Mesh();
		float h= _camera.pixelHeight;
		float w= _camera.pixelWidth;
		
		var uv= new Vector2[4];		
		uv[0] = new Vector2(0, 0);
		uv[1] = new Vector2(1, 0);
		uv[2] = new Vector2(0, 1);
		uv[3] = new Vector2(1, 1);
		
		var vertices = new List<Vector3>();
		var normals = new List<Vector3>();
		
		for ( int ii=0; ii < 4; ii++ ) {
			Vector3 vp= new Vector3( uv[ii].x * w, uv[ii].y * h, _camera.nearClipPlane );
			vertices.Add ( _camera.ScreenToWorldPoint( vp ) - objectPosition );
			normals.Add ( -Vector3.forward );
		}
		
		mesh.vertices = vertices.ToArray();
		mesh.normals = normals.ToArray();
		mesh.uv = uv;
		
		var tri = new int[6];
		
		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;
		
		tri[3] = 2;
		tri[4] = 3;
		tri[5] = 1;
		
		mesh.triangles = tri;
		_triangleCount= 2;
		return mesh;
	}	
}
