using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Hand : MonoBehaviour
{
    [SerializeField] private GameObject hand;
    [SerializeField] private List<GameObject> cardsInHand;


    UnoSound unoSound;

    void Awake()
    {
        unoSound = GameObject.Find("SoundController").GetComponent<UnoSound>();
    }

    public GameObject getHand() {
        return hand;
    }

    public List<GameObject> getCardsInHand() {
        return cardsInHand;
    }

    public void drawCard(GameObject hand) {
        GameObject card = Deck.getCardFromDeck();
        Sprite backCardSprite = Resources.Load<Sprite>("Sprites/Uno/Uno_Back");
        if (hand.name != "Player Hand") {
            card.GetComponent<SpriteRenderer>().sprite = backCardSprite;
            card.transform.localScale = new Vector2(50, 50);
        }
        card.transform.SetParent(hand.transform, false);
        CardUI.applyCardPosition(card, cardsInHand, hand);
        hand.GetComponent<Hand>().cardsInHand.Add(card);
    }



    public void playCard(GameObject card, GameObject playArea) {
        card.transform.SetParent(playArea.transform, true);
        cardsInHand.Remove(card);
        PlayAreaDeck.playAreaStack.Push(card);

        unoSound.playCardFlipSound();

        if (GameController.currTurn == "Player"
            && card.GetComponent<Card>().GetType() == typeof(WildCard)) {
                return;
        }

        if (cardsInHand.Count <= 1) {
            if (cardsInHand.Count == 0) {
                if (GameController.currTurn == "Player") {
                    GameController.playerWin = true;
                }
            UIController uiController = GameObject.Find("UIController").GetComponent<UIController>();
            uiController.toggleGameOverMenu();
            }
        }

        GameController.nextTurn();
    }


    /**********************************
    ************ HAND UI **************
    ***********************************/
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


    // Card animation to move card to play area
    IEnumerator moveToPlayArea(GameObject card, GameObject playArea) {
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

        if (cardsInHand.Count <= 0) {
            if (GameController.currTurn == "Player") {
                Debug.Log("PLAYER WON");
                GameController.playerWin = true;
            }
            UIController uiController = GameObject.Find("UIController").GetComponent<UIController>();
            uiController.toggleGameOverMenu();
        }
        // GameController.nextTurn();
    }

}
