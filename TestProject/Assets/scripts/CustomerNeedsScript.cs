using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CustomerNeedsScript : MonoBehaviour {
	
	public CraftingScript theCraftingScript;
	
	public List<string> choice;
	
	public int NeededItem;
	
	public int randomNumber;
	
	public bool itemNeeded;
	
	
	
	// Use this for initialization
	void Start () {
		itemNeeded = true;

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void AddingToList(){
		
		theCraftingScript = GetComponent<CraftingScript>();


		choice = new List<string>(theCraftingScript.recipeList);
		foreach(var value in choice){
			Debug.Log(value);
		}
		randomNumber = Random.Range(0,choice.Count);
		NeededItem = randomNumber;
	}
	
	void OnGUI(){
		if(itemNeeded==true){
			GUI.Box(new Rect(110,5,100,30),choice[NeededItem]);
		}
		
	}
}