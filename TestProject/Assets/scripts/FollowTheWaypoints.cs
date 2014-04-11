using UnityEngine;
using System.Collections;

public class FollowTheWaypoints : MonoBehaviour 
{
	public int targetWaypoint = 0;
	private Transform waypoints;
	public CustomerNeedsScript customerneedsscript;
	public CustomerSpawnScript customerSpawnScript;
	public Animator anim;
	public int pointInQueue=0;
	public bool wait=false;

	public enum State
	{
		Enter,
		Exit
	};

	public State customerState;

	
	// Use this for initialization
	void Start() 
	{
		anim=GetComponent<Animator>();
		customerState = State.Enter;
		waypoints = GameObject.Find ("Waypoints").transform;
		customerneedsscript = GetComponent<CustomerNeedsScript>();
		customerSpawnScript = Camera.main.gameObject.GetComponent<CustomerSpawnScript>();
		//customerSpawnScript = GetComponent<CustomerSpawnScript>();
	}

	void Update()
	{
		states();
		WaitFirst();
	}

	private void states()
	{
		switch(customerState) 
		{
		case State.Enter:
			enter();
			break;
	
		case State.Exit:
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
		else if (targetWaypoint == 1&&wait==false)
		{
			moveToWaypoints();
			anim.SetTrigger("sidewalk");
		}
	}

	private void exit()
	{
		wait=false;
		moveToWaypoints ();
		if(targetWaypoint == 2)
		{
			moveToWaypoints();
			Debug.Log ("move");
			anim.SetTrigger("sidewalk");
		}
		else if(targetWaypoint ==3)
		{
			moveToWaypoints();
			anim.SetTrigger("backwalk");
		}
		else
		{
			moveToWaypoints();
			customerSpawnScript.RemoveCustomer(pointInQueue);
			customerSpawnScript.queuePointToDelete=pointInQueue;
			Debug.Log ("kill");
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
		if(targetWaypoint==2&&customerState==State.Enter)
		{
			wait=true;
		}

	}

	private void WaitFirst() 
	{
		if(customerneedsscript.timer<1)
		{
			exit();
		}
	}

	IEnumerator WaitSecond() 
	{
		yield return new WaitForSeconds(0);
		exit();
	}
}

//private void serving()
//{
//	if (targetWaypoint == 2)
//	{
//		moveToWaypoints();
//	}
//	else 
//	{
//		customerState = State.Exit;
//	}
//}