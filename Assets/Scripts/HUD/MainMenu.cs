using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private string mainScene = "Main Scene";
    private string settingsScene = "SettingsMenu";

    public void Play() {
        SceneManager.LoadScene(mainScene);
    }

    public void LoadSettings() {
        SceneManager.LoadScene(settingsScene, LoadSceneMode.Additive);
    }

    public void Quit() {
        Application.Quit();
    }
}
