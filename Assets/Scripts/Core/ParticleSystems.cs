using UnityEngine;

// =====================================================================
//  PARTICLE SYSTEMS
//  Adds atmospheric particles to the scene:
//   - Dust motes floating in the office
//   - Creepy fog drifting around the animatronics
// =====================================================================
public class ParticleSystems : MonoBehaviour
{
    void Start()
    {
        // Dust motes floating in the office
        CreateDustMotes(new Vector3(0, 1.5f, -0.5f));

        // Mist around each animatronic
        foreach (var ai in Object.FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))
            CreateMist(ai.gameObject);
    }

    void CreateDustMotes(Vector3 position)
    {
        GameObject obj = new GameObject("DustParticles");
        obj.transform.position = position;

        ParticleSystem ps = obj.AddComponent<ParticleSystem>();

        // Stop and clear so we can configure
        ps.Stop();

        var main = ps.main;
        main.startLifetime = 8f;
        main.startSpeed = 0.05f;
        main.startSize = 0.04f;
        main.startColor = new Color(1f, 1f, 0.95f, 0.5f);
        main.maxParticles = 100;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.rateOverTime = 8f;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(7f, 3f, 4f);

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));

        ps.Play();
    }

    void CreateMist(GameObject parent)
    {
        GameObject obj = new GameObject("MistParticles");
        obj.transform.parent = parent.transform;
        obj.transform.localPosition = new Vector3(0, 0.2f, 0);

        ParticleSystem ps = obj.AddComponent<ParticleSystem>();
        ps.Stop();

        var main = ps.main;
        main.startLifetime = 3f;
        main.startSpeed = 0.3f;
        main.startSize = 0.8f;
        main.startColor = new Color(0.4f, 0.1f, 0.15f, 0.25f);
        main.maxParticles = 30;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        var emission = ps.emission;
        emission.rateOverTime = 6f;

        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.5f;

        var sizeOverLifetime = ps.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 0.3f);
        curve.AddKey(0.5f, 1f);
        curve.AddKey(1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);

        var renderer = ps.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Sprites/Default"));

        ps.Play();
    }
}
