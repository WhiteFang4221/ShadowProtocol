using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    public PlayerHealth(HealthData data) : base(data) { }

    protected override void HandleDeath()
    {
        // Вызов GameOver
        // GameManager.Instance.GameOver();
        // Debug.Log("Игрок умер!");
    }
}
