using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class StunArea : MonoBehaviour
{
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _stunableMask;
    private SphereCollider _collider;
    private float _radius; 
    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
        _collider.isTrigger = true; 
        _radius = _collider.radius * transform.lossyScale.x;
    }

    public void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _radius, _stunableMask);
        
        foreach (Collider collider in hitColliders)
        {
            IStunable stunable = collider.GetComponent<IStunable>();

            if (stunable != null)
            {
                if (IsInStunField(collider))
                {
                    stunable.Stun();
                }
            }
        }
    }

    private bool IsInStunField(Collider targetCollider)
    {
        Vector3 dirToTarget = (targetCollider.transform.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, targetCollider.transform.position);
        
        
        Debug.DrawRay(transform.position, dirToTarget * distanceToTarget, Color.red, 5f);

        return !Physics.Raycast(transform.position, dirToTarget, distanceToTarget, _obstacleMask);
;
    }
}