using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using static UnityEngine.Rendering.DebugUI;

public class HealthPowerUp : PowerUp
{
    [SerializeField] VisualEffect HealthVfx;

    private void Awake()
    {
        value = 10;
    }

    protected override void PowerUpFunction(Collider player)
    {
        player.GetComponent<PlayerSetting>().AddHealth(value);
        HealthVfx.Play();
        base.PowerUpFunction(player);

    }
}
