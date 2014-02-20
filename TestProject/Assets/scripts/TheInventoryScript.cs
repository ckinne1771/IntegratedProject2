using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheInventoryScript : MonoBehaviour 
{
	public List<string> allComponents;
	public List<Texture2D> allComponentImages;
	public Dictionary<string, Texture2D> componentInventory = new Dictionary<string,Texture2D>();
	public List<string> playerInventory;
	public Dictionary<string, Texture2D> playerInventoryImages = new Dictionary<string, Texture2D>();
	/*public List<string> itemsAvailable;
	public List<Texture2D> availableItemImages;
	public List<string> componentsAvailable;
	public List<Texture2D> availableComponentImages;
	//public List<int> itemIdentifier;
	//public List<int> componentIdentifier;*/
	
	// Use this for initialization
	void Start () 
	{
		/*for(int i = 0; i < itemsAvailable; i++)
		{
			allItems.Add(itemsAvailable[i], 0);
		}*/
		for(int i = 0; i <allComponents.Count; i++)
		{
			//allComponents.Add(componentsAvailable[i], componentIdentifier[i]);
			componentInventory.Add(allComponents[i], allComponentImages[i]);
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
		/*
		if(Input.GetButtonDown("Fire1"))
		{
			RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition;
			if(hit.collider.name("Ball"))
			 {
					playerInventory.Add (allItems.Key[0], allItems.Value[0]);
				}
				}

*/

		
	}
	
}




