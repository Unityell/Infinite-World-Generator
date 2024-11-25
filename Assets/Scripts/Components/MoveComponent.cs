using UnityEngine;
using YG;

public class MoveComponent : MonoBehaviour
{
    [SerializeField] private float RoadWidth;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float MaxRotationAngle;
    [SerializeField] private float RotationSpeed;

    [SerializeField] Config Config;
    [SerializeField] AnimationCurve SpeedCurve;

    float Number;
    float LastInput;
    
    private float currentRotation;

    float WaitTimer = 3f;

    void FixedUpdate()
    {
        var Speed = Config.CurrentGameSpeed / Config.MaxGameSpeed;

        float input = 0; 

        if(WaitTimer > 0)
        {
            WaitTimer -= Time.fixedDeltaTime;
        }
        else
        {   
            if (YandexGame.EnvironmentData.isMobile)
            {
                if (Input.touchCount > 0)
                {
                    float touchPositionX = Input.touches[0].position.x;
                    
                    float screenWidth = Screen.width;

                    float centerX = screenWidth / 2;

                    input = touchPositionX > centerX ? 1 : -1;
                }
            }
            else
            {
                input = Input.GetAxisRaw("Horizontal");
            }   
        }

        if (RoadWidth - Mathf.Abs(transform.position.z) <= 0.15f)
        {
            if ((transform.position.z < 0 && input == 1) || (transform.position.z > 0 && input == -1))
            {
                LastInput = 0;
            }
        }

        LastInput = Mathf.Lerp(LastInput, input, Time.fixedDeltaTime * 2);
        Number = Mathf.Clamp(Number - LastInput * SpeedCurve.Evaluate(Speed) * MoveSpeed, -RoadWidth, RoadWidth);

        var Position = transform.position;
        Position = Vector3.ClampMagnitude(Vector3.forward * Number, RoadWidth);
        Position.x = -60;
        transform.position = Position;

        if (LastInput != 0)
        {
            float targetRotation = LastInput * MaxRotationAngle;
            currentRotation = Mathf.Lerp(currentRotation, targetRotation, Time.fixedDeltaTime * RotationSpeed * SpeedCurve.Evaluate(Speed));
        }
        else
        {
            currentRotation = Mathf.Lerp(currentRotation, 0, Time.fixedDeltaTime * RotationSpeed * SpeedCurve.Evaluate(Speed));
        }

        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }
}