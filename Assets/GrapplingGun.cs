using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {
        
    // }

    

    // private LineRenderer lr;
    // private Vector3 grapplePoint;
    // public LayerMask whatIsGrappleable;
    // public Transform gunTip, camera, player;
    // private SpringJoint joint;

    // void Awake()
    // {
    //     lr = GetComponent<LineRenderer>();
    // }


    // // // Update is called once per frame
    // void Update()
    // {
    //     DrawRope();
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         StartGrapple();
    //     }
    //     else if (Input.GetMouseButtonUp(0))
    //     {
    //         StopGrapple();
    //     }
    // }

    // void StartGrapple()
    // {
    //     RaycastHit hit;
    //     float maxDistance = 100f;
    //     // ray origin is 1 meter above the camera
    //     Vector3 rayOrigin = camera.position + camera.forward + camera.up;
    //     if (Physics.Raycast(camera.position, rayOrigin, out hit, maxDistance, whatIsGrappleable))
    //     {
    //         grapplePoint = hit.point;
    //         joint = player.gameObject.AddComponent<SpringJoint>();
    //         joint.autoConfigureConnectedAnchor = false;
    //         joint.connectedAnchor = grapplePoint;


    //         float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

    //         joint.maxDistance = distanceFromPoint * 0.8f;
    //         joint.minDistance = distanceFromPoint * 0.25f;

    //         joint.spring = 4.5f;
    //         joint.damper = 7f;
    //         joint.massScale = 4.5f;
    //         // lr.positionCount = 2;
    //         // lr.SetPosition(0, transform.position);
    //         // lr.SetPosition(1, grapplePoint);
    //     }
    // }

    // void DrawRope()
    // {
    //     lr.SetPosition(0, gunTip.position);
    //     lr.SetPosition(1, grapplePoint);
    // }

    // void StopGrapple()
    // {
        
    // }
}
