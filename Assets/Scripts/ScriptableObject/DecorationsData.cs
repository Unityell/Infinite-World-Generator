using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecorationData")]
public class DecorationData : ScriptableObject
{
    [SerializeField, Range(0, 9)] protected int _LineLenght;
    public int LineLenght => _LineLenght;
    [SerializeField, Range(0, 9)] protected int _MaxCountInLine;
    public int MaxCountInLine => _MaxCountInLine;
    [SerializeField, Range(0, 9)] protected int _MinCountInLine;
    public int MinCountInLine => _MinCountInLine;
    [Header("Data")]
    [SerializeField] protected List<DecorationSettings> _Decorations;
    public List<DecorationSettings> Decorations =>      _Decorations; 

    void OnValidate()
    {
        if(_MinCountInLine > _MaxCountInLine)
        {
            _MinCountInLine = _MaxCountInLine;
        }
        if(_MaxCountInLine > _LineLenght)
        {
            _MaxCountInLine = _LineLenght;
        }
    }
}