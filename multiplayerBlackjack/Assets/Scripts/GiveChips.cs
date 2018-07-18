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


    //public List<int> chips = new List<int>();

    public Text balance;

    int chipValue;

    int iniBalance = 50;

    Vector3 chipEndPos = new Vector3(-0.446f, 0.325f, -1.061f);

    bool changePos = true;

    GameObject[] chips;

    // Use this for initialization
	void Start () {

        balance.text = "$ " + iniBalance.ToString();
	}
	
	// Update is called once per frame
	void Update () {

        chips = GameObject.FindGameObjectsWithTag("Chips");
	}


    //chip1Pos = new Vector3(-0.401f, -0.06999999f, -0.33f);


    // 5 chips of 1
    public void PlaceChip1()
    {
        chipValue = 1;

        if (iniBalance - chipValue >= 0)
        {
            StartCoroutine(Chips(chip1Prefab, new Vector3(-0.446f, 0.33f, -1.334f)));

            //Debug.Log(chipValue);

            iniBalance -= chipValue;
        }

        balance.text = "$ " + iniBalance.ToString();


        //Debug.Log(iniBalance);
    }

    // 2 chip of 5
    public void PlaceChip5()
    {
        chipValue = 5;

        if (iniBalance - chipValue >= 0)
        {
            StartCoroutine(Chips(chip5Prefab, new Vector3(-0.155f, 0.33f, -1.363f)));

            iniBalance -= chipValue;
        }

        balance.text = "$ " + iniBalance.ToString();
    }

    // 1 chip of 10
    public void PlaceChip10()
    {
        chipValue = 10;

        if (iniBalance - chipValue >= 0)
        {
            StartCoroutine(Chips(chip10Prefab, new Vector3(-0.14f, 0.33f, -1.363f)));

            iniBalance -= chipValue;
        }

        balance.text = "$ " + iniBalance.ToString();
    }

    // 1 chip of 25
    public void PlaceChip25()
    {
        chipValue = 25;

        if (iniBalance - chipValue >= 0)
        {
            StartCoroutine(Chips(chip25Prefab, new Vector3(0.438f, 0.33f, -1.363f)));

            iniBalance -= chipValue;
        }

        balance.text = "$ " + iniBalance.ToString();
    }


    //place the chips in the correct position in the screen
    public IEnumerator Chips(GameObject chipPrefab, Vector3 startPos){
        
        MeshFilter chip = Instantiate(chipPrefab).GetComponent<MeshFilter>();

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

        Debug.Log("change pos= " + changePos);

        //yield return null;
    }


    //FINISH THIS FUNCTION AND CALL IT WHERE PLAYER OR DEALER WINS 
    // if dealer wins endPos Dealer 
    //if player wins endPos player 
    public IEnumerator GetChips(Vector3 endPos){

        //get the chips gameobject

        //The step size is equal to speed times frame time
        float speed = 1f;

        float step = speed * Time.deltaTime;


        foreach (GameObject chip in chips)
        {
            while (Vector3.Distance(chip.transform.position, endPos) > 0.01)
            {
                //Debug.Log("chip end pos = "+ chipEndPos);

                chip.transform.position = Vector3.MoveTowards(chip.transform.position, chipEndPos, step);

                //Debug.Log("chip1= " + chip.transform.position);

                yield return null; //wait for the function to end
            }
        }

        yield return null;
    }

}
