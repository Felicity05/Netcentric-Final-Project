﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributeCardsToPlayers : MonoBehaviour
{
    public GameObject cardPrefab; //prefab of cards

    public CardModel card; //card to intantiate

    Vector3 deckPosition; //holds the position of the deck of cards

    int cardIndex;

    public CardStack cardStack;

    public Vector3 player1CardPos;
    public Vector3 dealerCardPos;

    List<int> playerHand = new List<int>();

    List<int> dealerHand = new List<int>();

    int PlayerHandValue;
    int DealerHandValue;

    public bool isBusted;
    public Button hit; //button to get one new card from the stack the game
    public Button stand;
    public Button leave;

    public PickUpCards cards;

    public CoroutineWithData dealerSecondCard;

    public Text winnerText;
    public Text playerHandVal;
    public Text dealerHandVal;

    public bool playerWin;

    void Start()
    {
        //randomized stack of cards
        cardStack = cardStack.GetComponent<CardStack>();

        //position of the deck of cards
        deckPosition = new Vector3(1.621f, 0.36f, 0.793f);

    }


    //start the function to distribute the cards to the player 
    public void DistributeCardsToStartGame()
    {
        StartCoroutine(DistributeToEveyone());
    }

    //distribute cards depending in the number of players
    public IEnumerator DistributeToEveyone()
    {

        //TODO do this for the amount of players in the game!!!!!!
        //depending on the amount of clients in the clientlist

        int clients = 0;
        //Always distribute to dealer

        switch (clients)
        {
            case 0:
                {
                    //distribute cards to 1 player 
                    CoroutineWithData cd = new CoroutineWithData(this, DistributeCards(card, new Vector3(-0.1025832f, 0.36f, -0.7630126f)));
                    yield return cd.coroutine;

                    CoroutineWithData cd1 = new CoroutineWithData(this, DistributeCards(card, new Vector3(0.02f, 0.43f, -0.72f)));
                    yield return cd1.coroutine;

                    //flip cards and add it to the player hand
                    playerHand.Add(GetCardFromDeck(cd.result));
                    yield return new WaitForSeconds(0.5f);
                    playerHand.Add(GetCardFromDeck(cd1.result));

                    player1CardPos = cd1.result.transform.position;

                    PlayerHandValue = cardStack.CardValue(playerHand);

                    yield return new WaitForSeconds(0.8f);

                    playerHandVal.text = PlayerHandValue.ToString();

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
            case 4:
                {
                    //distribute to 4 players 
                    break;
                }
            case 5:
                {
                    //distribute to 5 players 
                    break;
                }
        }



        ///////////////dealer////////////////
        yield return new WaitForSeconds(0.5f);

        //distribute cards to dealer
        CoroutineWithData cdD = new CoroutineWithData(this, DistributeCards(card, new Vector3(-0.49f, 0.449f, 0.26f)));
        yield return cdD.coroutine;

        dealerSecondCard = new CoroutineWithData(this, DistributeCards(card, new Vector3(-0.1845832f, 0.449f, 0.26f)));
        yield return dealerSecondCard.coroutine;

        //flip cards and add it to the dealer hand
        dealerHand.Add(GetCardFromDeck(cdD.result));
        yield return new WaitForSeconds(0.5f);

        //only flip when dealer starts to play
        dealerCardPos = dealerSecondCard.result.transform.position;

        DealerHandValue = cardStack.CardValue(dealerHand);

        yield return new WaitForSeconds(0.8f);

        dealerHandVal.text = DealerHandValue.ToString();

        //re enable buttons after giving the cads
        hit.interactable = true;
        stand.interactable = true;
        leave.interactable = true;

        //check if player has blackjack
        if (PlayerHandValue == 21 && playerHand.Count == 2)
        {
            winnerText.text = "Player has Blackjack";
            hit.interactable = false;
            stand.interactable = false;
            leave.interactable = false;
        }


        yield return null;

    }

    //Get cards from randomized card stack and flip them
    public int GetCardFromDeck(CardModel card)
    {
        cardIndex = cardStack.Pop();

        CardFlipper flipper = card.GetComponent<CardFlipper>();

        flipper.FlipCard(card.cardBack, card.faces[cardIndex], cardIndex);

        return cardIndex;
    }

    //function to distribute cards to players
    public IEnumerator DistributeCards(CardModel card1, Vector3 card1Pos)
    {
        card1 = Instantiate(cardPrefab).GetComponent<CardModel>();

        card1.transform.position = deckPosition;

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

    //hit function
    public IEnumerator GetOneCard()
    {
        //do the same switch here!!!

        float offsetx = 0.082f;
        float offsety = 0.055f;
        float offsetz = 0.02f;

        Vector3 offset = new Vector3(offsetx, offsety, offsetz);
        player1CardPos += offset; //position of previous card

        //Debug.Log(player1CardPos);
        CoroutineWithData cd = new CoroutineWithData(this, DistributeCards(card, player1CardPos));
        yield return cd.coroutine;

        //flip the card and add it to the player hand
        playerHand.Add(GetCardFromDeck(cd.result));

        PlayerHandValue = cardStack.CardValue(playerHand);

        yield return new WaitForSeconds(0.8f);
       
        playerHandVal.text = PlayerHandValue.ToString();

        //re enable buttons after giving the cads
        hit.interactable = true;
        stand.interactable = true;
        leave.interactable = true;


        //if player is busted dealer start to play
        if (PlayerHandValue == 21) 
        {
            hit.interactable = false;
            stand.interactable = false;
            leave.interactable = false;
            StartCoroutine(DealersTurn());

            //restart to play again

            yield return null;
        }
        else if(PlayerHandValue > 21)
        {
            //player is busted game ends!!!!
            hit.interactable = false;
            stand.interactable = false;
            leave.interactable = false;

            winnerText.text = "Dealer wins!!!";

            yield return new WaitForSeconds(1f);
            StartCoroutine(cards.TurnCards());
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(cards.PickCardsUp());

            playerHandVal.text = "Busted!";

            //restart to play again

            yield return null;
        }

    }

    //hit for dealer
    public IEnumerator GetDealerCards(){

        float offsetx = 0.3054168f;

        Vector3 offset = new Vector3(offsetx, 0f, 0f);
        dealerCardPos += offset; //position of previous card

        //Debug.Log(player1CardPos);
        CoroutineWithData cd = new CoroutineWithData(this, DistributeCards(card, dealerCardPos));
        yield return cd.coroutine;

        //flip the card and add it to the player hand
        dealerHand.Add(GetCardFromDeck(cd.result));

        DealerHandValue = cardStack.CardValue(dealerHand);

        yield return new WaitForSeconds(0.8f);

        dealerHandVal.text = DealerHandValue.ToString();

    }


    //dealers turn 
    public IEnumerator DealersTurn()
    {

        hit.interactable = false;
        leave.interactable = false;
        stand.interactable = false;

        dealerHand.Add(GetCardFromDeck(dealerSecondCard.result));

        DealerHandValue = cardStack.CardValue(dealerHand);

        yield return new WaitForSeconds(0.8f);

        dealerHandVal.text = DealerHandValue.ToString();

        //check if dealer has blackjack
        if (DealerHandValue == 21 && dealerHand.Count == 2)
        {
            winnerText.text = "Dealer has Blackjack";
            hit.interactable = false;
            stand.interactable = false;
            leave.interactable = false;
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
            winnerText.text = "Dealer wins!!!";

            yield return new WaitForSeconds(1f);
            StartCoroutine(cards.TurnCards());
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(cards.PickCardsUp());

            //restart to play again

            yield return null;
        }
        else if ((DealerHandValue > 21) || (PlayerHandValue <= 21 && PlayerHandValue > DealerHandValue))
        {
            winnerText.text = "Player Wins!!!";

            yield return new WaitForSeconds(1f);
            StartCoroutine(cards.TurnCards());
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(cards.PickCardsUp());

            //restart to play again

            yield return null;
        }
        else
        {
            winnerText.text = "Dealer wins!!!";

            yield return new WaitForSeconds(1f);
            StartCoroutine(cards.TurnCards());
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(cards.PickCardsUp());

            //restart to play again

            yield return null;
        }
    }

    public void StartAgain(){

        //enable the game buttons 
        hit.interactable = true;
        stand.interactable = true;
        leave.interactable = true;

        //rest text to empty
        winnerText.text = " ";

        playerHandVal.text = " ";
        dealerHandVal.text = " ";

        cardStack.Reset();
    }

}

