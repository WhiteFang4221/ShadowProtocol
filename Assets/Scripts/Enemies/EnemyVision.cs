using System;
using System.Collections;
using Reflex.Attributes;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [SerializeField] private FieldOfView _fov;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private float _memoryTime;
    [Inject] private IPlayerPosition _playerPosition;
    private WaitForSeconds _viewDelay => new (_enemyData.ViewDelay);
    
    public event Action PlayerSpotted;
    
    public EnemyData Data => _enemyData;

    private void FollowPlayer()
    {
        PlayerSpotted?.Invoke();
    }
}
