using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveChips : MonoBehaviour {

    public GameObject chip1Prefab;
    public GameObject chip5Prefab;
    public GameObject chip10Prefab;
    public GameObject chip25Prefab;
    public GameObject chip50Prefab;
    public GameObject chip100Prefab;
    public GameObject chip500Prefab;


    public List<int> chipVal;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //chip1Pos = new Vector3(-0.401f, -0.06999999f, -0.33f);


    public void PlaceChip1(){
        int chipValue = 1;
        StartCoroutine(Chips(chip1Prefab, new Vector3(-0.943f, 0.33f, -1.269f), new Vector3(-0.315f, 0.305f, -1.059f)));
    }

    //public void PlaceChip5()
    //{
    //    StartCoroutine(Chips(chip1Prefab, new Vector3(-0.943f, 0.33f, -1.269f), new Vector3(-0.315f, 0.305f, -1.059f)));
    //}
    //public void PlaceChip10()
    //{
    //    StartCoroutine(Chips(chip1Prefab, new Vector3(-0.943f, 0.33f, -1.269f), new Vector3(-0.315f, 0.305f, -1.059f)));
    //}
    //public void PlaceChip25()
    //{
    //    StartCoroutine(Chips(chip1Prefab, new Vector3(-0.943f, 0.33f, -1.269f), new Vector3(-0.315f, 0.305f, -1.059f)));
    //}
    //public void PlaceChip50()
    //{
    //    StartCoroutine(Chips(chip1Prefab, new Vector3(-0.943f, 0.33f, -1.269f), new Vector3(-0.315f, 0.305f, -1.059f)));
    //}
    //public void PlaceChip100()
    //{
    //    StartCoroutine(Chips(chip1Prefab, new Vector3(-0.943f, 0.33f, -1.269f), new Vector3(-0.315f, 0.305f, -1.059f)));
    //}
    //public void PlaceChip500(){
    //    StartCoroutine(Chips(chip1Prefab, new Vector3(-0.943f, 0.33f, -1.269f), new Vector3(-0.315f, 0.305f, -1.059f)));
    //}




    public IEnumerator Chips(GameObject chipPrefab, Vector3 startPos, Vector3 endPos){
        
        MeshFilter chip = Instantiate(chipPrefab).GetComponent<MeshFilter>();

        chip.transform.position = startPos;

        chip.transform.localScale = new Vector3(400f, 400f, 400f);

         //The step size is equal to speed times frame time.
        float speed = 1f;

        float step = speed * Time.deltaTime;

        //move card1
        while (Vector3.Distance(chip.transform.position, endPos) > 0.01)
        {
            chip.transform.position = Vector3.MoveTowards(chip.transform.position, endPos, step);

            Debug.Log("card1= " + chip.transform.position);

            yield return null; //wait for the function to end
        }

        //yield return null;
    }


}
