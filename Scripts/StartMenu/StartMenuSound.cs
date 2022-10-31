using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuSound : MonoBehaviour
{
    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioSource audioSource;


    public void playButtonSound() {
        audioSource.Play();
    }
}
