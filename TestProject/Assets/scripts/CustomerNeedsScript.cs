using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CustomerNeedsScript : MonoBehaviour {
	
	public CraftingScript theCraftingScript;
	public List<string> choice;
	public int NeededItem;
	public int randomNumber;
	public bool itemNeeded;
	public string itemRequested;
	public GameObject Player;
	public CustomerSpawnScript customerSpawnScript;


	
	// Use this for initialization
	void Start () 
	{
		//itemNeeded = true;
		Player = GameObject.Find("Player");
		theCraftingScript= Player.GetComponent<CraftingScript>();
		AddingToList();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void AddingToList(){

		choice = new List<string>(theCraftingScript.recipeList);
		randomNumber = Random.Range(0,choice.Count);
		NeededItem = randomNumber;
		itemRequested = choice[NeededItem];
	}
	
	void OnGUI(){
		if(itemNeeded==true)
		{
			GUI.Box(new Rect(110,5,100,30),itemRequested);
		}
		
	}
}