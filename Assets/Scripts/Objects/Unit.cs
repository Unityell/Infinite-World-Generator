using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField, ReadOnly] protected string Name;

    public string GetName()
    {
        return Name;
    }

    void OnValidate()
    {
        Name = gameObject.name;
    }
}