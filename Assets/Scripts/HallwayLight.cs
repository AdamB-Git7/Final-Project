using UnityEngine;

public class HallwayLight : MonoBehaviour
{
    [Header("Light Settings")]
    public KeyCode toggleKey = KeyCode.E;
    public Light hallwayLight;

    [Header("State")]
    public bool isOn = false;

    void Start()
    {
        if (hallwayLight != null)
            hallwayLight.enabled = isOn;
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleLight();
        }
    }

    public void ToggleLight()
    {
        isOn = !isOn;

        if (hallwayLight != null)
            hallwayLight.enabled = isOn;
    }
}
