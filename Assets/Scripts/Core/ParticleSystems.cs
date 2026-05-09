
// Imports the UnityEngine namespace.
using UnityEngine;








// Declares the class named ParticleSystems.
public class ParticleSystems : MonoBehaviour

// Opens a new code block.
{

    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {


        // Calls a method.
        CreateDustMotes(new Vector3(0, 1.5f, -0.5f));



        // Iterates through each item in the collection.
        foreach (var ai in Object.FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))

            // Calls a method.
            CreateMist(ai.gameObject);

    // Closes the current code block.
    }


    // Declares the method named CreateDustMotes.
    void CreateDustMotes(Vector3 position)

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject("DustParticles");

        // Updates an existing value.
        obj.transform.position = position;


        // Declares the variable ps and initializes it.
        ParticleSystem ps = obj.AddComponent<ParticleSystem>();



        // Calls a method.
        ps.Stop();


        // Declares the variable main and initializes it.
        var main = ps.main;

        // Updates an existing value.
        main.startLifetime = 8f;

        // Updates an existing value.
        main.startSpeed = 0.05f;

        // Updates an existing value.
        main.startSize = 0.04f;

        // Updates an existing value.
        main.startColor = new Color(1f, 1f, 0.95f, 0.5f);

        // Updates an existing value.
        main.maxParticles = 100;

        // Updates an existing value.
        main.simulationSpace = ParticleSystemSimulationSpace.World;


        // Declares the variable emission and initializes it.
        var emission = ps.emission;

        // Updates an existing value.
        emission.rateOverTime = 8f;


        // Declares the variable shape and initializes it.
        var shape = ps.shape;

        // Updates an existing value.
        shape.shapeType = ParticleSystemShapeType.Box;

        // Updates an existing value.
        shape.scale = new Vector3(7f, 3f, 4f);


        // Declares the variable renderer and initializes it.
        var renderer = ps.GetComponent<ParticleSystemRenderer>();

        // Updates an existing value.
        renderer.material = new Material(Shader.Find("Sprites/Default"));


        // Calls a method.
        ps.Play();

    // Closes the current code block.
    }


    // Declares the method named CreateMist.
    void CreateMist(GameObject parent)

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject("MistParticles");

        // Updates an existing value.
        obj.transform.parent = parent.transform;

        // Updates an existing value.
        obj.transform.localPosition = new Vector3(0, 0.2f, 0);


        // Declares the variable ps and initializes it.
        ParticleSystem ps = obj.AddComponent<ParticleSystem>();

        // Calls a method.
        ps.Stop();


        // Declares the variable main and initializes it.
        var main = ps.main;

        // Updates an existing value.
        main.startLifetime = 3f;

        // Updates an existing value.
        main.startSpeed = 0.3f;

        // Updates an existing value.
        main.startSize = 0.8f;

        // Updates an existing value.
        main.startColor = new Color(0.4f, 0.1f, 0.15f, 0.25f);

        // Updates an existing value.
        main.maxParticles = 30;

        // Updates an existing value.
        main.simulationSpace = ParticleSystemSimulationSpace.World;


        // Declares the variable emission and initializes it.
        var emission = ps.emission;

        // Updates an existing value.
        emission.rateOverTime = 6f;


        // Declares the variable shape and initializes it.
        var shape = ps.shape;

        // Updates an existing value.
        shape.shapeType = ParticleSystemShapeType.Sphere;

        // Updates an existing value.
        shape.radius = 0.5f;


        // Declares the variable sizeOverLifetime and initializes it.
        var sizeOverLifetime = ps.sizeOverLifetime;

        // Updates an existing value.
        sizeOverLifetime.enabled = true;

        // Declares the variable curve and initializes it.
        AnimationCurve curve = new AnimationCurve();

        // Calls a method.
        curve.AddKey(0f, 0.3f);

        // Calls a method.
        curve.AddKey(0.5f, 1f);

        // Calls a method.
        curve.AddKey(1f, 0f);

        // Updates an existing value.
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);


        // Declares the variable renderer and initializes it.
        var renderer = ps.GetComponent<ParticleSystemRenderer>();

        // Updates an existing value.
        renderer.material = new Material(Shader.Find("Sprites/Default"));


        // Calls a method.
        ps.Play();

    // Closes the current code block.
    }

// Closes the current code block.
}
