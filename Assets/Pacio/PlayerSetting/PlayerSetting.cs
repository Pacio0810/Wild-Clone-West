using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    float maxHealth = 100;
    float health = 50;

    float maxAmmo = 160;
    float ammo = 20;

    public void AddHealth(float amount)
    {
        health += amount;
        Mathf.Clamp(health, 0, maxHealth);
    }

    public void AddAmmo(float amount) 
    {
        ammo += amount;
        Mathf.Clamp(ammo, 0, maxAmmo);
    }
}
