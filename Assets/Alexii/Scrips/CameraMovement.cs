using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform playerTarget;
    [SerializeField] float cameraLerpSpeed;
    [SerializeField] Vector3 camera_offset;

    void Update()
    {
        if (playerTarget != null)
        transform.position = Vector3.Lerp(transform.position, playerTarget.position + camera_offset, cameraLerpSpeed * Time.deltaTime);
    }
}
