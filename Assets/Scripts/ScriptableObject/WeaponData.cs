using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon")]
public class WeaponData : ScriptableObject
{
    [SerializeField] protected Sprite _Icon;
    public Sprite Icon => _Icon;
    [SerializeField] protected int _Price;
    public int Price => _Price;
    [SerializeField] protected Weapon _Prefab;
    public Weapon Prefab => _Prefab;
    [Header("Localize")]
    [SerializeField] protected List<SimpleLocalize> _Name;
    public List<SimpleLocalize> Name => _Name;
    [SerializeField] protected List<SimpleLocalize> _Hint;
    public List<SimpleLocalize> Hint => _Hint;
}