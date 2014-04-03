using UnityEngine;
using System.Collections;

public class GlowAnimator : MonoBehaviour 
{
	public Animator anim;
	public bool glow=false;
	// Use this for initialization
	void Start () 
	{
		anim=GetComponent<Animator>();
		//anim.SetBool("glowing",false);
		//glow=false;
	}
	void Update()
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
	
	// Update is called once per frame

}
