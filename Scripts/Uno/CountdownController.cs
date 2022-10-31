using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownController : MonoBehaviour
{

    public static float currentTime = 0f;
    float startingTime = 30f;
    public static float aiTimer;

    public TMP_Text countdownText;
    void Start()
    {
        currentTime = startingTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1 * Time.deltaTime;
        countdownText.text = currentTime.ToString("0");

        if (currentTime <= 0) {
            currentTime = 0;
        }
    }

    public static void resetTimer() {
        currentTime = 30f;
    }

    public static void generateAITimer() {
        aiTimer = Random.Range(25f, 28f);
    }
}
