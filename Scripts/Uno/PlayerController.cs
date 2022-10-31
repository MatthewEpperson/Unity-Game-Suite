using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public Hand hand;
    [SerializeField] private List<GameObject> cardsInHand;

    public UIController uIController;

    void Start() {
        cardsInHand = hand.getCardsInHand();
    }


    public void onCardClick(GameObject card) {
        Card cardInfo = card.GetComponent<Card>();
        Card cardOnPlayArea = PlayAreaDeck.getCardFromPlayArea().GetComponent<Card>();
        bool isPlayable = false;
        
        if (isNumberCard(card) || isWildCard(card)) {
            isPlayable = true;
        }

        if (cardInfo.getColor() == cardOnPlayArea.getColor()) {
            // isActionCard(card);
            isPlayable = true;
        }

        if (isActionCard(card)) {
            isPlayable = true;
        }

        if (PlayAreaDeck.getCardFromPlayArea().GetComponent<Card>().GetType() == typeof(ActionCard)) {

        }

        if (isPlayable) {
            if (cardsInHand.Count - 1 == 1) {
                uIController.activateUnoButton();
            }
            StartCoroutine(CardUI.moveToPlayArea(card, PlayAreaDeck.getPlayArea()));
            hand.playCard(card, PlayAreaDeck.getPlayArea());
        }
    }



    private bool isNumberCard(GameObject card) {
        Card cardInfo = card.GetComponent<Card>();
        Card cardOnPlayArea = PlayAreaDeck.getCardFromPlayArea().GetComponent<Card>();

        if (cardInfo.GetType() == typeof(NumberCard) 
            && cardOnPlayArea.GetType() == typeof(NumberCard)) {
                if (((NumberCard)cardInfo).getNumber() == ((NumberCard)cardOnPlayArea).getNumber()) {
                    return true;
                }
        }

        return false;
    }


    private bool isActionCard(GameObject card) {
        Card cardInfo = card.GetComponent<Card>();
        Card topCardInfo = PlayAreaDeck.getCardFromPlayArea().GetComponent<Card>();
        string nextTurn = GameController.checkNextTurn(); // This just checks the next turn, it does NOT set the next turn

        if (cardInfo.GetType() == typeof(ActionCard)) {

            if (topCardInfo.GetType() == typeof(ActionCard)) {
                if (((ActionCard)cardInfo).getActionType() == "draw 2"
                    && (((ActionCard)topCardInfo).getActionType() == "draw 2")) {
                        AIController.hands[nextTurn].drawCard(AIController.handObjects[nextTurn]);
                        AIController.hands[nextTurn].drawCard(AIController.handObjects[nextTurn]);
                        GameController.nextTurn();
                        return true;
                } else if (((ActionCard)cardInfo).getActionType() == "skip"
                            && (((ActionCard)topCardInfo).getActionType() == "skip")) {
                        GameController.nextTurn();
                        return true;
                } else if (((ActionCard)cardInfo).getActionType() == "reverse"
                            && (((ActionCard)topCardInfo).getActionType() == "reverse")) {
                        GameController.isReversed = !GameController.isReversed;
                        return true;
                }
            }
            
            if (cardInfo.getColor() == topCardInfo.getColor()) {
                if (((ActionCard)cardInfo).getActionType() == "draw 2") {
                    AIController.hands[nextTurn].drawCard(AIController.handObjects[nextTurn]);
                    AIController.hands[nextTurn].drawCard(AIController.handObjects[nextTurn]);
                    GameController.nextTurn();
                } else if (((ActionCard)cardInfo).getActionType() == "skip") {
                    GameController.nextTurn();
                } else if (((ActionCard)cardInfo).getActionType() == "reverse") {
                    GameController.isReversed = !GameController.isReversed;
                }
                return true;
            }
            return false;
        }
        return false;
    }

    
    private bool isWildCard(GameObject card) {
        Card cardInfo = card.GetComponent<Card>();
        
        if (cardInfo.GetType() == typeof(WildCard)) {
            uIController.activatePickColorUI(cardInfo);
            int randNum = Random.Range(0, CardCreator.colors.Length);
            cardInfo.setColor(CardCreator.colors[randNum]);
            
            if (((WildCard)cardInfo).getWildType() == "draw 4 wild") {
                string nextTurn = GameController.checkNextTurn(); // This just checks the next turn, it does NOT set the next turn
                for (int i = 0; i < 4; i++) {
                    AIController.hands[nextTurn].drawCard(AIController.handObjects[nextTurn]);
                }
            }
        } else {
            return false;
        }

        return true;
    }
}
