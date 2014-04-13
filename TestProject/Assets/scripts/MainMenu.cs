using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GUISkin MyGUISkin;
	public Texture2D thing;

	void OnGUI () {
 		
	
		GUI.skin = MyGUISkin;
		GUILayout.BeginArea(new Rect((Screen.width/4-150),(Screen.height/2-20),200,200));
		GUILayout.BeginVertical();
		if(GUILayout.Button("Level Select"))
		{
			Application.LoadLevel("LevelSelect");
		}

		if(GUILayout.Button("Quit"))
		{
			Application.Quit();
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

}
