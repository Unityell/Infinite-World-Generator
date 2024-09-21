using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WeaponButton : MonoBehaviour
{
    [Inject] EventBus EventBus;
    Transform Position;
    Camera Camera;
    Pivot Pivot;
    [SerializeField] Image Image;
    Color Pick;
    Color UnPick;

    void Start()
    {
        EventBus.Event += SignalBox;
    }

    void OnDestroy()
    {
        EventBus.Event -= SignalBox;
    }

    void SignalBox(object Obj)
    {
        if(Obj.GetType() == typeof(PivotSignal))
        {
            PivotSignal Signal = Obj as PivotSignal;
            if(Signal.Pivot != Pivot)
            {
                ChangeColor(UnPick);
            }
        }
    }

    public void Setup(Transform Position, Camera Camera, Pivot Pivot, Color Pick, Color UnPick)
    {
        this.Position = Position;
        this.Camera = Camera;
        this.Pivot = Pivot;
        this.Pick = Pick;
        this.UnPick = UnPick;
        transform.position = Camera.WorldToScreenPoint(Position.position);
    }

    public void OnClick()
    {
        ChangeColor(Pick);
        PivotSignal Signal = new PivotSignal(Pivot);
        EventBus.Invoke(Signal);
    }

    public void ChangeColor(Color Color)
    {
       Image.color = Color; 
    }

    void Update()
    {
        if (Position != null)
        {
            transform.position = Camera.WorldToScreenPoint(Position.position);
        }
        
        if(Pivot.State == EnumWeaponPivotState.Active)
        {
            gameObject.SetActive(false);
        }
    }
}