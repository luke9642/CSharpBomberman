using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePowerUp : BasePowerUp {

    [SerializeField] int firePowerIncrease = 1;

    protected override void ApplyPowerUp(Player player)
    {
        base.ApplyPowerUp(player);
        player.playerFirePower += firePowerIncrease;
    }
}
