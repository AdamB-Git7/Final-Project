
// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.AI namespace.
using UnityEngine.AI;

// Imports the System.Collections.Generic namespace.
using System.Collections.Generic;











// Declares the class named AnimatronicAnimator.
public class AnimatronicAnimator : MonoBehaviour

// Opens a new code block.
{

    // Applies the Header("Walking animation") attribute.
    [Header("Walking animation")]

    // Declares the variable swayAmount and initializes it.
    public float swayAmount = 25f;

    // Declares the variable bobAmount and initializes it.
    public float bobAmount = 0.2f;

    // Declares the variable walkFrequency and initializes it.
    public float walkFrequency = 5f;


    // Applies the Header("Eye flicker") attribute.
    [Header("Eye flicker")]

    // Declares the variable flickerSpeed and initializes it.
    public float flickerSpeed = 8f;

    // Declares the variable minIntensity and initializes it.
    public float minIntensity = 0.5f;

    // Declares the variable maxIntensity and initializes it.
    public float maxIntensity = 6f;


    // Declares the variable agent.
    NavMeshAgent agent;

    // Declares the variable body.
    Transform body;

    // Declares the variable head.
    Transform head;

    // Declares the variable baseBodyPos.
    Vector3 baseBodyPos;

    // Declares the variable baseBodyRot.
    Quaternion baseBodyRot;

    // Declares the variable baseHeadRot.
    Quaternion baseHeadRot;

    // Declares the variable eyeRenderers.
    List<Renderer> eyeRenderers;





    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {

        // Updates an existing value.
        agent = GetComponent<NavMeshAgent>();



        // Updates an existing value.
        body = transform;

        // Updates an existing value.
        baseBodyPos = body.localPosition;

        // Updates an existing value.
        baseBodyRot = body.localRotation;



        // Updates an existing value.
        head = transform.Find("Head");

        // Checks the condition and runs the inline statement when it is true.
        if (head != null) baseHeadRot = head.localRotation;



        // Updates an existing value.
        eyeRenderers = new List<Renderer>();

        // Declares the variable allRenderers and initializes it.
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();

        // Iterates through each item in the collection.
        foreach (Renderer r in allRenderers)

        // Opens a new code block.
        {

            // Declares the variable lowerName and initializes it.
            string lowerName = r.gameObject.name.ToLower();

            // Checks whether the condition is true.
            if (lowerName.StartsWith("pupil") || lowerName.StartsWith("eye"))

                // Calls a method.
                eyeRenderers.Add(r);

        // Closes the current code block.
        }

    // Closes the current code block.
    }





    // Declares the method named Update.
    void Update()

    // Opens a new code block.
    {

        // Calls a method.
        AnimateWalk();

        // Calls a method.
        AnimateEyes();

    // Closes the current code block.
    }





    // Declares the method named AnimateWalk.
    void AnimateWalk()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (agent == null || body == null) return;



        // Declares the variable walking and initializes it.
        bool walking = agent.velocity.sqrMagnitude > 0.1f;


        // Checks whether the condition is true.
        if (!walking)

        // Opens a new code block.
        {


            // Updates an existing value.
            body.localPosition = Vector3.Lerp(body.localPosition, baseBodyPos, Time.deltaTime * 5f);

            // Updates an existing value.
            body.localRotation = Quaternion.Slerp(body.localRotation, baseBodyRot, Time.deltaTime * 5f);

            // Checks whether the condition is true.
            if (head != null)

                // Updates an existing value.
                head.localRotation = Quaternion.Slerp(head.localRotation, baseHeadRot, Time.deltaTime * 5f);

            // Returns from the current method.
            return;

        // Closes the current code block.
        }




        // Declares the variable t and initializes it.
        float t = Time.time * walkFrequency;

        // Declares the variable sway and initializes it.
        float sway = Mathf.Sin(t) * swayAmount;

        // Declares the variable bob and initializes it.
        float bob = Mathf.Abs(Mathf.Sin(t)) * bobAmount;


        // Updates an existing value.
        body.localPosition = baseBodyPos + new Vector3(0, bob, 0);

        // Updates an existing value.
        body.localRotation = baseBodyRot * Quaternion.Euler(0, 0, sway);



        // Checks whether the condition is true.
        if (head != null)

        // Opens a new code block.
        {

            // Declares the variable headTurn and initializes it.
            float headTurn = Mathf.Sin(t * 0.5f) * 20f;

            // Updates an existing value.
            head.localRotation = baseHeadRot * Quaternion.Euler(0, headTurn, 0);

        // Closes the current code block.
        }

    // Closes the current code block.
    }





    // Declares the method named AnimateEyes.
    void AnimateEyes()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (eyeRenderers == null || eyeRenderers.Count == 0) return;



        // Declares the variable wave and initializes it.
        float wave = (Mathf.Sin(Time.time * flickerSpeed) + 1f) * 0.5f;

        // Declares the variable pulse and initializes it.
        float pulse = Mathf.Lerp(minIntensity, maxIntensity, wave);


        // Iterates through each item in the collection.
        foreach (Renderer r in eyeRenderers)

        // Opens a new code block.
        {

            // Checks the condition and runs the inline statement when it is true.
            if (r == null || r.material == null) continue;


            // Checks whether the condition is true.
            if (r.material.HasProperty("_EmissionColor"))

                // Calls a method.
                r.material.SetColor("_EmissionColor", Color.red * pulse);

        // Closes the current code block.
        }

    // Closes the current code block.
    }

// Closes the current code block.
}
