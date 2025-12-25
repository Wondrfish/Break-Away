using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public int value = 10;

    [Header("Effects")]
    public AudioClip pickupSound;
    public ParticleSystem pickupEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Add score
            GameManager.Instance.AddScore(value);

            // Play sound (spawns a temporary AudioSource)
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Spawn particle effect
            if (pickupEffect != null)
            {
                ParticleSystem effect = Instantiate(pickupEffect, transform.position, Quaternion.identity);
                Destroy(effect.gameObject, effect.main.duration + 0.5f);
            }

            // Destroy the pickup coin
            Destroy(gameObject);
        }
    }
}
