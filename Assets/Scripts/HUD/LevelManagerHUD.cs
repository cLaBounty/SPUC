using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerHUD : MonoBehaviour
{
    [SerializeField] private WaveCountdownProgress progressBar;
    [SerializeField] private TMP_Text phaseText;
    [SerializeField] private TMP_Text enemyCountText;

    private LevelManager manager;
    private int displayedWaveCount = 0;
    private int displayedEnemyCount = 0;

    void Start()
    {
        manager = GameObject.FindObjectOfType<LevelManager>();
        progressBar.SetMaxValue(manager.initialPrepTime);
    }

    void Update()
    {
        // Phase
        if (displayedWaveCount != manager.waveCount) {
            if (manager.waveCount != manager.numberOfWaves) {
                phaseText.text = "Wave " + manager.waveCount;
                progressBar.SetMaxValue(manager.timeBetweenWaves);
            } else {
                phaseText.text = "Final Wave";
                progressBar.SetMaxValue(0f);
            }
            displayedWaveCount = displayedWaveCount + 1;
        }

        // Enemy Count
        if (displayedEnemyCount != manager.enemyCount) {
            enemyCountText.text = "Enemy Count: " + manager.enemyCount;
            displayedEnemyCount = manager.enemyCount;
        }
    }
}