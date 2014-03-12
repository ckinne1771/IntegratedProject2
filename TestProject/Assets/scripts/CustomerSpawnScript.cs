using UnityEngine;
using System.Collections;

public class CustomerSpawnScript : MonoBehaviour {
	
	public int NoOfCustomers = 3;
	public GameObject customerTemplate;
	private Transform targets;
	private int initialTarget = 0;
	
	GameObject[] customers;
	
	// Use this for initialization
	void Start () 
	{
		targets = GameObject.Find("Targets").transform;
		customers = new GameObject[NoOfCustomers];
		AddCustomerToList();
		AddCustomerToList();
		AddCustomerToList();
		GetFrontOfQueueOrder().itemNeeded = true;
	}
	
	public bool AddCustomerToList()
	{
		bool added = false;
		if(customers[NoOfCustomers -1] == null)
		{
			GameObject newCustomer = Instantiate(customerTemplate) as GameObject;
			
			for(int i = 0; i < NoOfCustomers; i++)
			{
				if(customers[i] == null && !added)
				{
					customers[i] = newCustomer;
					Transform targetWaypoint = targets.GetChild(initialTarget);
					customers[i].transform.position = targetWaypoint.transform.position;
					added = true;
					Debug.Log("Customer added");
					
					if (initialTarget + 1 < targets.childCount)
					{
						// Set next target as spawnpoint
						initialTarget++;
					}
					
				}
			}
		}
		return added;
	}
	
	public bool IsQueueEmpty()
	{
		if(customers[0] == null)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	
	public CustomerNeedsScript GetFrontOfQueueOrder()
	{
		return customers[0].GetComponent<CustomerNeedsScript>();
	}
	
	public bool RemoveCustomer(int pos)
	{
		if(customers[pos] != null)
		{
			GameObject delCustomer = customers[pos];
			customers[pos] = null;
			Destroy(delCustomer);
		}
		else
		{
			return false;
		}
		
		for(int i = pos; i < NoOfCustomers - pos; i++)
		{
			if(i + 1 < NoOfCustomers)
			{
				customers[i] = customers[i+1];
				if(customers[i] != null)
				{
					//customers[i].transform.position = targets[i].position;
				}
				customers[i+1] = null;
			}
		}
		if(!IsQueueEmpty())
		{
			GetFrontOfQueueOrder().itemNeeded = true;
		}
		return true;
	}
}
