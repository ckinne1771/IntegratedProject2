using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CraftingScript : MonoBehaviour 
{
	public CharacterControllerScript characterControllerScript;
	public CustomerNeedsScript customerNeedsScript;
	public TheInventoryScript inventoryScript;

	public List<string> recipeList;
	public List<string> craftingItems;
	public GameObject[] Customer;

	public GameObject customerTemplate;

	// Use this for initialization
	void Start () 
	{
		characterControllerScript = GetComponent<CharacterControllerScript>();
		inventoryScript = GetComponent<TheInventoryScript>();
		recipeList.Add("dogbowl");
		//customerNeedsScript.AddingToList();
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public bool Craft(string item1, string item2)
	{
		//temporary test code while the inventory and gui interactions arent done yet for component selecting
		//and inventory
		//"dog" and "bowl" will later be added by the player selecting the items
		//item = "dog"+"bowl";
		bool itemCrafted = false;
		//Debug.Log ("This is item1 and 2"+item1+ item2);

		if(recipeList.Contains(item1+item2))
		{
			itemCrafted = true;
			inventoryScript.AddItem(item1+item2);
			Debug.Log (item1+item2);
		}else if(recipeList.Contains(item2+item1))
		{
			itemCrafted = true;
			inventoryScript.AddItem(item2+item1);
			Debug.Log (item2+item1);
		}
		return itemCrafted;

		//characterControllerScript.currentState = CharacterControllerScript.PlayerState.Idle;
	}
}
