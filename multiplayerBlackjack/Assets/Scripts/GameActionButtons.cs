using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActionButtons : MonoBehaviour {

    CardModel cardModel;
    int cardIndex;

    public GameObject card;

	// Use this for initialization
	void Start () {
        cardModel = card.GetComponent<CardModel>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //GENERATE RANDOM CARDS TO BE GIVEN TO PLAYERS
    public void GenerateCard()
    {
        if (cardIndex >= cardModel.faces.Length)
        {
            //this never happens, so change this in the future
            cardIndex = 0;
            cardModel.ToggleFace(false);
        }
        else
        {
            cardModel.cardIndex = cardIndex;
            cardModel.ToggleFace(true);
            cardIndex = Random.Range(0, 51);
            Debug.Log(cardIndex);
        }
    }

    //GET ONE CARD FROM DEALER 
    public void Hit()
    {
        GenerateCard();
        Debug.Log("animation to get one card from dealer");
    }

    //DEALER DISTRIBUTE 2 CARDS TO PLAYERS TO START GAME 
    void Deal()
    {
        Debug.Log("needs animation to get the card from dealer");
    }

    //DON'T RECEIVE MORE CARDS FROM DEALER. INSTEAD WAIT FOR OTHER PLAYERS AND DEALER TO FINISH THEIR TURN
    void Stand()
    {
        Debug.Log("wait for dealer and other players to play");
    }

}
