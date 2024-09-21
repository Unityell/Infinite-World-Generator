using UnityEngine;

public class Enemy : Unit
{
    [HideInInspector] public float ScaleMin;
    [HideInInspector] public float ScaleMax;

    public void Initialization(float ScaleMin, float ScaleMax)
    {
        this.ScaleMin = ScaleMin;
        this.ScaleMax = ScaleMax;
    }
    
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

    void FixedUpdate()
    {
        transform.rotation = Quaternion.LookRotation((transform.position - Vector3.right * 10).normalized);
    }
}