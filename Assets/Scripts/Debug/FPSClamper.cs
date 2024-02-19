using UnityEngine;

public class FPSClamper : MonoBehaviour
{
    public int fps;

    void Start()
    {
        Application.targetFrameRate = fps;
    }
}
