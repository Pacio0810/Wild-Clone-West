using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public string message;

    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {

    }
}
