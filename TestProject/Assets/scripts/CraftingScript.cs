using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CraftingScript : MonoBehaviour 
{
	public TheInventoryScript inventoryScript;
	public Dictionary<string, string> TherecipeList = new Dictionary<string,string >();
	public List<string> recipeList;
	public List<string> craftingItems;
	public GameObject[] Customer;
	public List<GameObject> recipeItems;
	public GameObject customerTemplate;
	public tutorialCharacterControllerScript tutCharacterControllerScript;
	public CharacterControllerScript characterControllerScript;
	string currentScene;
	public bool itemCrafted;

	// Use this for initialization
	void Start () 
	{
		currentScene = Application.loadedLevelName;
		if(currentScene == "tutorialScene")
		{
			inventoryScript = GetComponent<TheInventoryScript>();
			tutCharacterControllerScript = GetComponent<tutorialCharacterControllerScript>();
			characterControllerScript = GetComponent<CharacterControllerScript>();
			recipeList.Add("orangeball");
			TherecipeList.Add("basketball","orangeball");
		}
		else
		{
		inventoryScript = GetComponent<TheInventoryScript>();
		tutCharacterControllerScript = GetComponent<tutorialCharacterControllerScript>();
		recipeList.Add("orangeball");
		recipeList.Add ("wheelmetal");
		TherecipeList.Add("basketball","orangeball");
		TherecipeList.Add("bike","wheelmetal");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
	public bool Craft(string _item1, string _item2)
	{
		itemCrafted = false;
		
		if(recipeList.Contains(_item1+_item2))
		{
			itemCrafted = true;
			inventoryScript.AddItem(_item1+_item2);
			characterControllerScript.Crafted=true;
			
		}else if(recipeList.Contains(_item2+_item1))
		{
			itemCrafted = true;
			inventoryScript.AddItem(_item2+_item1);
			characterControllerScript.Crafted=true;
		}
		tutCharacterControllerScript.currentStage="serveCustomer";
		Debug.Log ("served");
		return itemCrafted;	
	}
}
