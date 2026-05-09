
// Imports the UnityEngine namespace.
using UnityEngine;










// Declares the class named HallwayLightSystem.
public class HallwayLightSystem : MonoBehaviour

// Opens a new code block.
{

    // Applies the Header("Controls") attribute.
    [Header("Controls")]

    // Declares the variable leftLightKey and initializes it.
    public KeyCode leftLightKey = KeyCode.Z;

    // Declares the variable rightLightKey and initializes it.
    public KeyCode rightLightKey = KeyCode.X;


    // Applies the Header("Battery Drain") attribute.
    [Header("Battery Drain")]

    // Declares the variable drainPerSecond and initializes it.
    public float drainPerSecond = 4f;


    // Declares the variable leftLight.
    Light leftLight;

    // Declares the variable rightLight.
    Light rightLight;

    // Declares the variable cameraSystem.
    SecurityCamera cameraSystem;





    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {


        // Updates an existing value.
        leftLight  = CreateLight("LeftHallwayLight",  new Vector3(-5.25f, 2.5f, 0));

        // Updates an existing value.
        rightLight = CreateLight("RightHallwayLight", new Vector3( 5.25f, 2.5f, 0));



        // Updates an existing value.
        cameraSystem = Object.FindFirstObjectByType<SecurityCamera>();

    // Closes the current code block.
    }



    // Declares the method named CreateLight.
    Light CreateLight(string name, Vector3 pos)

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject(name);

        // Updates an existing value.
        obj.transform.position = pos;

        // Declares the variable light and initializes it.
        Light light = obj.AddComponent<Light>();

        // Updates an existing value.
        light.type = LightType.Point;

        // Updates an existing value.
        light.color = new Color(1f, 0.95f, 0.7f);

        // Updates an existing value.
        light.intensity = 0f;

        // Updates an existing value.
        light.range = 8f;

        // Updates an existing value.
        light.shadows = LightShadows.Soft;

        // Returns the specified value.
        return light;

    // Closes the current code block.
    }





    // Declares the method named Update.
    void Update()

    // Opens a new code block.
    {


        // Declares the variable leftHeld and initializes it.
        bool leftHeld  = Input.GetKey(leftLightKey);

        // Declares the variable rightHeld and initializes it.
        bool rightHeld = Input.GetKey(rightLightKey);



        // Calls a method.
        SetLight(leftLight,  leftHeld);

        // Calls a method.
        SetLight(rightLight, rightHeld);



        // Checks whether the condition is true.
        if ((leftHeld || rightHeld) && cameraSystem != null)

            // Calls a method.
            cameraSystem.DrainBatteryExternal(drainPerSecond * Time.deltaTime);

    // Closes the current code block.
    }



    // Declares the method named SetLight.
    void SetLight(Light light, bool on)

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (light == null) return;

        // Declares the variable target and initializes it.
        float target = on ? 4f : 0f;


        // Updates an existing value.
        light.intensity = Mathf.MoveTowards(light.intensity, target, Time.deltaTime * 20f);

    // Closes the current code block.
    }

// Closes the current code block.
}
