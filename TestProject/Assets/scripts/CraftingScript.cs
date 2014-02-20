using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingScript : MonoBehaviour 
{
	public CharacterControllerScript characterControllerScript;
	public TheInventoryScript inventoryScript;
	public string item;
	public Dictionary<string, int> recipeList = new Dictionary<string,int >();
	public List<string> craftingItems;

	// Use this for initialization
	void Start () 
	{
		characterControllerScript = GetComponent<CharacterControllerScript>();
		inventoryScript = GetComponent<TheInventoryScript>();
		recipeList.Add("dogbowl",0);
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public void Craft()
	{
		//temporary test code while the inventory and gui interactions arent done yet for component selecting
		//and inventory
		//"dog" and "bowl" will later be added by the player selecting the items
		//item = "dog"+"bowl";
		if(recipeList.ContainsKey(item))
		{
			Debug.Log(item);
			inventoryScript.playerInventory.Add(item);
		}
		characterControllerScript.currentState = CharacterControllerScript.PlayerState.Idle;
	}
}
