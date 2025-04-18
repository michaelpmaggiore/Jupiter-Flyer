﻿using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;

public class PlayerController : MonoBehaviour {
	
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text countText;
	//public Text winText;
	public Transform camera;

	[Tooltip("AudioSource component to play the rocket sound.")]
    public AudioSource audioSource;
    
    [Tooltip("Rocket sound effect AudioClip.")]
    public AudioClip rocketSound;

	//public GameObject rocketThrusterImage;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private int count;

	private bool isGrounded = false;
	public float jumpForce = 50000f; // Adjust for higher/lower jumps

	private Color thrustColor = Color.red;
	private Renderer renderer;

	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		// Set the count to zero 
		count = 0;

		// Run the SetCountText function to update the UI (see below)
		SetCountText ();

		renderer = GetComponent<Renderer>();

		// Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
		//winText.text = "";

		// if (audioSource == null)
        // {
        //     audioSource = GetComponent<AudioSource>();
        // }
        
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

	// Each physics step..
	private void FixedUpdate()
    {

		if (Input.GetKeyDown(KeyCode.W))
		{
    		// Start playing the rocket sound on loop when W is first pressed.
        	audioSource.loop = true;  // Ensure looping is enabled.
        	audioSource.Play();
    		
			//rocketThrusterImage.SetActive(true);
			// Change the color of the object to indicate thrust
			renderer.material.color = thrustColor;
        
		}

		if (Input.GetKey(KeyCode.W))
		{
    		// Apply force to the ball.
    		rb.AddForce(Camera.main.transform.forward * speed, ForceMode.Acceleration);
		}		

		if (Input.GetKeyUp(KeyCode.W))
		{
    		// Stop playing the rocket sound when W is released.
    		audioSource.Stop();
			//rocketThrusterImage.SetActive(false);
			// Reset the color of the object to its original color
			renderer.material.color = Color.white;
    		
		}

		//Jump if space is pressed and the ball is on the ground
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
			rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + jumpForce, rb.linearVelocity.z);
            isGrounded = false; // Prevent double jumping
        }
    }

	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			// Make the other game object (the pick up) inactive, to make it disappear
			other.gameObject.SetActive (false);

			// Add one to the score variable 'count'
			count = count + 1;

			// Run the 'SetCountText()' function (see below)
			SetCountText ();
		}
	}

	// Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
	void SetCountText()
	{
		// Update the text field of our 'countText' variable
		// countText.text = "Count: " + count.ToString ();

		// // Check if our 'count' is equal to or exceeded 12
		// if (count >= 12) 
		// {
		// 	// Set the text value of our 'winText'
		// 	winText.text = "You Win!";
		// }
	}

	// Detect ground contact
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // Make sure ground objects have the "Ground" tag
        {
            isGrounded = true;
        }
    }

    // Detect leaving the ground
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}