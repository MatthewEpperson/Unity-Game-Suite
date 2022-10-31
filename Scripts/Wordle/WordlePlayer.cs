using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;

public class WordlePlayer : MonoBehaviour
{

    public string playerInputWord; // String to keep track of what player types from input field
    private string[] words = System.IO.File.ReadAllLines("Assets/Scripts/Wordle/words.csv"); // Parses words.csv into an array
    public static string correctWord;

    [SerializeField]
    public int attempts; // keeps track of attempts the player has used

    public static bool playerWin;
    public static bool gameOver;

    private WordleUI wordleUI; 

    public TMP_Text corrWord; // This is just for testing and displays on screen. Will remove once we merge into production.

    public static TMP_InputField wordInputField; // this is for input field itself



    // Start is called before the first frame update
    void Start()
    {
        correctWord = generateWord(); // pulls a random word from words.csv and assigns it
        playerWin = false;
        gameOver = false;

        Debug.Log($"Correct word: {correctWord}");
        wordleUI = GameObject.Find("Background").GetComponent<WordleUI>();

        wordInputField = GameObject.Find("PlayerWordGuess").GetComponent<TMP_InputField>();
        wordInputField.ActivateInputField();
    }

        // This is a default unity function. Whatever is in this function gets executed every single frame.
    void Update() {
        if (Time.timeScale != 0) {
            if (!WordleUI.isBlockAnimPlaying) {
                wordInputField.ActivateInputField();
            } else {
                wordInputField.DeactivateInputField();
            }

            if (Input.GetKeyDown("return") && !WordleUI.isBlockAnimPlaying) {
                StartCoroutine(wordleUI.changeBlockColor());

                if (isValidWord()) {
                    attempts--;
                }

                if (playerInputWord.Equals(correctWord) || attempts <= 0) {
                    if (playerInputWord.Equals(correctWord)) {
                        playerWin = true;
                    }
                    gameOver = true;
                }

            } else {
                wordInputField.MoveTextEnd(true);
            }
        } else {
            wordInputField.DeactivateInputField();
        }
    }

    
    /* playerGuess is whatever the player types into the input field and automatically updates. 
        We have to assign it to playerInputWord so we can actually do logic with it in the code.
    */
    public void readPlayerInput(string playerGuess) {
        playerInputWord = Regex.Replace(playerInputWord, @"[^a-zA-Z]", "");
        playerInputWord = playerGuess.ToLower();
        wordInputField.text = Regex.Replace(playerInputWord, @"[^a-zA-Z]", "");
    }


    private string generateWord() {
        int randomInd = Random.Range(0, words.Length);
        return words[randomInd];
    }

    // Determines if the word is valid (already constrained to 5 characters, but just in case)
    public bool isValidWord() {
        if (playerInputWord.Length == 5 && ((IList)words).Contains(playerInputWord)) {
            return true;
        }
        return false;
    }

    
    // Returns the word the player entered, but only if it is a valid word.
    public string getPlayerWord() {
        if (isValidWord()) {
            return playerInputWord;
        }
        return null;
    }
}
