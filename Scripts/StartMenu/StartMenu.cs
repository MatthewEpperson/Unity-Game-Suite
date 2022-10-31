using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public TMP_Text currGame;
    private string[] gamesList = {"Uno", "Wordle", "Go"};

    private string[] gamesImages = {"unocover", "wordlecover", "gocover"};

    private Sprite[] images;
    private int gamesListIndex = 0;

    public GameObject placeHolderBox;

    private Sprite gameImage;

    public Image placeHolderImage;
    void Start() {
        images = Resources.LoadAll<Sprite>("Sprites/Main Menu Sprites");
        placeHolderImage = placeHolderBox.GetComponent<Image>();
        foreach (Sprite sprite in images) {
            if (sprite.name == "unocover") {
                gameImage = sprite;
                break;
            }
        }

        placeHolderImage.sprite = gameImage;
    }

    public void quitGame() {
        Debug.Log("QUIT!");
        Application.Quit();
    }

    public void credits() {
        Debug.Log("credits");
    }

    public void settings() {
        Debug.Log("settings");
    }

    public void games() {
        Debug.Log("games");
    }

    public void nextGame() {
        if (gamesListIndex + 1 >= gamesList.Length) {
            gamesListIndex = 0;
        } else {
            gamesListIndex++;
        }

        foreach (Sprite sprite in images) {
            if (sprite.name == gamesImages[gamesListIndex]) {
                gameImage = sprite;
                break;
            }
        }

        placeHolderImage.sprite = gameImage;
        currGame.text = gamesList[gamesListIndex];
    }

    public void prevGame() {
        if (gamesListIndex - 1 < 0) {
            gamesListIndex = gamesList.Length - 1;
        } else {
            gamesListIndex--;
        }
        currGame.text = gamesList[gamesListIndex];

        foreach (Sprite sprite in images) {
            if (sprite.name == gamesImages[gamesListIndex]) {
                gameImage = sprite;
                break;
            }
        }

        placeHolderImage.sprite = gameImage;
    }

    public void playGame() {
        SceneManager.LoadScene(gamesList[gamesListIndex]);
    }
}