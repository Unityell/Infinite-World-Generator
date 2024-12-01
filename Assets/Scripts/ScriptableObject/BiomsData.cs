using UnityEngine;

[CreateAssetMenu(fileName = "BiomsData")]
public class BiomsData : ScriptableObject
{
    [Header("Biom Settings")]
    [SerializeField] protected string       _name;
    public string Name =>                   _name;
    [SerializeField] protected float        _chance;
    public float Chance =>                  _chance;
    [SerializeField, Range(0, 9)] protected int _LineLenght;
    public int LineLenght => _LineLenght;
    [Header("Biom Lenght")]
    [SerializeField] protected float        _minBiomLenght;
    public float MinBiomLenght =>           _minBiomLenght;
    [SerializeField] protected float        _maxBiomLenght;
    public float MaxBiomLenght =>           _maxBiomLenght;
    [SerializeField] protected RoadsData    _roadsData;
    public RoadsData RoadsData =>           _roadsData;
    [SerializeField] protected DecorationData _decorationData;
    public DecorationData DecorationData => _decorationData;
    [SerializeField] protected ObstacleData _obstacleData;
    public ObstacleData ObstacleData =>     _obstacleData;
}