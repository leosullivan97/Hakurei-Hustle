using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private AudioClip springSound;
    public float bounceForce = 10f; // Force applied to bounce the player

    // Triggered when another collider enters the trap's collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the "Player" tag
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerBounce(collision.gameObject);
        }
    }

    // Applies a bounce effect to the player if they enter the trap
    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>(); // Access the player's Rigidbody2D component

        // Only proceed if the player has a Rigidbody2D
        if (rb)
        {
            SoundManager.instance.PlaySound(springSound);

            rb.velocity = new Vector2(rb.velocity.x, 0f); // Reset player's vertical velocity

            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse); // Apply bounce force upwards
        }
    }
}
