using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Config")]
public class Config : ScriptableObject
{
    [Header("GameSettings")]
    [Range(0, 7500), SerializeField] protected float    _maxGameSpeed;
    public float MaxGameSpeed =>        _maxGameSpeed; 
    [Range(0, 7500), SerializeField] protected float    _startGameSpeed;
    public float StartGameSpeed =>      _startGameSpeed; 

    [SerializeField, Range(0.1f, 1000)] 
    protected float                     _acceleration;
    public float Acceleration =>        _acceleration;  

    [SerializeField, ReadOnly] 
    public float                        CurrentGameSpeed;
    [SerializeField, ReadOnly] 
    public float                        CurrentDistance;
    [SerializeField, ReadOnly] 
    public int                          Score;

    private void OnValidate()
    {
        _maxGameSpeed = _maxGameSpeed < 0 ? 0 : _maxGameSpeed;
    }
}