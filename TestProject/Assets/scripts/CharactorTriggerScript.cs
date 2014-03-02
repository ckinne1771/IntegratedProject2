using UnityEngine;
using System.Collections;

public class CharactorTriggerScript : MonoBehaviour {

	public CustomerAIScript theCustomerAIScript;

	public GameObject TheSpawnPoint;
	public GameObject TriggerCustomer;

	// Use this for initialization
	void Start () {
		TheSpawnPoint = GameObject.Find("SpawnPoint");
		TriggerCustomer = CustomerAIScript.thisCustomer;
		theCustomerAIScript =TriggerCustomer.GetComponent<CustomerAIScript>();
		transform.position = TheSpawnPoint.transform.position;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter (Collider other){
		Debug.Log("Collision");
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, 0.5f);
		Debug.Log(hitColliders[1]); 

		if(other.gameObject.collider == hitColliders[1])
		{
			Debug.Log("Ignoring");
			Physics.IgnoreCollision (this.gameObject.collider, hitColliders[1]);
		}

		else if(other.gameObject.collider!= hitColliders[1] )
		{
			if(other.gameObject.tag=="Customer")
			{
		
				theCustomerAIScript.speed = 0f;
				Debug.Log ("Speed altered");
			}
		}
 
	} 
	   
} 
    