using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class MachineGunAI : AIComponent
{
    [SerializeField, ReadOnly] MachineGun Gun;
    [SerializeField] float RadarRadius = 1f;
    [SerializeField] float ScanTimer;
    HashSet<Enemy> Targets = new HashSet<Enemy>();
    Enemy CurrentTarget;

    [Header("Debug Draw")]
    [SerializeField] float boxWidth = 2f;
    [SerializeField] float boxLength = 4f;
    [SerializeField] float boxDeep = 4f;
    [SerializeField] Vector3 boxCenterOffset = Vector3.zero;

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
        Vector3 boxCenter = transform.position;
        boxCenter.y = 0.1f;
        boxCenter += boxCenterOffset;

        Vector3 halfSize = new Vector3(boxWidth * 0.5f, 0.1f, boxLength * 0.5f);
        Vector3 corner = boxCenter - halfSize;

        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(corner + halfSize + Vector3.up * boxDeep / 2, new Vector3(boxWidth, boxDeep, boxLength));
    } 

    Enemy GetClosestTarget(Vector3 Center)
    {
        if (Targets.Count == 0)
            return null;

        Enemy closestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (var target in Targets)
        {
            if (target == null)
                continue;

            float distanceSqr = (target.transform.position - Center).sqrMagnitude;
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
        Vector3 halfSize = new Vector3(boxWidth * 0.5f, 0.1f, boxLength * 0.5f);
        Vector3 scanBoxSize = new Vector3(boxWidth, boxDeep, boxLength) / 2;

        while (gameObject.activeSelf)
        {
            Vector3 boxCenter = transform.position;
            boxCenter.y = 0.1f;
            boxCenter += boxCenterOffset;

            Vector3 corner = boxCenter - halfSize;

            Vector3 centerUp = corner + halfSize + Vector3.up * boxDeep / 2;

            var CurrentTargets = Physics.OverlapBox(centerUp, scanBoxSize, Quaternion.identity, LayerMask.GetMask("Enemy"));

            CurrentTargets = CurrentTargets.Where(target => target.gameObject.activeSelf).ToArray();

            Targets.Clear();
            Targets.AddRange(CurrentTargets.Select(target => target.GetComponent<Enemy>()));

            var NonTargetUnits = new HashSet<Enemy>(Targets.Where(unit => !unit.IsTarget));

            if (Gun)
            {
                if (NonTargetUnits.Count > 0)
                {
                    Targets.Clear();
                    Targets.AddRange(NonTargetUnits);
                }

                var closestTargetPosition = CurrentTarget ? CurrentTarget.transform.position : transform.position;
                var closestTarget = GetClosestTarget(closestTargetPosition);
                SetTarget(closestTarget);
            }

            yield return new WaitForSeconds(ScanTimer);
        }
    }

    void SetTarget(Enemy ClosestEnemy)
    {
        if (ClosestEnemy)
        {
            ClosestEnemy.IsTarget = true;
            Gun.ChangeState(EnumWeaponState.OnTarget);
            Gun.TargetPosition = ClosestEnemy.transform;
        }
        else
        {
            Gun.ChangeState(EnumWeaponState.NoTarget);
            Gun.TargetPosition = null;
        }
    }
}