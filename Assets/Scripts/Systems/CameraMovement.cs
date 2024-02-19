using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] 
    private float   _springArmMin;
    [SerializeField] 
    private float   _springArmMax;
    [SerializeField] 
    private float   _scrollSpeed;
    private Vector3 _savePositionMax;
    private Vector3 _savePositionMin;

    private void Start()
    {
        _savePositionMax = transform.position + transform.forward * _springArmMax;
        _savePositionMin = transform.position + transform.forward * _springArmMin;
    }

    private void Update()
    {
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");

        if (scroll != 0)
        {
            Vector3 vector = scroll < 0 ? _savePositionMin : _savePositionMax;
            transform.position = Vector3.MoveTowards(transform.position, vector, Time.deltaTime * _scrollSpeed);
        }
    }
}