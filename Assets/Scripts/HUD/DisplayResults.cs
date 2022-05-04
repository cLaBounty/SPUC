using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayResults : MonoBehaviour
{
    /*
     * # of Waves Completed
     * Enemies Killed
     * Damage Dealt
     * Damage Taken
     * Oil Drill Damage Taken
    **/
    [Header("Result Text")]
    [SerializeField] private string win = "You Win!!";
    [SerializeField] private string loss = "You Lose";
    [SerializeField] private string oilDrillLoss = "Your Oil Drill Was Destroyed";
    [SerializeField] private Color winColor;
    [SerializeField] private Color loseColor;

    [Header("Statistics Text")]
    [SerializeField] private string wavesCompleted = "Waves Completed: ";
    [SerializeField] private string enemiesKilled = "Enemies Killed: ";
    [SerializeField] private string damageDealt = "Damage Dealt: ";
    [SerializeField] private string damageTaken = "Damage Taken: ";
    [SerializeField] private string oilDrillDamageTaken = "Oil Drill Damage Taken: ";

    [Header("Game Object References")]
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text wavesCompletedText;
    [SerializeField] private TMP_Text enemiesKilledText;
    [SerializeField] private TMP_Text damageDealtText;
    [SerializeField] private TMP_Text damageTakenText;
    [SerializeField] private TMP_Text oilDrillDamageTakenText;

    private void Start() {
        // Result Text
        switch(LevelManager.endCause) {
            case EndCause.Win:
                resultText.text = win;
                resultText.color = winColor;
                break;
            case EndCause.PlayerLoss:
                resultText.text = loss;
                resultText.color = loseColor;
                break;
            case EndCause.OilDrillLoss:
                resultText.text = oilDrillLoss;
                resultText.color = loseColor;
                break;
        }

        // Statistics Text
        wavesCompletedText.text = wavesCompleted + (Player.WavesCompleted - 1).ToString("n0");
        enemiesKilledText.text = enemiesKilled + Player.EnemiesKilled.ToString("n0");
        damageDealtText.text = damageDealt + Player.DamageDealt.ToString("n0");
        damageTakenText.text = damageTaken + Player.DamageTaken.ToString("n0");
        oilDrillDamageTakenText.text = oilDrillDamageTaken + OilDrill.DamageTaken.ToString("n0");
    }

    private void OnDestroy() {
        ResetStats();
    }

    private void ResetStats() {
        Player.WavesCompleted = 0;
        Player.EnemiesKilled = 0;
        Player.DamageDealt = 0f;
        Player.DamageTaken = 0f;
        OilDrill.DamageTaken = 0f;
    }
}
