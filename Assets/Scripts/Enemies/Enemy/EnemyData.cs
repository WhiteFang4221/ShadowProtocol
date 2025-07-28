using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "DataAsset/Enemy")] 
public class EnemyData : ScriptableObject
{
    public readonly float MinDistanceToTarget = 0.1f;
    
    [SerializeField, Range(0, 5)] private float _speed = 3.5f;
    [SerializeField, Range(1, 10)] private float _followSpeed = 7f;
    [SerializeField, Range(1, 360)] private int _rotationSpeed = 5;
    private float _timeToWait;
    
    public float Speed
    {
        get { return _speed; }

        set
        {
            if (value >= 0 && value <= 5)
                _speed = value;
            else
                throw new ArgumentOutOfRangeException(nameof(_speed));
        }
    }
    
    public float FollowSpeed
    {
        get { return _followSpeed; }

        set
        {
            if (value >= 5 && value <= 10)
                _followSpeed = value;
            else
                throw new ArgumentOutOfRangeException(nameof(_followSpeed));
        }
    }
    
    public int RotationSpeed
    {
        get { return _rotationSpeed; }

        set
        {
            if (value >= 1 && value <= 90)
                _rotationSpeed = value;
            else
                throw new ArgumentOutOfRangeException(nameof(_rotationSpeed));
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
