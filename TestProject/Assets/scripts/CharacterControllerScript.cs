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
	public CustomerSpawnScript customerSpawnScript;
	public List<string> itemsTodelete;
	public GameObject Customer;
	public string item1;
	public string item2;
	public string recipeitem;
	public Texture2D images;
	public GameObject customerTemplate;
	//private bool noCompletedItems = true;
	
	
	// Use this for initialization
	void Start () 
	{
		
		CraftingTable = GameObject.Find("CraftingTable");
		RecyclingBin = GameObject.Find("RecyclingBin");
		ComponentsArea = GameObject.Find("ComponentsArea");
		Till = GameObject.Find("Till");
		currentState=PlayerState.Idle;
		
		
		craftingScript = GetComponent<CraftingScript>();
		inventoryScript = GetComponent<TheInventoryScript>();
		item1="";
		item2="";
		
	}
	
	// Update is called once per frame
	//moves player
	void Update()
	{
		
		if(Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			
			if(hit.collider != null &&currentState==PlayerState.Idle)
				
			{
				if(hit.transform.gameObject.tag=="Components" && item2=="")
				{
					Debug.Log("component");
					this.gameObject.transform.position = (ComponentsArea.transform.position + new Vector3(0,2,0));
					//currentState= PlayerState.SelectingComponents;
					
					if(hit.transform.gameObject.name=="orange")
					{
						inventoryScript.AddItem(inventoryScript.allComponents[0]);
						//currentState= PlayerState.Idle;
						Debug.Log(inventoryScript.playerInventory.Keys.ToString());
						collectItems(hit.transform.gameObject.name);
						
					}
					else if (hit.transform.gameObject.name=="ball")
					{
						inventoryScript.AddItem(inventoryScript.allComponents[1]);
						//currentState= PlayerState.Idle;
						Debug.Log(inventoryScript.playerInventory.Keys.ToString());
						collectItems(hit.transform.gameObject.name);
					}
					else if(hit.transform.gameObject.name=="wheel")
					{
						inventoryScript.AddItem(inventoryScript.allComponents[2]);
						//currentState=PlayerState.Idle;
						Debug.Log(inventoryScript.playerInventory.Keys.ToString());
						collectItems(hit.transform.gameObject.name);
					}
					else if(hit.transform.gameObject.name=="metal")
					{
						inventoryScript.AddItem(inventoryScript.allComponents[3]);
						//currentState=PlayerState.Idle;
						Debug.Log(inventoryScript.playerInventory.Keys.ToString());
						collectItems(hit.transform.gameObject.name);
					}
				}
				
				
				/*else if(hit.transform.gameObject.tag=="recyclingbin")
				{
					Debug.Log("recycling");
					this.gameObject.transform.position = (RecyclingBin.transform.position + new Vector3(2,0,0));
					currentState = PlayerState.Recycling;
				}*/
				else if(hit.transform.gameObject.tag=="craftingtable")
				{
					/*if(inventoryScript.playerInventory.ContainsKey("orangeball") || inventoryScript.playerInventory.ContainsKey("wheelmetal"))
					{
						noCompletedItems = false;
						Debug.Log("Status Changed");
					}*/
					
					
					Debug.Log("crafting");
					this.gameObject.transform.position = (CraftingTable.transform.position + new Vector3(-2,0,0));
					this.gameObject.transform.rotation = CraftingTable.transform.rotation;
					currentState = PlayerState.Crafting;
					//recipeitem = craftingScript.TherecipeList[recipeitem];
					
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
		GUI.color=Color.black;
		GUI.Label(new Rect(Screen.width/2,Screen.height/8,100,100),"You are holding "+item1+" "+item2+recipeitem);
		
		//crafting
		if(currentState == PlayerState.Crafting )
		{
			
			if(craftingScript.Craft(item1, item2))
			{
				inventoryScript.RemoveItem(item1);
				inventoryScript.RemoveItem(item2);
			}
			item1 = "";
			item2 = "";
			currentState = PlayerState.Idle;
			
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
					/*if(item=="orangeball" || item =="wheelmetal")
					{
						noCompletedItems=true;
					}*/
				}
				currentState=PlayerState.Idle;
			}
			if(GUILayout.Button("Exit"))
			{
				currentState=PlayerState.Idle;
			}
			
			GUILayout.EndVertical();
			foreach(string itemToDelete in itemsTodelete)
			{
				inventoryScript.playerInventory.Remove(itemToDelete);
			}
			itemsTodelete.Clear();
		}
		
		//serving
		if(!customerSpawnScript.IsQueueEmpty() && currentState == PlayerState.Serving)
		{
			if(inventoryScript.playerInventory.ContainsKey(customerSpawnScript.GetFrontOfQueueOrder().item))
			{
				inventoryScript.RemoveItem(customerSpawnScript.GetFrontOfQueueOrder().item);
				customerSpawnScript.GetFrontOfQueueOrder().itemNeeded = false;
				//noCompletedItems=true;
				customerSpawnScript.RemoveCustomer(0);
				recipeitem="";
			}
			currentState=PlayerState.Idle;
			
		}
		
		/*if(noCompletedItems==false){
			GUI.TextArea(new Rect(240, 10,300,50), "Completed items must be given to the customer or recycled before crafting again!");
		}*/
		
	}
	
	void collectItems(string _item)
	{
		if(item1=="")
		{
			item1=_item;
		}
		else
		{
			item2=_item;
		}
		
	}
	
}