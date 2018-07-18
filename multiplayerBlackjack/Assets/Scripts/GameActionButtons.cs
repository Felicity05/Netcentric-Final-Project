﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameActionButtons : MonoBehaviour {

    public Button deal; //button to start the game
    public Button stand;
    public Button leave;
    public Button hit;
   
    public DistributeCardsToPlayers cardActions;

	// Use this for initialization
	void Start () {
        stand.interactable = false;
        leave.interactable = false;
        hit.interactable = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //GET ONE CARD FROM DEALER 
    public void Hit()
    {
        StartCoroutine(cardActions.GetOneCard());
        hit.interactable = false;
        stand.interactable = false;
        leave.interactable = false;
       
        //Debug.Log("OK!!!!");
    }

    //DISTRIBUTE 2 CARDS TO PLAYERS AND DEALER TO START GAME 
    public void Deal()
    {
        cardActions.DistributeCardsToStartGame();
        Debug.Log("disable button after clicking on it");
        deal.interactable = false;
        stand.interactable = false;
        leave.interactable = false;
        hit.interactable = false;
    }

    //DON'T RECEIVE MORE CARDS FROM DEALER. INSTEAD WAIT FOR OTHER PLAYERS AND DEALER TO FINISH THEIR TURN
    public void Stand()
    {
        StartCoroutine(cardActions.DealersTurn());
        stand.interactable = false; 
        Debug.Log("wait for dealer and/or other players to play");
    }

}
