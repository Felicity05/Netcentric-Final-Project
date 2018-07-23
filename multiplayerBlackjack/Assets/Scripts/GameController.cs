using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class GameController : MonoBehaviour {

    public static GameController Instance { set; get; }

    //client and server objects
    public GameObject serverPrefab;
    public GameObject clientPrefab;

    public GameObject ConnectMenu;

    string playerName;

    public bool firstPlayer;


    //allows to erase after writing into the textfield
    [SerializeField]
    public InputField nameInput;


    //dictionary where key is the userName and the value is the stake
    public Dictionary<string, int> usersMoney = new Dictionary<string, int>();

    int ID = 0;


    public void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); //to don't destroy the game controller object
    }

    //Generates usernames for the players
    public string GenerateUserName(string text)
    {
        int code = UnityEngine.Random.Range(100, 500);
        string userName;

        //if the user enter a name and a lastname
        if (text.Contains(" "))
        {
            string[] input = text.Split(); //split the text by the space

            string uname = input[0].Substring(0, 3); //get the first letter of the name

            string lastName = input[1].Substring(0, 4); //get the first 4 letters of the last name

            userName = uname + lastName + code; //generate the user name 
        }
        else //if user only enters name (no space)
        {
            userName = text + code;
        }

        nameInput.text = ""; //to clear the text dialog after hitting enter TODO pass to the next scene when clicking enter

        return userName;
    }

    int userCode; //to generate unique codes for users ASK TITI ABOUT THIS
    //TODO REVIEW THIS FUNCTION I MAY BE DOING SOMETHING WRONG HERE 
    int GenerateUniqueCode(){
        
        while (userCode != UnityEngine.Random.Range(100, 500)) {

            userCode = UnityEngine.Random.Range(100, 500);
        }
           
        return userCode;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game1");
    }

    //run the server
    public void HostGame()
    {
        try
        {
            //create a server 
            myServer1 server1 = Instantiate(serverPrefab).GetComponent<myServer1>();
            server1.Init();

            //create a client
            myClient1 client1 = Instantiate(clientPrefab).GetComponent<myClient1>();

            //is hosting the game?
            client1.isHost = true;

            //if no name is entered
            client1.clientName = nameInput.text;
            if (client1.clientName == "")
                client1.clientName = "Host";
            else //if name entered
            {
                playerName = GenerateUserName(nameInput.text);
                client1.clientName = playerName;
            }

            //connect to localhost because you are connecting to yourself
            client1.ConnectToServer("127.0.0.1", 8000);

            firstPlayer = true;

        }
        catch (Exception ex)
        {
            Debug.Log("Error when creating the server: " + ex.Message);
        }
    }

    //client connect to server
    public void JoinGame()
    {
        //add that when hitting enter also pases to the game

        //gets the addres that's in the input field which is localhost
        string hostAdd = GameObject.Find("InputHost").GetComponent<InputField>().text;
        if (hostAdd == "")
            hostAdd = "127.0.0.1";


        //create the client
        try
        {
            myClient1 client1 = Instantiate(clientPrefab).GetComponent<myClient1>();

            client1.isHost = false;

            //if no name is entered assigned a unique client name
            client1.clientName = nameInput.text;
            if (client1.clientName == "")
                client1.clientName = "Guest" + GenerateUniqueCode().ToString();
            else  //if name entered
            {
                playerName = GenerateUserName(nameInput.text);
                client1.clientName = playerName;
            }


            //connect client to server
            client1.ConnectToServer(hostAdd, 8000);
            ConnectMenu.SetActive(false);

            firstPlayer = false;

            //TODO change scene to go to GAME
        }
        catch (Exception ex)
        {
            Debug.Log("Error when creating the client: " + ex.Message);
        }



        //dictionary that holds the initial stake of each client 
        //it updates whenever the user win or loose a bet while playing
        //usersMoney.Add(userName, 50); 

        ////print to console user and value
        //foreach (string key in usersMoney.Keys)
        //{
        //    int val = usersMoney[key];
        //    Debug.Log(key + " joined with an initial stake of: " + val);
        //}
    }


    //destroy server and client when clicking the back button
    public void DestroyServerNClient(){
        
        myServer1 s = FindObjectOfType<myServer1>();
        if (s != null){
            Destroy(s.gameObject);
        }

        myClient1 c = FindObjectOfType<myClient1>();
        if (c != null)
        {
            Destroy(c.gameObject);
        }

        Debug.Log("Client and server shutted down!");
    }


    //option for single player 
    public void Play() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


} //end of class
