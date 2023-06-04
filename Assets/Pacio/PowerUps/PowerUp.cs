using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PowerUp : MonoBehaviour
{
    BoxCollider box;
    MeshRenderer meshRenderer;

    [SerializeField] protected float value;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        box = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PowerUpFunction(other);
        }
    }

    protected virtual void PowerUpFunction(Collider player)
    {
        box.enabled = false;
        meshRenderer.enabled = false;
    } 
}
