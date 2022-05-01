using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthIncrease : UsableItem
{
    [SerializeField] private float maxHealthIncreaseValue = 20f;

    protected override void Init() {
        HideCrosshair();
    }

    protected override void Use() {
        player.IncreaseMaxHealth(maxHealthIncreaseValue);
        IncreaseLivingRobotHealth();
        hotBar.HandleItemUse(itemObject);
        SFXManager.instance.Play("Eat", 0.95f, 1.05f); // ToDo: find powerup sound effect
    }

    private void IncreaseLivingRobotHealth() {
        List<Enemy> livingRobots = player.GetLivingRobots();
        foreach(Enemy robot in livingRobots) {
            robot.GainHealth(maxHealthIncreaseValue);
        }
    }
}