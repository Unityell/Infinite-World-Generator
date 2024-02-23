using UnityEngine;

public class Enemy : Unit
{
    [SerializeField, Range(0, 100)] 
    public float SpawnChance;
    public float SpawnDistanceMin;
    public float SpawnDistanceMax;
    [ReadOnly] public bool IsTarget;
    Animator Anim;

    void Start()
    {
        Anim = GetComponentInChildren<Animator>();
    }

    void OnDisable()
    {
        IsTarget = false;
    }

    public void Death()
    {
        Anim.SetInteger("State", 4);
    }
}