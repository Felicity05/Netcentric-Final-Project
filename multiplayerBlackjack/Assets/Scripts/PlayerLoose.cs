using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoose : MonoBehaviour {
    
    DistributeCardsToPlayers handValue;

    PickUpCards cards;

	// Use this for initialization
	void Start () {

        handValue = handValue.GetComponent<DistributeCardsToPlayers>();

        cards = cards.GetComponent<PickUpCards>();
	}
	
	// Update is called once per frame
	void Update () {

        PlayerLose();
	}

    public void PlayerLose()
    {
        if (handValue.PlayerHandValue > 21)
        {
            Debug.Log("PLAYER LOOSE!!");

            cards.PickCards();
        }
    }
     
}
