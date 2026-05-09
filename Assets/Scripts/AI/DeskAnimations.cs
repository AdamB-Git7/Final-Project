
// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the System.Collections.Generic namespace.
using System.Collections.Generic;








// Declares the class named DeskAnimations.
public class DeskAnimations : MonoBehaviour

// Opens a new code block.
{

    // Applies the Header("Fan") attribute.
    [Header("Fan")]

    // Declares the variable fanSpinSpeed and initializes it.
    public float fanSpinSpeed = 720f;


    // Applies the Header("Monitor flicker") attribute.
    [Header("Monitor flicker")]

    // Declares the variable flickerSpeed and initializes it.
    public float flickerSpeed = 8f;

    // Declares the variable screenColorMin and initializes it.
    public Color screenColorMin = new Color(0.02f, 0.06f, 0.02f);

    // Declares the variable screenColorMax and initializes it.
    public Color screenColorMax = new Color(0.15f, 0.4f, 0.15f);


    // Declares the variable fanHead.
    Transform fanHead;

    // Declares the variable screens.
    List<Renderer> screens;





    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {


        // Declares the variable fan and initializes it.
        GameObject fan = GameObject.Find("FanHead");

        // Checks the condition and runs the inline statement when it is true.
        if (fan != null) fanHead = fan.transform;



        // Updates an existing value.
        screens = new List<Renderer>();

        // Starts a for loop.
        for (int i = 0; i < 3; i++)

        // Opens a new code block.
        {

            // Declares the variable s and initializes it.
            GameObject s = GameObject.Find("Screen" + i);

            // Checks whether the condition is true.
            if (s != null)

            // Opens a new code block.
            {

                // Declares the variable r and initializes it.
                Renderer r = s.GetComponent<Renderer>();

                // Checks the condition and runs the inline statement when it is true.
                if (r != null) screens.Add(r);

            // Closes the current code block.
            }

        // Closes the current code block.
        }

    // Closes the current code block.
    }





    // Declares the method named Update.
    void Update()

    // Opens a new code block.
    {



        // Checks whether the condition is true.
        if (fanHead != null)

            // Calls a method.
            fanHead.Rotate(0, 0, fanSpinSpeed * Time.deltaTime, Space.Self);



        // Checks whether the condition is true.
        if (screens != null)

        // Opens a new code block.
        {

            // Starts a for loop.
            for (int i = 0; i < screens.Count; i++)

            // Opens a new code block.
            {

                // Checks the condition and runs the inline statement when it is true.
                if (screens[i] == null || screens[i].material == null) continue;



                // Declares the variable wave and initializes it.
                float wave = (Mathf.Sin(Time.time * flickerSpeed + i * 1.3f) + 1f) * 0.5f;

                // Declares the variable c and initializes it.
                Color c = Color.Lerp(screenColorMin, screenColorMax, wave);

                // Calls a method.
                screens[i].material.SetColor("_EmissionColor", c);

            // Closes the current code block.
            }

        // Closes the current code block.
        }

    // Closes the current code block.
    }

// Closes the current code block.
}
