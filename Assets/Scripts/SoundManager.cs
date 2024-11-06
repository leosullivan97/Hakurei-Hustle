using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Singleton instance of SoundManager
    public static SoundManager instance { get; private set; }

    // AudioSource component for playing sounds
    private AudioSource source;

    private void Awake()
    {
        // Get the AudioSource component attached to this GameObject
        source = GetComponent<AudioSource>();

        // Check if an instance already exists
        if (instance == null)
        {
            // Set the current instance and mark it as not destroyed on load
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy duplicate instances
            Destroy(gameObject);
        }
    }

    // Method to play a sound effect
    public void PlaySound(AudioClip _sound)
    {
        // Play the specified sound using PlayOneShot
        source.PlayOneShot(_sound);
    }
}
