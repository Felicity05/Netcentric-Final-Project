using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour {

    public RectTransform mainIcon;
    public float timeStep;
    public float stepAngle;

    float startTime;


	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {

        if(Time.time - startTime >= timeStep){

            Vector3 iconAngle = mainIcon.localEulerAngles;

            iconAngle.z += stepAngle;

            mainIcon.localEulerAngles = iconAngle;

            startTime = Time.time;
        }
		
	}
}
