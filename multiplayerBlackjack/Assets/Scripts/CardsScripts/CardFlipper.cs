using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFlipper : MonoBehaviour {


    SpriteRenderer spriteRenderer;
    CardModel model;

    public AnimationCurve scaleCurve;
    public float duration = 0.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        model = GetComponent<CardModel>();
    }


    /* FLIP 2 CARDS TO FACE WHEN RECEIVED BY THE PLAYER 
     * FLIP 1 CARD TO FACE WHEN RECEIVED BY THE DEALER 
     * FLIP CARDS TO BACK WHEN GAME ENDS (WHEN ALL THE PLAYERS END PLAYING)
     * IF PLAYER HAS MORE THAN 21 DON'T FLIP DEALER CARD, THEN FLIP ALL CARDS BACK AND PICK UP THE CARDS
     */


    public void FlipCard(Sprite startImg, Sprite endImg, int cardIndex)
    {
        StopCoroutine(Flip(startImg, endImg, cardIndex));
        StartCoroutine(Flip(startImg, endImg, cardIndex));
    }


    IEnumerator Flip(Sprite startImg, Sprite endImg, int cardIndex)
    {
        spriteRenderer.sprite = startImg;

        float time = 0;
        while (time <=1f)
        {
            float scale = scaleCurve.Evaluate(time);
            time = time + Time.deltaTime / duration;

            Vector3 localScale = transform.localScale;
            localScale.x = scale;
            transform.localScale = localScale;

            if (time > 0.5)
            {
                spriteRenderer.sprite = endImg;
            }

            yield return new WaitForFixedUpdate();
        }

        model.cardIndex = cardIndex;
    }


}
