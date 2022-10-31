using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WordleUI : MonoBehaviour
{
    public GameObject letterBlock; 
    private Transform blocksPanel;
    private List<List<GameObject>> letterBlocksList;
    private WordlePlayer wordlePlayer;

    private GameObject gameOverMenu;
    private GameObject title;
    public static bool isBlockAnimPlaying;
    private string playerInputWord;

    private int playerWordIndex = 0;

    private Dictionary<char, int> letterCount = new Dictionary<char, int>(); // Dictionary to count amount of each letter in word


    // Start is called before the first frame update
    void Start()
    {
        gameOverMenu = GameObject.Find("Game Over Menu");
        title = GameObject.Find("Title");
        isBlockAnimPlaying = false;
        blocksPanel = GameObject.Find("BlocksPanel").GetComponent<Transform>();
        letterBlocksList = new List<List<GameObject>>();
        wordlePlayer = GameObject.Find("Game Manager").GetComponent<WordlePlayer>();
        /* The inner loop create the 5 blocks horizontally for the words.
            We want 6 rows so we do it 6 times with the outer loop. 
        */
        for (int j = 0; j < wordlePlayer.attempts; j++) {
            letterBlocksList.Add(new List<GameObject>());
            for (int i = 0; i < 5; i++) {
                GameObject gameObj = Instantiate(letterBlock, new Vector3(0,0,0), 
                                            Quaternion.identity, blocksPanel);
                letterBlocksList[j].Add(gameObj);
            }
        }

        StartCoroutine(hoverTitle());

        toggleGameOverMenu();

    }



    private int blockRow = 0; // Used to keep track of which row of blocks to change
    void Update() {

        /* NOTE 1: Handle logic to update every block as player types in Input Field. 
        */
        playerInputWord = wordlePlayer.playerInputWord;
        string letter = " ";

        if (!isBlockAnimPlaying) {
            if (playerInputWord.Length <= 0 || Input.GetKey("backspace")) {
                playerWordIndex = playerInputWord.Length <= 0 ? 0 : playerInputWord.Length;
                letter = " ";
            } else if (playerInputWord.Length > 0 && !Input.GetKey("backspace")) {
                playerWordIndex = playerInputWord.Length <= 0 ? 0 : playerInputWord.Length - 1;
                letter = playerInputWord[playerWordIndex].ToString();
            }
            if (playerWordIndex >= 5) {
                playerWordIndex -= 1;
            }

            Transform blockText = letterBlocksList[blockRow][playerWordIndex].transform.Find("Text");

            if (!WordlePlayer.gameOver) {
                blockText.GetComponent<TMP_Text>().text = letter;
            }
        }

        // END OF NOTE 1
    }


    // Populate letterCount dictionary with the occurrences of each letter in Correct Word
    private void countLettersCorrectWord() {
        letterCount.Clear();
        foreach (char letter in WordlePlayer.correctWord) {
            if (letterCount.ContainsKey(letter)) {
                letterCount[letter]++;
            } else {
                letterCount.TryAdd(letter, 1);
            }
        }
    }



    // This coroutine add a rotation animation and applies the color when the image is flat
    IEnumerator rotateBlock(GameObject block, Color32 color) {
        Quaternion oldPosition = block.transform.rotation;
        float rotateSpeed = 350f;
        Quaternion endingPos = Quaternion.Euler(new Vector3(90, 0, 0));
        
        block.GetComponent<WordleSound>().playBlockFlipSound();

        while (Vector3.Distance(block.transform.rotation.eulerAngles, endingPos.eulerAngles) > 0.01f) {
            block.transform.rotation = Quaternion.RotateTowards(block.transform.rotation, endingPos, rotateSpeed * Time.deltaTime);
            yield return null;
        }

        endingPos = Quaternion.Euler(new Vector3(0, 0, 0));
        while (Vector3.Distance(block.transform.rotation.eulerAngles, endingPos.eulerAngles) > 0.01f) {
            if (block.transform.rotation.x >= -0.50f) {
                block.GetComponent<Image>().color = color;
            }
            block.transform.rotation = Quaternion.RotateTowards(block.transform.rotation, endingPos, rotateSpeed * Time.deltaTime);
            yield return null;
        }
        block.GetComponent<Image>().color = color;
        block.transform.rotation = endingPos;
    }



    // Bounces block when player guesses correct word similar to actual wordle
    IEnumerator bounceBlock(GameObject block) {

        Vector3 oldBlockPos = block.transform.position;

        float block_x = block.transform.position.x;
        float block_y = block.transform.position.y;
        float block_z = block.transform.position.z;
    
        float bounceSpeed = 175f;

        while (block_y < (oldBlockPos.y + 25f)) {
            block_y += bounceSpeed * Time.deltaTime;
            block.transform.position = new Vector3(block_x, block_y, block_z);
            yield return null;
        }

        while (block_y > oldBlockPos.y) {
            block_y -= bounceSpeed * Time.deltaTime;
            block.transform.position = new Vector3(block_x, block_y, block_z);
            yield return null;
        }

        block.transform.position = oldBlockPos;
        yield return null;
    }



    public IEnumerator changeBlockColor() {
        isBlockAnimPlaying = true;
        countLettersCorrectWord();

        string playerInputWord = wordlePlayer.playerInputWord;
        string correctWord = WordlePlayer.correctWord;

        int attempts = wordlePlayer.attempts;

        if (attempts <= 0) { // if player is out of attempts, don't execute 
            yield return null;
        }

        Color32 yellow = new Color32(255, 235, 32, 255);
        Color32 green = new Color32(159, 224, 139, 255);
        Color32 gray = new Color32(132, 139, 161, 221);
        Color32 red = new Color32(222, 109, 78, 215);
        Color32 defaultColor = new Color32(255, 255, 255, 255);

        // Stores correct color of a block. This is way we can handle block animation after handling logic.
        string[] blockColors = new string[5];


        // Flashes blocks red signifying that the user did not input a valid word
        if (!wordlePlayer.isValidWord()) {
            for (int i = 0; i < 4; i++) { // flash four times
                for (int j = 0; j < 5; j++) {
                    letterBlocksList[blockRow][j].GetComponent<Image>().color = red;
                    blocksPanel.GetComponent<WordleSound>().playErrorSound();
                }
                yield return new WaitForSeconds(0.1f);
                for (int j = 0; j < 5; j++) {
                    letterBlocksList[blockRow][j].GetComponent<Image>().color = defaultColor;
                }
                yield return new WaitForSeconds(0.1f);
            }
            isBlockAnimPlaying = false;
            yield break;
        }

       
       // Counts green blocks AKA correct positions
        for (int i = 0; i < playerInputWord.Length; i++) {
            char currLetter = correctWord[i];
            char guessLetter = playerInputWord[i];

            if (currLetter == guessLetter) { // Turn block green if guessLetter is in correct position
                blockColors[i] = "green";
                letterCount[currLetter] = letterCount[currLetter] - 1;
                continue;
            }
        }


        // Counts the yellow and gray blocks
        for (int i = 0; i < playerInputWord.Length; i++) {
            char currLetter = correctWord[i];
            char guessLetter = playerInputWord[i];
            if (currLetter != guessLetter && correctWord.Contains(guessLetter)) { // Turn block yellow if in wrong position
                if (letterCount[guessLetter] > 0) {
                    blockColors[i] = "yellow";
                    letterCount[guessLetter] = letterCount[guessLetter] - 1;
                    continue;
                }
            }
            if (currLetter != guessLetter) {
                blockColors[i] = "gray";
            }
        }


        // Logic for animation of blocks
        for (int i = 0; i < blockColors.Length; i++) {
            if (blockColors[i].Equals("green")) {
                StartCoroutine(rotateBlock(letterBlocksList[blockRow][i], green));
            } else if (blockColors[i].Equals("yellow")) {
                StartCoroutine(rotateBlock(letterBlocksList[blockRow][i], yellow));
            } else if (blockColors[i].Equals("gray")) {
                StartCoroutine(rotateBlock(letterBlocksList[blockRow][i], gray));
            }
            yield return new WaitForSeconds(0.3f);
        }


        if (WordlePlayer.gameOver) {
            if (WordlePlayer.playerWin) {
                blocksPanel.GetComponent<WordleSound>().playVictorySound();
            } else {
                blocksPanel.GetComponent<WordleSound>().playLoseSound();
                GameObject.Find("Main Camera").GetComponent<AudioSource>().Stop();
            }
            for (int i = 0; i < 5; i++) {
                StartCoroutine(bounceBlock(letterBlocksList[blockRow][i]));
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(0.5f);
            toggleGameOverMenu();

        }

        WordlePlayer.wordInputField.text = ""; // Clear input field after animations are done

        isBlockAnimPlaying = false;
        
        if (blockRow + 1 >= 6) {
            blockRow = 5;
        } else {
            blockRow++;
        }
    }



    IEnumerator hoverTitle() {
        Vector3 oldPos = title.transform.position;

        float speed = 15.0f;

        float title_x = title.transform.position.x;
        float title_y = title.transform.position.y;
        float title_z = title.transform.position.z;

        while (true) {
            while (title_y > (oldPos.y - 10.0f)) {
                title_y -= speed * Time.deltaTime;
                title.transform.position = new Vector3(title_x, title_y, title_z);
                yield return null;
            }

            while (title_y < (oldPos.y)) {
                title_y += speed * Time.deltaTime;
                title.transform.position = new Vector3(title_x, title_y, title_z);
                yield return null;
            }

            title.transform.position = oldPos;
        }

    }


    private void toggleGameOverMenu() {

        gameOverMenu.SetActive(!gameOverMenu.activeSelf);

        if (gameOverMenu.activeSelf) {

            TMP_Text gameOverText = GameObject.Find("Game Over Text").GetComponent<TMP_Text>();

            if (WordlePlayer.playerWin) {
                gameOverText.text = "You WON!";
            } else {
                gameOverText.text = $"You LOST!\n The correct word was: {WordlePlayer.correctWord.ToUpper()}";
            }

        }
    }
}