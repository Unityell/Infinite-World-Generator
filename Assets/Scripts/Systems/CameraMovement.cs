using UnityEngine;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    [Inject] EventBus EventBus;
    [SerializeField, ReadOnly] EnumCameraMode _state;
    [SerializeField] private Transform _gameModePosition;
    [SerializeField] private Transform _updatePosition;
    [SerializeField] private float LerpSpeed;

    private void Start()
    {
        EventBus.Event += SignalBox;
    }

    void SignalBox(object Obj)
    {
        if(Obj.GetType() == typeof(CameraModeSignal))
        {
            CameraModeSignal Signal = Obj as CameraModeSignal;
            ChangeCameraMode(Signal.State);
        }
    }

    void ChangeCameraMode(EnumCameraMode NewState)
    {
        _state = NewState;
    }

    private void Update()
    {
        Transform Position = _state == EnumCameraMode.Game ? _gameModePosition : _updatePosition;

        transform.position = Vector3.Lerp(transform.position, Position.position, Time.deltaTime * LerpSpeed);

        transform.rotation = Quaternion.Slerp(transform.rotation, Position.rotation, Time.deltaTime * LerpSpeed);
    }

    void OnDestroy()
    {
        EventBus.Event -= SignalBox;
    }
}