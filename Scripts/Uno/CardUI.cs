using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardUI : MonoBehaviour
{

    public static void applyCardPosition(GameObject card, List<GameObject> cardsInHand, GameObject hand) {
        float xOffset = 20f;
        float zOffset = 0.5f;

        try {
            GameObject prevCard = cardsInHand[cardsInHand.Count - 1];
            card.transform.position = hand.GetComponent<RectTransform>().anchoredPosition;
            card.transform.position = new Vector3(prevCard.transform.position.x + xOffset, 
                                        hand.transform.position.y,
                                        prevCard.transform.position.z - zOffset);
        } catch (ArgumentOutOfRangeException) {
            card.transform.position = new Vector3(hand.transform.position.x, 
                                        hand.transform.position.y,
                                        card.transform.position.z);
        }
    }

    public static IEnumerator moveToPlayArea(GameObject card, GameObject playArea) {
        Vector3 oldPos = card.transform.position;
        Vector3 endPos = PlayAreaDeck.getPlayArea().transform.position;
        endPos = new Vector3(endPos.x, endPos.y, PlayAreaDeck.getCardFromPlayArea().transform.position.z - 1.0f);

        Quaternion rotateEndPos = Quaternion.Euler(new Vector3(card.transform.rotation.x,
                                            card.transform.rotation.y,
                                            UnityEngine.Random.Range(-360f, 360f)));

        float rotateSpeed = 500f;
        float moveSpeed = 1000.0f;

        while (Vector3.Distance(card.transform.position, endPos) > 0.01f) {
            card.transform.position = Vector3.MoveTowards(card.transform.position,
                                                            endPos, moveSpeed * Time.deltaTime);
            card.transform.rotation = Quaternion.RotateTowards(card.transform.rotation, rotateEndPos, rotateSpeed * Time.deltaTime);
            yield return null;
        }


        card.transform.position = endPos;
    }


    public static IEnumerator moveToHand(GameObject card, GameObject hand) {
        Vector3 oldPos = card.transform.position;
        List<GameObject> cardsInHand = hand.GetComponent<Hand>().getCardsInHand();
        Vector3 endPos = cardsInHand[cardsInHand.Count - 1].transform.position;
        endPos = new Vector3(endPos.x, endPos.y, endPos.z - 1.0f);

        float moveSpeed = 1000.0f;

        while (Vector3.Distance(card.transform.position, endPos) > 0.01f) {
            card.transform.position = Vector3.MoveTowards(card.transform.position,
                                                            endPos, moveSpeed * Time.deltaTime);
            yield return null;
        }


        card.transform.position = endPos;
    }

        
}
