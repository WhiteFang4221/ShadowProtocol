using UnityEngine;

[CreateAssetMenu(fileName = "HealthConfig", menuName = "ConfigAsset/Health")] 
public class HealthConfig: ScriptableObject
{
    [SerializeField] private int _maxHealth;
    
    public int MaxHealth => _maxHealth;

    private void OnValidate()
    {
        _maxHealth = Mathf.Max(1, _maxHealth); 
    }
}