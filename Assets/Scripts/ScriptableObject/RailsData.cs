using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RailsData")]
public class RailsData : ScriptableObject
{
    [SerializeField] 
    protected List<Rail>        _rails;

    public List<Rail> Rails =>  _rails; 
}