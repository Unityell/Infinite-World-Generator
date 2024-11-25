using UnityEngine;

public class GameCamera : MonoBehaviour
{
    [SerializeField] Transform Player;
    float Timer = 2.5f;

    void FixedUpdate()
    {
        if(Timer > 0) {Timer -= Time.fixedDeltaTime; return;}
        transform.position = Vector3.Lerp(transform.position, new Vector3(-73.5f, 24, Player.position.z / 1.5f), Time.deltaTime * 5f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 7.5f), Time.fixedDeltaTime * 5f);
    }
}