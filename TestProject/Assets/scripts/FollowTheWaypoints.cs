using UnityEngine;

public class FollowTheWaypoints : MonoBehaviour 
{
	public float movementSpeed = 3f;
	private int targetWaypoint = 0;
	private Transform waypoints;

	
	// Use this for initialization
	void Start () 
	{
		waypoints = GameObject.Find("Waypoints").transform;
	}
	
	// Fixed update
	void FixedUpdate()
	{
		moveToWaypoints();
	}
	
	// Handle walking the waypoints
	private void moveToWaypoints()
	{
		Transform _targetWaypoint = waypoints.GetChild(targetWaypoint);
		Vector3 relative = _targetWaypoint.position - transform.position;
		Vector3 movementNormal = Vector3.Normalize(relative);
		float distanceToWaypoint = relative.magnitude;
		
		if (distanceToWaypoint < 0.1)
		{
			if (targetWaypoint + 1 < waypoints.childCount)
			{
				// Set new waypoint as target
				targetWaypoint++;
			}
		}
		else
		{
			// Walk towards waypoint
			rigidbody2D.AddForce(new Vector2(movementNormal.x, movementNormal.y) * movementSpeed);
		}
	}
}