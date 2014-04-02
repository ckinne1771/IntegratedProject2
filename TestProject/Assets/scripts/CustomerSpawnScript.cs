using UnityEngine;
using System.Collections;

public class CustomerSpawnScript : MonoBehaviour {
	
	public int MaxNoOfCustomers = 4;
	int noOfCustomers=0;
	public GameObject customerTemplate;
	private Transform targets;
	private int initialTarget = 0;
	string currentScene;
	int limiter = 0;
	GameObject[] customers;

	// Use this for initialization
	void Start () 
	{
		currentScene = Application.loadedLevelName;
		if(currentScene == "tutorialScene")
		{
			targets = GameObject.Find("Targets").transform;
			customers = new GameObject[MaxNoOfCustomers];
			AddCustomerToList();
			GetFrontOfQueueOrder().itemNeeded = true;
		}
		else
		{
		targets = GameObject.Find("Targets").transform;
		customers = new GameObject[MaxNoOfCustomers];
		AddCustomerToList();
		AddCustomerToList();
		AddCustomerToList();
		AddCustomerToList();
		GetFrontOfQueueOrder().itemNeeded = true;
		}
	}
	void Update()
	{
		StartCoroutine("spawnCustomersRegularly");
	}
	
	public bool AddCustomerToList()
	{
		bool added = false;
		if(customers[MaxNoOfCustomers -1] == null && limiter==0)
		{
			GameObject newCustomer = Instantiate(customerTemplate) as GameObject;
			
			for(int i = 0; i < MaxNoOfCustomers; i++)
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
					noOfCustomers++;
					
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
			noOfCustomers--;
		}
		else
		{
			return false;
		}
		
		for(int i = pos; i < MaxNoOfCustomers - pos; i++)
		{
			if(i + 1 < MaxNoOfCustomers)
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

	public void AddingTutorialCustomer()
	{
		
		AddCustomerToList();
		GetFrontOfQueueOrder().itemNeeded = true;
		limiter = 1;
		
	}
	IEnumerator spawnCustomersRegularly()
	{
		if(noOfCustomers<MaxNoOfCustomers)
		{
		AddCustomerToList();
		GetFrontOfQueueOrder().itemNeeded = true;
		}
		yield return new WaitForSeconds(20.0f);
		
	}

}
