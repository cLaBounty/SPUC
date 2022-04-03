using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    private string settingsScene = "SettingsMenu";

    public Slider volumeSlider;
    public Slider sensitivitySlider;
    public AudioMixer audioMixer;

    private void Start() {
        float volume = 0f;
        audioMixer.GetFloat("Volume", out volume);
        volumeSlider.value = volume;
        sensitivitySlider.value = MouseLook.Sensitivity;
    }

    public void SetVolume(float value) {
        audioMixer.SetFloat("Volume", value);
    }

    public void SetSensitivity(float value) {
        MouseLook.Sensitivity = value;
    }

    public void Back() {
        SceneManager.UnloadScene(settingsScene);
    }
}
