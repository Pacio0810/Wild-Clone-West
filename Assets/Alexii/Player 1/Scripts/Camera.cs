using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] Transform playerTarget;
    [SerializeField] float cameraLerpSpeed;
    [SerializeField] Vector3 camera_offset;


    void Update()
    {
        this.transform.position = playerTarget.position + camera_offset;
    }
}
