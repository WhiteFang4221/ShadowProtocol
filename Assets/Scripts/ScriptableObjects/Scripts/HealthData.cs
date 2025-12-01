using UnityEngine;

[CreateAssetMenu(fileName = "HealthData", menuName = "DataAsset/Health")] 
public class HealthData: ScriptableObject
{
    [SerializeField] private int _maxHealth;
    
    public int MaxHealth => _maxHealth;

    private void OnValidate()
    {
        _maxHealth = Mathf.Max(1, _maxHealth); 
    }
}