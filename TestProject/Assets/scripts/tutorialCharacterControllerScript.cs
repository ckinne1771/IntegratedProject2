using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tutorialCharacterControllerScript : MonoBehaviour 
{
	//controls the player's state so certain events can be managed
	public enum PlayerState 
	{
		Idle,
		Crafting,
		SelectingComponents,
		Serving,
		Recycling
	};

	Animator anim;
	//textures for inventory
	Texture2D slot1Image;
	Texture2D slot2Image;
	//bool used to decide whether the player can pick up more components
	bool heldItem = false;
	//places that the player will move to when they click on the right area
	GameObject CraftingTable;
	GameObject RecyclingBin;
	GameObject ComponentsArea;
	GameObject Till;
	//score modifier
	int scoreModifier;
	//whether the player can click stuff or not
	bool playerHasControl = false; 
	//whether the player has the correct items in the inventory and can craft
	bool readytocraft=false;
	//guiskins for the game
	public GUISkin myGUISkin;
	public GUISkin guiskin2;
	//sound effects
	public AudioClip tillsound;
	public AudioClip popsound;
	public AudioClip hammer;
	//players state
	public PlayerState currentState;
	//items for the tutorial that glow at certain points
	public GameObject glowitems;
	public GameObject component;
	//customer prefab
	public GameObject customerTemplate;
	//scripts that are referenced to
	public TheInventoryScript inventoryScript;
	public CraftingScript craftingScript;
	public CustomerSpawnScript customerSpawnScript;
	//items to be crafted
	public string item1;
	public string item2;
	//item customer wants
	public string recipeitem;
	//the part of the tutorial the player is on
	public string currentStage;
	public List<string> stage;
	public static int part;
	//the players score
	public static int score;
	//keeps track if the player has the correct tutorial items
	public bool gotitem1=false;
	public bool gotitem2=false;
	public bool gotitem3=false; 
	public bool gotitem4=false;
	//if true, can serve second customer
	public bool itemCrafted = false;
	//timer
	public float timer = 60;

	public List<Texture2D> ComponentSprites;
	// Use this for initialization
	void Start () 
	{
		//animator for player
		anim = GetComponent<Animator>();
		CraftingTable = GameObject.Find("CraftingTable");
		RecyclingBin = GameObject.Find("RecyclingBin");
		ComponentsArea = GameObject.Find("ComponentsArea");
		Till = GameObject.Find("Till");
		currentState=PlayerState.Idle;
		craftingScript = GetComponent<CraftingScript>();
		inventoryScript = GetComponent<TheInventoryScript>();
		//initialising components
		item1="";
		item2="";
		slot1Image=ComponentSprites[0];
		slot2Image=ComponentSprites[0];
		currentStage = stage[0];
		//starts timer
		InvokeRepeating ("Countdown", 1f, 1f);

		
	} 
	
	// Update is called once per frame
	//moves player
	void Update()
	{
		//code for determinining where is clicked and where the player goes or an item is picked up accordingly
		if(Input.GetMouseButtonDown(0)&&playerHasControl)
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			
			if(hit.collider != null &&currentState==PlayerState.Idle)
				
			{
				if(hit.transform.gameObject.tag=="Components" && item2==""&&!inventoryScript.playerInventory.ContainsKey(this.gameObject.name)&&!heldItem)
				{
					//play sound effect
					audio.PlayOneShot(popsound);
					this.gameObject.transform.position = (ComponentsArea.transform.position + new Vector3(0,2,0));
					
					if(hit.transform.gameObject.name=="orange")
					{
						inventoryScript.AddItem(hit.transform.gameObject.name);
						collectItems(hit.transform.gameObject.name);
						gotitem1=true;
						anim.SetBool("issideview",false);
					}
					if(hit.transform.gameObject.name=="ball")
					{
						inventoryScript.AddItem(hit.transform.gameObject.name);
						collectItems(hit.transform.gameObject.name);
						gotitem2=true;
						anim.SetBool("issideview",false);
					}
					if(hit.transform.gameObject.name=="wheel")
					{
						inventoryScript.AddItem (hit.transform.gameObject.name);
						collectItems (hit.transform.gameObject.name);
						gotitem3=true;
						anim.SetBool("issideview",false);
					}
					if(hit.transform.gameObject.name=="metal")
					{
						inventoryScript.AddItem (hit.transform.gameObject.name);
						collectItems (hit.transform.gameObject.name);
						gotitem4=true;
						anim.SetBool("issideview",false);
					}
					if (hit.transform.gameObject.name=="orange")
					{
						if(slot1Image==ComponentSprites[0])
						{
							slot1Image = ComponentSprites[1];
						}
						else
						{
							slot2Image = ComponentSprites[1];
						}
					}
					
					if (hit.transform.gameObject.name=="ball")
					{
						if(slot1Image==ComponentSprites[0])
						{
							slot1Image = ComponentSprites[2];
						}
						else
						{
							slot2Image = ComponentSprites[2];
						}
					}
					if (hit.transform.gameObject.name=="wheel")
					{
						if(slot1Image==ComponentSprites[0])
						{
							slot1Image = ComponentSprites[3];
						}
						else
						{
							slot2Image = ComponentSprites[3];
						}
					}
					
					if (hit.transform.gameObject.name=="metal")
					{
						if(slot1Image==ComponentSprites[0])
						{
							slot1Image = ComponentSprites[4];
						}
						else
						{
							slot2Image = ComponentSprites[4];
						}
					}
				}
				
				else if(hit.transform.gameObject.tag=="craftingtable"&&readytocraft)
				{
					//positions the player to face crafting table
					//resets inventory
					this.gameObject.transform.position = (CraftingTable.transform.position + new Vector3(-2,0,0));
					anim.SetBool("issideview",true);
					currentState = PlayerState.Crafting;
					currentStage="serveCustomer";
					slot1Image=ComponentSprites[0];
					slot2Image=ComponentSprites[0];
					
				}
				else if(hit.transform.gameObject.tag=="Till")
				{
					//positions the player to face till
					this.gameObject.transform.position = (Till.transform.position + new Vector3(0,-1,0));
					currentState = PlayerState.Serving;
					anim.SetBool("issideview",false);
				}
				else if(hit.transform.gameObject.tag=="recyclingbin")
				{
					//positions the player to face recycling bin
					this.gameObject.transform.position = (RecyclingBin.transform.position + new Vector3(0,-1,0));
					currentState = PlayerState.Recycling;
				}
			}
		}
		//adjusts score modifier
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

	//starts timer for score modifiers
	void Countdown()
	{
		timer--;
	}
	
	
	
	//handles player states
	public void OnGUI()
	{
		GUI.skin=myGUISkin;
		//inventory box
		GUI.Box (new Rect ((Screen.width/2) -47, Screen.height - 110, 185,75),"Inventory");
		GUI.skin=guiskin2;
		GUI.Box (new Rect((Screen.width/2) - 40, Screen.height - 60, 80,50),slot1Image);
		GUI.Box (new Rect((Screen.width/2) + 50, Screen.height - 60, 80,50),slot2Image);
		GUI.skin= myGUISkin;

		//displays crafted! text after crafting an item for so many seconds
		if(craftingScript.itemCrafted&&craftingScript.crafted)
		{
			GUI.Box (new Rect(Screen.width/2,Screen.height/2,200,100),"Crafted!");
			StartCoroutine("WaitTime");
		}

		//crafting state, checks to see if item is crafted, if so makes item
		//empties inventory and adds an item if crafted
		if(currentState == PlayerState.Crafting )
		{
			
			if(craftingScript.Craft(item1, item2))
			{
				inventoryScript.RemoveItem(item1);
				inventoryScript.RemoveItem(item2);
				
			}
			
			item1 = "";
			item2 = "";
			slot1Image=ComponentSprites[0];
			slot2Image=ComponentSprites[0];
			if (inventoryScript.playerInventory.ContainsKey("wheelmetal"))
			{
				slot1Image = ComponentSprites[5];
			}
			if (inventoryScript.playerInventory.ContainsKey("orangeball"))
			{
				slot1Image = ComponentSprites[6];
			}
			heldItem=true;
			currentState = PlayerState.Idle;
			
		}
		//recycling
		if(currentState == PlayerState.Recycling)
		{
			//resets part in tutorial to before picking up components
			if(part==1)
			{
				currentStage="grabItems";
			}
			else if(part==2)
			{
				currentStage="Crafting2";
			}
			//sets any item bools to false again
			gotitem1=false;
			gotitem2=false;
			gotitem3=false;
			gotitem4=false;
			readytocraft=false;
			itemCrafted=false;
			//clears inventory
			if(inventoryScript.playerInventory.Count>0)
			{
			
			currentState=PlayerState.Idle;
			inventoryScript.playerInventory.Clear();
			inventoryScript.RemoveItem(item1);
			inventoryScript.RemoveItem(item2);
			
			item1="";
			item2="";
			slot1Image=ComponentSprites[0];
			slot2Image=ComponentSprites[0];
			inventoryScript.playerInventory.Clear();
			heldItem=false;
			}
			currentState=PlayerState.Idle;
		}
		
		//serving
		//clears inventory
		//customer is served if they have the right item
		//moves on tutorial
		if(!customerSpawnScript.IsQueueEmpty() && currentState == PlayerState.Serving)
		{
			slot1Image=ComponentSprites[0];
			slot2Image=ComponentSprites[0];
			if(part==1)
			{
				if(inventoryScript.playerInventory.ContainsKey(customerSpawnScript.GetFrontOfQueueOrder().first))
				{ 
					inventoryScript.RemoveItem(customerSpawnScript.GetFrontOfQueueOrder().item);
					customerSpawnScript.GetFrontOfQueueOrder().itemNeeded = false;
					customerSpawnScript.GetFrontOfQueueOrder().followTheWaypoints.targetWaypoint=1;
					customerSpawnScript.GetFrontOfQueueOrder().followTheWaypoints.customerState=FollowTheWaypoints.State.Exit;
					recipeitem="";
					score += (20*scoreModifier);
					timer=60; 
					currentStage="done!"; 
					audio.PlayOneShot(tillsound);
					glowitems.gameObject.transform.FindChild("Till").gameObject.GetComponent<GlowAnimator>().glow=false;
				} 
			} 

			if(part==2)
			{
				if(inventoryScript.playerInventory.ContainsKey(customerSpawnScript.GetFrontOfQueueOrder().second))
				{
					inventoryScript.RemoveItem(customerSpawnScript.GetFrontOfQueueOrder().item);
					customerSpawnScript.GetFrontOfQueueOrder().itemNeeded = false;
					customerSpawnScript.GetFrontOfQueueOrder().followTheWaypoints.targetWaypoint=1;
					customerSpawnScript.GetFrontOfQueueOrder().followTheWaypoints.customerState=FollowTheWaypoints.State.Exit;
					recipeitem="";
					score += (20*scoreModifier);
					timer=60;
					currentStage="finallyDone!";
					audio.PlayOneShot(tillsound);
					glowitems.gameObject.transform.FindChild("Till").gameObject.GetComponent<GlowAnimator>().glow=false;
				}
			}
			craftingScript.inventoryScript.RemoveItem("orangeball");
			craftingScript.inventoryScript.RemoveItem("wheelmetal");
			heldItem=false;


				currentState=PlayerState.Idle;
			
		}

		//this displays the players score
		GUI.TextField(new Rect(10,10,100,20),"Score; " +score);

		//this controls the events of the tutorial.
		switch(currentStage)
		{
		case("customerIntro"):
			part=1;
			GUI.Box(new Rect(Screen.width/2,Screen.height/12+30,160,50),"We have a new customer");
			if(GUI.Button(new Rect(Screen.width/2,Screen.height/4,200,100),"Click here\n to continue"))
			{
				currentStage="customerWant";
			}
			break;
		case("customerWant"):
			GUI.Box(new Rect(Screen.width/2,Screen.height/8,270,100),"Looks like they want a basketball!\nThe customer will\n always say what \nitem they want");
			if(GUI.Button(new Rect(Screen.width/2,Screen.height/3,200,100),"Click here\n to continue"))
			{
				currentStage="grabItems";
			}
			break;
		case("grabItems"):
			GUI.Box(new Rect(Screen.width/2,Screen.height/8,200,100),"Go grab the items \n needed to make the basketball");
			glowitems.gameObject.transform.FindChild("orange").gameObject.GetComponent<GlowAnimator>().glow=true;
			glowitems.gameObject.transform.FindChild("ball").gameObject.GetComponent<GlowAnimator>().glow=true;
			playerHasControl=true;
			if(gotitem1&&gotitem2)
			{
				readytocraft=true;
				glowitems.gameObject.transform.FindChild("orange").gameObject.GetComponent<GlowAnimator>().glow=false;
				glowitems.gameObject.transform.FindChild("ball").gameObject.GetComponent<GlowAnimator>().glow=false;
				currentStage="craftItems";
			}
			break;
		case("craftItems"):
			playerHasControl = false;
			GUI.Box(new Rect(Screen.width/2,Screen.height/4,200,50),"Good! Now craft them \n into something nice!");
			glowitems.gameObject.transform.FindChild("CraftingTable").gameObject.GetComponent<GlowAnimator>().glow=true;
			playerHasControl=true;
			break;
		case("serveCustomer"):
			glowitems.gameObject.transform.FindChild("CraftingTable").gameObject.GetComponent<GlowAnimator>().glow=false;
			readytocraft=false;
			playerHasControl=false;
			GUI.Box(new Rect(Screen.width/2,Screen.height/4,200,50),"Ok! Now you need to take \n the item to the till");
			glowitems.gameObject.transform.FindChild("Till").gameObject.GetComponent<GlowAnimator>().glow=true;
			playerHasControl=true;
			break;
		case("done!"):
			playerHasControl=false;
			GUI.Box(new Rect(Screen.width/2,Screen.height/4,200,50),"Congratulations, you've served \n your first customer");
			StartCoroutine("levelWait");
			break;
		case("movetolevel"):
			playerHasControl=false;
			StartCoroutine("secondCustomerWait");
			break;
		case("secondCustomer"):
			itemCrafted=false;
			part=2;
			customerSpawnScript.AddingTutorialCustomer();
			GUI.Box (new Rect(Screen.width/2,Screen.height/4,200,100),"We have another customer! \n This one wants a bike!");
			if(GUI.Button(new Rect(Screen.width/2,Screen.height/2,200,100),"Click here\n to continue"))
			{
				currentStage="Crafting2";
			}
			break;
	
		case("Crafting2"):
			playerHasControl=true;
			GUI.Box (new Rect(Screen.width/2,Screen.height/4,250,100),"Now, grab the\n items needed to\n make a bike\n and craft it.");
			glowitems.gameObject.transform.FindChild("metal").gameObject.GetComponent<GlowAnimator>().glow=true;
			glowitems.gameObject.transform.FindChild("wheel").gameObject.GetComponent<GlowAnimator>().glow=true;
			if(gotitem3&&gotitem4)
			{
				readytocraft=true;
				glowitems.gameObject.transform.FindChild("metal").gameObject.GetComponent<GlowAnimator>().glow=false;
				glowitems.gameObject.transform.FindChild("wheel").gameObject.GetComponent<GlowAnimator>().glow=false;
			}
			if(itemCrafted)
			{
				currentStage="serve2";
			}
			break;
		case("serve2"):
			readytocraft=false;
			GUI.Box(new Rect(Screen.width/2,Screen.height/4,200,70),"Ok! Now you need to take \n the item to the till");
			glowitems.gameObject.transform.FindChild("Till").gameObject.GetComponent<GlowAnimator>().glow=true;
			break;
		case("finallyDone!"):
		
			GUI.Box(new Rect(Screen.width/2,Screen.height/4,200,70),"Congratulations, you've served \n another customer");
			StartCoroutine("levelWait2");
			break;
		
		case("movetolevel2"):
			playerHasControl=false;
			Application.LoadLevel("LevelSelect");
			break;
		}
	}
	 
	//this method is used to add items to be crafted
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
	//these give delays at parts of the tutorial
	public IEnumerator levelWait()
	{
		yield return new WaitForSeconds(2.0f);
		currentStage="movetolevel";

	}
	public IEnumerator secondCustomerWait()
	{
		yield return new WaitForSeconds(2.0f);
		currentStage="secondCustomer";
	}
	public IEnumerator levelWait2()
	{
		yield return new WaitForSeconds(2.0f);
		currentStage="movetolevel2";
		
	}
	IEnumerator WaitTime()
	{
		yield return new WaitForSeconds(3.0f);
		craftingScript.crafted=false;
	}
}