using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{

    public Slider volumeSlider;
    AudioSource bgMusic;

    void Start() {
        bgMusic = GameObject.Find("Main Camera").GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update() {
        if (Input.GetKey("escape")) {
            SceneManager.LoadScene("StartMenuScene");
        }
        
        setVolumeBySlider();
    }

    void setVolumeBySlider() {
        bgMusic.volume = volumeSlider.value;
    }
}
