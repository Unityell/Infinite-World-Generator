using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float DeactiveDistance;
    [SerializeField] float BulletSpeed;
    Vector3 StartPos;

    public void Setup()
    {
        StartPos = transform.position;
    }

    void FixedUpdate()
    {
        Vector3 newPosition = transform.position + transform.forward * BulletSpeed * Time.deltaTime;

        RaycastHit hit;

        if (Physics.Linecast(transform.position, newPosition, out hit, LayerMask.GetMask("Enemy")))
        {
            hit.collider.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = newPosition;

            if ((transform.position - StartPos).sqrMagnitude > DeactiveDistance * DeactiveDistance)
            {
                gameObject.SetActive(false);
            }
        }
    }
}