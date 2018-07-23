using UnityEngine;
using UnityEngine.UI;


public class GameActionButtons : MonoBehaviour {

    public static GameActionButtons Instance { set; get; }

    myClient1 client1;

    public Button deal; //button to start the game
    public Button stand;
    public Button leave;
    public Button hit;

    public Text placeBet;
   
    public HandleCardsOperations cardActions;

    public Transform chatMessageContainer;
    public GameObject messagePrefab;

	// Use this for initialization
	void Start () {
        Instance = this;

        client1 = FindObjectOfType<myClient1>();

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
        placeBet.gameObject.SetActive(false);
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

    public void ChatMessage(string msg){

        GameObject go = Instantiate(messagePrefab) as GameObject;
        go.transform.SetParent(chatMessageContainer);

        go.transform.localScale = Vector3.one;

        go.GetComponentInChildren<Text>().text = msg;
    }

    public void SendChatMessage(){

        InputField input = GameObject.Find("SendDialog").GetComponent<InputField>();

        if (input.text == ""){
            //Debug.Log("no text chat to send");
            return;
        }

        //Debug.Log("text in input field:" + input.text);
        client1.SendData("CMSG|" + input.text);

        input.text = "";
    }
}
