using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseLogic : MonoBehaviour
{

    Scene scene;
    string sceneName;

    void Start() {
        scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
    }
    public void pauseGame() {
        Debug.Log("Game is Paused");
        Time.timeScale = 0;
    }

    public void resumeGame() {
        Time.timeScale = 1;
    }

    public void goToStartMenu() {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartMenuScene");
    }

    public void restartGame() {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}
