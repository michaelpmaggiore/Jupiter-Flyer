using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource.loop = true; // Set the AudioSource to loop
        audioSource.clip = backgroundMusic; // Assign the background music clip
        audioSource.volume = 0.5f; // Set the volume (optional)
        audioSource.Play(); // Start playing the background music
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
