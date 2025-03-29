using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Player")] 
public class PlayerData : ScriptableObject
{
    [SerializeField, Range(0, 15)] private float _speed = 15;
    [SerializeField, Range(0.1f, 1)] private float _rotationPerFrame = 0.2f;
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

    public float RotationPerFrame
    {
        get { return _rotationPerFrame; }
        set
        {
            if (value >= 0.1f && value <= 1)
                _rotationPerFrame = value;
            else
                throw new ArgumentOutOfRangeException(nameof(_rotationPerFrame));
        }
    }
}
