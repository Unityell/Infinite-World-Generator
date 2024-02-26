using UnityEngine;
using Zenject;

public class CameraMovement : MonoBehaviour
{
    [Inject] EventBus EventBus;
    [SerializeField, ReadOnly] EnumGameMode State;
    [SerializeField] private Transform GameModePosition;
    [SerializeField] private Transform UpdatePosition;
    [SerializeField] private float LerpSpeed;

    private void Start()
    {
        EventBus.Event += SignalBox;
    }

    void SignalBox(object Obj)
    {
        if(Obj.GetType() == typeof(GameModeSignal))
        {
            GameModeSignal Signal = Obj as GameModeSignal;
            ChangeCameraMode(Signal.State);
        }
    }

    void ChangeCameraMode(EnumGameMode NewState)
    {
        State = NewState;
    }

    private void Update()
    {
        Transform Position = State == EnumGameMode.Game ? GameModePosition : UpdatePosition;

        transform.position = Vector3.Lerp(transform.position, Position.position, Time.deltaTime * LerpSpeed);

        transform.rotation = Quaternion.Slerp(transform.rotation, Position.rotation, Time.deltaTime * LerpSpeed);
    }

    void OnDestroy()
    {
        EventBus.Event -= SignalBox;
    }
}