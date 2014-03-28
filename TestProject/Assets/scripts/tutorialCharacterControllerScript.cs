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
	bool gotitem1=false;
	bool gotitem2=false;
	bool gotItem3=false; 
	bool gotItem4=false;
	int scoreModifier;
	public float timer = 60;
	public bool itemCrafted = false;
	public static int part;
	Animator anim;
	public Texture2D slot1Image;
	public Texture2D slot2Image;

	public List<Texture2D> ComponentSprites;
	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator>();
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

		if (inventoryScript.playerInventory.ContainsKey("orange"))
		    {
				slot1Image = ComponentSprites[0];
			}

		if (inventoryScript.playerInventory.ContainsKey("ball"))
		    {
				slot2Image = ComponentSprites[1];
			}
		if (inventoryScript.playerInventory.ContainsKey("wheel"))
		{
			slot1Image=ComponentSprites[2];
		}

		if (inventoryScript.playerInventory.ContainsKey("metal"))
		{
			slot2Image=ComponentSprites[3];
		}



	
		
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
						anim.SetBool("issideview",false);
					}
					if(hit.transform.gameObject.name=="ball")
					{
						inventoryScript.AddItem(hit.transform.gameObject.name);
						collectItems(hit.transform.gameObject.name);
						InstantiateComponents(hit.transform.gameObject);
						gotitem2=true;
						anim.SetBool("issideview",false);
					}
					if(hit.transform.gameObject.name=="wheel")
					{
						inventoryScript.AddItem (hit.transform.gameObject.name);
						collectItems (hit.transform.gameObject.name);
						InstantiateComponents (hit.transform.gameObject);
						gotItem3=true;
						anim.SetBool("issideview",false);
					}
					if(hit.transform.gameObject.name=="metal")
					{
						inventoryScript.AddItem (hit.transform.gameObject.name);
						collectItems (hit.transform.gameObject.name);
						InstantiateComponents (hit.transform.gameObject);
						gotItem4=true;
						anim.SetBool("issideview",false);
					}
				}
				
				else if(hit.transform.gameObject.tag=="craftingtable")
				{
					Debug.Log ("crafting");
					this.gameObject.transform.position = (CraftingTable.transform.position + new Vector3(-2,0,0));
					//this.gameObject.transform.rotation = CraftingTable.transform.rotation;
					anim.SetBool("issideview",true);
					currentState = PlayerState.Crafting;
					Debug.Log ("serving");
					currentStage="serveCustomer";
					slot1Image=null;
					slot2Image=null;

					//currentStage="serveCustomer";
					
				}
				else if(hit.transform.gameObject.tag=="Till")
				{
						Debug.Log ("till");
					this.gameObject.transform.position = (Till.transform.position + new Vector3(0,-1,0));
					currentState = PlayerState.Serving;
					anim.SetBool("issideview",false);
				}
				/*else if(hit.transform.gameObject.tag=="recyclingbin")
				{
					Debug.Log ("bin");
					this.gameObject.transform.position = (RecyclingBin.transform.position + new Vector3(0,-1,0));
					currentState = PlayerState.Recycling;
				}*/
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
		Debug.Log (itemCrafted);
		Debug.Log (currentState);
	}
	
	
	
	//handles player states
	public void OnGUI()
	{
		GUI.Box (new Rect ((Screen.width/2) -47, Screen.height - 80, 185,75),"Inventory");
		GUI.Box (new Rect((Screen.width/2) - 40, Screen.height - 60, 80,50),slot1Image);
		GUI.Box (new Rect((Screen.width/2) + 50, Screen.height - 60, 80,50),slot2Image);

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
			
			currentState=PlayerState.Idle;
			
			inventoryScript.playerInventory.Clear();
			inventoryScript.RemoveItem(item1);
			inventoryScript.RemoveItem(item2);
			
			if(components.Count > 0)
			{
				while(components.Count > 0)
				{
					GameObject delObj = components[0];
					components.RemoveAt(0);
					Destroy(delObj);
				} 
			}
			item1="";
			item2=""; 
		}
		
		//serving
		if(!customerSpawnScript.IsQueueEmpty() && currentState == PlayerState.Serving)
		{
			if(part==1)
			{
				Debug.Log ("Pass1");
				if(inventoryScript.playerInventory.ContainsKey(customerSpawnScript.GetFrontOfQueueOrder().first))
				{
					Debug.Log ("served"); 
					inventoryScript.RemoveItem(customerSpawnScript.GetFrontOfQueueOrder().item);
					customerSpawnScript.GetFrontOfQueueOrder().itemNeeded = false;
					//noCompletedItems=true;
					customerSpawnScript.RemoveCustomer(0);
					recipeitem="";
					score += (20*scoreModifier);
					timer=60; 
					currentStage="done!"; 
				} 
			} 

			if(part==2)
			{
				if(inventoryScript.playerInventory.ContainsKey(customerSpawnScript.GetFrontOfQueueOrder().second))
				{
					Debug.Log ("served"); 
					inventoryScript.RemoveItem(customerSpawnScript.GetFrontOfQueueOrder().item);
					customerSpawnScript.GetFrontOfQueueOrder().itemNeeded = false;
					//noCompletedItems=true;
					customerSpawnScript.RemoveCustomer(0);
					recipeitem="";
					score += (20*scoreModifier);
					timer=60;
					currentStage="finallyDone!";
				}
			}

				currentState=PlayerState.Idle;
			
		}
		
		GUI.TextField(new Rect(10,10,100,20),"Score; " +score);
		switch(currentStage)
		{
		case("customerIntro"):
			part=1;
			GUI.Box(new Rect(Screen.width/2-80,Screen.height/16,160,50),"We have a new customer");
			//highlight customer anim
			StartCoroutine("CustomerWant");
			//currentStage ="customerWant";
			//unhighlight
			break;
		case("customerWant"):
			GUI.Box(new Rect(Screen.width/2-125,Screen.height/16,250,60),"Looks like they want a basketball!\n The customer will always\n say what item they want");
			//highlight customerwant
			StartCoroutine("grabItems");
			//unhighlight
			break;
		case("grabItems"):
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/16,200,50),"Go grab the items \n needed to make the basketball");
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
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/16,200,50),"Good! Now craft them \n into something nice!");
			playerHasControl=true;
			/*if(itemCrafted==true)
			{
				currentStage="serveCustomer";
			}*/
			//highlight craft bench anim
			//StartCoroutine("WaitTime");
			//unhighlight
			break;
		case("serveCustomer"):
			playerHasControl=false;
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/16,200,50),"Ok! Now you need to take \n the item to the till");
			playerHasControl=true;
			//highlight till anim
			//StartCoroutine("WaitTime");
			//unhighlight
			break;
		case("done!"):
			playerHasControl=false;
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/16,200,50),"Congratulations, you've served \n your first customer");
			//StartCoroutine("WaitTime");
			StartCoroutine("levelWait");
			break;
		case("movetolevel"):
			playerHasControl=false;
			StartCoroutine("secondCustomerWait");
			break;
		case("secondCustomer"):
			itemCrafted=false;
			part=2;
			GUI.Box (new Rect(Screen.width/2-100,Screen.height/16,200,50),"We have another customer! \n This one wants a bike!");
			customerSpawnScript.AddingTutorialCustomer ();
			StartCoroutine ("secondCustomerTransition");
			break;
	
		case("Crafting2"):
			playerHasControl=true;
			GUI.Box (new Rect(Screen.width/2-100,Screen.height/16,250,50),"Now, grab the items needed \n to make a bike and craft it.");
			if(itemCrafted==true)
			{
				currentStage="serve2";
			}
			break;
		case("serve2"):
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/16,200,50),"Ok! Now you need to take \n the item to the till");
			break;
		case("finallyDone!"):
		
			GUI.Box(new Rect(Screen.width/2-100,Screen.height/16,200,50),"Congratulations, you've served \n another customer");
			StartCoroutine("levelWait2");
			break;
		
		case("movetolevel2"):
			playerHasControl=false;
			Application.LoadLevel("LevelSelect");
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
		yield return new WaitForSeconds(2.0f);
		currentStage="customerWant";
	}
	public IEnumerator grabItems()
	{
		yield return new WaitForSeconds(2.0f);
		currentStage="grabItems";
	}
	public IEnumerator levelWait()
	{
		yield return new WaitForSeconds(2.0f);
		currentStage="movetolevel";

	}
	public IEnumerator secondCustomerWait()
	{
		yield return new WaitForSeconds(3.0f);
		currentStage="secondCustomer";
	}
	public IEnumerator secondCustomerTransition()
	{
		yield return new WaitForSeconds(3.0f);
		currentStage="Crafting2";
	}
	public IEnumerator levelWait2()
	{
		yield return new WaitForSeconds(2.0f);
		currentStage="movetolevel2";
		
	}
	IEnumerator WaitTime()
	{
		yield return new WaitForSeconds(1.0f);
		craftingScript.crafted=false;
	}
}