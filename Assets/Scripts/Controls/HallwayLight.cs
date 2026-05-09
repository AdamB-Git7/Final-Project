using UnityEngine;

// =====================================================================
//  HALLWAY LIGHT SYSTEM
// =====================================================================
//  Player holds Z to flash the left hallway light.
//  Player holds X to flash the right hallway light.
//  Useful for peeking through windows to see if an animatronic is there.
//  Drains the camera battery while held.
// =====================================================================
public class HallwayLightSystem : MonoBehaviour
{
    [Header("Controls")]
    public KeyCode leftLightKey = KeyCode.Z;
    public KeyCode rightLightKey = KeyCode.X;

    [Header("Battery Drain")]
    public float drainPerSecond = 4f;

    Light leftLight;            // Unity's Light component
    Light rightLight;
    SecurityCamera cameraSystem; // we drain its battery

    // =================================================================
    //  Start() — create the two hallway lights at runtime
    // =================================================================
    void Start()
    {
        // Make a light just outside each door
        leftLight  = CreateLight("LeftHallwayLight",  new Vector3(-5.25f, 2.5f, 0));
        rightLight = CreateLight("RightHallwayLight", new Vector3( 5.25f, 2.5f, 0));

        // Find the camera system so we can drain its battery
        cameraSystem = Object.FindFirstObjectByType<SecurityCamera>();
    }

    // Helper: build a Light component on a new GameObject
    Light CreateLight(string name, Vector3 pos)
    {
        GameObject obj = new GameObject(name);
        obj.transform.position = pos;
        Light light = obj.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = new Color(1f, 0.95f, 0.7f);  // warm yellow
        light.intensity = 0f;                       // off by default
        light.range = 8f;
        light.shadows = LightShadows.Soft;
        return light;
    }

    // =================================================================
    //  Update() — runs every frame
    // =================================================================
    void Update()
    {
        // GetKey returns true while the key is HELD (not just pressed)
        bool leftHeld  = Input.GetKey(leftLightKey);
        bool rightHeld = Input.GetKey(rightLightKey);

        // Fade each light to on or off
        SetLight(leftLight,  leftHeld);
        SetLight(rightLight, rightHeld);

        // While either light is on, drain the camera battery
        if ((leftHeld || rightHeld) && cameraSystem != null)
            cameraSystem.DrainBatteryExternal(drainPerSecond * Time.deltaTime);
    }

    // Smoothly raise/lower a light's intensity
    void SetLight(Light light, bool on)
    {
        if (light == null) return;
        float target = on ? 4f : 0f;
        // MoveTowards = step toward target by a fixed amount per frame
        light.intensity = Mathf.MoveTowards(light.intensity, target, Time.deltaTime * 20f);
    }
}
