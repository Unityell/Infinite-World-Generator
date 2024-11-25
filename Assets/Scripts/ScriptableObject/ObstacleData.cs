using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData")]
public class ObstacleData : ScriptableObject
{
    [SerializeField] protected List<Vector3> _Points;
    public List<Vector3> Points => _Points; 
    
    [SerializeField] protected List<Obstacle> _Obstacles;
    public List<Obstacle> Obstacles => _Obstacles; 
}