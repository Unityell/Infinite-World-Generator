using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour 
{
    public float updateInterval = 0.5f; 

    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;
    int w,h;

    GUIStyle textStyle = new GUIStyle();

    void Start()
    {
        timeleft = updateInterval;
        w = Screen.width;
        h = Screen.height;
        textStyle.alignment = TextAnchor.UpperRight;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.fontSize = h * 2 / 90;
        textStyle.normal.textColor = Color.white;
    }

    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        if (timeleft <= 0.0)
        {
            fps = (accum / frames);
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(-25, 25, w, h), "FPS: " + fps.ToString("F2"), textStyle);
    }
}