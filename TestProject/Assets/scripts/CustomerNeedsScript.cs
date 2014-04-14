using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CustomerNeedsScript : MonoBehaviour {

	public Animator anim;
	public GUISkin MyGUISkin;
	public CraftingScript theCraftingScript;
	public CharacterControllerScript characterControllerScript;
	public List<string> choice;
	public int NeededItem;
	public int randomNumber;
	public bool itemNeeded;
	public string itemRequested;
	public GameObject Player;
	public CustomerSpawnScript customerSpawnScript;
	public FollowTheWaypoints followTheWaypoints;
	public string item;
	public Vector3 customerPos;
	public Transform target;
	public string firstItem;
	public string secondItem;
	string currentScene;
	public string first;
	public string second;
	public float timer=60;
	public bool waiting;

	
	// Use this for initialization
	void Start () 
	{
		anim=gameObject.GetComponent<Animator>();
		currentScene=Application.loadedLevelName;

		//sets timer for customer to get impatient and leave
		if(currentScene=="tutorialScene" || currentScene=="InventoryTest")
		{
			timer=60;
		}

		if(currentScene=="Level2")
		{
			timer=40;
		}
		if(currentScene=="Level3")
		{
			timer=30;
		}

		Player = GameObject.FindGameObjectWithTag("Player");
		theCraftingScript= Player.GetComponent<CraftingScript>();
		characterControllerScript=Player.GetComponent<CharacterControllerScript>();
		followTheWaypoints=GetComponent<FollowTheWaypoints>();
		AddingToList();
		InvokeRepeating("Countdown",1f,1f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		target= this.gameObject.transform;
		customerPos = Camera.main.WorldToScreenPoint(target.position);
		EmotionControl();
	}

	//sets the customers' wanted item
	public void AddingToList(){
		
		choice = new List<string>(theCraftingScript.TherecipeList.Keys);
		randomNumber = Random.Range(0,choice.Count);
		NeededItem = randomNumber;
		itemRequested = choice[NeededItem];
		item = theCraftingScript.TherecipeList[itemRequested];
		firstItem  =choice[0];
		first = theCraftingScript.TherecipeList[firstItem];
		secondItem =choice[1];
		second = theCraftingScript.TherecipeList[secondItem];
		
	}
	//acts as a timer
	void Countdown()
	{
		timer--;
	}
	/*
	 * This code sets the customers emotion animation
	 */ 
	public void EmotionControl()
	{
		if(currentScene=="tutorialScene" || currentScene=="InventoryTest")
		{
		if(timer<=59&&followTheWaypoints.wait==true)
		{
			anim.SetTrigger("inQueue");
		}
		
		if(timer <40 && timer>29&&followTheWaypoints.wait==true)
		{
			anim.SetTrigger("wait1");
		}
		if(timer <30 && timer>0&&followTheWaypoints.wait==true)
		{
			anim.SetTrigger("wait2");
		}
		}

		if(currentScene=="Level2")
		{

			if(timer<=39&&followTheWaypoints.wait==true)
				{
					anim.SetTrigger("inQueue");
				}
				
			if (timer <30 && timer>20&&followTheWaypoints.wait==true)
				{
					anim.SetTrigger("wait1");
				}
			if (timer <20 && timer>0&&followTheWaypoints.wait==true)
				{
					anim.SetTrigger("wait2");
				}
	
		}

		if(currentScene=="Level3")
		{

			if(timer<=29&&followTheWaypoints.wait==true)
			{
				anim.SetTrigger("inQueue");
			}
			
			if (timer <25 && timer>15&&followTheWaypoints.wait==true)
			{
				anim.SetTrigger("wait1");
			}
			if (timer <15 && timer>0&&followTheWaypoints.wait==true)
			{
				anim.SetTrigger("wait2");
			}
			 
			
		}
	}
	/*
	 * This code displays the customer's wanted item
	 */
	void OnGUI()
	{
		GUI.skin=MyGUISkin;
		if(currentScene=="tutorialScene")
		{
			if(tutorialCharacterControllerScript.part==1)
			{
				GUI.Box(new Rect(customerPos.x,Screen.height-customerPos.y-90,120,60),firstItem);
			}
			else if(tutorialCharacterControllerScript.part==2)
			{
				GUI.Box(new Rect(customerPos.x,Screen.height-customerPos.y-90,120,60),secondItem);
			}
			
		} 
		else
		{
			GUI.Box (new Rect(customerPos.x+30,Screen.height-customerPos.y-90,120, 60),itemRequested);
		}

}
}