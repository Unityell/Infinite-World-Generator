using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField, ReadOnly] protected EnumWeaponState State;
    [ReadOnly] public Transform TargetPosition;
    [SerializeField] protected Transform Turret;
    [SerializeField] protected Transform Gun;
    [SerializeField] protected float RotateSpeed;
    [SerializeField] protected float GunRotateSpeed;
    [SerializeField] protected float AttackSpeed;
    [SerializeField] protected Bullet BulletPrefab;
    [SerializeField] protected Transform[] ShootPosition;
    [SerializeField, ReadOnly] protected List<Bullet> BulletPool;

    void Start()
    {
        StartCoroutine(Shoot());
    }

    public virtual void ChangeState(EnumWeaponState NewState)
    {
        State = NewState;
    }

    protected virtual IEnumerator Shoot()
    {
        while (gameObject.activeSelf)
        {
            if(State == EnumWeaponState.OnTarget && IsCannonAimingAtTarget() && TargetPosition)
            {
                for (int i = 0; i < ShootPosition.Length; i++)
                {
                    var Bullet = GetBullet();
                    Bullet.transform.position = ShootPosition[i].transform.position;
                    Bullet.transform.rotation = Quaternion.LookRotation(ShootPosition[i].transform.forward);
                    Bullet.Setup(TargetPosition);
                }                
            }

            yield return new WaitForSeconds(AttackSpeed);
        }
    }

    protected virtual Bullet GetBullet()
    {
        var Bullet = BulletPool.Find(x => !x.gameObject.activeSelf);

        if(Bullet)
        {
            Bullet.gameObject.SetActive(true);
            return Bullet;
        }
        else
        {
            Bullet = Instantiate(BulletPrefab);
            BulletPool.Add(Bullet);
            return Bullet;
        }
    }

    protected virtual void Rotate(Vector3 targetPosition)
    {
        RotateTurret(targetPosition);
        RotateGun(targetPosition);
    }

    void RotateTurret(Vector3 targetPosition)
    {
        Vector3 targetDirectionXZ = targetPosition - Turret.position;
        targetDirectionXZ.y = 0f;

        Quaternion targetRotationXZ_Turret = Quaternion.LookRotation(targetDirectionXZ);

        Quaternion targetRotationY_Turret = Quaternion.Euler(0f, targetRotationXZ_Turret.eulerAngles.y, 0f);

        Turret.rotation = SmoothRotate(Turret.rotation, targetRotationY_Turret, RotateSpeed);
    }

    void RotateGun(Vector3 targetPosition)
    {
        Vector3 targetDirection = targetPosition - Gun.position;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        Quaternion targetRotationX = Quaternion.Euler(targetRotation.eulerAngles.x, Gun.rotation.eulerAngles.y, Gun.rotation.eulerAngles.z);

        Gun.rotation = Quaternion.Lerp(Gun.rotation, targetRotationX, Time.deltaTime * GunRotateSpeed);
    }

    Quaternion SmoothRotate(Quaternion fromRotation, Quaternion toRotation, float speed)
    {
        return Quaternion.RotateTowards(fromRotation, toRotation, speed * Time.deltaTime);
    }

    public virtual bool IsCannonAimingAtTarget()
    {
        if (TargetPosition == null)
        {
            return false;
        }

        Vector3 directionToTarget = (TargetPosition.position - Gun.position).normalized;

        Vector3 cannonDirection = Gun.transform.forward;

        float dotProduct = Vector3.Dot(directionToTarget, cannonDirection);

        return dotProduct >= 0.9f;
    }
}