﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameActionButtons : MonoBehaviour {

    //public Button deal; //button to start the game

    public DistributeCardsToPlayers cardActions;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //GET ONE CARD FROM DEALER 
    public void Hit()
    {
        StartCoroutine(cardActions.GetOneCard(cardActions.cardPos));
        Debug.Log("animation to get one card from dealer");
    }

    //DISTRIBUTE 2 CARDS TO PLAYERS AND DEALER TO START GAME 
    public void Deal()
    {
        cardActions.DistributeCardsToStartGame();
        Debug.Log("disable button after clicking on it");
    }

    //DON'T RECEIVE MORE CARDS FROM DEALER. INSTEAD WAIT FOR OTHER PLAYERS AND DEALER TO FINISH THEIR TURN
    void Stand()
    {
        Debug.Log("wait for dealer and other players to play");
    }

}