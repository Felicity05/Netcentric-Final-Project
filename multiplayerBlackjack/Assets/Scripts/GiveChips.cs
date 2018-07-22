using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveChips : MonoBehaviour {

    public static GiveChips Instance { set; get; }

    public GameObject chip1Prefab;
    public GameObject chip5Prefab;
    public GameObject chip10Prefab;
    public GameObject chip25Prefab;

    public List<int> bet = new List<int>();

    public Text balance;
    public Text playerBet;

    int chipValue;

    public int iniBalance = 50;
    public int inBet = 0;

    Vector3 dealerPos = new Vector3(-0.07f, 0.325f, 0.953f);

    Vector3 playerPos = new Vector3(-0.07f, 0.323f, 0.953f);

    bool changePos;

    GameObject[] chips;

    int minBet = 5;
    int maxBet = 25;

    //chips buttons
    public Button chip1;
    public Button chip5;
    public Button chip10;
    public Button chip25;
    public Button deal;


    bool isServer; //if the player is the actual server then it is the first one in playing

    public bool betSelected; //to let know the server the bet is been selected


    //get the list of clients from the server
    public myServer1 server1;

    myClient1 client1;

    string msg = "CBET|";

    public int chipVal;

    int numPlayers;

    int player1;
    int player2;


    //HERE IS WHERE THE PLAYER STARTS TO PLAY BY PLACING THE MIN BET AND ENABLING THE DEAL BUTTON TO BEGING RECEIVING CARDS

    /* players turns 
     */


    // Use this for initialization
    void Start () {

        Instance = this;

        balance.text = "$ " + iniBalance.ToString();

        client1 = FindObjectOfType<myClient1>();

        //PlayersTurn();

        Debug.Log("player1: "+ client1.players[1].name + "player2: " + client1.players[2].name);

	}
	
	// Update is called once per frame
	void Update () {
        
        chips = GameObject.FindGameObjectsWithTag("Chip");
	}

    void PlayersTurn(){

        if (client1.isHost) {
            EnableChips();
            player1 = 1;
        }
        else {
            DisableChips();
            player2 = 2;
        }
    }

    public void IdentifyChip1()
    {
        chipVal = 1;

        changePos = true;

        Vector3 startPos;

        Vector3 chipEndPos;

        if (client1.isHost) //first player
        {
            startPos = new Vector3(-0.446f, 0.33f, -1.334f);

            chipEndPos = new Vector3(-0.446f, 0.325f, -1.061f);
        } 
        else //second player
        {
            startPos = new Vector3(-0.446f, 0.33f, -1.334f);

            chipEndPos = new Vector3(-1.218f, 0.325f, -0.774f);
        }


        Debug.Log("button chip 1 cliked");

        SendPositionToServer(startPos, chipEndPos);

    }

    void SendPositionToServer(Vector3 startPos, Vector3 endPos)
    {
        //send the position to the server
        msg += chipVal + "|";

        //start position
        msg += startPos.x.ToString() + "|";
        msg += startPos.y.ToString() + "|";
        msg += startPos.z.ToString() + "|";

        //end position
        msg += endPos.x.ToString() + "|";
        msg += endPos.y.ToString() + "|";
        msg += endPos.z.ToString() + "|";

      
        //Debug.Log("data to send: " + msg);

        client1.SendData(msg);

        msg = "CBET|"; //reset the message
    }


    //ALL THE PLACE CHIP FUNCTIONS ARE CALLED WHEN THE BUTTON OF THAT CHIP IS CLICKED ON THE GAME

    public void PlaceChips(int chip, float sposX, float sposY, float sposZ, float eposX, float eposY, float eposZ)
    {
        Vector3 spos = new Vector3(sposX, sposY, sposZ);

        Vector3 epos = new Vector3(eposX, eposY, eposZ);

        chip = chipVal;

        switch (chip)
        {
            case 1:
                PlaceChip1(spos, epos);    
                break;
            
            default:
                break;
        }

    }

    void PlaceChipsByPlayer()
    {


        
    }





    //public int IdentifyChip5()
    //{
    //    int chip 5;
    //}

    //public int IdentifyChip10()
    //{
    //    int chip 10;
    //}

    //public int IdentifyChip25()
    //{
    //    int chip 25;
    //}







    //chips of 1
    public void PlaceChip1(Vector3 chip1Pos, Vector3 chip1endPos)
    {
        chipValue = 1;

        if (iniBalance - chipValue >= 0 && inBet + chipValue <= maxBet)
        {
            StartCoroutine(Chips(chip1Prefab, chip1Pos, chip1endPos));

            //Debug.Log(chipValue);

            iniBalance -= chipValue;

            inBet += chipValue;

        }

        balance.text = "$ " + iniBalance.ToString();
        playerBet.text = "$ " + inBet.ToString();


        //Debug.Log(iniBalance);
    }

    /*
    //chip of 5
    public void PlaceChip5()
    {
        chipValue = 5;

        if (iniBalance - chipValue >= 0 && inBet + chipValue <= maxBet)
        {
            StartCoroutine(Chips(chip5Prefab, new Vector3(-0.155f, 0.33f, -1.363f)));

            iniBalance -= chipValue;

            inBet += chipValue;
        }

        balance.text = "$ " + iniBalance.ToString();
        playerBet.text = "$ " + inBet.ToString();

    }

    //chip of 10
    public void PlaceChip10()
    {
        chipValue = 10;

        if (iniBalance - chipValue >= 0 && inBet + chipValue <= maxBet)
        {
            StartCoroutine(Chips(chip10Prefab, new Vector3(-0.14f, 0.33f, -1.363f)));

            iniBalance -= chipValue;

            inBet += chipValue;
        }

        balance.text = "$ " + iniBalance.ToString();
        playerBet.text = "$ " + inBet.ToString();

    }

    //chip of 25
    public void PlaceChip25()
    {
        chipValue = 25;

        if (iniBalance - chipValue >= 0 && inBet + chipValue <= maxBet)
        {
            StartCoroutine(Chips(chip25Prefab, new Vector3(0.438f, 0.33f, -1.363f)));

            iniBalance -= chipValue;

            inBet += chipValue;
        }

        balance.text = "$ " + iniBalance.ToString();
        playerBet.text = "$ " + inBet.ToString();
    }*/




    //place the player's chips in the correct position in the screen
    public IEnumerator Chips(GameObject chipPrefab, Vector3 startPos, Vector3 chipEndPos){

        //disable all the chip buttons 
        DisableChips();

        Chip chip = Instantiate(chipPrefab).GetComponent<Chip>();

        chip.transform.position = startPos;
        chip.transform.localScale = new Vector3(400f, 400f, 400f);

        //The step size is equal to speed times frame time
        float speed = 1f;

        float step = speed * Time.deltaTime;

        //move card1
        while (Vector3.Distance(chip.transform.position, chipEndPos) > 0.001)
        {
            //Debug.Log("chip end pos = "+ chipEndPos);

            chip.transform.position = Vector3.MoveTowards(chip.transform.position, chipEndPos, step);

            //Debug.Log("chip1= " + chip.transform.position);

            yield return null; //wait for the function to end
        }

        ////offset to change the position of the chips every time they get placed in the table
        //if (changePos){
        //    chipEndPos += new Vector3(0.089f, 0.004f, 0.032f);   // for player 1
        //    changePos = false;
        //}
        //else
        //{
        //    chipEndPos += new Vector3(-0.0817381f, 0.02f, 0.02f);
        //    changePos = true;
        //}

        //  Debug.Log("change pos= " + changePos);

        yield return new WaitForSeconds(0.8f);

        //enable all the buttons again

        if (inBet >= minBet)
        {
            betSelected = true;
            deal.interactable = true; //------> maybe i have to activate this button in other place 
            client1.SendData("CEC|"); //send to the server
        }
        EnableChips();

        yield return null;
    }


    public void EnableChips(){
        chip1.interactable = true;
        chip5.interactable = true;
        chip10.interactable = true;
        chip25.interactable = true;
    }

    public void DisableChips(){
        chip1.interactable = false;
        chip5.interactable = false;
        chip10.interactable = false;
        chip25.interactable = false;
    }

    //FINISH THIS FUNCTION AND CALL IT WHERE PLAYER OR DEALER WINS 
    // if dealer wins endPos Dealer 
    //if player wins endPos player 
    public IEnumerator GetChips(Vector3 endPos){

        //get the chips gameobject

        //The step size is equal to speed times frame time
        float speed = 1.8f;

        float step = speed * Time.deltaTime;

        foreach (GameObject chip in chips)
        {
            if (chip.transform.localScale == new Vector3 (400f, 400f, 400f))
            {
                while (Vector3.Distance(chip.transform.position, endPos) > 0.01)
                {

                    //Debug.Log("chip pos= " + chip.transform.position);
                   // Debug.Log("chip end pos = " + chipEndPos);

                    chip.transform.position = Vector3.MoveTowards(chip.transform.position, endPos, step);

                  //  Debug.Log("picking up the chips");

                    yield return null; //wait for the function to end
                }
            }
        }

        yield return null;
    }

}
