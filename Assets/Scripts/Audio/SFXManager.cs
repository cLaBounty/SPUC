using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] AudioMixerGroup audioMixerGroup;

    public static SFXManager instance;
    public Sound[] sounds;

    Dictionary <string, Sound> soundDictionary;

    void Awake(){
        if (instance == null)
            instance = this;
        else{
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        soundDictionary = new Dictionary <string, Sound>();

        foreach (Sound s in sounds){
            s.source        = gameObject.AddComponent<AudioSource>();
            s.source.clip   = s.clip;
            s.source.volume = s.volume;
            s.source.pitch  = s.pitch;
            s.source.loop   = s.loop;
            s.source.outputAudioMixerGroup = audioMixerGroup;
            soundDictionary[s.name] = s;
        }
    }

    public void Play(string name){
        soundDictionary[name].source.Play();
    }

    public void Play(string name, float min, float max){
        soundDictionary[name].source.pitch = Random.Range(min, max);
        soundDictionary[name].source.Play();
    }

    public void Play(string name, bool loop){
        soundDictionary[name].source.loop = loop;
        soundDictionary[name].source.Play();
    }

    public void Play(string name, float min, float max, bool loop){
        soundDictionary[name].source.pitch = Random.Range(min, max);
        soundDictionary[name].source.loop = loop;
        soundDictionary[name].source.Play();
    }

    public void Play(string[] names){
        string name = names[Random.Range(0, names.Length)];
        soundDictionary[name].source.Play();
    }

    public void Play(string[] names, float min, float max){
        string name = names[Random.Range(0, names.Length)];
        soundDictionary[name].source.pitch = Random.Range(min, max);
        soundDictionary[name].source.Play();
    }

    public void Stop(string name){
        soundDictionary[name].source.Stop();
    }
}
