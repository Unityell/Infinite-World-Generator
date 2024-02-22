using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MachineGunAI : AIComponent
{
    [SerializeField, ReadOnly] MachineGun Gun;
    [SerializeField] float RadarRadius = 1f;
    [SerializeField] float ScanTimer;
    HashSet<Transform> Targets = new HashSet<Transform>();

    void OnValidate()
    {
        if(!Gun)
        {
            Gun = GetComponent<MachineGun>();

            if(!Gun) print($"{gameObject.name}: Where is the Gun?");
        }
    }

    void Start()
    {
        StartCoroutine(Radar());
    }

    void OnDrawGizmosSelected()
    {
        float angleStep = 360f / 36;

        Vector3 prevPoint = Vector3.zero;
        Vector3 firstPoint = Vector3.zero;

        for (int i = 0; i <= 36; i++)
        {
            float angle = angleStep * i;
            float x = transform.position.x + Mathf.Sin(Mathf.Deg2Rad * angle) * RadarRadius;
            float y = transform.position.y;
            float z = transform.position.z + Mathf.Cos(Mathf.Deg2Rad * angle) * RadarRadius;

            Vector3 newPoint = new Vector3(x, y, z);

            if (i > 0)
            {
                Debug.DrawLine(prevPoint, newPoint, Color.green);
            }
            else
            {
                firstPoint = newPoint;
            }

            prevPoint = newPoint;
        }

        Debug.DrawLine(prevPoint, firstPoint, Color.green);
    }

    Transform GetClosestTarget()
    {
        if (Targets.Count == 0)
            return null;

        Transform closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (var target in Targets)
        {
            if (target == null)
                continue;

            float distanceSqr = (target.position - transform.position).sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestTarget = target;
            }
        }
        return closestTarget;
    }

    IEnumerator Radar()
    {
        while (gameObject.activeSelf)
        {
            var CurrentTargets = Physics.OverlapSphere(transform.position, RadarRadius, LayerMask.GetMask("Enemy")).ToList();

            for (int i = 0; i < CurrentTargets.Count; i++)
            {
                if (!CurrentTargets[i].gameObject.activeSelf)
                {
                    CurrentTargets.Remove(CurrentTargets[i]);
                }
            }

            Targets.Clear();

            for (int i = 0; i < CurrentTargets.Count; i++)
            {
                Targets.Add(CurrentTargets[i].transform);
            }

            if (Gun)
            {
                var ClosestEnemy = GetClosestTarget();

                if (ClosestEnemy)
                {
                    Gun.ChangeState(EnumWeaponState.OnTarget);
                    Gun.TargetPosition = ClosestEnemy;
                }
                else
                {
                    Gun.ChangeState(EnumWeaponState.NoTarget);
                    Gun.TargetPosition = null;
                }
            }

            yield return new WaitForSeconds(ScanTimer);
        }
    }
}