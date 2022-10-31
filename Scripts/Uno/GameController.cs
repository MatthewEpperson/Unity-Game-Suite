using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    [SerializeField] private GameObject opponentHand1;
    [SerializeField] private GameObject opponentHand2;
    [SerializeField] private GameObject opponentHand3;
    [SerializeField] private GameObject playerHand;

    public static string[] players = {"Player", "Opponent 1", "Opponent 2", "Opponent 3"};
    private static Dictionary<string, GameObject> hands = new Dictionary<string, GameObject>();
    public static string currTurn;

    public static bool playerWin;

    // private List<GameObject> hands = new List<GameObject>();

    void Start()
    {
        hands.Clear();
        
        playerWin = false;

        currTurn = players[0]; // Game always starts with player

        if (hands.Count == 0) {
            hands.Add(players[0], playerHand);
            hands.Add(players[1], opponentHand1);
            hands.Add(players[2], opponentHand2);
            hands.Add(players[3], opponentHand3);
        }

        Debug.Log(hands.Count);

        StartCoroutine(dealStartCard(PlayAreaDeck.getPlayArea(), PlayAreaDeck.playAreaStack));

        foreach (GameObject hand in hands.Values) {
            Hand currHand = hand.GetComponent<Hand>();
            StartCoroutine(dealInitialCards(currHand.getCardsInHand(), currHand.getHand()));
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (CountdownController.currentTime <= 0) {
            Hand hand = hands[currTurn].GetComponent<Hand>();
            if (hand.getCardsInHand().Count == 1) {
                hand.drawCard(hands[currTurn]);
            }
            hand.drawCard(hands[currTurn]);
            nextTurn();
        }
        Debug.Log(currTurn);
    }

    IEnumerator dealInitialCards(List<GameObject> cardsInhand, GameObject hand) {
        yield return new WaitForSeconds(0.3f); // wait 0.3 seconds to allow deck to be shuffled before dealing cards
        Sprite backCardSprite = Resources.Load<Sprite>("Sprites/Uno/UNO_Back");
        for (int i = 0; i < 7; i++) {
            GameObject card = Deck.getCardFromDeck();
            if (hand == opponentHand1 || hand == opponentHand2 || hand == opponentHand3) {
                card.GetComponent<SpriteRenderer>().sprite = backCardSprite;
                card.transform.localScale = new Vector2(50, 50);
            }
            card.transform.SetParent(hand.transform, false);
            Hand.applyCardPosition(card, cardsInhand, hand);
            cardsInhand.Add(card);
        }
    }

    IEnumerator dealStartCard(GameObject playAreaDeck, Stack<GameObject> playAreaStack) {
        yield return new WaitForSeconds(0.3f);
        GameObject card = Deck.getCardFromDeck();
        while (card.GetComponent<Card>().GetType() == typeof(WildCard)) {
            card = Deck.getCardFromDeck();
        }
        card.transform.SetParent(playAreaDeck.transform, false);
        card.transform.position = playAreaDeck.transform.position;
        playAreaStack.Push(card);
    }



    public static bool isPlayable(GameObject card) {
        Card cardInfo = card.GetComponent<Card>();
        Card cardOnPlayArea = PlayAreaDeck.getCardFromPlayArea().GetComponent<Card>();

        if (cardInfo.getColor() == cardOnPlayArea.getColor()) {
                return true;
        } else if (cardInfo.GetType() == typeof(NumberCard) &&
                    cardOnPlayArea.GetType() == typeof(NumberCard)) {
            if (((NumberCard)cardInfo).getNumber() == ((NumberCard)cardOnPlayArea).getNumber()) {
                return true;
            }
        } else if (cardInfo.GetType() == typeof(WildCard)) {
            return true;
        }

        return false;
    }



    public static bool isReversed = false;

    // THIS SETS THE NEXT TURN
    public static void nextTurn() {
        int indexOfTurn = Array.IndexOf(players, currTurn);
        
        if (isReversed == false) {
            if (indexOfTurn >= players.Length - 1) {
                indexOfTurn = 0;
            } else {
                indexOfTurn++;
            }
        } else {
            if (indexOfTurn - 1 < 0) {
                indexOfTurn = players.Length - 1;
            } else {
                indexOfTurn--;
            }
        }

        if (indexOfTurn != 0) {
            CountdownController.generateAITimer();
        }
        currTurn = players[indexOfTurn];
        CountdownController.resetTimer();
    }


    // THIS ONLY CHECKS WHAT THE NEXT TURN IS, IT DOES NOT SET THE NEXT TURN
    public static string checkNextTurn() {
        int indexOfTurn = Array.IndexOf(players, currTurn);
        if (isReversed == false) {
            if (indexOfTurn >= players.Length - 1) {
                indexOfTurn = 0;
            } else {
                indexOfTurn++;
            }
        } else {
            if (indexOfTurn - 1 < 0) {
                indexOfTurn = players.Length - 1;
            } else {
                indexOfTurn--;
            }
        }

        return players[indexOfTurn];
    }

}
