using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CustomerNeedsScript : MonoBehaviour {
	
	public CraftingScript theCraftingScript;

	public CustomerAIScript AIscript;
	
	public List<string> choice;
	
	public int NeededItem;
	
	public int randomNumber;
	
	public bool itemNeeded;
	
	public string itemRequested;

	public GameObject Player;

	//public bool orderCompleted;
	
	// Use this for initialization
	void Start () 
	{

		itemNeeded = true;
		Player = GameObject.Find("Player");
		theCraftingScript= Player.GetComponent<CraftingScript>();
		AIscript = GetComponent<CustomerAIScript>();

	}

	public void AddingToList(){
		
		
		choice = new List<string>(theCraftingScript.recipeList);

		
	}

	public void RandomList(){
		//randomNumber = Random.Range(0,choice.Count);
		//NeededItem = randomNumber;
		itemRequested = choice[0];
		Debug.Log (choice[0]);
	}
	
	// Update is called once per frame
	void Update () {

		if (itemNeeded == false){
			AIscript.speed=2;
		}



	}


	 

	 
	void OnGUI(){
		if(itemNeeded==true)
		{
		

			//GUI.Box(new Rect(110,5,100,30),choice[0]); 
			GUI.Box (new Rect(110,5,100,30),"Dogbowl");

		}
		
	}
}