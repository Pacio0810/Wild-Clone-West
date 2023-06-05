using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : PowerUp
{
    [SerializeField] protected Collider player;

    // Start is called before the first frame update
    void Awake()
    {
        value = 20;
        message = "Pick Up Ammo [E]";
    }

    protected override void PowerUpFunction(Collider player)
    {
        player.GetComponent<PlayerSetting>().AddAmmo(value);
        base.PowerUpFunction(player);
    }

    protected override void Interact()
    {
        PowerUpFunction(player);
    }
}
