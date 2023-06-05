using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    float maxHealth = 100;
    [SerializeField]float health = 50;

    float maxAmmo = 160;
    [SerializeField]float ammo = 20;
    float curveValue = 0;
    [SerializeField] float curveTimer;

    [SerializeField] SkinnedMeshRenderer meshRenderer;
    [SerializeField] AnimationCurve FresnelCurve;
    public float speed;

    public void AddHealth(float amount)
    {
        health += amount;
        Mathf.Clamp(health, 0, maxHealth);
        StartCoroutine(Courutine(curveTimer));
    }
    IEnumerator Courutine(float timer)
    {
        //float boh = meshRenderer.sharedMaterial.GetFloat("_Float") <= 0 ? 1 : 0;
        //meshRenderer.sharedMaterial.SetFloat("_Float",Mathf.Lerp(meshRenderer.sharedMaterial.GetFloat("_Float"), boh, Time.deltaTime * speed));
        //if (meshRenderer.sharedMaterial.GetFloat("_Float") <= 0.1) 
        //{
        //    meshRenderer.sharedMaterial.SetFloat("_Float", 0);
        //}
        yield return new WaitForSeconds(timer);
        curveValue += timer;
        meshRenderer.sharedMaterial.SetFloat("_FresnelPower", FresnelCurve.Evaluate(curveValue));

        if (curveValue >= 2)
        {
            curveValue = 0;
            StopCoroutine(Courutine(curveTimer));
        }
        else
        {
            StartCoroutine(Courutine(curveTimer));
        }
    }

    public void AddAmmo(float amount)
    {
        ammo += amount;
        Mathf.Clamp(ammo, 0, maxAmmo);
    }
}
