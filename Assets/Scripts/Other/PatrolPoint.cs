using UnityEngine;
using UnityEngine.Serialization;

public class PatrolPoint : MonoBehaviour
{
    [field: SerializeField] public float WaitTime = 0f;
    public Transform Transform {get; private set; }

    private void Awake()
    {
        Transform = transform;
    }
}
