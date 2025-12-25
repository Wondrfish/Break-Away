using UnityEngine;

public class SlowTimePowerUp : PowerUp
{
    public float slowFactor = 0.3f;

    protected override void ApplyPowerUp(GameObject player)
    {
        GameManager.Instance.SlowTime(duration, slowFactor);
    }
}
