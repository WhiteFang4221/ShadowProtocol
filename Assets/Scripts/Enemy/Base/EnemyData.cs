using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "DataAsset/Enemy")] 
public class EnemyData : ScriptableObject
{
    [SerializeField, Range(0, 15)] private float _speed = 15;
    
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
}
