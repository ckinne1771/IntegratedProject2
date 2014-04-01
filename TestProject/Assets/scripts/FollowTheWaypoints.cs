using UnityEngine;
using System.Collections;

public class FollowTheWaypoints : MonoBehaviour 
{
	public float movementSpeed = 5f;
	private int targetWaypoint = 0;
	private Transform waypoints;
	public CustomerNeedsScript customerneedsscript;

	public enum State
	{
		Enter,
		Serving,
		Exit
	}

	private State state;
	
	// Use this for initialization
	void Start () 
	{
		waypoints = GameObject.Find ("Waypoints").transform;
		customerneedsscript = GetComponent<CustomerNeedsScript> ();
	}
	
	// Fixed update
	void FixedUpdate()
	{
		if (targetWaypoint == 0) 
		{
			moveToWaypoints();
		}
		if (targetWaypoint == 1) 
		{
			StartCoroutine(WaitFirst());
		}
		if (targetWaypoint == 2)
		{
			StartCoroutine(WaitSecond());
		}
	}

	void Update()
	{

	}

	// Handle walking the waypoints
	private void moveToWaypoints()
	{
		Transform _targetWaypoint = waypoints.GetChild(targetWaypoint);
		Vector3 relative = _targetWaypoint.position - transform.position;
		Vector3 movementNormal = Vector3.Normalize(relative);
		float distanceToWaypoint = relative.magnitude;
		rigidbody2D.isKinematic = false;

		if (distanceToWaypoint < 0.1)
		{
			if (targetWaypoint + 1 < waypoints.childCount)
			{
				targetWaypoint++; // Set new waypoint as target
			}
			else
			{
				customerneedsscript.itemNeeded=true;
			}
			rigidbody2D.isKinematic = true;
		}
		else
		{
			// Walk towards waypoint
			rigidbody2D.AddForce(new Vector2(movementNormal.x, movementNormal.y) * (Time.deltaTime + 15));
		}
	}

	IEnumerator WaitFirst() 
	{
		yield return new WaitForSeconds(5);
		moveToWaypoints();
	}

	IEnumerator WaitSecond() 
	{
		yield return new WaitForSeconds(15);
		moveToWaypoints();
	}
}

//state = State.Enter;
//switch (state) 
//{
//case State.Enter:
//	break;
//}