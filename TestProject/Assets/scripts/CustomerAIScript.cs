using UnityEngine;
using System.Collections;
using Pathfinding;

public class CustomerAIScript : MonoBehaviour 
{
	public Transform target; // The target
	public Vector3 targetPosition; // The targets position
	public  Seeker seeker; // The seeker component
	public float nextWaypointDistance = 0; 	// The max distance from the AI to a waypoint for it to continue to the next waypoint
	public CustomerNeedsScript theCustomerNeedsScript; // The Customer Needs Script
	public Path path; // The calculated path
	[System.NonSerialized]
	public float speed = 2; // The AI's speed per second
	public static GameObject thisCustomer; // Customer Game Object
	public enum State{Enter, Idle, Serving, Exit}; // Enum of states

	private CharacterController controller; // The character controller
	private State state; // The current state the customer is in
	private int currentWaypoint = 0; //The waypoint the enemies are currently moving towards

	IEnumerator Start() 
	{
		thisCustomer = this.gameObject;
		state = State.Enter; // Sets the current state to enter
		while (true) 
		{
		switch(state) 
			{
				case State.Enter:
				Enter();
				break;

				case State.Serving:
				Serving();
				break;

				case State.Exit:
				Exit();
				break;

				case State.Idle:
				Idle();
				break;
			}
			yield return 0;
		}
	}

	// Enter State
	private void Enter()
	{
		theCustomerNeedsScript = GetComponent<CustomerNeedsScript> (); // Get the customer needs script
		targetPosition = target.transform.position; // Set the target position
		seeker = GetComponent<Seeker> (); // Get the seeker component
		controller = GetComponent<CharacterController> (); // Get the character controller
		seeker.StartPath (transform.position, targetPosition, OnPathComplete); // Start a new path to the targetPosition, return the result to the OnPathComplete function
		state = State.Serving; // Change the state to serving state
	}

	// Serving state
	private void Serving()
	{
	}

	// Exit State
	private void Exit()
	{
	}

	// Idle state
	private void Idle()
	{
	}

	// When the path has been completed...
	private void OnPathComplete(Path p) 
	{
		// Error report
		if (!p.error) 
		{
			path = p;
			currentWaypoint = 1; //Reset the waypoint counter
		}
	}
	
	private void FixedUpdate() 
	{
		// No path to move after yet
		if (path == null) 
		{
			return;
		}

		// Write to the Debug window when the path has been completed
		if (currentWaypoint >= path.vectorPath.Count) 
		{
			Debug.Log ("End Of Path Reached");
			return;
		}
		
		//Check if customers are close enough to the next waypoint, if customers are then proceed to follow the next waypoint
		if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) 
		{
			currentWaypoint++;
			return;
		}

		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized; // Direction to the next waypoint
		dir *= speed * Time.fixedDeltaTime; 
		controller.Move(dir);
	}
}