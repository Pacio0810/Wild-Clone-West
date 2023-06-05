using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : Interactable
{
    public bool IsLocked;
    Material material;

    private void Awake()
    {
        material = GetComponent<Material>();
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

        material.color = Color.green;
    }
}
