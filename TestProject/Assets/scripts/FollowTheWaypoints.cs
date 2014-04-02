using UnityEngine;
using System.Collections;

public class FollowTheWaypoints : MonoBehaviour 
{
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
	void Start() 
	{
		state = State.Enter;
		waypoints = GameObject.Find ("Waypoints").transform;
		customerneedsscript = GetComponent<CustomerNeedsScript> ();
	}

	void Update()
	{
		states();
	}

	private void states()
	{
		switch(state) 
		{
		case State.Enter:
			//Debug.Log (state);
			enter();
			break;
				
		case State.Serving:
			StartCoroutine(WaitFirst());
			//Debug.Log(state);
			break;
				
		case State.Exit:
			//Debug.Log(state);
			StartCoroutine(WaitSecond());
			break;
		}
	}

	private void enter()
	{
		if (targetWaypoint == 0) 
		{
			moveToWaypoints();	
		} 
		else 
		{
			state = State.Serving;
		}
	}

	private void serving()
	{
		if (targetWaypoint == 1)
		{
			moveToWaypoints();
		}
		else 
		{
			state = State.Exit;
		}
	}

	private void exit()
	{
		if (targetWaypoint == 2)
		{
			moveToWaypoints();
		}
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
		yield return new WaitForSeconds(30);
		serving();
	}

	IEnumerator WaitSecond() 
	{
		yield return new WaitForSeconds(1);
		exit();
	}
}