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

	public GUISkin MyGUISkin;
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
	public Texture2D images;
	public GameObject customerTemplate;
	public List<GameObject> components;
	public GameObject component;
	public int score;
	private int scoreModifier;
	public float timer;
	public bool Crafted;
	public Animator anim;
	public Texture2D slot1Image;
	public Texture2D slot2Image;
	public List<Texture2D> ComponentSprites;
	// Use this for initialization
	void Start () 
	{
		anim=GetComponent<Animator>();
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
		//InvokeRepeating("Countdown",1f,1f);
		
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
						audio.PlayOneShot(popsound);
						inventoryScript.AddItem(hit.transform.gameObject.name);
						collectItems(hit.transform.gameObject.name);
						//InstantiateComponents(hit.transform.gameObject);
						anim.SetBool("issideview",false);
					}
				}

				else if(hit.transform.gameObject.tag=="craftingtable")
				{
					this.gameObject.transform.position = (CraftingTable.transform.position + new Vector3(-2,0,0));
					anim.SetBool("issideview",true);
					currentState = PlayerState.Crafting;
					
				}
				else if(hit.transform.gameObject.tag=="Till")
				{
					this.gameObject.transform.position = (Till.transform.position + new Vector3(0,-1,0));
					currentState = PlayerState.Serving;
					anim.SetBool("issideview",false);
				}
				else if(hit.transform.gameObject.tag=="recyclingbin")
				{
					this.gameObject.transform.position = (RecyclingBin.transform.position + new Vector3(0,-1,0));
					currentState = PlayerState.Recycling;
					anim.SetBool("issideview",false);
				}
			}
			
		}

		/*if (timer>40)
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
		}*/

	}

	/*void Countdown()
	{
		timer--;
		Debug.Log (timer);
	}*/
	
	
	
	//handles player states
	public void OnGUI()
	{
		GUI.skin = MyGUISkin;
		if(craftingScript.itemCrafted&&craftingScript.crafted)
		{
			GUI.Box (new Rect(Screen.width/2,Screen.height/4,110,50),"Crafted!");
			StartCoroutine("WaitTime");
		}

		GUI.Box (new Rect ((Screen.width/2) -47, Screen.height - 110, 185,75),"Inventory");
		GUI.skin=guiskin2;
		GUI.Box (new Rect((Screen.width/2) - 40, Screen.height - 60, 80,50),slot1Image);
		GUI.Box (new Rect((Screen.width/2) + 50, Screen.height - 60, 80,50),slot2Image);

		if(currentState == PlayerState.Crafting )
		{
			
			if(craftingScript.Craft(item1, item2))
			{
				inventoryScript.RemoveItem(item1);
				inventoryScript.RemoveItem(item2);
				slot1Image=null;
				slot2Image=null;

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
			slot1Image=null;
			slot2Image=null;
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
				ScoreModifier();
				score += (20*scoreModifier);
				Debug.Log(customerSpawnScript.GetFrontOfQueueOrder().timer);
				customerSpawnScript.GetFrontOfQueueOrder().timer=60;
				audio.PlayOneShot(tillsound);
			}
			currentState=PlayerState.Idle;
			
		}
		GUI.skin=MyGUISkin;
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

	public void ScoreModifier()
	{
	timer=customerSpawnScript.GetFrontOfQueueOrder().timer;
	if (timer>49)
	{
		scoreModifier=3;
	}
	
	if (timer <50 && timer>29)
	{
		scoreModifier=2;
	}
	if (timer <30 && timer>0)
	{
		scoreModifier=1;
	}
	}
	
}