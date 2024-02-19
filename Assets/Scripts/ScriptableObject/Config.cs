using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Config")]
public class Config : ScriptableObject
{
    [Header("GameSettings")]
    [SerializeField] 
    protected float                 _maxGameSpeed;
    public float MaxGameSpeed =>    _maxGameSpeed; 

    [SerializeField, Range(0, 100)] 
    protected float                 _acceleration;
    public float Acceleration =>    _acceleration;  

    [SerializeField, ReadOnly] 
    public float                    TargetGameSpeed;
    [SerializeField, ReadOnly] 
    public float                    CurrentGameSpeed;
    [SerializeField, ReadOnly] 
    public float                    CurrentDistance;

    private void OnValidate()
    {
        _maxGameSpeed = _maxGameSpeed < 0 ? 0 : _maxGameSpeed;
    }
}