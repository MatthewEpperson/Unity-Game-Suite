using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class WordleSound : MonoBehaviour
{

    [SerializeField] private AudioSource sound;
    [SerializeField] private AudioSource typeSound;
    [SerializeField] private AudioSource errorSound;
    [SerializeField] private AudioSource victorySound;
    [SerializeField] private AudioSource loseSound;


    public void playTypeSound() {
        if (Regex.IsMatch(WordlePlayer.wordInputField.text, @"^[a-zA-Z]+$")
        || Input.GetKeyDown("backspace")) {
            Debug.Log("Type sound hit");
            typeSound.Play();
        }

    }

    public void playBlockFlipSound() {
        sound.Play();
    }

    public void playErrorSound() {
        errorSound.Play();
    }

    public void playVictorySound() {
        victorySound.Play();
    }

    public void playLoseSound() {
        loseSound.Play();
    }

}
