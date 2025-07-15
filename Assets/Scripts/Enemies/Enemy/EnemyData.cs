using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "DataAsset/Enemy")] 
public class EnemyData : ScriptableObject
{
    public readonly float MinDistanceToTarget = 0.1f;
    
    [SerializeField, Range(0, 15)] private float _speed = 15;
    private float _timeToWait;
    
    public float Speed
    {
        get { return _speed; }

        set
        {
            if (value >= 0 && value <= 15)
                _speed = value;
            else
                throw new ArgumentOutOfRangeException(nameof(_speed));
        }
    }
    
    public float TimeToWait
    {
        get { return _timeToWait; }

        set
        {
            if (value >= 0)
                _timeToWait = value;
            else
                throw new ArgumentOutOfRangeException(nameof(_timeToWait));
        }
    }
}
