using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKey : Interactable
{
    BoxCollider box;
    MeshRenderer meshRenderer;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
    }
    protected override void Interact()
    {
        box.enabled = false;
        meshRenderer.enabled = false;
        // set di una variabile del player a true??
    }
}
