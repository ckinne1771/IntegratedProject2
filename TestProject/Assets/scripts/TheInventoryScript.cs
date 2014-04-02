using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheInventoryScript : MonoBehaviour 
{
	public List<string> allComponents;
	public List<Texture2D> allComponentImages;
	public Dictionary<string, Texture2D> componentInventory = new Dictionary<string,Texture2D>();
	public Dictionary<string, int> playerInventory = new Dictionary<string,int>();
	
	// Use this for initialization
	void Start () 
	{

		for(int i = 0; i <allComponents.Count; i++)
		{
			componentInventory.Add(allComponents[i], allComponentImages[i]);
		}
	}

	
	// Update is called once per frame
	void Update () 
	{

	}

	public bool AddItem (string item)
	{
		if(playerInventory.ContainsKey(item))
		{
			playerInventory[item] += 1;
		}
		else
		{
			playerInventory.Add(item, 1);
		}
		//Debug.Log ("items added");
		return true;
	}

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




