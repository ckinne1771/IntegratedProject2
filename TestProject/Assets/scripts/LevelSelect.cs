using UnityEngine;
using System.Collections;

public class LevelSelect : MonoBehaviour {

	public GUISkin MyGUISkin;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () 
	{
		GUI.skin = MyGUISkin;
		GUILayout.BeginArea(new Rect(Screen.width/2-(Screen.width/10),Screen.height/4-50,Screen.width/6,Screen.height));
		GUILayout.BeginVertical();
		GUILayout.Box("Level Select");
		if(GUILayout.Button("Tutorial",GUILayout.Height(Screen.height/8)))
		{
			Application.LoadLevel("tutorialScene");
		}
		if(GUILayout.Button("2",GUILayout.Height(Screen.height/12)))
		{
			Application.LoadLevel("InventoryTest");
		}
		if(GUILayout.Button("3",GUILayout.Height(Screen.height/12)))
		{
			Application.LoadLevel("Level2");
		}
		if(GUILayout.Button("4",GUILayout.Height(Screen.height/12)))
		{
			Application.LoadLevel("Level3");
		}
		if(GUILayout.Button("Back",GUILayout.Height(Screen.height/12)))
		{
			Application.LoadLevel("Project");
		}
		GUILayout.EndVertical();
		//GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
