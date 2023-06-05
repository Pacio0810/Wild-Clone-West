using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKey : Interactable
{
    BoxCollider box;
    MeshRenderer meshRenderer;
    [SerializeField]OpenChest Chest;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    protected override void Interact()
    {
        box.enabled = false;
        meshRenderer.enabled = false;
        Chest.IsLocked = true;
    }
}
