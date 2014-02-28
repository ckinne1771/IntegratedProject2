using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CraftingScript : MonoBehaviour 
{
	public CharacterControllerScript characterControllerScript;
	public CustomerNeedsScript customerNeedsScript;
	public TheInventoryScript inventoryScript;
	public string item;
	public List<string> recipeList;
	public List<string> craftingItems;
	public GameObject Customer;
	public string item1;
	public string item2;

	// Use this for initialization
	void Start () 
	{
		Customer = GameObject.Find("Customer");
		customerNeedsScript = Customer.GetComponent<CustomerNeedsScript>();
		characterControllerScript = GetComponent<CharacterControllerScript>();
		inventoryScript = GetComponent<TheInventoryScript>();
		recipeList.Add("dogbowl");
		customerNeedsScript.AddingToList();
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
		item1=characterControllerScript.item1;
		item2=characterControllerScript.item2;
		Debug.Log ("This is item1 and 2"+item1+ item2);
		foreach(string recipe in recipeList)
		{
			if(recipe.Equals(item1+item2)||recipe.Equals(item2+item1))
			{
				Debug.Log (recipe);
				inventoryScript.playerInventory.Add (recipe,1);
			}
	
		}

		characterControllerScript.currentState = CharacterControllerScript.PlayerState.Idle;
	}
}
