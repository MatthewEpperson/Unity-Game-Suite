using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardButton : MonoBehaviour
{

    [SerializeField] Hand playerHand;
    [SerializeField] GameObject playerHandObj;

    public UIController uIController;

    public void drawNewCard() {
        if (GameController.currTurn == "Player") {
            playerHand.drawCard(playerHandObj);
            GameObject newCard = playerHand.getCardsInHand()[playerHand.getCardsInHand().Count - 1];
            if (!GameController.isPlayable(newCard)) {
                StartCoroutine(CardUI.moveToHand(newCard, playerHandObj));
                GameController.nextTurn();
            } else {
                if (newCard.GetComponent<Card>().GetType() == typeof(WildCard)) {
                    uIController.activatePickColorUI(newCard.GetComponent<Card>());
                }
                StartCoroutine(CardUI.moveToPlayArea(newCard, PlayAreaDeck.getPlayArea()));
                playerHand.playCard(newCard, PlayAreaDeck.getPlayArea());
            }
        }
    }
}
