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

    public Vector3 cardPos;

    private void Start()
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

                    //flip cards 
                    GetCardFromDeck(cd.result);
                    yield return new WaitForSeconds(0.5f);
                    GetCardFromDeck(cd1.result);

                    cardPos = cd1.result.transform.position;
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
        CoroutineWithData cdD = new CoroutineWithData(this, DistributeCards(card, new Vector3(-0.368f, 0.41f, 0.115f)));
        yield return cdD.coroutine;

        CoroutineWithData cdD1 = new CoroutineWithData(this, DistributeCards(card, new Vector3(-0.215f, 0.465f, 0.135f)));
        yield return cdD1.coroutine;

        //flip cards 
        GetCardFromDeck(cdD.result);
        yield return new WaitForSeconds(0.5f);
        //only flip when dealer starts to play
        //GetCardFromDeck(cdD1.result);
       
        yield return null;

    }

    //Get cards from randomized card stack and flip them
    public void GetCardFromDeck(CardModel card)
    {

        cardIndex = cardStack.Pop();

        CardFlipper flipper = card.GetComponent<CardFlipper>();

        flipper.FlipCard(card.cardBack, card.faces[cardIndex], cardIndex);

        //Debug.Log(cardIndex);

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
        while (Vector3.Distance(card1.transform.position, card1Pos) > 0.05)
        {
            card1.transform.position = Vector3.MoveTowards(card1.transform.position, card1Pos, step);

           // Debug.Log("card1= " + card1);

            yield return null; //wait for the function to end
        }

        yield return card1;
    }

    //hit function
    public IEnumerator GetOneCard(Vector3 newPos)
    {
        //do the same switch here!!!

        float offsetx = 0.082f;
        float offsety = 0.055f;
        float offsetz = 0.02f;

        Vector3 offset = new Vector3(offsetx, offsety, offsetz);
        newPos += offset;

        Debug.Log(newPos);
        CoroutineWithData cd = new CoroutineWithData(this, DistributeCards(card, newPos));
        yield return cd.coroutine;

        //flip the card
        GetCardFromDeck(cd.result);

    }
}
