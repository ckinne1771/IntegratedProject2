using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CustomerNeedsScript : MonoBehaviour {
	
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
	
	// Use this for initialization
	void Start () 
	{
		//itemNeeded = true;
		Player = GameObject.Find("Player");
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
		
	}
	
	void OnGUI(){
		if(itemNeeded==true)
		{
			GUI.Box(new Rect(customerPos.x,Screen.height-customerPos.y-50,70,30),itemRequested);
		}
		
	}
}