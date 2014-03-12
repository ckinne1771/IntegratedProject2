using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CraftingScript : MonoBehaviour 
{
	public CharacterControllerScript characterControllerScript;
	public CustomerNeedsScript customerNeedsScript;
	public TheInventoryScript inventoryScript;
	public Dictionary<string, string> TherecipeList = new Dictionary<string,string >();
	public List<string> recipeList;
	public List<string> craftingItems;
	public GameObject[] Customer;
	public List<GameObject> recipeItems;
	public GameObject customerTemplate;

	// Use this for initialization
	void Start () 
	{
		characterControllerScript = GetComponent<CharacterControllerScript>();
		inventoryScript = GetComponent<TheInventoryScript>();
		recipeList.Add("orangeball");
		recipeList.Add ("wheelmetal");
		TherecipeList.Add("basketball","orangeball");
		TherecipeList.Add("bike","wheelmetal");
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	public bool Craft(string _item1, string _item2)
	{
		bool itemCrafted = false;
		
		if(recipeList.Contains(_item1+_item2))
		{
			itemCrafted = true;
			inventoryScript.AddItem(_item1+_item2);
			
		}else if(recipeList.Contains(_item2+_item1))
		{
			itemCrafted = true;
			inventoryScript.AddItem(_item2+_item1);
		}
		return itemCrafted;	
	}
}
