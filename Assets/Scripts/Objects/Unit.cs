using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private string _name;

    public string GetName()
    {
        return _name;
    }
}