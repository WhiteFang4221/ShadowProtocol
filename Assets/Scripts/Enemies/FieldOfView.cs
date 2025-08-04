using Reflex.Attributes;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private EnemyData _enemyData;
    [Inject] private IPlayerPosition _playerPosition;

    private Coroutine _findTargetCoroutine;
    private Coroutine _findNearbyTargetCoroutine;

    public float ViewRadius => _enemyData.ViewRadius;
    public float NearbyRadius => _enemyData.NearbyRadius;
    public float HalfViewAngle => _enemyData.ViewAngle / 2f;
    
    public bool IsPlayerInField()
    {
        Vector3 dirToPlayer = (_playerPosition.Transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _playerPosition.Transform.position);

        bool isInRadius = distanceToPlayer <= ViewRadius;
        bool isInAngle = Vector3.Angle(transform.forward, dirToPlayer) <= HalfViewAngle;
        bool hasLineOfSight = !Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, _enemyData.ObstacleMask);

        return isInRadius && isInAngle && hasLineOfSight;
    }

    public bool IsPlayerInNearestZone()
    {
        Vector3 dirToPlayer = (_playerPosition.Transform.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, _playerPosition.Transform.position);
        bool isInRadius = distanceToPlayer <= NearbyRadius;
        bool hasLineOfSight = !Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, _enemyData.ObstacleMask);

        return isInRadius && hasLineOfSight;
    }
}