using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterControllerScript : MonoBehaviour 
{
	public enum PlayerState 
	{
		Idle,
		Crafting,
		SelectingComponents,
		Serving,
		Recycling
	};

	//player state comment
	public PlayerState currentState;
	//public float maxSpeed = 10.0f;
	//bool facingRight = true;
	GameObject CraftingTable;
	GameObject RecyclingBin;
	GameObject ComponentsArea;
	GameObject Till;
	public TheInventoryScript inventoryScript;
	public CraftingScript craftingScript;
	//public CustomerNeedsScript customerNeedsScript;
	public List<string> itemsTodelete;

	// Use this for initialization
	void Start () 
	{
		CraftingTable = GameObject.Find("CraftingTable");
		RecyclingBin = GameObject.Find("RecyclingBin");
		ComponentsArea = GameObject.Find("ComponentsArea");
		Till = GameObject.Find("Till");

		craftingScript = GetComponent<CraftingScript>();
		inventoryScript = GetComponent<TheInventoryScript>();
		//customerNeedsScript.GetComponent<CustomerNeedsScript>();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		//float moveSide = Input.GetAxis("Horizontal");
		//float moveUpDown = Input.GetAxis("Vertical");
		//rigidbody2D.velocity = new Vector2(moveSide*maxSpeed,moveUpDown*maxSpeed);
	}
	
	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if(hit.collider != null)
				
			{
				if(hit.transform.gameObject.tag=="componentsarea")
				{
					Debug.Log("component");
					this.gameObject.transform.position = (ComponentsArea.transform.position + new Vector3(0,-2,0));
					currentState= PlayerState.SelectingComponents;
				}
				else if(hit.transform.gameObject.tag=="recyclingbin")
				{
					Debug.Log("recycling");
					this.gameObject.transform.position = (RecyclingBin.transform.position + new Vector3(0,-2,0));
					currentState = PlayerState.Recycling;
				}
				else if(hit.transform.gameObject.tag=="craftingtable")
				{
					Debug.Log("crafting");
					this.gameObject.transform.position = (CraftingTable.transform.position + new Vector3(2,0,0));
					this.gameObject.transform.rotation = CraftingTable.transform.rotation;
					currentState = PlayerState.Crafting;
				}
				else if(hit.transform.gameObject.tag=="Till")
				{
					this.gameObject.transform.position = (Till.transform.position + new Vector3(0,-1,0));
					currentState = PlayerState.Serving;
				}
				
			}

		}

	}
	 public void OnGUI()
	{
		if(currentState == PlayerState.SelectingComponents)
		{
		foreach (string item in inventoryScript.allComponents)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Box(inventoryScript.componentInventory[item],GUILayout.Width(50.0f), GUILayout.Height(50.0f));
				if(GUILayout.Button(string.Format("{0})",item)))
				{
					Debug.Log(string.Format("Got {0}",item));
					inventoryScript.playerInventory.Add(item);
				}
			GUILayout.EndHorizontal();
		}
		}

		if(currentState == PlayerState.Crafting)
		{
			foreach(string item in inventoryScript.playerInventory)
			{
				GUILayout.BeginHorizontal();
				if(GUILayout.Button(string.Format("{0}",item)))
				{
					craftingScript.craftingItems.Add(item);
					itemsTodelete.Add(item);
				}
				GUILayout.EndHorizontal();
			}
			foreach(string itemToDelete in itemsTodelete)
			{
				inventoryScript.playerInventory.Remove(itemToDelete);
			}
			itemsTodelete.Clear();

			if(GUILayout.Button("Craft"))
			{
				foreach(string item in craftingScript.craftingItems)
				{
					craftingScript.item+=item;
				}
				craftingScript.Craft();
				craftingScript.craftingItems.Clear();
				craftingScript.item = "";
			}
		}

		if(currentState == PlayerState.Recycling && inventoryScript.playerInventory.Count>0)
		{
			foreach(string item in inventoryScript.playerInventory)
			{
				GUILayout.BeginVertical();
				if(GUILayout.Button(string.Format("{0}",item)))
				{
					itemsTodelete.Add(item);
				}
			}
				GUILayout.EndVertical();
				foreach(string itemToDelete in itemsTodelete)
				{
					inventoryScript.playerInventory.Remove(itemToDelete);
				}
			itemsTodelete.Clear();
		}

		/*if(currentState == PlayerState.Serving)
		{
			foreach(string item in inventoryScript.playerInventory)
			{
				if(inventoryScript.playerInventory.Contains(customerNeedsScript.ItemCustomerWants))
				{
					customerNeedsScript.itemNeeded = false;
				}
			}

		}*/
	}

}
