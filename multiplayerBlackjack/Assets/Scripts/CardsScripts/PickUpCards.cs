using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpCards : MonoBehaviour {

    public CardModel[] cards;

    public CardFlipper flipper;

	// Use this for initialization
	void Start () {
        

       
    }

    // Update is called once per frame
	void Update () {
		
        cards = FindObjectsOfType<CardModel>();

    }

    //private void OnGUI()
    //{
    //    if (GUI.Button(new Rect(10,10,100,80), "TurnCards"))
    //    {
    //        StartCoroutine(PickCards());
    //    }
    //}


    //public IEnumerator PickCards(){
        
    //    TurnCards();
    //    //yield return new WaitForSeconds(2f);
    //    //StartCoroutine(PickCardsUp());
    //    yield return null;
    //}



    //all the cards are facing, turn the cards to their back
    public IEnumerator TurnCards(){
        
        for (int i = 0; i < cards.Length; i++)
        {
            flipper = cards[i].GetComponent<CardFlipper>();
            if (cards[i].cardIndex != 0){
                Debug.Log("TURN CARDS BACK!!!!!!");
                flipper.FlipCard(cards[i].faces[cards[i].cardIndex], cards[i].cardBack, cards[i].cardIndex);
            }
            //Debug.Log(cards[i].cardIndex);
        }
        yield return null;
    }

    public IEnumerator PickCardsUp()
    {
        // The step size is equal to speed times frame time.
        float speed = 4f;

        float step = speed * Time.deltaTime;

        Vector3 pickedUpDeck = new Vector3 (-1.675f,0.306f,0.903f);

        foreach (CardModel card in cards)
        {
            //move card1
            while (Vector3.Distance(card.transform.position, pickedUpDeck) > 0.01)
            {
                card.transform.position = Vector3.MoveTowards(card.transform.position, pickedUpDeck, step);

                // Debug.Log("card1= " + card1);

                yield return null; //wait for the function to end
            }
        }
    }

}
