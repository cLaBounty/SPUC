using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] private string settingsScene = "SettingsMenu";
    [SerializeField] private string howToPlayScene = "HowToPlayScene";

    public GameObject pauseMenuUI;

    void Update()
    {
        if (InventoryCanvas.InventoryIsOpen) return;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }
    }

    private void Pause() {
        GameIsPaused = true;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SFXManager.instance.Stop("Beam");
    }

    public void Resume() {
        GameIsPaused = false;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SFXManager.instance.Play("Accept");
    }

    public void LoadHowToPlay() {
        SceneManager.LoadScene(howToPlayScene, LoadSceneMode.Additive);
        SFXManager.instance.Play("Accept");
    }

    public void LoadSettings() {
        SceneManager.LoadScene(settingsScene, LoadSceneMode.Additive);
        SFXManager.instance.Play("Accept");
    }

    public void Quit() {
        SFXManager.instance.Play("Back");
        Application.Quit();
    }

    public void PlayHoverSound() {
        SFXManager.instance.Play("Hover");
    }
}
