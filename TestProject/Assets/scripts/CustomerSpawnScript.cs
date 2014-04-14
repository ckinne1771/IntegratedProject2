using UnityEngine;
using System.Collections;

public class CustomerSpawnScript : MonoBehaviour {

	//public variables used to control the flow of customers
	public int maxTotalNoOfCustomers=10;
	public int MaxNoOfCustomersAtOnce = 5;
	public int currentNoOfCustomers=0;
	public int totalNoOfCustomers=0;
	//customer prefab used to spawn customers
	public GameObject customerTemplate;
	//customer spawn point
	private Transform targets;
	private int initialTarget = 0;
	string currentScene;
	int limiter = 0;
	// array of customers
	public GameObject[] customers;
	//where to remove a customer from in the array
	public int queuePointToDelete;

	// Use this for initialization
	void Start () 
	{
		//initiates first wave of customers
		currentScene = Application.loadedLevelName;
		if(currentScene == "tutorialScene")
		{
			targets = GameObject.Find("Targets").transform;
			customers = new GameObject[MaxNoOfCustomersAtOnce];
			AddCustomerToList();
			GetFrontOfQueueOrder().itemNeeded = true;
		}
		else
		{
		targets = GameObject.Find("Targets").transform;
		customers = new GameObject[MaxNoOfCustomersAtOnce];
		AddCustomerToList();
		GetFrontOfQueueOrder().itemNeeded = true;
		}
	}
	void Update()
	{	
		//adds customers regularly if not tutorial scene
		if(currentScene != "tutorialScene")
		{
		StartCoroutine("spawnCustomersRegularly");
		}
	}
	
	public bool AddCustomerToList()
	{
		//adds a customer to the array of customers and sets its point in queue
		//also iterates number of customers currently and in total
		bool added = false;
		if(customers[MaxNoOfCustomersAtOnce -1] == null && limiter==0)
		{
			GameObject newCustomer = Instantiate(customerTemplate) as GameObject;
			for(int i = 0; i < MaxNoOfCustomersAtOnce; i++)
			{
				if(customers[i] == null && !added)
				{
					customers[i] = newCustomer;
					customers[i].gameObject.GetComponent<FollowTheWaypoints>().pointInQueue=currentNoOfCustomers;
					Transform targetWaypoint = targets.GetChild(initialTarget);
					customers[i].transform.position = targetWaypoint.transform.position;
					added = true;
					
					if (initialTarget + 1 < targets.childCount)
					{
						// Set next target as spawnpoint
						initialTarget++;
					}
					currentNoOfCustomers++;
					totalNoOfCustomers++;
					
				}
			}
		}
		return added;
	}

	//check to see if there are customers
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

	//returns first customer
	public CustomerNeedsScript GetFrontOfQueueOrder()
	{
		return customers[0].GetComponent<CustomerNeedsScript>();
	}

	//removes a customer from the queue and moves other customers accordingly
	public bool RemoveCustomer(int pos)
	{
		if(customers[pos] != null)
		{
			GameObject delCustomer = customers[pos];
			customers[pos] = null;
			Destroy(delCustomer);
			currentNoOfCustomers--;
		}
		else
		{
			return false;
		}
		
		for(int i = pos; i < MaxNoOfCustomersAtOnce - pos; i++)
		{
			if(i + 1 < MaxNoOfCustomersAtOnce)
			{
				customers[i] = customers[i+1];
				customers[i+1] = null;
			}
		}
		//if there is a customer, the first one shows their want
		if(!IsQueueEmpty())
		{
			GetFrontOfQueueOrder().itemNeeded = true;
		}
		foreach(GameObject customer in customers)
		{
			if(customer!=null)
			{
			if(customer.gameObject.GetComponent<FollowTheWaypoints>().pointInQueue>queuePointToDelete)
			{
			customer.gameObject.GetComponent<FollowTheWaypoints>().pointInQueue--;
			}
			}
		}
		return true;
	}

	//adds tutorial customer
	public void AddingTutorialCustomer()
	{
		AddCustomerToList();
		GetFrontOfQueueOrder().itemNeeded = true;
		limiter = 1;
		
	}
	//method to spawn customers
	IEnumerator spawnCustomersRegularly()
	{
		yield return new WaitForSeconds(2.0f);
		if(totalNoOfCustomers<=maxTotalNoOfCustomers&& limiter==0)
		{
			if(currentNoOfCustomers<MaxNoOfCustomersAtOnce)
			{
			AddCustomerToList();
			GetFrontOfQueueOrder().itemNeeded = true;
			}
		}
	}

}
