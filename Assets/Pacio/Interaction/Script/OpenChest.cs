using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : Interactable
{
    public bool IsLocked;
    MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    protected override void Interact()
    {
        if (IsLocked)
        {
            Open();
        }
        else
        {
            message = "It's Locked";
        }
    }

    public void Open()
    {
        // play animation

        meshRenderer.material.color = Color.green;
    }
}
