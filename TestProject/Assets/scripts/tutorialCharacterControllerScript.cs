using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tutorialCharacterControllerScript : MonoBehaviour 
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
	public GameObject customerTemplate;
	public List<GameObject> components;
	public GameObject component;
	public static int score;
	public bool playerHasControl = false;
	public List<string> stage;
	public string currentStage;
	public bool gotitem1;
	public bool gotitem2;
	private int scoreModifier;
	public float timer = 60;
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
		currentStage = stage[0];
		InvokeRepeating ("Countdown", 1f, 1f);
		
	} 
	
	// Update is called once per frame
	//moves player
	void Update()
	{
		
		if(Input.GetMouseButtonDown(0)&&playerHasControl)
		{
			Debug.Log ("raycast");
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			
			if(hit.collider != null &&currentState==PlayerState.Idle)
				
			{
				Debug.Log ("idleraycast");
				if(hit.transform.gameObject.tag=="Components" && item2=="")
				{
					
					this.gameObject.transform.position = (ComponentsArea.transform.position + new Vector3(0,2,0));
					
					if(hit.transform.gameObject.name=="orange")
					{
						inventoryScript.AddItem(hit.transform.gameObject.name);
						collectItems(hit.transform.gameObject.name);
						InstantiateComponents(hit.transform.gameObject);
						gotitem1=true;
					}
					if(hit.transform.gameObject.name=="ball")
					{
						inventoryScript.AddItem(hit.transform.gameObject.name);
						collectItems(hit.transform.gameObject.name);
						InstantiateComponents(hit.transform.gameObject);
						gotitem2=true;
					}
				}
				
				else if(hit.transform.gameObject.tag=="craftingtable")
				{
					Debug.Log ("crafting");
					this.gameObject.transform.position = (CraftingTable.transform.position + new Vector3(-2,0,0));
					this.gameObject.transform.rotation = CraftingTable.transform.rotation;
					currentState = PlayerState.Crafting;
					currentStage="serveCustomer";
					
				}
				else if(hit.transform.gameObject.tag=="Till")
				{
						Debug.Log ("till");
					this.gameObject.transform.position = (Till.transform.position + new Vector3(0,-1,0));
					currentState = PlayerState.Serving;
				}
			}
		}
		if (timer >40)
		{
			scoreModifier=3;
		}

		if (timer > 40 && timer <20)
		{
			scoreModifier=2;
		}
		if (timer <20 && timer >0)
		{
			scoreModifier=1;
		}
}

	void Countdown()
	{
		timer--;
		Debug.Log(timer);
	}
	
	
	
	//handles player states
	public void OnGUI()
	{

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
				currentStage="done!";
			}
			currentState=PlayerState.Idle;
			
		}
		
		GUI.TextField(new Rect(10,10,100,20),"Score; " +score);
		switch(currentStage)
		{
		case("customerIntro"):
			GUI.Box(new Rect(Screen.width/2-80,Screen.height/2,160,50),"We have a new customer");
			//highlight customer anim
			StartCoroutine("CustomerWant");
			//currentStage ="customerWant";
			//unhighlight
			break;
		case("customerWant"):
			GUI.Box(new Rect(Screen.width/2-75,Screen.height/2,250,60),"Looks like they want an basketball! \n The customer will always \n say what item they want");
			//highlight customerwant
			StartCoroutine("grabItems");
			//unhighlight
			break;
		case("grabItems"):
			        GUI.Box(new Rect(Screen.width/2-100,Screen.height/2,200,50),"Go grab the items \n needed to make the basketball");
			//highlight orange paint anim
			//highlight ball anim
			playerHasControl=true;
			if(gotitem1&&gotitem2)
			{
				currentStage="craftItems";
			}
			//unhighlight
			//unhighlight
			break;
		case("craftItems"):
			playerHasControl = false;
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/2,200,50),"Good! Now craft them \n into something nice!");
			playerHasControl=true;
			//highlight craft bench anim
			//StartCoroutine("WaitTime");
			//unhighlight
			break;
		case("serveCustomer"):
			playerHasControl=false;
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/2,200,50),"Ok! Now you need to take \n the item to the till");
			playerHasControl=true;
			//highlight till anim
			//StartCoroutine("WaitTime");
			//unhighlight
			break;
		case("done!"):
			playerHasControl=false;
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/2,200,50),"Congratulations, you've served \n your first customer");
			//StartCoroutine("WaitTime");
			break;
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

	public IEnumerator CustomerWant()
	{
		yield return new WaitForSeconds(3.0f);
		currentStage="customerWant";
	}
	public IEnumerator grabItems()
	{
		yield return new WaitForSeconds(3.0f);
		currentStage="grabItems";
	}
	
}
