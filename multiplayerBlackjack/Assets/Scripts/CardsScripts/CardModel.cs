using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModel : MonoBehaviour {

    SpriteRenderer spriteRenderer;

    public Sprite[] faces;
    public Sprite cardBack;

    public bool isFaced;

    GameObject card;

    public int cardIndex; //index of card in the faces array

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); //get the renderer component of the card
    }


    public void ToggleFace(bool showFace){

        if (showFace)
        {
            spriteRenderer.sprite = faces[cardIndex];
            isFaced = true;
        }
        else
        {
            spriteRenderer.sprite = cardBack;
            isFaced = false;
        }
    }
}
