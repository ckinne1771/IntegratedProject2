using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheInventoryScript : MonoBehaviour 
{
	//this class is used to keep track of the players inventory and add and remove items from it

	public Dictionary<string, int> playerInventory = new Dictionary<string,int>();

	//adds an item to the inventory
	public bool AddItem (string item)
	{
		if(!playerInventory.ContainsKey(item))
		{
			playerInventory.Add(item, 1);
		}
		return true;
	}

	//removes an item from the inventory
	public bool RemoveItem (string item)
	{
		if(playerInventory.ContainsKey(item))
		{
			playerInventory[item] -= 1;
			if(playerInventory[item] < 1)
			{
				playerInventory.Remove(item);
			}
			return true;
		}
		return false;
	}

	
}




