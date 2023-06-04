using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("Ref")]
    private PlayerController player;
    public Transform cam;
    public Transform gunTip;

    private Vector3 lastGrapplePoint;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer;

    [Header("Grappling")]
    public float distance;
    public float grappleDelayTime;
    public float trajectory;
    public LayerMask grapplableMask;
    public LineRenderer cable;
    private bool grappling;

    [Header("Input")]
    KeyCode grappleKey = KeyCode.E;

    private void Start()
    {
        player = GetComponent<PlayerController>();
        cable.enabled = false;

    }

    private void Update()
    {
        if (Input.GetKeyDown(grappleKey))
        {
            StartGrapple();
        }

        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if (grappling)
        {
            cable.SetPosition(0, gunTip.position);
        }
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0)
        {
            return;
        }
        grappling = true;
        RaycastHit hit;
        cable.enabled = true;
        if (Physics.Raycast(gunTip.position, cam.forward, out hit, distance, grapplableMask))
        {
            lastGrapplePoint = hit.point;
            Debug.DrawRay(gunTip.position, cam.forward * distance, Color.red);

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            lastGrapplePoint = gunTip.position + cam.forward * distance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
            player.isGrappling = false;
        }
        cable.SetPosition(1, lastGrapplePoint);
    }

    private void ExecuteGrapple()
    {
        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = lastGrapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + trajectory;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = trajectory;

        //player.DirectJumpToPosition(lastGrapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        grappling = false;

        grapplingCdTimer = grapplingCd;
        cable.enabled = false;
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return lastGrapplePoint;
    }
}
