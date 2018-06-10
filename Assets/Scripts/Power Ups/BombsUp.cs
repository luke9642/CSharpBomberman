using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombsUp : BasePowerUp
{
    [SerializeField] int bombsIncrement = 1;

    protected override void ApplyPowerUp(Player player)
    {
        base.ApplyPowerUp(player);
        player.playerBombsLimit += bombsIncrement;
    }
}
