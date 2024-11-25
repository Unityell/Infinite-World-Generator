using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : Unit
{
    public override void Initialization(HashSet<object> Systems)
    {
        this.Systems.Add(GetComponent<CharacterController>());
        this.Systems.UnionWith(Systems);
        Components = GetComponents<Components>().ToHashSet();
        InitComponents();        
        ExtractSystems(Systems);
    }
}