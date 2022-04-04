using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelManagerHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text phaseText;
    [SerializeField] private TMP_Text enemyCountText;

    private LevelManager manager;
    private Phase displayedPhase = Phase.Prep;
    private int displayedEnemyCount = 0;

    void Start()
    {
        manager = GameObject.FindObjectOfType<LevelManager>();
    }

    void Update()
    {
        // Phase
        if (displayedPhase != manager.currentPhase) {
            if (manager.currentPhase == Phase.Prep) {
                phaseText.text = manager.currentPhase.ToString();
            } else {
                phaseText.text = manager.currentPhase.ToString() + " " + manager.waveCount;
            }
        }

        // Enemy Count
        if (displayedEnemyCount != manager.enemyCount) {
            enemyCountText.text = "Enemy Count: " + manager.enemyCount;
        }
    }
}
