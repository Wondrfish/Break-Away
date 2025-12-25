using UnityEngine;

public abstract class PowerUp : MonoBehaviour
{
    [Header("Power-Up Settings")]
    public float duration = 5f;

    [Header("Sound")]
    public AudioClip pickupSound;

    protected abstract void ApplyPowerUp(GameObject player);

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play sound
            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            ApplyPowerUp(other.gameObject);

            Destroy(gameObject);
        }
    }
}
