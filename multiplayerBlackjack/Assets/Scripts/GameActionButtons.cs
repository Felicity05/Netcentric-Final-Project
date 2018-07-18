using UnityEngine;
using UnityEngine.UI;


public class GameActionButtons : MonoBehaviour {

    public Button deal; //button to start the game
    public Button stand;
    public Button leave;
    public Button hit;

    public Text placeBet;
   
    public DistributeCardsToPlayers cardActions;

	// Use this for initialization
	void Start () {
        stand.interactable = false;
        deal.interactable = false;
        hit.interactable = false;

        //DON'T LET PLAYERS PLAY UNTIL BET IS PLACED
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
        placeBet.text = " ";
    }

    //DON'T RECEIVE MORE CARDS FROM DEALER. INSTEAD WAIT FOR OTHER PLAYERS AND DEALER TO FINISH THEIR TURN
    public void Stand()
    {
        StartCoroutine(cardActions.DealersTurn());
        Debug.Log("wait for dealer and/or other players to play");
    }

    public void Leave()
    {
        Debug.Log("ask player if he/she wants to leave the game");
        deal.interactable = false;
        stand.interactable = false;
        leave.interactable = false;
        hit.interactable = false;
        //if yes
        //Application.Quit();
        //else
        //go back to the game and re enable buttons
    }
}
