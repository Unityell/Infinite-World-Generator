using UnityEngine;

[CreateAssetMenu(fileName = "BiomsData")]
public class BiomsData : ScriptableObject
{
    [Header("Biom Settings")]
    [SerializeField] 
    protected string                        _name;
    public string Name =>                   _name;
    [SerializeField] 
    protected float                         _chance;
    public float Chance =>                  _chance;
    [Header("Biom Lenght")]
    [SerializeField] 
    protected float                         _minBiomLenght;
    public float MinBiomLenght =>           _minBiomLenght;
    [SerializeField] 
    protected float                         _maxBiomLenght;
    public float MaxBiomLenght =>           _maxBiomLenght;
    [SerializeField] 
    protected RailsData                     _railsData;
    public RailsData RailsData =>           _railsData;
    [SerializeField] 
    protected DecorationData                _decorationData;
    public DecorationData DecorationData => _decorationData;
}