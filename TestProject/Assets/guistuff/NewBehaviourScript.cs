using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	public GUISkin MyGuiSkin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {

		GUI.skin = MyGuiSkin;

		GUI.Label(new Rect(100,100,130,70), "Label");

		GUI.Button(new Rect(100, 200,130,70), "Button");

		GUI.Box (new Rect(100, 300,130,70), "Box");
	}


}
