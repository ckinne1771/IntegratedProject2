using UnityEngine;
using System.Collections;

public class FollowTheWaypoints : MonoBehaviour 
{
	//this script is used to move the customers in the shop

	//waypoint customer is travelling to
	public int targetWaypoint = 0;
	private Transform waypoints;
	//scripts referenced
	public CustomerNeedsScript customerneedsscript;
	public CustomerSpawnScript customerSpawnScript;
	//animator for customers
	public Animator anim;
	//where the customer is in queue
	public int pointInQueue=0;
	//if they are in motion or not
	public bool wait=false;


	//states used to find what the customer is doing for animation
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
	}

	void Update()
	{
		states();
		if(Application.loadedLevelName!="tutorialScene")
		{
		WaitFirst();
		}
	}

	//used to switch between states and control when the customer moves, where to and when they change states
	//also sets movement animations
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

	//method called when customer is in enter state
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
	//method called when customer is in serving state
	private void serving()
	{
		
		if(customerneedsscript.itemNeeded==false)
		{
			customerState = State.Exit;
		}
		
	}

	//method called when customer is in exit state
	private void exit()
	{
		wait=false;
		if(targetWaypoint < 3)
		{
			anim.SetTrigger("sidewalk");
			moveToWaypoints();
			targetWaypoint=3;
		}
		if(targetWaypoint == 3)
		{
			anim.SetTrigger("sidewalk");
			moveToWaypoints();
		}
		if(targetWaypoint ==4)
		{
			anim.SetTrigger("backwalk");
			moveToWaypoints();
		}
		if(targetWaypoint ==5)
		{
			moveToWaypoints();
			customerSpawnScript.RemoveCustomer(pointInQueue);
			customerSpawnScript.queuePointToDelete=pointInQueue;
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
		//handles some movement between states
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
		//sets whether the customer is using a moving animation or an emotion one
		if(targetWaypoint==2&&customerState==State.Waiting)
		{
			wait=true;
		}
		if(targetWaypoint==3&&customerState==State.Waiting)
		{
			wait=false;
		}

	}

	//sets customer to leave if times out
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
	
}