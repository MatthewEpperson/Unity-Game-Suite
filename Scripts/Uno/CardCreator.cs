using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class CardCreator : MonoBehaviour
{
    public GameObject deck;
    public GameObject actionCard;
    public GameObject numberCard;
    public GameObject wildCard;

    public static List<GameObject> listOfCards = new List<GameObject>();

    public static string[] colors = {"white", "yellow", "navy", "gold"};

    public static string[] actionTypes = {"draw 2", "reverse", "skip"};
    public static string[] wildTypes = {"draw 4 wild", "color wild"};

    public static Sprite[] cardSprites;

    void Start() {
        listOfCards.Clear();
        cardSprites = Resources.LoadAll<Sprite>("Sprites/Uno/UNO_cards_deck");
    }

    public void createNumberCard(int number, string color, Transform parent) {
        GameObject card = Instantiate(this.numberCard, new Vector3(0,0,0), Quaternion.identity, parent);

        NumberCard cardInfo = card.GetComponent<NumberCard>();
        card.name = $"{color} card {number}";
        cardInfo.setColor(color);
        cardInfo.setNumber(number);

        Sprite cardSprite = null;

        foreach (Sprite sprite in cardSprites) {
            if (sprite.name == card.name) {
                cardSprite = sprite;
                break;
            }
        }

        card.GetComponent<SpriteRenderer>().sprite = cardSprite;
        listOfCards.Add(card);
    }



    public void createActionCard(string actionType, string color, Transform parent) {
        GameObject card = Instantiate(this.actionCard, new Vector3(0,0,0), Quaternion.identity, parent);

        ActionCard cardInfo = card.GetComponent<ActionCard>();
        card.name = $"{color} {actionType} card";
        cardInfo.setColor(color);
        cardInfo.setActionType(actionType);

        Sprite cardSprite = null;

        foreach (Sprite sprite in cardSprites) {
            if (sprite.name == card.name) {
                cardSprite = sprite;
                break;
            }
        }

        card.GetComponent<SpriteRenderer>().sprite = cardSprite;
        listOfCards.Add(card);
    }



    public void createWildCard(string wildType, string color, Transform parent) {
        GameObject card = Instantiate(this.wildCard, new Vector3(0,0,0), Quaternion.identity, parent);

        WildCard cardInfo = card.GetComponent<WildCard>();
        card.name = $"{wildType} card";
        cardInfo.setColor(color);
        cardInfo.setWildType(wildType);

        Sprite cardSprite = null;

        foreach (Sprite sprite in cardSprites) {
            if (sprite.name == card.name) {
                cardSprite = sprite;
                break;
            }
        }

        card.GetComponent<SpriteRenderer>().sprite = cardSprite;
        listOfCards.Add(card);
    }



    public void createAllWildCards() {
        for (int i = 0; i < 2; i++) {
            for (int j = 0; j < 4; j++) {
                createWildCard(wildTypes[i], "black", deck.transform);
            }
        }
    }




    public void createAllActionCards() {
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 3; j++) {
                createActionCard(actionTypes[j], colors[i], deck.transform);
                createActionCard(actionTypes[j], colors[i], deck.transform);
            }
        }
    }



    public void createAllNumberCards() {
        for (int i = 0; i < 4; i++) { // Create cards for all 4 colors
            for (int j = 0; j < 19; j++) { // Create the numbers for the cards
                if (j == 0) { // Only one 0 in each color
                    createNumberCard(j, colors[i], deck.transform);
                    continue;
                } else {
                    if (j >= 10) {
                        createNumberCard(j - 9, colors[i], deck.transform);
                    } else {
                        createNumberCard(j, colors[i], deck.transform);
                    }
                }
            }
        }
    }

    
}
