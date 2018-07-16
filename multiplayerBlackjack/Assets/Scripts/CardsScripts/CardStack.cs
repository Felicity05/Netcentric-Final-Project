using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStack : MonoBehaviour {

    //TODO delete card from deck when given to players or dealer

    //holds the value of the cards key is teh card, value is the value of the card ex, 2 of all hands, value 2
    Dictionary<int, int> cardsValue = new Dictionary<int, int>();

    List<int> cards;

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
	
}
