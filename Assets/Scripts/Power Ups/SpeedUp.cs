using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : BasePowerUp {

    [SerializeField] float speedIncrease = 0.75f;

    protected override void ApplyPowerUp(Player player)
    {
        base.ApplyPowerUp(player);
        player.playerSpeed += speedIncrease;
    }
}
