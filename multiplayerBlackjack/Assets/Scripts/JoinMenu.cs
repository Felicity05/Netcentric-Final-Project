using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinMenu : MonoBehaviour {

	public void JoinGame()
    {
        //add the name of the next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //FadeOut(SceneManager.GetActiveScene().buildIndex + 1);

    }

    //public void FadeOut(int levelIndex){
    //    screenAnimation.SetTrigger("FadeOut");
    //}
}
