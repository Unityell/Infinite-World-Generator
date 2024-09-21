using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField, Range(0, 9)] protected int _LineLenght;
    public int LineLenght => _LineLenght;
    [SerializeField, Range(0, 9)] protected int _MaxCountInLine;
    public int MaxCountInLine => _MaxCountInLine;
    [SerializeField, Range(0, 9)] protected int _MinCountInLine;
    public int MinCountInLine => _MinCountInLine;
    [SerializeField] protected List<EnemiesSettings>              _Enemies;
    public List<EnemiesSettings> Enemies =>                        _Enemies; 

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