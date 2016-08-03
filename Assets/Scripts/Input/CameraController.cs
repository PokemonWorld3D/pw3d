using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Transform target;							//Target to follow
//	public float targetHeight = 1.7f;					//Vertical offset adjustment
	public float distance = 1.0f;						//Default distance
	public float offsetFromWall = 0.1f;				// Bring camera away from any colliding objects
	public float maxDistance = 20.0f;					// Maximum zoom Distance
	public float minDistance = 0.6f;					// Minimum zoom Distance
	public float xSpeed = 200.0f;						// Orbit speed (Left/Right)
	public float ySpeed = 200.0f;						// Orbit speed (Up/Down)
	public float xMinLimit = -360.0f;
	public float xMaxLimit = 360.0f;
	public float curXMin = 0.0f;
	public float curXMax = 360.0f;
	public float yMinLimit = -80.0f;					// Looking up limit
	public float yMaxLimit = 80.0f;					// Looking down limit
	public float curYMin = -80.0f;
	public float curYMax = 80.0f;
	public float zoomRate = 40.0f;						// Zoom Speed
	public float rotationDampening = 3.0f;				// Auto Rotation speed (higher = faster)
	public float zoomDampening = 5.0f;					// Auto Zoom speed (Higher = faster)
	public LayerMask collisionLayers = -1;				// What the camera will collide with
	
	private float xDeg = 0.0f, yDeg = 0.0f, currentDistance, desiredDistance, correctedDistance;
	public bool targetIsPokemon = false;
	public PokemonComponents targetsComponents;


	void Start()
	{
		Vector3 angles = transform.eulerAngles;
		xDeg = angles.x;
		yDeg = angles.y;
		currentDistance = distance;
		desiredDistance = distance;
		correctedDistance = distance;
	}

	void FixedUpdate()
	{
		if(target)
		{
			xDeg += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;
			yDeg -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;
		
			xDeg = Physics_Calculations.ClampAngle(xDeg, curXMin, curXMax);
			yDeg = Physics_Calculations.ClampAngle(yDeg, curYMin, curYMax);

			// Set camera rotation
			Quaternion rotation = Quaternion.Euler(yDeg, xDeg, 0.0f);
			
			// Calculate the desired distance
			desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
			desiredDistance = Mathf.Clamp (desiredDistance, minDistance, maxDistance);
			correctedDistance = desiredDistance;
			
			// Calculate desired camera position
			Vector3 position = target.position - (rotation * Vector3.forward * desiredDistance);
			
			// Check for collision using the true target's desired registration point as set by user using height
			RaycastHit collisionHit;
			Vector3 trueTargetPosition = new Vector3(target.position.x, target.position.y, target.position.z);
			
			// If there was a collision, correct the camera position and calculate the corrected distance
			bool isCorrected = false;
			if (Physics.Linecast(trueTargetPosition, position, out collisionHit, collisionLayers))
			{
				// Calculate the distance from the original estimated position to the collision location,
				// subtracting out a safety "offset" distance from the object we hit.  The offset will help
				// keep the camera from being right on top of the surface we hit, which usually shows up as
				// the surface geometry getting partially clipped by the camera's front clipping plane.
				correctedDistance = Vector3.Distance(trueTargetPosition, collisionHit.point) - offsetFromWall;
				isCorrected = true;
			}
			
			// For smoothing, lerp distance only if either distance wasn't corrected, or correctedDistance is more than currentDistance
			currentDistance = !isCorrected || correctedDistance > currentDistance ? Mathf.Lerp(currentDistance, correctedDistance, Time.deltaTime * zoomDampening) : correctedDistance;
			// Keep within limits
			currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
			
			// Recalculate position based on the new currentDistance
			position = target.position - (rotation * Vector3.forward * currentDistance);
			
			//Finally Set rotation and position of camera
			transform.rotation = rotation;
			transform.position = position;
		}
	}

	public void SetTrainerTarget(Transform target)
	{
		targetIsPokemon = false;
		this.target = target;
	}
	public void SetPokemonTarget(PokemonComponents components)
	{
		this.target = components.cameraFocus;
		targetIsPokemon = true;
		targetsComponents = components;
	}
	public void RotateBehindTarget()
	{
		xDeg = target.eulerAngles.y;
	}
}