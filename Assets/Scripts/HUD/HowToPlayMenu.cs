using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class HowToPlayMenu : MonoBehaviour
{
    [SerializeField] private string howToPlayScene = "HowToPlayScene";

    public AudioMixer audioMixer;

    public void Back() {
        SFXManager.instance.Play("Back");
        SceneManager.UnloadScene(howToPlayScene);
    }

    public void PlayHoverSound() {
        SFXManager.instance.Play("Hover");
    }
}
