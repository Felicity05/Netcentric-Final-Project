using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void JoinMenu()
    {
        //call the join menu
        Debug.Log("Spawn the join game menu");

    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
