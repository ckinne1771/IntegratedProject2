﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CraftingScript : MonoBehaviour 
{
	public CharacterControllerScript characterControllerScript;
	public TheInventoryScript inventoryScript;
	public string item;
	public List<string> recipeList;
	public List<string> craftingItems;

	// Use this for initialization
	void Start () 
	{
		characterControllerScript = GetComponent<CharacterControllerScript>();
		inventoryScript = GetComponent<TheInventoryScript>();
		recipeList.Add("dogbowl");
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
		foreach(string thing in recipeList)
		{
			if(thing.Equals(item))
			{
				Debug.Log (item);
				inventoryScript.playerInventory.Add (item);
			}
	
		}

		characterControllerScript.currentState = CharacterControllerScript.PlayerState.Idle;
	}
}
