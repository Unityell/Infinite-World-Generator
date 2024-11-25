using System.Collections;
using UnityEngine;

public class ObstacleMoveComponent : MonoBehaviour
{
    [SerializeField] float Speed;

    bool Travel;
    float TargetSide;
    float targetRotation;
    float currentRotation;

    void OnEnable()
    {
        Travel = false;
        currentRotation = 0;

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForFixedUpdate();

        Travel = Random.Range(0, 3) == 0;

        if (Travel)
        {
            TargetSide = -transform.position.z;
            targetRotation = TargetSide > 0 ? 15 : -15;
        }
    }

    void FixedUpdate()
    {
        transform.position += Vector3.left * Speed * Time.fixedDeltaTime;

        if (Travel)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, TargetSide), Time.fixedDeltaTime * 10f);

            currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.fixedDeltaTime * 2f);
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);

            if(TargetSide == transform.position.z)
            {
                Travel = false;
            }
        }
        else
        {
            currentRotation = Mathf.Lerp(currentRotation, 0, Time.fixedDeltaTime * 5f);
            transform.rotation = Quaternion.Euler(0, currentRotation, 0);
        }
    }
}