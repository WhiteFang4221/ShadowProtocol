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
