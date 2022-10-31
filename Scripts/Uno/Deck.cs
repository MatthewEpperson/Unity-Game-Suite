using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private GameObject cardDeck;

    [SerializeField] private CardCreator cardCreator;
    public static Stack<GameObject> deck = new Stack<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        deck.Clear();
        cardCreator.createAllNumberCards();
        cardCreator.createAllActionCards();
        cardCreator.createAllWildCards();
        shuffleDeck();
    }


    private void shuffleDeck() { // Uses this algorithm https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        int cardCount = CardCreator.listOfCards.Count;

        for (int i = 0; i < cardCount; i++) {
            GameObject temp = CardCreator.listOfCards[i];
            int randomIndex = Random.Range(i, cardCount);
            CardCreator.listOfCards[i] = CardCreator.listOfCards[randomIndex];
            CardCreator.listOfCards[randomIndex] = temp;
        }

        foreach (GameObject card in CardCreator.listOfCards) {
            deck.Push(card);
        }

    }


    public static GameObject getCardFromDeck() {
        if (deck.Count > 0) {
            return deck.Pop();
        }
        return null;
    }

}
