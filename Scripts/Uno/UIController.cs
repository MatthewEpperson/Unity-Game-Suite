using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{

    public GameObject pickColorPanel;

    public GameObject unoButton;

    private Card card;

    public Image playerBar;
    public Image opponent1Bar;
    public Image opponent2Bar;
    public Image opponent3Bar;

    public GameObject gameOverMenu;

    public TMP_Text gameOverText;

    public static bool colorPanelActive = false;
    public static bool unoButtonActive = false;

    // Start is called before the first frame update
    void Start()
    {
        colorPanelActive = false;
        unoButtonActive = false;
        pickColorPanel.SetActive(false);
        unoButton.SetActive(false);
    }

    void Update()
    {
        if (GameController.currTurn == "Player") {
            playerBar.color = new Color32(255, 100, 15, 255);
        } else {
            playerBar.color = new Color32(100, 100, 100, 255);
        }

        if (GameController.currTurn == "Opponent 1") {
            opponent1Bar.color = new Color32(255, 100, 15, 255);
        } else {
            opponent1Bar.color = new Color32(100, 100, 100, 255);
        }

        if (GameController.currTurn == "Opponent 2") {
            opponent2Bar.color = new Color32(255, 100, 15, 255);
        } else {
            opponent2Bar.color = new Color32(100, 100, 100, 255);
        }

        if (GameController.currTurn == "Opponent 3") {
            opponent3Bar.color = new Color32(255, 100, 15, 255);
        } else {
            opponent3Bar.color = new Color32(100, 100, 100, 255);
        }
    }


    public void activateUnoButton() {
        if (unoButton.activeSelf == false) {
            unoButtonActive = true;
            unoButton.SetActive(true);
        }
    }

    public void deactivateUnoButton() {
        if (unoButton.activeSelf == true) {
            GameController.nextTurn();
            unoButtonActive = false;
            unoButton.SetActive(false);
        }
    }


    public void activatePickColorUI(Card cardInfo) {
        // pickColorPanel = GameObject.Find("Choose Color Panel");
        card = cardInfo;
        if (pickColorPanel.activeSelf == false) {
            colorPanelActive = true;
            pickColorPanel.SetActive(true);
        }
    }

    public void deactivatePickColorUI() {
        if (pickColorPanel.activeSelf == true) {
            colorPanelActive = false;
            pickColorPanel.SetActive(false);
        }
    }

    public void setCardColor(string color) {
        card.setColor(color);
        deactivatePickColorUI();
        Sprite cardSprite = null;

        if (PlayAreaDeck.getCardFromPlayArea().GetComponent<WildCard>().getWildType() != "draw 4 wild") {
            foreach (Sprite sprite in CardCreator.cardSprites) {
                if (sprite.name == $"{color} wild card") {
                    cardSprite = sprite;
                    break;
                }
            }
            card.GetComponent<SpriteRenderer>().sprite = cardSprite;
            GameController.nextTurn();
            return;
        }


        foreach (Sprite sprite in CardCreator.cardSprites) {
            if (sprite.name == $"{color} draw 4 card") {
                cardSprite = sprite;
                break;
            }
        }

        card.GetComponent<SpriteRenderer>().sprite = cardSprite;
        GameController.nextTurn();
        GameController.nextTurn();
    }


    public void toggleGameOverMenu() {

        gameOverMenu.SetActive(!gameOverMenu.activeSelf);
        if (gameOverMenu.activeSelf) {
            if (GameController.playerWin) {
                gameOverText.text = "You WON!";
            } else {
                gameOverText.text = $"You LOST!";
            }

        }
        
        Time.timeScale = 0;
    }

}
