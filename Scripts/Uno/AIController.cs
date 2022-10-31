using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject playerObj;
    [SerializeField] GameObject opponentObj1;
    [SerializeField] GameObject opponentObj2;
    [SerializeField] GameObject opponentObj3;


    [SerializeField] Hand oppHand1;
    [SerializeField] Hand oppHand2;
    [SerializeField] Hand oppHand3;
    [SerializeField] Hand playerHand;


    [SerializeField] GameController gameController;
    private Hand hand;
    private GameObject handGameObj;
    private List<GameObject> playableCards = new List<GameObject>();

    public static Dictionary<string, Hand> hands = new Dictionary<string, Hand>();
    public static Dictionary<string, GameObject> handObjects = new Dictionary<string, GameObject>();


    void Start()
    {
        hands.Clear();
        handObjects.Clear();
        playableCards.Clear();

        if (hands.Count == 0) {
            hands.Add(GameController.players[0], playerHand);
            hands.Add(GameController.players[1], oppHand1);
            hands.Add(GameController.players[2], oppHand2);
            hands.Add(GameController.players[3], oppHand3);
        }

        if (handObjects.Count == 0) {
            handObjects.Add(GameController.players[0], playerObj);
            handObjects.Add(GameController.players[1], opponentObj1);
            handObjects.Add(GameController.players[2], opponentObj2);
            handObjects.Add(GameController.players[3], opponentObj3);
        }
        // StartCoroutine(findAllPlayableCards());
    }


    // Update is called once per frame
    void Update()
    {
        if (GameController.currTurn.Equals("Opponent 1")) {
            hand = oppHand1;
            handGameObj = opponentObj1;
        }
        if (GameController.currTurn.Equals("Opponent 2")) {
            hand = oppHand2;
            handGameObj = opponentObj2;
        }
        if (GameController.currTurn.Equals("Opponent 3")) {
            hand = oppHand3;
            handGameObj = opponentObj3;
        }

        if (GameController.currTurn != "Player") {
            StartCoroutine(findAllPlayableCards());
            int randNum = Random.Range(0, playableCards.Count);
            if (CountdownController.currentTime <= CountdownController.aiTimer) {
                Sprite spriteCard = null;
                GameObject card = playableCards[randNum];
                foreach (Sprite sprite in CardCreator.cardSprites) {
                    if (sprite.name == card.gameObject.name) {
                        spriteCard = sprite;
                        break;
                    }
                }
                card.GetComponent<SpriteRenderer>().sprite = spriteCard;
                card.transform.localScale = new Vector2(14, 14);
                StartCoroutine(CardUI.moveToPlayArea(card, PlayAreaDeck.getPlayArea()));
                hand.playCard(checkCardType(card), PlayAreaDeck.getPlayArea());
            }
        }
    }


    private GameObject checkCardType(GameObject card) {
        if (isActionCard(card)) {
            return card;
        }

        if (isWildCard(card)) {
            return card;
        }
        return card;
    }


    private bool isActionCard(GameObject card) {
        Card cardInfo = card.GetComponent<Card>();
        if (cardInfo.GetType() == typeof(ActionCard)) {
            if (((ActionCard)cardInfo).getActionType() == "draw 2") {
                string nextTurn = GameController.checkNextTurn(); // This just checks the next turn, it does NOT set the next turn
                Debug.Log($"Next Turn: {nextTurn}");
                hands[nextTurn].drawCard(handObjects[nextTurn]);
                hands[nextTurn].drawCard(handObjects[nextTurn]);
                GameController.nextTurn();
                return true;
            } else if (((ActionCard)cardInfo).getActionType() == "skip") {
                GameController.nextTurn();
                return true;
            } else if (((ActionCard)cardInfo).getActionType() == "reverse") {
                GameController.isReversed = !GameController.isReversed;
                return true;
            }
        } else {
            return false;
        }
        return true;
    }


    private bool isWildCard(GameObject card) {
        Card cardInfo = card.GetComponent<Card>();
        Sprite cardSprite = null;

        int randNum = Random.Range(0, CardCreator.colors.Length);
        
        if (cardInfo.GetType() == typeof(WildCard)) {
            cardInfo.setColor(CardCreator.colors[randNum]);
            
            if (((WildCard)cardInfo).getWildType() == "draw 4 wild") {
                string nextTurn = GameController.checkNextTurn(); // This just checks the next turn, it does NOT set the next turn
                for (int i = 0; i < 4; i++) {
                    hands[nextTurn].drawCard(handObjects[nextTurn]);
                }

                foreach (Sprite sprite in CardCreator.cardSprites) {
                    if (sprite.name == $"{CardCreator.colors[randNum]} draw 4 card") {
                        cardSprite = sprite;
                        break;
                    }
                }

                card.GetComponent<SpriteRenderer>().sprite = cardSprite;
                GameController.nextTurn();
            }
        } else {
            return false;
        }

        foreach (Sprite sprite in CardCreator.cardSprites) {
            if (sprite.name == $"{CardCreator.colors[randNum]} wild card") {
                cardSprite = sprite;
                break;
            }
        }

                card.GetComponent<SpriteRenderer>().sprite = cardSprite;

        return true;
    }


    IEnumerator findAllPlayableCards() {
        playableCards.Clear();
        foreach(GameObject card in hand.getCardsInHand()) {
            if (GameController.isPlayable(card)) {
                playableCards.Add(card);
            }
        }
        if (playableCards.Count == 0) {
            hand.drawCard(handGameObj);
            GameObject newCard = hand.getCardsInHand()[hand.getCardsInHand().Count - 1];
            if (GameController.isPlayable(newCard)) {
                playableCards.Add(newCard);
            } else {
                GameController.nextTurn();
            }
        }
        yield return null;
    }

}
