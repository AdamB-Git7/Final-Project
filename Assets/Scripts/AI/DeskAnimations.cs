using UnityEngine;
using System.Collections.Generic;

// =====================================================================
//  DESK ANIMATIONS — visible animations on the player's desk
// =====================================================================
//   - Spinning fan blades
//   - Flickering green monitor screens
// =====================================================================
public class DeskAnimations : MonoBehaviour
{
    [Header("Fan")]
    public float fanSpinSpeed = 720f;  // degrees per second (720 = 2 rotations/sec)

    [Header("Monitor flicker")]
    public float flickerSpeed = 8f;
    public Color screenColorMin = new Color(0.02f, 0.06f, 0.02f);  // dim green
    public Color screenColorMax = new Color(0.15f, 0.4f, 0.15f);   // brighter green

    Transform fanHead;        // the spinning fan sphere
    List<Renderer> screens;   // list of monitor screens

    // =================================================================
    //  Start() — find scene objects we want to animate
    // =================================================================
    void Start()
    {
        // Find the fan GameObject
        GameObject fan = GameObject.Find("FanHead");
        if (fan != null) fanHead = fan.transform;

        // Find the 3 monitor screens (Screen0, Screen1, Screen2)
        screens = new List<Renderer>();
        for (int i = 0; i < 3; i++)
        {
            GameObject s = GameObject.Find("Screen" + i);
            if (s != null)
            {
                Renderer r = s.GetComponent<Renderer>();
                if (r != null) screens.Add(r);
            }
        }
    }

    // =================================================================
    //  Update() — runs every frame
    // =================================================================
    void Update()
    {
        // Rotate the fan around its Z axis
        // Time.deltaTime keeps the speed consistent across frame rates
        if (fanHead != null)
            fanHead.Rotate(0, 0, fanSpinSpeed * Time.deltaTime, Space.Self);

        // Flicker each monitor's emission color using a sine wave
        if (screens != null)
        {
            for (int i = 0; i < screens.Count; i++)
            {
                if (screens[i] == null || screens[i].material == null) continue;

                // Each monitor is offset by i*1.3 so they don't pulse in sync
                float wave = (Mathf.Sin(Time.time * flickerSpeed + i * 1.3f) + 1f) * 0.5f;
                Color c = Color.Lerp(screenColorMin, screenColorMax, wave);
                screens[i].material.SetColor("_EmissionColor", c);
            }
        }
    }
}
