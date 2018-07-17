using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoose : MonoBehaviour {
    
    public DistributeCardsToPlayers handValue;

    public PickUpCards cards;

    bool playerlose;

	// Use this for initialization
	void Start () {

        handValue = GetComponent<DistributeCardsToPlayers>();

        cards = GetComponent<PickUpCards>();
	}
	
	// Update is called once per frame
	void Update () {

        //PlayerLose();

	}

    //public void PlayerLose()
    //{
    //    if (handValue.PlayerHandValue > 21)
    //    {
    //        playerlose = true;
    //        Debug.Log("PLAYER LOOSE!!");

    //        cards.TurnCards();
    //    }
    //}
     
}
