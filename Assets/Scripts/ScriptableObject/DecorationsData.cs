using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DecorationData")]
public class DecorationData : ScriptableObject
{
    [SerializeField] 
    protected List<Decoration>                  _decorations;
    public List<Decoration> Decorations =>      _decorations; 
}