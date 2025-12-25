using UnityEngine;

public class ShrinkPowerUp : PowerUp
{
    protected override void ApplyPowerUp(GameObject player)
    {
        player.transform.localScale *= 0.5f;
        player.GetComponent<MonoBehaviour>().StartCoroutine(Reset(player));
    }

    private System.Collections.IEnumerator Reset(GameObject player)
    {
        yield return new WaitForSeconds(duration);
        player.transform.localScale *= 2f;
    }
}
