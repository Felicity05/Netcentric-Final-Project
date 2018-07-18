using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameController : MonoBehaviour {

    public static GameController Instance { set; get; }

    public GameObject serverPrefab;
    public GameObject clientPrefab;

    public GameObject ConnectMenu;
    public InputField nameInput;

    private int sceneIndex = 0;


    private string userName;

    [SerializeField]
    private InputField inputField;

    private bool ok = false;


    //dictionary where key is the userName and the value is the stake
    public Dictionary<string, int> usersMoney = new Dictionary<string, int>();

   

    public void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject); //to don't destroy the game controller object
    }

    //validate user input
    public bool isInputValid()
    {
        //TODO 
        //check if input is empty
        if (inputField.text == ""){
            Debug.Log("empty field");
            return false;
        } //check if entered only first name
        else if(!inputField.text.Contains(" ")){
            Debug.Log("need last name also");
            return false;
        } //check if entered numbers
        //check if entered invald characters

        return true;
    }

    public void GenerateUserName(string text){

        if (isInputValid())
        {

            int code = UnityEngine.Random.Range(100, 500);

            string[] input = text.Split(); //split the input by space

            string uname = input[0].Substring(0, 1); //get the first letter of the name

            string lastName = input[1].Substring(0, 4); //get the first 4 letters of the last name

            userName = uname + lastName + code; //generate the user name 

            inputField.text = "";

            ok = true;
        }

    } //end of function


    public void HostGame(){

        Debug.Log("host game");

        try
        {
            myServer1 server1 = Instantiate(serverPrefab).GetComponent<myServer1>();
            server1.Init();

            myClient1 client1 = Instantiate(clientPrefab).GetComponent<myClient1>();
            client1.clientName = nameInput.text;
            if (client1.clientName == null)
                client1.clientName = "Host";

            client1.ConnectToServer("127.0.0.1", 8000);

        }
        catch (Exception ex)
        {
            Debug.Log("Error when creating the server: " + ex.Message);
        }
    }


    public void JoinGame()
    {
        //add that when hitting enter also pases to the game
        Debug.Log("connect");

        ////TODO add input field for port as well

        //gets the addres that's in the input filed which is localhost
        string hostAdd = GameObject.Find("InputHost").GetComponent<InputField>().text;
        if (hostAdd == "")
            hostAdd = "127.0.0.1";


        //create the client
        try
        {
            myClient1 client1 = Instantiate(clientPrefab).GetComponent<myClient1>();
            client1.clientName = nameInput.text;
            if (client1.clientName == null)
                client1.clientName = "Client";

            client1.ConnectToServer(hostAdd, 8000);
            ConnectMenu.SetActive(false);


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
    public void BackButton(){
        
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
    public void Play(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


    //generate dictionary where key is the userName and the value is the bet


} //end of class
