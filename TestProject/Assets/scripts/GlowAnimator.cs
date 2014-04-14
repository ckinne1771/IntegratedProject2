using UnityEngine;
using System.Collections;

public class GlowAnimator : MonoBehaviour 
{
	//this script is used to turn on and off the glow animation in the tutorial

	public Animator anim;
	public bool glow=false;
	// Use this for initialization
	void Start () 
	{
		anim=GetComponent<Animator>();
	}
	void Update()
	{
		if(Application.loadedLevelName=="tutorialScene")
		{
		if(glow==true)
		{
			anim.SetBool("glowing",true);
		}
		if(glow==false)
		{
			anim.SetBool("glowing",false);
		}
		}
	}
	
	// Update is called once per frame

}
