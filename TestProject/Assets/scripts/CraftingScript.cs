using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CraftingScript : MonoBehaviour 
{
	//dictionary of items that can be crafted
	public Dictionary<string, string> TherecipeList = new Dictionary<string,string >();
	public List<string> recipeList;
	//referenced scripts
	public TheInventoryScript inventoryScript;
	public tutorialCharacterControllerScript tutCharacterControllerScript;
	public CharacterControllerScript characterControllerScript;
	//current scene
	string currentScene;
	public bool itemCrafted;
	public bool crafted=false;


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
			recipeList.Add("wheelmetal");
			TherecipeList.Add ("bike","wheelmetal");
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

	//checks to see if the items to be crafted make an item, and if so, crafts the item
	//the inventory is then cleared
	public bool Craft(string _item1, string _item2)
	{
		itemCrafted = false;
		
		if(recipeList.Contains(_item1+_item2))
		{
			itemCrafted = true;
			inventoryScript.AddItem(_item1+_item2);
			tutCharacterControllerScript.itemCrafted=true;
			tutCharacterControllerScript.currentStage="serveCustomer";
			crafted=true;
			tutCharacterControllerScript.audio.Play();
			characterControllerScript.audio.Play();
		}
		else if(recipeList.Contains(_item2+_item1))
		{
			itemCrafted = true;
			inventoryScript.AddItem(_item2+_item1);
			tutCharacterControllerScript.itemCrafted=true;
			tutCharacterControllerScript.currentStage="serveCustomer";
			crafted= true;
			tutCharacterControllerScript.audio.Play();
			characterControllerScript.audio.Play();
		}
		inventoryScript.RemoveItem (_item1);
		inventoryScript.RemoveItem (_item2);
		return itemCrafted;
	}
	
}
