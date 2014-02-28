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
	GameObject CraftingTable;
	GameObject RecyclingBin;
	GameObject ComponentsArea;
	GameObject Till;
	public TheInventoryScript inventoryScript;
	public CraftingScript craftingScript;
	public CustomerNeedsScript customerNeedsScript;
	public List<string> itemsTodelete;
	public GameObject Customer;
	public string item1;
	public string item2;
	public Texture2D images;

	// Use this for initialization
	void Start () 
	{
		CraftingTable = GameObject.Find("CraftingTable");
		RecyclingBin = GameObject.Find("RecyclingBin");
		ComponentsArea = GameObject.Find("ComponentsArea");
		Customer = GameObject.Find("Customer");
		Till = GameObject.Find("Till");

		craftingScript = GetComponent<CraftingScript>();
		inventoryScript = GetComponent<TheInventoryScript>();
		customerNeedsScript = Customer.GetComponent<CustomerNeedsScript>();
		item1=null;
		item2=null;

	}
	
	// Update is called once per frame
	//moves player
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

	//handles player states
	 public void OnGUI()
	{
		//selecting components
		if(currentState == PlayerState.SelectingComponents)
		{
		foreach (string item in inventoryScript.allComponents)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Box(inventoryScript.componentInventory[item],GUILayout.Width(50.0f), GUILayout.Height(50.0f));
				if(GUILayout.Button(string.Format("{0})",item)))
				{
					Debug.Log(string.Format("Got {0}",item));
					inventoryScript.playerInventory.Add(item,1);
					images = inventoryScript.componentInventory[item];
					Debug.Log (images);
					inventoryScript.playerInventoryImages.Add (item, images);

				}
			GUILayout.EndHorizontal();
		}
		}

		//crafting
		if(currentState == PlayerState.Crafting)
		{
			foreach(string item in inventoryScript.playerInventory.Keys)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Box(inventoryScript.playerInventoryImages[item],GUILayout.Width(50.0f), GUILayout.Height(50.0f));
				if(GUILayout.Button(string.Format("{0}",item))&&item2==null)
				{
					if(item1==null)
					{
						item1=item;
						Debug.Log ("this is item1"+item1);
						itemsTodelete.Add(item);
					}
					else
					{
						item2=item;
						itemsTodelete.Add(item);
					}
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
				Debug.Log("This is item1 again"+item1);
				craftingScript.Craft();
				craftingScript.craftingItems.Clear();
				craftingScript.item = "";
			}
		}

		//recycling
		if(currentState == PlayerState.Recycling && inventoryScript.playerInventory.Count>0)
		{
			foreach(string item in inventoryScript.playerInventory.Keys)
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

		//serving
		if(currentState == PlayerState.Serving && customerNeedsScript.itemNeeded)
		{
			foreach(string item in inventoryScript.playerInventory.Keys)
			{
				/*if(inventoryScript.playerInventory.Keys.(customerNeedsScript.itemRequested))
				{
					customerNeedsScript.itemNeeded = false;

				}*/

					if(item==customerNeedsScript.itemRequested)
					{
						customerNeedsScript.itemNeeded = false;
					}
			}

		}
	}

}
