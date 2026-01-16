using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private EnemyConfig enemyConfig;
    [Inject] private IPlayerPosition _playerPosition;

    public float ViewRadius => enemyConfig.ViewRadius;
    public float HalfViewAngle => enemyConfig.ViewAngle / 2f;
    
    public bool IsPlayerInField()
    {
        Vector3 dirToPlayer = (_playerPosition.Transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _playerPosition.Transform.position);

        bool isInRadius = distanceToPlayer <= ViewRadius;
        bool isInAngle = Vector3.Angle(transform.forward, dirToPlayer) <= HalfViewAngle;
        bool hasLineOfSight = !Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, enemyConfig.ObstacleMask);

        return isInRadius && isInAngle && hasLineOfSight;
    }

    /*public bool IsPlayerInNearestZone()
    {
        Vector3 dirToPlayer = (_playerPosition.Transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _playerPosition.Transform.position);
        bool isInRadius = distanceToPlayer <= NearbyRadius;
        bool hasLineOfSight = !Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, _enemyData.ObstacleMask);

        return isInRadius && hasLineOfSight;
    }*/
}