using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI interactText;
    
    public void UpdateText(string message)
    {
        if (interactText != null)
        {
            interactText.text = message;
        }
    }
}
