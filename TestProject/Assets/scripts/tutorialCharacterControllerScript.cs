﻿using UnityEngine;
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

	public GUISkin myGUISkin;
	public GUISkin guiskin2;
	public AudioClip tillsound;
	public AudioClip popsound;
	public AudioClip hammer;
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
	public bool gotitem1=false;
	public bool gotitem2=false;
	public bool gotitem3=false; 
	public bool gotitem4=false;
	public bool readytocraft=false;
	int scoreModifier;
	public float timer = 60;
	public bool itemCrafted = false;
	public static int part;
	Animator anim;
	public Texture2D slot1Image;
	public Texture2D slot2Image;
	public bool heldItem = false;

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
		slot1Image=ComponentSprites[0];
		slot2Image=ComponentSprites[0];
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
				if(hit.transform.gameObject.tag=="Components" && item2==""&&!inventoryScript.playerInventory.ContainsKey(this.gameObject.name)&&!heldItem)
				{
					audio.PlayOneShot(popsound);
					this.gameObject.transform.position = (ComponentsArea.transform.position + new Vector3(0,2,0));
					
					if(hit.transform.gameObject.name=="orange")
					{
						inventoryScript.AddItem(hit.transform.gameObject.name);
						collectItems(hit.transform.gameObject.name);
						//InstantiateComponents(hit.transform.gameObject);
						gotitem1=true;
						anim.SetBool("issideview",false);
					}
					if(hit.transform.gameObject.name=="ball")
					{
						inventoryScript.AddItem(hit.transform.gameObject.name);
						collectItems(hit.transform.gameObject.name);
						//InstantiateComponents(hit.transform.gameObject);
						gotitem2=true;
						anim.SetBool("issideview",false);
					}
					if(hit.transform.gameObject.name=="wheel")
					{
						inventoryScript.AddItem (hit.transform.gameObject.name);
						collectItems (hit.transform.gameObject.name);
						//InstantiateComponents (hit.transform.gameObject);
						gotitem3=true;
						anim.SetBool("issideview",false);
					}
					if(hit.transform.gameObject.name=="metal")
					{
						inventoryScript.AddItem (hit.transform.gameObject.name);
						collectItems (hit.transform.gameObject.name);
						//InstantiateComponents (hit.transform.gameObject);
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
				else if(hit.transform.gameObject.tag=="recyclingbin")
				{
					Debug.Log ("bin");
					this.gameObject.transform.position = (RecyclingBin.transform.position + new Vector3(0,-1,0));
					currentState = PlayerState.Recycling;
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
		Debug.Log (itemCrafted);
		Debug.Log (currentState);
	}
	
	
	
	//handles player states
	public void OnGUI()
	{
		GUI.skin=myGUISkin;
		GUI.Box (new Rect ((Screen.width/2) -47, Screen.height - 110, 185,75),"Inventory");
		GUI.skin=guiskin2;
		GUI.Box (new Rect((Screen.width/2) - 40, Screen.height - 60, 80,50),slot1Image);
		GUI.Box (new Rect((Screen.width/2) + 50, Screen.height - 60, 80,50),slot2Image);
		GUI.skin= myGUISkin;
		if(craftingScript.itemCrafted&&craftingScript.crafted)
		{
			GUI.Box (new Rect(Screen.width/2,Screen.height/2,100,60),"Crafted!");
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
			if(part==1)
			{
				currentStage="grabItems";
			}
			else if(part==2)
			{
				currentStage="Crafting2";
			}
			gotitem1=false;
			gotitem2=false;
			gotitem3=false;
			gotitem4=false;
			readytocraft=false;
			itemCrafted=false;
			if(inventoryScript.playerInventory.Count>0)
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
			slot1Image=ComponentSprites[0];
			slot2Image=ComponentSprites[0];
			inventoryScript.playerInventory.Clear();
			heldItem=false;
			}
			currentState=PlayerState.Idle;
		}
		
		//serving
		if(!customerSpawnScript.IsQueueEmpty() && currentState == PlayerState.Serving)
		{
			slot1Image=ComponentSprites[0];
			slot2Image=ComponentSprites[0];
			if(part==1)
			{
				Debug.Log ("Pass1");
				if(inventoryScript.playerInventory.ContainsKey(customerSpawnScript.GetFrontOfQueueOrder().first))
				{
					Debug.Log ("served"); 
					inventoryScript.RemoveItem(customerSpawnScript.GetFrontOfQueueOrder().item);
					customerSpawnScript.GetFrontOfQueueOrder().itemNeeded = false;
					//noCompletedItems=true;
					//customerSpawnScript.RemoveCustomer(0);
					customerSpawnScript.GetFrontOfQueueOrder().followTheWaypoints.targetWaypoint=1;
					customerSpawnScript.GetFrontOfQueueOrder().followTheWaypoints.customerState=FollowTheWaypoints.State.Exit;
					recipeitem="";
					score += (20*scoreModifier);
					timer=60; 
					currentStage="done!"; 
					audio.PlayOneShot(tillsound);
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
					//customerSpawnScript.RemoveCustomer(0);
					customerSpawnScript.GetFrontOfQueueOrder().followTheWaypoints.targetWaypoint=1;
					customerSpawnScript.GetFrontOfQueueOrder().followTheWaypoints.customerState=FollowTheWaypoints.State.Exit;
					recipeitem="";
					score += (20*scoreModifier);
					timer=60;
					currentStage="finallyDone!";
					audio.PlayOneShot(tillsound);
				}
			}
			craftingScript.inventoryScript.RemoveItem("orangeball");
			craftingScript.inventoryScript.RemoveItem("wheelmetal");
			heldItem=false;


				currentState=PlayerState.Idle;
			
		}
		
		GUI.TextField(new Rect(10,10,100,20),"Score; " +score);
		switch(currentStage)
		{
		case("customerIntro"):
			part=1;
			GUI.Box(new Rect(Screen.width/2,Screen.height/12+30,160,50),"We have a new customer");
			//highlight customer anim
			//StartCoroutine("CustomerWant");
			//unhighlight
			if(GUI.Button(new Rect(Screen.width/2,Screen.height/2,120,40),"Click here\n to continue"))
			{
				currentStage="customerWant";
			}
			break;
		case("customerWant"):
			GUI.Box(new Rect(Screen.width/2,Screen.height/16,270,100),"Looks like they want a basketball!\nThe customer will\n always say what \nitem they want");
			//highlight customerwant
			if(GUI.Button(new Rect(Screen.width/2,Screen.height/2,120,40),"Click here\n to continue"))
			{
				currentStage="grabItems";
			}
			//unhighlight
			break;
		case("grabItems"):
			GUI.Box(new Rect(Screen.width/2,Screen.height/16,200,100),"Go grab the items \n needed to make the basketball");
			//highlight orange paint anim
			//highlight ball anim
			playerHasControl=true;
			if(gotitem1&&gotitem2)
			{
				readytocraft=true;
				Debug.Log ("bob");
			}
			//unhighlight
			//unhighlight
			break;
		case("craftItems"):
			playerHasControl = false;
			GUI.Box(new Rect(Screen.width/2,Screen.height/16,200,50),"Good! Now craft them \n into something nice!");
			playerHasControl=true;
			//highlight craft bench anim
			//unhighlight
			break;
		case("serveCustomer"):
			readytocraft=false;
			playerHasControl=false;
			GUI.Box(new Rect(Screen.width/2,Screen.height/12+30,200,50),"Ok! Now you need to take \n the item to the till");
			playerHasControl=true;
			//highlight till anim
			//unhighlight
			break;
		case("done!"):
			playerHasControl=false;
			GUI.Box(new Rect(Screen.width/2,Screen.height/12+30,200,50),"Congratulations, you've served \n your first customer");
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
			GUI.Box (new Rect(Screen.width/2,Screen.height/16,200,100),"We have another customer! \n This one wants a bike!");
			customerSpawnScript.AddingTutorialCustomer ();
			if(GUI.Button(new Rect(Screen.width/2,Screen.height/2,120,40),"Click here\n to continue"))
			{
				currentStage="Crafting2";
			}
			break;
	
		case("Crafting2"):
			playerHasControl=true;
			GUI.Box (new Rect(Screen.width/2,Screen.height/16,250,100),"Now, grab the\n items needed to\n make a bike\n and craft it.");
			if(gotitem3&&gotitem4)
			{
				readytocraft=true;
			}
			if(itemCrafted)
			{
				currentStage="serve2";
			}
			break;
		case("serve2"):
			readytocraft=false;
			GUI.Box(new Rect(Screen.width/2,Screen.height/16,200,70),"Ok! Now you need to take \n the item to the till");
			break;
		case("finallyDone!"):
		
			GUI.Box(new Rect(Screen.width/2,Screen.height/12+30,200,70),"Congratulations, you've served \n another customer");
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
		yield return new WaitForSeconds(3.0f);
		craftingScript.crafted=false;
	}
}