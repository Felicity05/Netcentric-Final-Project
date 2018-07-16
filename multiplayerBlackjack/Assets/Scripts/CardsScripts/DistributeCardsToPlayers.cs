using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributeCardsToPlayers : MonoBehaviour
{
    public GameObject cardPrefab; //prefab of cards

    //player1 cards'
    public CardModel card1Player1;
    public CardModel card2Player1;

    //dealer cards'
    public CardModel card1Dealer;
    public CardModel card2Dealer;

    Vector3 deckPosition; //holds the position of the deck of cards

    CardFlipper flipperCard1;
    CardFlipper flipperCard2;
    CardFlipper flipperDealer1;
    int cardIndex;


    private void Start()
    {
        //spawn 2 cards for player 1
        card1Player1 = Instantiate(cardPrefab).GetComponent<CardModel>();
        card2Player1 = Instantiate(cardPrefab).GetComponent<CardModel>();

        //spawn 2 cards for dealer
        card1Dealer = Instantiate(cardPrefab).GetComponent<CardModel>();
        card2Dealer = Instantiate(cardPrefab).GetComponent<CardModel>();

        //get the fliper script to flip the cards 
        flipperCard1 = card1Player1.GetComponent<CardFlipper>();
        flipperCard2 = card2Player1.GetComponent<CardFlipper>();
        flipperDealer1 = card1Dealer.GetComponent<CardFlipper>();

        //position of the deck of cards
        deckPosition = new Vector3(1.621f, 0.36f, 0.793f);

        //placing cards in the position of the deck 
        card1Player1.transform.position = deckPosition;
        card2Player1.transform.position = deckPosition;
        card1Dealer.transform.position = deckPosition;
        card2Dealer.transform.position = deckPosition;
    }



    //start the function to distribute the cards to the player 
    public void TaskOnClick()
    {

        StartCoroutine(DistributeToEveyone());

    }

    //GENERATE RANDOM CARDS TO BE GIVEN TO PLAYERS
    public void GenerateCard(CardModel card, CardFlipper flipper)
    {
        cardIndex = Random.Range(0, 51);

        flipper.FlipCard(card.cardBack, card.faces[cardIndex], cardIndex);

        Debug.Log(cardIndex);

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
                    //distribute to 1 player 
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


        //player 1
        StartCoroutine(DistributeCards(card1Player1, card2Player1, new Vector3(-0.1025832f, 0.36f, -0.7630126f), new Vector3(0.02f, 0.43f, -0.72f)));

        yield return new WaitForSeconds(1f);

        //flip first card of player
        GenerateCard(card1Player1, flipperCard1);

        yield return new WaitForSeconds(0.5f);

        //flip second card of player
        GenerateCard(card2Player1, flipperCard2);

        ///////////////dealer////////////////
        yield return new WaitForSeconds(1f);

        StartCoroutine(DistributeCards(card1Dealer, card2Dealer, new Vector3(-0.368f, 0.41f, 0.115f), new Vector3(-0.215f, 0.465f, 0.135f)));

        yield return new WaitForSeconds(1f);

        GenerateCard(card1Dealer, flipperDealer1);

        yield return null;

    }


    //function to distribute cards to players
    public IEnumerator DistributeCards(CardModel card1, CardModel card2, Vector3 card1Pos, Vector3 card2Pos)
    {

        // The step size is equal to speed times frame time.
        float speed = 3.5f;

        float step = speed * Time.deltaTime;

        //move card1
        while (Vector3.Distance(card1.transform.position, card1Pos) > 0.05)
        {
            card1.transform.position = Vector3.MoveTowards(card1.transform.position, card1Pos, step);

            Debug.Log("card1= " + card1);

            yield return null; //wait for the function to end
        }


        //move card2
        while (Vector3.Distance(card2.transform.position, card2Pos) > 0.05)
        {
            card2.transform.position = Vector3.MoveTowards(card2.transform.position, card2Pos, step);

            Debug.Log("card2= " + card2);

            yield return null; //wait for the function to end
        }

    }
}

