using UnityEngine;
using System.Collections;

//this script controls the menu scene
public class MainMenu : MonoBehaviour {

	public GUISkin MyGUISkin;
	public Texture2D thing;

	void OnGUI () {
 		
		//gui used to go to the level select scene or quit the game
		GUI.skin = MyGUISkin;
		GUILayout.BeginArea(new Rect((Screen.width/4-150),Screen.height/2-40,200,200));
		GUILayout.BeginVertical();
		if(GUILayout.Button("Level Select",GUILayout.Height(Screen.height/10)))
		{
			Application.LoadLevel("LevelSelect");
		}

		if(GUILayout.Button("Quit",GUILayout.Height(Screen.height/10)))
		{
			Application.Quit();
		}
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

}
