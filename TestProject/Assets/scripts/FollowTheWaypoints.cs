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
		Exit,
		Waiting,
		Serving
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
			exit();
			break;

		case State.Waiting:
			if(wait==true)
			{
				anim.SetTrigger("inQueue");
			}
			else
			{
				moveToWaypoints();
				if(targetWaypoint==1)
				{
				anim.SetTrigger("sidewalk");
				}
			}
			if(this.gameObject==customerSpawnScript.customers[0])
			{
				wait=false;
				customerState=State.Serving;
			}
			break;

		case State.Serving:
			if(targetWaypoint==0)
			{
				moveToWaypoints();
			}
			if(targetWaypoint==1)
			{
				anim.SetTrigger("sidewalk");
				moveToWaypoints();
			}
			else if(targetWaypoint==2)
			{
				moveToWaypoints();
				anim.SetTrigger("sidewalk");
			}
			else if(targetWaypoint==3)
			{
				wait=true;
				anim.SetTrigger("inQueue");
				serving ();
			}

			break;
		}

	}

	private void enter()
	{
		if (targetWaypoint == 0) 
		{
			moveToWaypoints();
		} 
		else if (targetWaypoint ==1)
		{
			moveToWaypoints();
			anim.SetTrigger("sidewalk");
		}
	}

	private void exit()
	{
		wait=false;
		if(targetWaypoint < 3)
		{
			anim.SetTrigger("sidewalk");
			moveToWaypoints();
			targetWaypoint=3;
			Debug.Log ("move");
		}
		if(targetWaypoint == 3)
		{
			anim.SetTrigger("sidewalk");
			moveToWaypoints();
			Debug.Log ("move");
		}
		if(targetWaypoint ==4)
		{
			anim.SetTrigger("backwalk");
			moveToWaypoints();
			Debug.Log ("bob");
		}
		if(targetWaypoint ==5)
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
		if(targetWaypoint==1&&customerState==State.Enter)
		{
			customerState=State.Waiting;
		}
		if(customerState==State.Enter||customerState==State.Waiting)
		{
			if(this.gameObject==customerSpawnScript.customers[0])
			{
				customerState=State.Serving;
			}
		
			else
			{
			customerState=State.Waiting;
			}
			}
		if(targetWaypoint==2&&customerState==State.Waiting)
		{
			wait=true;
		}
		if(targetWaypoint==3&&customerState==State.Waiting)
		{
			wait=false;
		}

	}

	private void WaitFirst() 
	{
		if(customerneedsscript.timer<1)
		{
			customerState=State.Exit;
		}
	}

	IEnumerator WaitSecond() 
	{
		wait=false;
		yield return new WaitForSeconds(0);
		exit();
	}


private void serving()
{
	
	if(customerneedsscript.itemNeeded==false)
		{
		customerState = State.Exit;
		}
	
}
}