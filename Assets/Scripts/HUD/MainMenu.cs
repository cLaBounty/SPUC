using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string mainScene = "MainScene";
    [SerializeField] private string howToPlayScene = "HowToPlayScene";
    [SerializeField] private string settingsScene = "SettingsMenu";

    public void Play() {
        SFXManager.instance.Play("Accept");
        SceneManager.LoadScene(mainScene);
    }

    public void LoadHowToPlay() {
        SFXManager.instance.Play("Accept");
        SceneManager.LoadScene(howToPlayScene, LoadSceneMode.Additive);
    }

    public void LoadSettings() {
        SFXManager.instance.Play("Accept");
        SceneManager.LoadScene(settingsScene, LoadSceneMode.Additive);
    }

    public void Quit() {
        SFXManager.instance.Play("Back");
        Application.Quit();
    }
    
    public void PlayHoverSound() {
        SFXManager.instance.Play("Hover");
    }
}
