using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveChips : MonoBehaviour {

    public GameObject chip1Prefab;
    public GameObject chip5Prefab;
    public GameObject chip10Prefab;
    public GameObject chip25Prefab;
    //public GameObject chip50Prefab;
    //public GameObject chip100Prefab;
    //public GameObject chip500Prefab;


    public List<int> bet = new List<int>();

    public Text balance;
    public Text playerBet;

    int chipValue;

    public int iniBalance = 50;
    public int inBet = 0;

    public Vector3 chipEndPos = new Vector3(-0.446f, 0.325f, -1.061f);

    Vector3 dealerPos = new Vector3(-0.07f, 0.325f, 0.953f);

    Vector3 playerPos = new Vector3(-0.07f, 0.323f, 0.953f);

    bool changePos = true;

    GameObject[] chips;

    int minBet = 5;
    int maxBet = 25;

    //chips buttons
    public Button chip1;
    public Button chip5;
    public Button chip10;
    public Button chip25;
    public Button deal;

    // Use this for initialization
    void Start () {

        balance.text = "$ " + iniBalance.ToString();
	}
	
	// Update is called once per frame
	void Update () {

        chips = GameObject.FindGameObjectsWithTag("Chip");
	}



    //chips of 1
    public void PlaceChip1()
    {
        chipValue = 1;

        if (iniBalance - chipValue >= 0 && inBet + chipValue <= maxBet)
        {
            StartCoroutine(Chips(chip1Prefab, new Vector3(-0.446f, 0.33f, -1.334f)));

            //Debug.Log(chipValue);

            iniBalance -= chipValue;

            inBet += chipValue;

           
        }

        balance.text = "$ " + iniBalance.ToString();
        playerBet.text = "$ " + inBet.ToString();


        //Debug.Log(iniBalance);
    }

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
    }


    //place the player's chips in the correct position in the screen
    public IEnumerator Chips(GameObject chipPrefab, Vector3 startPos){

        //disable all the chip buttons 
        chip1.interactable = false;
        chip5.interactable = false;
        chip10.interactable = false;
        chip25.interactable = false;

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

        //offset
        if (changePos){
            chipEndPos += new Vector3(0.089f, 0.004f, 0.032f);
            changePos = false;
        }
        else
        {
            chipEndPos += new Vector3(-0.0817381f, 0.02f, 0.02f);
            changePos = true;
        }

        //  Debug.Log("change pos= " + changePos);

        yield return new WaitForSeconds(0.8f);

        //enable all the buttons again

        if (inBet >= minBet)
        {
            deal.interactable = true;
        }
        chip1.interactable = true;
        chip5.interactable = true;
        chip10.interactable = true;
        chip25.interactable = true;


        yield return null;
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
