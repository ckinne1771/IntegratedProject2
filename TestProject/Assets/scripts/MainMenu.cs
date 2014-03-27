using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GUISkin MyGUISkin;


	void OnGUI () {

		GUI.skin = MyGUISkin;
		GUILayout.BeginArea(new Rect((Screen.width/2-150),(Screen.height/2-75),300,150));
		GUILayout.BeginVertical();
		if(GUILayout.Button("Level Select"))
		{
			Application.LoadLevel("LevelSelect");
		}

		if(GUILayout.Button("MileStones"))
		{

		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

}
