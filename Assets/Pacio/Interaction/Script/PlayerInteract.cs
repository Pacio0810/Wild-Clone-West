using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    // distanza da settare
    [SerializeField] float distance = 3.5f;
    [SerializeField] LayerMask mask;

    UIPlayer playerUI;

    private void Awake()
    {
        playerUI = GetComponent<UIPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.UpdateText(string.Empty);

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        RaycastHit hitInfo;

        // evitare di fare questo check ogni frame?

        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                // text Update
                playerUI.UpdateText(hitInfo.collider.GetComponent<Interactable>().message);

                if (Input.GetKey(KeyCode.E))
                {
                    hitInfo.collider.GetComponent<Interactable>().BaseInteract();
                }
            }
        }
    }
}
