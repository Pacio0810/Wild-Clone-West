using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] Transform playerTarget;
    [SerializeField] float cameraLerpSpeed;
    [SerializeField] Vector3 camera_offset;

    void Update()
    {
        this.transform.position = playerTarget.position + camera_offset;
    }
}
