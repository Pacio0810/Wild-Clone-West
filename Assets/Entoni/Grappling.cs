using UnityEngine;

public class Grappling : MonoBehaviour
{
    public Vector3 lastGrappled;
    public LayerMask grappable;
    public LineRenderer cable;
    public float maxDistanceGrapple;
    public bool IsGrappling;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        cable = GetComponent<LineRenderer>();
        cable.enabled = false;
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ShootRay();
        }
    }
    private void LateUpdate()
    {
        if (IsGrappling)
        {
            cable.SetPosition(0, transform.position);
        }
    }
    void ShootRay()
    {
        RaycastHit hit;
        Vector3 camerafr = Camera.main.transform.forward;
        if (Physics.Raycast(transform.position, camerafr, out hit, maxDistanceGrapple, grappable))
        {
            lastGrappled = hit.point;
            StartGrappling();
            Debug.DrawRay(transform.position, camerafr * maxDistanceGrapple, Color.red, 4f);
        }
        else
        {
            IsGrappling = false;
        }

    }
    void StartGrappling()
    {
        cable.enabled = true;
        cable.SetPosition(1, lastGrappled);
        IsGrappling = true;
        JumpToPos(lastGrappled, 30f);
    }

    public Vector3 GrapplingJumpVelocity(Vector3 startPoint, Vector3 end, float trajectory)
    {
        float gravity = Physics.gravity.y;
        float displacementY = end.y - startPoint.y;
        Vector3 dispXY = new Vector3(end.x - startPoint.x, 0f, end.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectory);

        Vector3 velocityXZ = dispXY / (Mathf.Sqrt(-2 * trajectory / gravity) + Mathf.Sqrt(2 * (displacementY - trajectory) / gravity));

        return velocityXZ - velocityY;
    }
    void JumpToPos(Vector3 target, float trajetory)
    {
        //Move with RigidBody not .Move
        //player.characterController.Move(GrapplingJumpVelocity(transform.position, target, trajetory));
    }
}
