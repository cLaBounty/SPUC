using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    private string settingsScene = "SettingsMenu";

    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider sensitivitySlider;
    public AudioMixer audioMixer;

    private void Start() {
        float musicVolume = 0f;
        audioMixer.GetFloat("MusicVolume", out musicVolume);
        musicVolumeSlider.value = musicVolume;

        float sfxVolume = 0f;
        audioMixer.GetFloat("SFXVolume", out sfxVolume);
        sfxVolumeSlider.value = sfxVolume;

        sensitivitySlider.value = MouseLook.Sensitivity;
    }

    public void SetMusicVolume(float value) {
        audioMixer.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value) {
        audioMixer.SetFloat("SFXVolume", value);
    }

    public void SetSensitivity(float value) {
        MouseLook.Sensitivity = value;
    }

    public void Back() {
        SceneManager.UnloadScene(settingsScene);
    }
}
