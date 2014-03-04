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
		recipeList.Add("orangeball");
		recipeList.Add ("wheelmetal");
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	public bool Craft(string item1, string item2)
	{
		bool itemCrafted = false;
	
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
		
	}
}
