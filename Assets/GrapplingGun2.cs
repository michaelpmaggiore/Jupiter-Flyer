using UnityEngine;

public class GrapplingGun2 : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform camera, player;

    [Tooltip("AudioSource component to play the rocket sound.")]
    public AudioSource audioSource;
    
    [Tooltip("Rocket sound effect AudioClip.")]
    public AudioClip rocketSound;


    private SpringJoint joint;

    // At the start of the game..
	void Start ()
	{

		if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        // Ensure the AudioSource is set up correctly.
        if (audioSource != null && rocketSound != null)
        {
            audioSource.clip = rocketSound;
            audioSource.loop = false;
        }
        else
        {
            Debug.LogWarning("AudioSource or RocketSound AudioClip is missing!");
        }
	}

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }


    // // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
            audioSource.loop = false;  // Ensure looping is enabled.
            audioSource.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    void StartGrapple()
    {
        RaycastHit hit;
        float maxDistance = 1000f;
        if (Physics.Raycast(player.position, camera.forward, out hit, maxDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;


            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            // Grapple parameters
            joint.spring = 10f; // Higher value means more pulling force
            joint.damper = 4f; // Higher value means less springy effect
            joint.massScale = 10f; // Higher value mean more player feels lighter, more force

            lr.positionCount = 2;
            lr.SetPosition(0, player.position);
            lr.SetPosition(1, grapplePoint);
        }
    }

    void DrawRope()
    {
        if (!joint) return;
        lr.SetPosition(0, player.position);
        lr.SetPosition(1, grapplePoint);
    }

    void StopGrapple()
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    void LateUpdate() {
        DrawRope();
    }
}
