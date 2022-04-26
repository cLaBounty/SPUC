using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] AudioSource combatMusic;
    [SerializeField] AudioSource passiveMusic;
    [SerializeField] float changeRate = 0.5f;

    LevelManager levelManager;

    float combatChangeRate = 0;
    float passiveChangeRate = 0;

    public enum MUSIC_STATE{
        COMBAT,
        PASSIVE,
    } 

    void Start(){
        passiveMusic.Play();
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    public void ChangeMusicState(MUSIC_STATE state){
        switch(state){
            case MUSIC_STATE.COMBAT:  
                combatChangeRate = 1f;  
                passiveChangeRate = -2f;
                if (!combatMusic.isPlaying) combatMusic.Play();
                break;
            case MUSIC_STATE.PASSIVE: 
                combatChangeRate = -2f; 
                passiveChangeRate =  1f;  
                if (!passiveMusic.isPlaying) passiveMusic.Play();
                break;
        }
    }

    void Update(){
        if (combatChangeRate != 0f){
            combatMusic.volume += combatChangeRate * changeRate * Time.deltaTime;

            if (combatMusic.volume <= 0) combatMusic.Stop();

            if (combatMusic.volume < 0 || combatMusic.volume > 1){
                combatMusic.volume = Mathf.Clamp(combatMusic.volume, 0, 1);
                combatChangeRate = 0f;
            }
        }

        if (passiveChangeRate != 0f){
            passiveMusic.volume += passiveChangeRate * changeRate * Time.deltaTime;

            if (passiveMusic.volume <= 0) passiveMusic.Stop();

            if (passiveMusic.volume < 0 || passiveMusic.volume > 1){
                passiveMusic.volume = Mathf.Clamp(passiveMusic.volume, 0, 1);
                passiveChangeRate = 0f;
            }
        }
    }

    void LateUpdate(){
        if (levelManager.enemyCount == 0 && passiveMusic.volume == 0f)
            ChangeMusicState(MUSIC_STATE.PASSIVE);
        else if (levelManager.enemyCount != 0 && combatMusic.volume == 0f)
            ChangeMusicState(MUSIC_STATE.COMBAT);
    }

    void OnDestroy(){
        passiveMusic.Stop();
        combatMusic.Stop();
    }
}
