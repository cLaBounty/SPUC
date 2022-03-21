using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string mainScene = "MainScene";

    public void Play() {
        SceneManager.LoadScene(mainScene);
    }

    public void Quit() {
        Application.Quit();
    }
}
