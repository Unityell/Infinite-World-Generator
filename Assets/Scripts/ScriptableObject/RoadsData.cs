using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoadsData")]
public class RoadsData : ScriptableObject
{
    [SerializeField] 
    protected List<Road>        _roads;

    public List<Road> Roads =>  _roads; 
}