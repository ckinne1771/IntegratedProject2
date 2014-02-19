using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewInventoryScript : MonoBehaviour 
{
	public Dictionary<string, int> allItems = new Dictionary<string, int>();
	public Dictionary<string, Texture2D> allItemImages = new Dictionary<string, Texture2D>();
	public Dictionary<string, int> allComponents = new Dictionary<string, int >();
	public Dictionary<string, Texture2D> allComponentImages = new Dictionary<string, Texture2D>();
	public Dictionary<string, int> playerInventory = new Dictionary<string, int >();
	public List<string> itemsAvailible;
	public List<Texture2D> availibleItemImages;
	public List<string> componentsAvailible;
	public List<Texture2D> availibleComponentImages;
	public List<int> itemIdentifier;
	public List<int> componentIdentifier;
	
	// Use this for initialization
	void Start () 
	{
		for(int i = 0; i < itemsAvailible.Count; i++)
		{
			allItems.Add(itemsAvailible[i], itemIdentifier[i]);
			allItemImages.Add(itemsAvailible[i], availibleItemImages[i]);
		}
		for(int i = 0; i < componentsAvailible.Count; i++)
		{
			allComponents.Add(componentsAvailible[i], componentIdentifier[i]);
			allComponentImages.Add(componentsAvailible[i], availibleComponentImages[i]);
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




