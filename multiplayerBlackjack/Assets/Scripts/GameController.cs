using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private string userName;

    private List<string>users = new List<string>();


	// Use this for initialization
	void Start () {

        Debug.Log(users + "list cpacity"+ users.Capacity);
		
	}


    //validate user input
    public void isInputValid(string input){}
        //TODO 
        //check if input is empty
        //check if entered only first name
        //check if entered numbers
        //check if entered invald characters



    public void GenerateUserName(string text){
        
        int code = Random.Range(100, 500);

        string[] input = text.Split(); //split the input by space

        string uname = input[0].Substring(0, 1); //get the first letter of the name

        string lastName = input[1].Substring(0, 4); //get the first 4 letters of the last name

        userName = uname + lastName + code; //generate the user name 

        // if the list is empty add element to the list
        if (users.Capacity == 0)
        {
            users.Add(userName);
            Debug.Log("empty list add element");
        }
        else if (!users.Contains(userName)) //if not empty and userName not in list
        { 
            users.Add(userName);

            foreach (string s in users)
            {
                Debug.Log("your user name is: " + s);
            }

        } else{
            
            Debug.LogError("user name repeated");

        }

    } //end of function


    //toggle function to accept the stake


    //generate dictionary where key is the userName and the value is the initial stake
    //generate dictionary where key is the userName and the value is the bet


} //end of class
