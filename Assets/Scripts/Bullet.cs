using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float DeactiveDistance;
    [SerializeField] float BulletSpeed;
    Vector3 StartPos;
    Transform TargetPos;


    public void Setup(Transform TargetPos)
    {
        this.TargetPos = TargetPos;
        StartPos = transform.position;
    }

    void FixedUpdate()
    {
        if(TargetPos)
        {
            if(!TargetPos.gameObject.activeSelf) 
            {
               TargetPos = null; 
               return;
            }
            
            transform.position = Vector3.MoveTowards(transform.position, TargetPos.position, BulletSpeed * Time.deltaTime);

            if(transform.position == TargetPos.position)
            {
                TargetPos.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }            
        }
        else
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
}