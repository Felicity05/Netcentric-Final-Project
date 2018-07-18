using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStack : MonoBehaviour {

    List<int> cards;

    CardModel card;

	// Use this for initialization
	void Start () 
    {
        Shuffle();
	}
	
    //suffle cards in random order
    public void Shuffle()
    {
        if (cards == null)
        {
            cards = new List<int>();
        }
        else
        {
            cards.Clear();
        }

        for (int i = 0; i < 52; i++)
        {
            cards.Add(i);
        }

        int n = cards.Count;
        while (n > 1) 
        {
            n--;
            int k = Random.Range(0, n + 1);
            //swap
            int temp = cards[k];
            cards[k] = cards[n];
            cards[n] = temp;

        }
    }
	
    //removes a card from the deck
    public int Pop()
    {
        int temp = cards[0];
        cards.RemoveAt(0);
        return temp;
    }

    //MAYBE I DON'T NEED THIS HERE
    //add a card to the players hand
    public void Push(List<int> hand, int card)
    {
        hand.Add(card);
    }

    public int CardValue(List<int> hand){

        int total = 0;
        int aces = 0;


        for (int i = 0; i < hand.Count; i++)
        {
            int cardValue = hand[i] % 13;

            if (cardValue <= 8){
                cardValue += 2;
                total += cardValue;
            }
            else if (cardValue > 8 && cardValue < 12)
            {
                cardValue = 10;
                total += cardValue;
            }
            else
            {
                aces++;
            }
        }

        for (int j = 0; j < aces; j++)
        {
            if (total + 11 <= 21)
            {
                total += 11;
            }
            else
            {
                total += 1;
            }
        }

        return total;
    }

    public void Reset()
    {
        cards.Clear();
    }
}
