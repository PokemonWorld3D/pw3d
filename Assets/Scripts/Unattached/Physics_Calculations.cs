using UnityEngine;
using System.Collections;

public static class Physics_Calculations
{
//	public static bool AcquiringGround(SuperCharacterController controller)
//	{
//		return controller.currentGround.IsGrounded(false, 0.01f);
//	}
//	public static bool MaintainingGround(SuperCharacterController controller)
//	{
//		return controller.currentGround.IsGrounded(true, 0.5f);
//	}
//
	public static float ClampAngle(float angle, float min, float max)
	{
		if(angle < -360.0f)
			angle += 360.0f;

		if(angle > 360.0f)
			angle -= 360.0f;

		return Mathf.Clamp(angle, min, max);
	}

	public static float CalculateJumpVerticalSpeed(float jumpHeight, float gravity)
	{
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}
	
	public static float AngleAroundAxis(Vector3 directionA, Vector3 directionB, Vector3 axis)
	{
		// Project A and B onto the plane orthogonal target axis
		directionA = directionA - Vector3.Project (directionA, axis);
		directionB = directionB - Vector3.Project (directionB, axis);
		
		// Find (positive) angle between A and B
		float angle = Vector3.Angle (directionA, directionB);
		
		// Return angle multiplied with 1 or -1
		return angle * (Vector3.Dot (axis, Vector3.Cross (directionA, directionB)) < 0 ? -1 : 1);
	}

	public static Vector3 CalculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
	{
		// calculate vectors
		Vector3 toTarget = target - origin;
		Vector3 toTargetXZ = toTarget;
		toTargetXZ.y = 0;
		
		// calculate xz and y
		float y = toTarget.y;
		float xz = toTargetXZ.magnitude;
		
		// calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
		// where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
		// so xz = v0xz * t => v0xz = xz / t
		// and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
		float t = timeToTarget;
		float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
		float v0xz = xz / t;
		
		// create result vector for calculated starting speeds
		Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
		result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
		result.y = v0y;                                // set y to v0y (starting speed of y plane)
		
		return result;
	}
}
