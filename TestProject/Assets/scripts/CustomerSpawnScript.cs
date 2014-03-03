using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class CustomerSpawnScript : MonoBehaviour {

	GameObject[] customers;
	public Transform[] targets;
	public int NoOfCustomers = 3;
	public GameObject customerTemplate;

	// Use this for initialization
	void Start () 
	{
		customers = new GameObject[NoOfCustomers];
		AddCustomerToList();
		AddCustomerToList();
		AddCustomerToList();
	}
	
	// Update is called once per frame
	void Update () 
	{
		              

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
					customers[i].transform.position = targets[i].position;
					added = true;
					Debug.Log("Customer added");
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
					customers[i].transform.position = targets[i].position;
				}
				customers[i+1] = null;
			}
		}

		return true;
	}
}
