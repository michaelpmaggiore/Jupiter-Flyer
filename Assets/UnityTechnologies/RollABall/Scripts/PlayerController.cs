using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float speed;
	
	public Transform camera;

    public AudioSource audioSource;
    
    public AudioClip rocketSound;

    public MeshRenderer meshRenderer;

    public Material normalMaterial;
    public Material boostMaterial;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;


	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

        meshRenderer.material = normalMaterial;

        
        // Ensure the AudioSource is set up correctly.
        if (audioSource != null && rocketSound != null)
        {
            audioSource.clip = rocketSound;
            audioSource.loop = true;
            audioSource.volume = 50.0f;
        }
        else
        {
            Debug.LogWarning("AudioSource or RocketSound AudioClip is missing!");
        }
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            audioSource.Play();
            meshRenderer.material = boostMaterial;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            audioSource.Stop();
            meshRenderer.material = normalMaterial;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(Camera.main.transform.forward * speed, ForceMode.Acceleration);
        }
    }

}