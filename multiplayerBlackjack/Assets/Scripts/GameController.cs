using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private string userName;

    [SerializeField]
    private InputField inputField;

    private bool ok = false;

    //dictionary where key is the userName and the value is the stake
    public Dictionary<string, int> usersMoney = new Dictionary<string, int>();

    //validate user input
    public bool isInputValid()
    {
        //TODO 
        //check if input is empty
        if (inputField.text == ""){
            Debug.LogError("empty field");
            return false;
        } //check if entered only first name
        else if(!inputField.text.Contains(" ")){
            Debug.LogError("need last name also");
            return false;
        } //check if entered numbers
        //check if entered invald characters

        return true;
    }

    public void GenerateUserName(string text){

        if (isInputValid())
        {

            int code = Random.Range(100, 500);

            string[] input = text.Split(); //split the input by space

            string uname = input[0].Substring(0, 1); //get the first letter of the name

            string lastName = input[1].Substring(0, 4); //get the first 4 letters of the last name

            userName = uname + lastName + code; //generate the user name 

            inputField.text = "";

            ok = true;
        }

    } //end of function


    public void JoinGame()
    {
        //add that when hitting enter also pases to the game

        if (ok)
        {
            usersMoney.Add(userName, 50); //initial stake, it updates whenever the user win or loose a bet while playing

            //log to console user and value
            foreach (string key in usersMoney.Keys)
            {
                int val = usersMoney[key];
                Debug.Log(key + " joined with an initial stake of: " + val);
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }




    //generate dictionary where key is the userName and the value is the bet


} //end of class
