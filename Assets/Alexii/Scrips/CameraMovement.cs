using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform playerTarget;
    [SerializeField] float cameraLerpSpeed;
    [SerializeField] Vector3 offset;
    
    void Update()
    {
        if (playerTarget != null)
        transform.position = Vector3.Lerp(transform.position , playerTarget.position + offset, cameraLerpSpeed * Time.deltaTime);
    }
}
