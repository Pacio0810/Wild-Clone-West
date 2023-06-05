using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChest : Interactable
{
    public bool IsLocked;
    MeshRenderer meshRenderer;
    string defaultMessage = "Open Chest [E]";

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        message = defaultMessage;
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
            StartCoroutine(Coroutine());
        }
    }

    public void Open()
    {
        // play animation

        meshRenderer.material.color = Color.green;
        enabled = false;
    }

    IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(1);
        message = defaultMessage;
    }

}
