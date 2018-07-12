using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChat : MonoBehaviour {

    public Animator animator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenChatDialog(){

        animator.SetBool("Open", true); // opens dialog
    }

    public void CloseDialog(){

        animator.SetBool("Open", false);

        Debug.Log("closing chat");
    }

    public void AnimationComplete()
    {
        //deactivate chatDialog
        //reactivate chatButton
        //add enevent in animation
    }
}
