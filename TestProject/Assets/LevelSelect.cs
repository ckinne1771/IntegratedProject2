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
		GUILayout.BeginArea(new Rect(0,Screen.height/2-(Screen.height/4),Screen.width,Screen.height));
		//GUILayout.BeginVertical();
		//GUILayout.Box("Level Select");

		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("Tutorial",GUILayout.Height(Screen.height/4)))
		{
			Application.LoadLevel("tutorialScene");
		}
		if(GUILayout.Button("2",GUILayout.Height(Screen.height/4)))
		{
			Application.LoadLevel("InventoryTest");
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if(GUILayout.Button("3",GUILayout.Height(Screen.height/4)))
		{
			//Application.LoadLevel("3");
		}
		if(GUILayout.Button("4",GUILayout.Height(Screen.height/4)))
		{
			//Application.LoadLevel("4");
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		//GUILayout.EndVertical();
		GUILayout.EndArea();
	}
}
