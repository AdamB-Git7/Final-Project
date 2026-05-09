using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

// =====================================================================
//  ANIMATRONIC ANIMATOR — animates the animatronic body
// =====================================================================
//  All done in code (no Animator Controller).
//   - Body sways side-to-side while walking
//   - Body bobs up and down while walking
//   - Head turns left/right
//   - Eye pupils pulse red
// =====================================================================
public class AnimatronicAnimator : MonoBehaviour
{
    [Header("Walking animation")]
    public float swayAmount = 25f;     // degrees of side-to-side
    public float bobAmount = 0.2f;     // meters up/down
    public float walkFrequency = 5f;   // how fast the cycle is

    [Header("Eye flicker")]
    public float flickerSpeed = 8f;
    public float minIntensity = 0.5f;
    public float maxIntensity = 6f;

    NavMeshAgent agent;
    Transform body;              // the animatronic itself
    Transform head;              // the head child
    Vector3 baseBodyPos;
    Quaternion baseBodyRot;
    Quaternion baseHeadRot;
    List<Renderer> eyeRenderers; // pupils that we change emission color on

    // =================================================================
    //  Start() — runs once
    // =================================================================
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Save the resting pose so we can return to it when not walking
        body = transform;
        baseBodyPos = body.localPosition;
        baseBodyRot = body.localRotation;

        // Find the head child (named "Head")
        head = transform.Find("Head");
        if (head != null) baseHeadRot = head.localRotation;

        // Build a list of all "eye"/"pupil" renderers in the children
        eyeRenderers = new List<Renderer>();
        Renderer[] allRenderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in allRenderers)
        {
            string lowerName = r.gameObject.name.ToLower();
            if (lowerName.StartsWith("pupil") || lowerName.StartsWith("eye"))
                eyeRenderers.Add(r);
        }
    }

    // =================================================================
    //  Update() — runs every frame
    // =================================================================
    void Update()
    {
        AnimateWalk();
        AnimateEyes();
    }

    // -----------------------------------------------------------------
    //  Animate the walking sway/bob using sine waves
    // -----------------------------------------------------------------
    void AnimateWalk()
    {
        if (agent == null || body == null) return;

        // Are we actually moving? sqrMagnitude > 0.1 = "yes"
        bool walking = agent.velocity.sqrMagnitude > 0.1f;

        if (!walking)
        {
            // Smoothly return to the resting pose
            body.localPosition = Vector3.Lerp(body.localPosition, baseBodyPos, Time.deltaTime * 5f);
            body.localRotation = Quaternion.Slerp(body.localRotation, baseBodyRot, Time.deltaTime * 5f);
            if (head != null)
                head.localRotation = Quaternion.Slerp(head.localRotation, baseHeadRot, Time.deltaTime * 5f);
            return;
        }

        // We're walking — apply sine-wave sway and bob
        // Sin(t) returns a value between -1 and +1 that oscillates
        float t = Time.time * walkFrequency;
        float sway = Mathf.Sin(t) * swayAmount;          // side-to-side
        float bob = Mathf.Abs(Mathf.Sin(t)) * bobAmount; // up only (no negative)

        body.localPosition = baseBodyPos + new Vector3(0, bob, 0);
        body.localRotation = baseBodyRot * Quaternion.Euler(0, 0, sway);

        // Head turns left/right slowly as if looking around
        if (head != null)
        {
            float headTurn = Mathf.Sin(t * 0.5f) * 20f;
            head.localRotation = baseHeadRot * Quaternion.Euler(0, headTurn, 0);
        }
    }

    // -----------------------------------------------------------------
    //  Animate the eye glow pulsing red
    // -----------------------------------------------------------------
    void AnimateEyes()
    {
        if (eyeRenderers == null || eyeRenderers.Count == 0) return;

        // Map sin (-1 to 1) into (0 to 1), then lerp between min and max
        float wave = (Mathf.Sin(Time.time * flickerSpeed) + 1f) * 0.5f;
        float pulse = Mathf.Lerp(minIntensity, maxIntensity, wave);

        foreach (Renderer r in eyeRenderers)
        {
            if (r == null || r.material == null) continue;
            // Only adjust materials that have an emission property
            if (r.material.HasProperty("_EmissionColor"))
                r.material.SetColor("_EmissionColor", Color.red * pulse);
        }
    }
}
