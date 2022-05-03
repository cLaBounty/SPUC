using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private string settingsScene = "SettingsMenu";

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
        if (value == musicVolumeSlider.minValue)
            audioMixer.SetFloat("MusicVolume", -80f);
        else
            audioMixer.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value) {
        if (value == sfxVolumeSlider.minValue)
            audioMixer.SetFloat("SFXVolume", -80f);
        else
            audioMixer.SetFloat("SFXVolume", value);
    }

    public void SetSensitivity(float value) {
        MouseLook.Sensitivity = value;
    }

    public void Back() {
        SFXManager.instance.Play("Back");
        SceneManager.UnloadScene(settingsScene);
    }

    public void PlayHoverSound() {
        SFXManager.instance.Play("Hover");
    }

    public void PlayAcceptSound() {
        SFXManager.instance.Play("Accept");
    }
}
