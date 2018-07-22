using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCube : MonoBehaviour
{

    public static MoveCube Instance { set; get; }

    public GameObject cube;

    myClient1 client1;

    string msg = "CMOV|";

    Vector3 currentPos = new Vector3(0.03f, -4.8f, -9.07f);

    Vector3 endPos = new Vector3(2.4f, 5.6f, 3.0f);

    public Button button;



	// Use this for initialization
	void Start () {

        client1 = FindObjectOfType<myClient1>();

        //Button btn = button.GetComponent<Button>();

        button.onClick.AddListener(TaskOnClick);

	}

    public void TaskOnClick()
    {
        msg += currentPos.x.ToString() + "|";
        msg += currentPos.y.ToString() + "|";
        msg += currentPos.z.ToString() + "|";

        msg += currentPos.x.ToString() + "|";
        msg += currentPos.y.ToString() + "|";
        msg += currentPos.z.ToString() + "|";

        client1.SendData(msg);

        Debug.Log("position sent");
    }



    public IEnumerator MoveCubes(float sx, float sy, float sz, float ex, float ey, float ez)
    {
        Vector3 cPos = new Vector3(sx, sy, sz);

        Vector3 ePos = new Vector3(ex, ey, ez);


        if (Vector3.Distance(cPos, ePos) > 0.001)
        {
            cube.transform.position = Vector3.MoveTowards(cPos, ePos, Time.deltaTime * 3);

        }

        yield return null;
    }



	// Update is called once per frame
	void Update () {
        
	}
}
