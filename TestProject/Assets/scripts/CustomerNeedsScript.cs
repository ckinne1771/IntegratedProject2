using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CustomerNeedsScript : MonoBehaviour {

	public GUISkin MyGUISkin;
	public CraftingScript theCraftingScript;
	public List<string> choice;
	public int NeededItem;
	public int randomNumber;
	public bool itemNeeded;
	public string itemRequested;
	public GameObject Player;
	public CustomerSpawnScript customerSpawnScript;
	public string item;
	public Vector3 customerPos;
	public Transform target;
	public string firstItem;
	public string secondItem;
	string currentScene;
	public string first;
	public string second;
	
	// Use this for initialization
	void Start () 
	{
		//itemNeeded = true;
		currentScene=Application.loadedLevelName;
		Player = GameObject.FindGameObjectWithTag("Player");
		theCraftingScript= Player.GetComponent<CraftingScript>();
		AddingToList();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		target= this.gameObject.transform;
		customerPos = Camera.main.WorldToScreenPoint(target.position);

	}
	
	public void AddingToList(){
		
		choice = new List<string>(theCraftingScript.TherecipeList.Keys);
		randomNumber = Random.Range(0,choice.Count);
		NeededItem = randomNumber;
		itemRequested = choice[NeededItem];
		item = theCraftingScript.TherecipeList[itemRequested];
		firstItem  =choice[0];
		first = theCraftingScript.TherecipeList[firstItem];
		secondItem =choice[1];
		second = theCraftingScript.TherecipeList[secondItem];
		
	}
	
	void OnGUI()
	{
		if(itemNeeded==true)
		{
			if(currentScene=="tutorialScene")
			{
				if(tutorialCharacterControllerScript.part==1)
				{
					GUI.Box(new Rect(customerPos.x,Screen.height-customerPos.y-50,70,30),firstItem);
				}
				else if(tutorialCharacterControllerScript.part==2)
				{
					GUI.Box(new Rect(customerPos.x,Screen.height-customerPos.y-50,70,30),secondItem);
				}

		} 
			else
			{
				GUI.skin=MyGUISkin;
				GUI.Box (new Rect(customerPos.x+30,Screen.height-customerPos.y-90,110, 55),itemRequested);
			}
	}
}
}