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
	public string item1;
	public string item2;
	public string recipeitem;
	public Texture2D images;
	public GameObject customerTemplate;
	public List<GameObject> components;
	public GameObject component;
	public static int score;
	private int scoreModifier;
	public float timer=60;
	public bool Crafted;
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
		//customerSpawnScript = GetComponent<CustomerSpawnScript>();
		item1="";
		item2="";
		InvokeRepeating("Countdown",1f,1f);
		
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

					this.gameObject.transform.position = (ComponentsArea.transform.position + new Vector3(0,2,0));

					if(hit.transform.gameObject.tag=="Components")
					{
					
						inventoryScript.AddItem(hit.transform.gameObject.name);
						collectItems(hit.transform.gameObject.name);
						InstantiateComponents(hit.transform.gameObject);
						//components.Add(hit.transform.gameObject);
					}
				}

				else if(hit.transform.gameObject.tag=="craftingtable")
				{
					this.gameObject.transform.position = (CraftingTable.transform.position + new Vector3(-2,0,0));
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

		if (timer>40)
		{
			scoreModifier=3;
		}

		if (timer <40 && timer>20)
		{
			scoreModifier=2;
		}
		if (timer <20 && timer>0)
		{
			scoreModifier=1;
		}

	}

	void Countdown()
	{
		timer--;
		Debug.Log (timer);
	}
	
	
	
	//handles player states
	public void OnGUI()
	{
		if(craftingScript.itemCrafted&&craftingScript.crafted)
		{
			GUI.Box (new Rect(Screen.width/2,Screen.height/2,60,20),"Crafted!");
			StartCoroutine("WaitTime");
		}

		if(currentState == PlayerState.Crafting )
		{
			
			if(craftingScript.Craft(item1, item2))
			{
				inventoryScript.RemoveItem(item1);
				inventoryScript.RemoveItem(item2);

			}

			if(components.Count > 0)
			{
				while(components.Count > 0)
				{
					GameObject delObj = components[0];
					components.RemoveAt(0);
					Destroy(delObj);
				}
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
				score += (20*scoreModifier);
				timer=60;
			}
			currentState=PlayerState.Idle;
			
		}

		GUI.TextField(new Rect(10,10,100,20),"Score; " +score); 

		if(customerSpawnScript.IsQueueEmpty()==true)
		{
			Debug.Log ("empty");
			Application.LoadLevel("LevelSelect");
		}
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

	void InstantiateComponents(GameObject _component)
	{
		component = Instantiate(_component) as GameObject;
		component.name = component.name.Substring(0, _component.name.Length);
		component.transform.position=this.gameObject.transform.position + new Vector3(0,0 + 2 * components.Count,0);
		component.transform.parent=this.gameObject.transform;
		components.Add(component);
	}

	IEnumerator WaitTime()
	{
		yield return new WaitForSeconds(1.0f);
		craftingScript.crafted=false;
	}
	
}