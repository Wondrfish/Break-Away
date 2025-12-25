using UnityEngine;

public class TeleportPowerUp : PowerUp
{
    [Header("Teleport Settings")]
    public float teleportRange = 30f;
    public float minTeleportDistance = 10f;
    public LayerMask obstacleMask;
    public int maxAttempts = 20;

    protected override void ApplyPowerUp(GameObject player)
    {
        Vector3 originalPos = player.transform.position;
        Vector3 newPos = originalPos;

        bool foundSafeSpot = false;

        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomDir2D = Random.insideUnitCircle.normalized *
                                  Random.Range(minTeleportDistance, teleportRange);

            Vector3 candidatePos = originalPos + new Vector3(randomDir2D.x, 0, randomDir2D.y);

            if (!Physics.CheckSphere(candidatePos, 2f, obstacleMask))
            {
                newPos = candidatePos;
                foundSafeSpot = true;
                break;
            }
        }

        if (foundSafeSpot)
            player.transform.position = newPos;
        else
            Debug.Log("No safe teleport found.");
    }
}
