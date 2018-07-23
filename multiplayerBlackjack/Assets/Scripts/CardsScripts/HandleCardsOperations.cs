using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HandleCardsOperations : MonoBehaviour
{
    public static HandleCardsOperations Instance { set; get; }

    public GameObject cardPrefab; //prefab of cards

    public CardModel card; //card to instantiate

    Vector3 deckPosition; //holds the position of the deck of cards

    int cardIndex; //position of card in the deck

    public CardStack cardStack; //script to access the stack of cards

    //position in screen of player's and dealer's hand 
    public Vector3 player1CardPos;
    public Vector3 dealerCardPos;

    //saves the player's and dealer's hand
    List<int> playerHand = new List<int>();
    List<int> dealerHand = new List<int>();

    //saves the value of the player's and dealer's hand 
    int PlayerHandValue;
    int DealerHandValue;

    public bool isBusted;

    //buttons in screen
    public Button hit; //button to get one new card from the stack's game
    public Button deal; //start the game 
    public Button stand; //wait for dealer to play
    public Button leave; //leave the room

    //chips buttons
    public Button chip1;
    public Button chip5;
    public Button chip10;
    public Button chip25;

    //text to tell the user to place bet
    public Text placeBet;

    //save the second card of the dealer to flip it in the right time
    public CoroutineWithData dealerSecondCard;

    //useful text to tell the players the value of their hand and the dealer's hand
    public Text winnerText;
    public Text dealerHandVal;
    public Text playerHandVal;
    public Text player1HandVal;

    //panel for no more balance
    public CanvasRenderer panelNoMoreMoney;

    public PickUpCards cards; //script to play the animations and pick up the cards. 
    public GameObject[] cardsPrefabToDestroy; //all the cards to pick up

    public GiveChips handleChips; //script to have access to all the chips operations
    public GameObject[] chipsToDestroy; //all the chips at the end of the game 

    //position of dealer and player to pick up the chips when someone wins
    Vector3 dealerPos = new Vector3(-0.07f, 0.3f, 0.953f);
    Vector3 playerPos = new Vector3(-0.07f, 0.3f, -1.418f);

    //get the list of clients from the server

    myClient1 client1; //access the client

    string cardID = "";

    void Start()
    {
        Instance = this;

        //randomized stack of cards
        cardStack = cardStack.GetComponent<CardStack>();

        //position of the deck of cards
        deckPosition = new Vector3(1.621f, 0.36f, 0.793f);

        client1 = FindObjectOfType<myClient1>();

    }

    private void Update()
    {
        cardsPrefabToDestroy = GameObject.FindGameObjectsWithTag("Card");

        chipsToDestroy = GameObject.FindGameObjectsWithTag("Chip");
    }


    //////start the function to distribute the cards to the player///// 
    public void DistributeCardsToStartGame()
    {
        //disable chips after deal is done
        DisableChips();

        StartCoroutine(DistributeToEveyone());
    }

    /////distribute cards depending in the number of players/////
    public IEnumerator DistributeToEveyone()
    {

        //TODO do this for the amount of players in the game!!!!!!
        //depending on the amount of clients in the clientlist

        int clients = 0;

        /* After each player has selected their bet 
         * 
         * 
         * 
         * 
         * 
         * distribute cards in the following order
         * case 2 players:
         *      card1 player1
         *      card1 player2
         *      card1 dealer
         *      card2 player1
         *      card2 player2
         *      card2 dealer
         * 
         * case 3 players:
         *      card1 player1
         *      card1 player2
         *      card1 player3
         *      card1 dealer
         *      card2 player1
         *      card2 player2
         *      card2 player3
         *      card2 dealer
         *      
         *
         */





        switch (clients)
        {
            case 0:
                {
                    //distribute cards to 1 player 
                    CoroutineWithData cd = new CoroutineWithData(this, DistributeCards("",card, new Vector3(-0.048f, 0.331f, -0.83f)));
                    yield return cd.coroutine;

                    CoroutineWithData cd1 = new CoroutineWithData(this, DistributeCards("",card, new Vector3(0.062f, 0.36f, -0.77f)));
                    yield return cd1.coroutine;

                    //flip cards and add it to the player hand
                    playerHand.Add(GetCardFromDeck(cd.result));
                    yield return new WaitForSeconds(0.5f);
                    playerHand.Add(GetCardFromDeck(cd1.result));

                    player1CardPos = cd1.result.transform.position;

                    PlayerHandValue = cardStack.CardValue(playerHand);

                    yield return new WaitForSeconds(0.8f);

                    playerHandVal.text = PlayerHandValue.ToString();

                    //if (client1.isHost){
                    //    playerHandVal.text = PlayerHandValue.ToString();
                    //}
                    //else{
                    //    player1HandVal.text = PlayerHandValue.ToString(); 
                    //}



                    break;
                }
            case 2:
                {
                    //distribute to 2 players 
                    break; 
                }
            case 3:
                {
                    //distribute to 3 players 
                    break;
                }
            
        }



        ///////////////dealer////////////////
        yield return new WaitForSeconds(0.5f);

        //distribute cards to dealer
        CoroutineWithData cdD = new CoroutineWithData(this, DistributeCards("", card, new Vector3(-0.49f, 0.449f, 0.26f)));
        yield return cdD.coroutine;

        dealerSecondCard = new CoroutineWithData(this, DistributeCards("", card, new Vector3(-0.1845832f, 0.449f, 0.26f)));
        yield return dealerSecondCard.coroutine;

        //flip cards and add it to the dealer hand
        dealerHand.Add(GetCardFromDeck(cdD.result));
        yield return new WaitForSeconds(0.5f);

        //only flip when dealer starts to play
        dealerCardPos = dealerSecondCard.result.transform.position;

        DealerHandValue = cardStack.CardValue(dealerHand);

        yield return new WaitForSeconds(0.8f);

        dealerHandVal.text = DealerHandValue.ToString();

        //check if player has blackjack
        if (PlayerHandValue == 21 && playerHand.Count == 2)
        {

            client1.SendData("CPWIN|" + client1.clientName + " has Blackjack!!!");
            //winnerText.text = "Player has Blackjack!!!";

            //disable buttons 
            DisableAllButtons();

            //give the chips to the player
            handleChips.iniBalance += (2 * handleChips.inBet);
            handleChips.balance.text = "$ " + handleChips.iniBalance.ToString();
            StartCoroutine(handleChips.GetChips(playerPos));

            //pick cards and start again
            StartCoroutine(StartAgain());

            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        //re enable buttons after giving the cards
        EnableAllButtons();

        yield return null;
    }

    ///////////Get cards from randomized card stack and flip them/////////
    public int GetCardFromDeck(CardModel card)
    {
        cardIndex = cardStack.Pop();

        CardFlipper flipper = card.GetComponent<CardFlipper>();

        flipper.FlipCard(card.cardBack, card.faces[cardIndex], cardIndex);

        return cardIndex;
    }

    ///////////function to distribute cards to players///////////
    public IEnumerator DistributeCards(string cardID, CardModel card1, Vector3 card1Pos)
    {
        card1 = Instantiate(cardPrefab).GetComponent<CardModel>();

        card1.transform.position = deckPosition;

        if(cardID.Equals("p1c"))
        {
            card1.transform.rotation = Quaternion.Euler(90f, -20f, 0f); //rotate -20 degrees in y axis
        }


        // The step size is equal to speed times frame time.
        float speed = 3.5f;

        float step = speed * Time.deltaTime;

        //move card1
        while (Vector3.Distance(card1.transform.position, card1Pos) > 0.01)
        {
            card1.transform.position = Vector3.MoveTowards(card1.transform.position, card1Pos, step);

           // Debug.Log("card1= " + card1);

            yield return null; //wait for the function to end
        }

        yield return card1;
    }

    ////////hit function for players/////////
    public IEnumerator GetOneCard()
    {
        //do the same switch here!!!

        float offsetx = 0.082f;
        float offsety = 0.055f;
        float offsetz = 0.02f;

        Vector3 offset = new Vector3(offsetx, offsety, offsetz);
        player1CardPos += offset; //position of previous card

        //Debug.Log(player1CardPos);
        CoroutineWithData cd = new CoroutineWithData(this, DistributeCards("", card, player1CardPos));
        yield return cd.coroutine;

        //flip the card and add it to the player hand
        playerHand.Add(GetCardFromDeck(cd.result));

        PlayerHandValue = cardStack.CardValue(playerHand);

        yield return new WaitForSeconds(0.8f);
       
        playerHandVal.text = PlayerHandValue.ToString();

        //re enable buttons after giving the cards
        EnableAllButtons();


        //if player is busted dealer start to play
        if (PlayerHandValue == 21) 
        {
            //disable buttons 
            DisableAllButtons();
            StartCoroutine(DealersTurn());

            /*restart to play again handled in the dealers turn coroutine*/

            yield return null;
        }
        else if(PlayerHandValue > 21)
        {
            //player is busted game ends!!!!

            //disable buttons 
            DisableAllButtons();

            //client1.SendData("CDWIN|" + "Dealer win!!!");
            winnerText.text = "Dealer wins!!!";

            playerHandVal.text = "Busted!";

            //give the chips to the dealer
            StartCoroutine(handleChips.GetChips(dealerPos));

            //pick cards and start again
            StartCoroutine(StartAgain());

            yield return null;
        }

    }

    /////////hit for dealer//////////
    public IEnumerator GetDealerCards(){

        float offsetx = 0.3054168f;

        Vector3 offset = new Vector3(offsetx, 0f, 0f);
        dealerCardPos += offset; //position of previous card

        //Debug.Log(player1CardPos);
        CoroutineWithData cd = new CoroutineWithData(this, DistributeCards("", card, dealerCardPos));
        yield return cd.coroutine;

        //flip the card and add it to the player hand
        dealerHand.Add(GetCardFromDeck(cd.result));

        DealerHandValue = cardStack.CardValue(dealerHand);

        yield return new WaitForSeconds(0.8f);

        dealerHandVal.text = DealerHandValue.ToString();

    }


    //////////dealer's turn/////////// 
    public IEnumerator DealersTurn()
    {
        //disable buttons 
        DisableAllButtons();

        dealerHand.Add(GetCardFromDeck(dealerSecondCard.result));

        DealerHandValue = cardStack.CardValue(dealerHand);

        yield return new WaitForSeconds(0.8f);

        dealerHandVal.text = DealerHandValue.ToString();

        //check if dealer has blackjack
        if (DealerHandValue == 21 && dealerHand.Count == 2)
        {
            winnerText.text = "Dealer has Blackjack!!!";

            //give the chips to the dealer
            StartCoroutine(handleChips.GetChips(new Vector3(1.462f, 0.325f, -0.22f)));

            //pick cards and start again
            StartCoroutine(StartAgain());

            yield return null;
        }

        while(DealerHandValue < 17){

            yield return new WaitForSeconds(0.5f);

            StartCoroutine(GetDealerCards());

            DealerHandValue = cardStack.CardValue(dealerHand);

            yield return new WaitForSeconds(0.8f);

            dealerHandVal.text = DealerHandValue.ToString();

            yield return new WaitForSeconds(1f);
        }

        yield return null;

        //check for who wins dealer or player
        if (DealerHandValue == PlayerHandValue)
        {
            //client1.SendData("CDWIN|" + "Dealer win!!!");
            winnerText.text = "Dealer wins!!!";

            //disable buttons 
            //DisableAllButtons();

            //give the chips to the dealer
            StartCoroutine(handleChips.GetChips(dealerPos));

            //pick cards and start again
            StartCoroutine(StartAgain());

            yield return null;
        }
        else if ((DealerHandValue > 21) || (PlayerHandValue <= 21 && PlayerHandValue > DealerHandValue))
        {

            //client1.SendData("CPWIN|" + client1.clientName + " win!!!");
            winnerText.text = "Player Wins!!!";

            //give the chips to the player
            handleChips.iniBalance += (2 * handleChips.inBet);
            handleChips.balance.text = "$ " + handleChips.iniBalance.ToString();
            StartCoroutine(handleChips.GetChips(playerPos));

            //pick cards and start again
            StartCoroutine(StartAgain());

            yield return null;
        }
        else
        {
            //client1.SendData("CDWIN|" + "Dealer win!!!");
            winnerText.text = "Dealer wins!!!";

            //give the chips to the dealer
            StartCoroutine(handleChips.GetChips(dealerPos));
            
            //pick cards and start again
            StartCoroutine(StartAgain());

            yield return null;
        }
    }

    /////////Re Start the game/////////
    public void ResetGame(){

        if (handleChips.iniBalance == 0)
        {
            panelNoMoreMoney.gameObject.SetActive(true);

            //disable text to tell user to place bet
            placeBet.gameObject.SetActive(false);
        }
        else
        {
            //enable text to tell user to place bet
            placeBet.gameObject.SetActive(true);
        }
       
            Debug.Log("Restarting the game....");

            //disable buttons 
            DisableAllButtons();

            //enable chips
            EnableChips();

            //clearing the text field
            winnerText.text = " ";
            playerHandVal.text = " ";
            dealerHandVal.text = " ";
            handleChips.playerBet.text = " ";

            //reset the hand values 
            PlayerHandValue = 0;
            DealerHandValue = 0;

            //clearing the player and dealer hands
            playerHand.Clear();
            dealerHand.Clear();

            //reset the bet value
            handleChips.inBet = 0;

            //clear the card stack 
            cardStack.Reset();

            //re start the card stack
            cardStack.Shuffle();

            //destroy all the cards game objects on the scene to start a new game
            foreach (GameObject Card in cardsPrefabToDestroy)
            {
                Destroy(Card);
                Debug.Log("Cards have been destroyed!");
            }

            //destroy all the chips in the scene to start a new game 
            foreach (GameObject Chip in chipsToDestroy)
            {
                if (Chip.transform.localScale == new Vector3(400f, 400f, 400f))
                {
                    Destroy(Chip);
                    Debug.Log("Chips have been destroyed!");
                }
            }
    }

    ///////////turn cards back and pick them up /////////
    ////////// reset values to start playing again /////////
    IEnumerator StartAgain()
    {
        
        //turn cards back and pick them up
        yield return new WaitForSeconds(1f);
        StartCoroutine(cards.TurnCards());
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(cards.PickCardsUp());

        //restart to play again
        yield return new WaitForSeconds(3.5f);
        ResetGame();

        yield return null;
    }

    //enable the game buttons
    void EnableAllButtons()
    {
        hit.interactable = true;
        stand.interactable = true;
        leave.interactable = true;
    }

    //disable all the game buttons
    void DisableAllButtons()
    {
        deal.interactable = false;
        hit.interactable = false;
        stand.interactable = false;
        leave.interactable = false;
    }

    //disable chips
    void DisableChips()
    {
        chip1.interactable = false;
        chip5.interactable = false;
        chip10.interactable = false;
        chip25.interactable = false;
    }

    //enable chips
    void EnableChips()
    {
        chip1.interactable = true;
        chip5.interactable = true;
        chip10.interactable = true;
        chip25.interactable = true;
    }

    //option yes in menu no more money
    public void PlayAgainAfterNoMoney()
    {
        Debug.Log("Start with 50 chips again!");

        handleChips.iniBalance = 50;

        handleChips.balance.text = "$ " + handleChips.iniBalance; 

        handleChips.chipEndPos = new Vector3(-0.446f, 0.325f, -1.061f);

        panelNoMoreMoney.gameObject.SetActive(false);

        // enable text to tell user to place bet
        placeBet.gameObject.SetActive(true);
    }

    //option no in no more money menu
    //in multiplayer disconnect client of server
    public void DonotPlayAgain(){

        Debug.Log("change scene");
        SceneManager.LoadScene("Menu");
    }
}

