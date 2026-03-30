using UnityEngine;
using UnityEditor;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class SceneBuilder : EditorWindow
{
    [MenuItem("Night Shift/Build Scene")]
    static void BuildScene()
    {
        if (!EditorUtility.DisplayDialog("Build Scene",
            "This will CLEAR the scene and rebuild everything.\nContinue?", "Build", "Cancel"))
            return;

        ClearScene();
        CreateMaterials();
        BuildOffice();
        BuildHallways();
        BuildLighting();
        BuildNavMesh();
        BuildDoors();
        BuildEnemy();
        BuildGameManager();
        TestNavMesh();
    }

    static void ClearScene()
    {
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            if (obj == null) continue;
            if (obj.transform.parent != null) continue;
            if (obj.GetComponent<Camera>() != null) continue;
            Object.DestroyImmediate(obj);
        }
    }

    static Material wallMat, floorMat, ceilingMat, doorMat, darkMat, tileMat;

    static void CreateMaterials()
    {
        wallMat = CreateMat("WallMaterial", new Color(0.18f, 0.16f, 0.22f));
        floorMat = CreateMat("FloorMaterial", new Color(0.08f, 0.08f, 0.1f));
        ceilingMat = CreateMat("CeilingMaterial", new Color(0.06f, 0.06f, 0.08f));
        doorMat = CreateMat("DoorMaterial", new Color(0.35f, 0.1f, 0.1f));
        darkMat = CreateMat("DarkMaterial", new Color(0.04f, 0.04f, 0.06f));
        tileMat = CreateMat("TileMaterial", new Color(0.12f, 0.12f, 0.16f));
    }

    static Material CreateMat(string matName, Color color)
    {
        string path = "Assets/Materials/" + matName + ".mat";
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null)
        {
            mat = new Material(Shader.Find("Standard"));
            mat.color = color;
            mat.SetFloat("_Glossiness", 0.15f);
            AssetDatabase.CreateAsset(mat, path);
        }
        else
        {
            mat.color = color;
        }
        return mat;
    }

    static void BuildOffice()
    {
        GameObject office = new GameObject("SecurityOffice");

        float roomW = 8f;
        float roomD = 5f;
        float roomH = 3.5f;
        float wallT = 0.2f;

        MakeBox(office.transform, "Floor", Vector3.zero, new Vector3(roomW, 0.1f, roomD), floorMat, true);
        MakeBox(office.transform, "Ceiling", new Vector3(0, roomH, 0), new Vector3(roomW, 0.1f, roomD), ceilingMat, true);
        MakeBox(office.transform, "BackWall", new Vector3(0, roomH / 2, -roomD / 2), new Vector3(roomW, roomH, wallT), wallMat, true);
        MakeBox(office.transform, "FrontWall", new Vector3(0, roomH / 2, roomD / 2), new Vector3(roomW, roomH, wallT), wallMat, true);

        float doorGap = 1.5f;

        // Left wall with door opening
        MakeBox(office.transform, "LeftWallFront", new Vector3(-roomW/2, roomH/2, doorGap + 0.5f), new Vector3(wallT, roomH, roomD/2 - doorGap), wallMat, true);
        MakeBox(office.transform, "LeftWallBack", new Vector3(-roomW/2, roomH/2, -(doorGap + 0.5f)), new Vector3(wallT, roomH, roomD/2 - doorGap), wallMat, true);

        // Left door frame (no colliders)
        var lft = MakeBox(office.transform, "LeftFrameTop", new Vector3(-roomW/2, 2.6f, 0), new Vector3(0.3f, 0.15f, doorGap * 2 + 0.1f), darkMat, false);
        Object.DestroyImmediate(lft.GetComponent<Collider>());
        var lfl = MakeBox(office.transform, "LeftFrameL", new Vector3(-roomW/2, roomH/2, doorGap), new Vector3(0.3f, roomH, 0.1f), darkMat, false);
        Object.DestroyImmediate(lfl.GetComponent<Collider>());
        var lfr = MakeBox(office.transform, "LeftFrameR", new Vector3(-roomW/2, roomH/2, -doorGap), new Vector3(0.3f, roomH, 0.1f), darkMat, false);
        Object.DestroyImmediate(lfr.GetComponent<Collider>());

        // Right wall with door opening
        MakeBox(office.transform, "RightWallFront", new Vector3(roomW/2, roomH/2, doorGap + 0.5f), new Vector3(wallT, roomH, roomD/2 - doorGap), wallMat, true);
        MakeBox(office.transform, "RightWallBack", new Vector3(roomW/2, roomH/2, -(doorGap + 0.5f)), new Vector3(wallT, roomH, roomD/2 - doorGap), wallMat, true);

        // Right door frame (no colliders)
        var rft = MakeBox(office.transform, "RightFrameTop", new Vector3(roomW/2, 2.6f, 0), new Vector3(0.3f, 0.15f, doorGap * 2 + 0.1f), darkMat, false);
        Object.DestroyImmediate(rft.GetComponent<Collider>());
        var rfl = MakeBox(office.transform, "RightFrameL", new Vector3(roomW/2, roomH/2, doorGap), new Vector3(0.3f, roomH, 0.1f), darkMat, false);
        Object.DestroyImmediate(rfl.GetComponent<Collider>());
        var rfr = MakeBox(office.transform, "RightFrameR", new Vector3(roomW/2, roomH/2, -doorGap), new Vector3(0.3f, roomH, 0.1f), darkMat, false);
        Object.DestroyImmediate(rfr.GetComponent<Collider>());

        // Bridge floors at door openings
        MakeBox(office.transform, "LeftBridgeFloor", new Vector3(-roomW/2, 0, 0), new Vector3(1.5f, 0.1f, doorGap * 2), floorMat, true);
        MakeBox(office.transform, "RightBridgeFloor", new Vector3(roomW/2, 0, 0), new Vector3(1.5f, 0.1f, doorGap * 2), floorMat, true);

        // Desk
        MakeBox(office.transform, "Desk", new Vector3(0, 0.75f, -0.2f), new Vector3(5f, 0.08f, 1.4f), darkMat, true);

        var deskFront = MakeBox(office.transform, "DeskFront", new Vector3(0, 0.38f, 0.5f), new Vector3(5f, 0.75f, 0.06f), darkMat, false);
        Object.DestroyImmediate(deskFront.GetComponent<Collider>());

        float[] legX = { -2.3f, 2.3f, -2.3f, 2.3f };
        float[] legZ = { -0.8f, -0.8f, 0.45f, 0.45f };
        for (int i = 0; i < 4; i++)
            MakeBox(office.transform, "DeskLeg" + i, new Vector3(legX[i], 0.35f, legZ[i]), new Vector3(0.08f, 0.7f, 0.08f), darkMat, true);

        // Monitors
        for (int i = -1; i <= 1; i++)
        {
            MakeBox(office.transform, "Monitor" + (i + 2), new Vector3(i * 1.2f, 1.15f, -0.5f), new Vector3(0.9f, 0.6f, 0.05f), darkMat, false);

            GameObject screen = GameObject.CreatePrimitive(PrimitiveType.Cube);
            screen.name = "Screen" + (i + 2);
            screen.transform.parent = office.transform;
            screen.transform.localPosition = new Vector3(i * 1.2f, 1.15f, -0.48f);
            screen.transform.localScale = new Vector3(0.8f, 0.5f, 0.01f);
            Material screenMat = new Material(Shader.Find("Standard"));
            screenMat.color = new Color(0.05f, 0.15f, 0.05f);
            screenMat.EnableKeyword("_EMISSION");
            screenMat.SetColor("_EmissionColor", new Color(0.02f, 0.06f, 0.02f));
            screen.GetComponent<Renderer>().material = screenMat;
        }

        // Chair
        MakeBox(office.transform, "ChairSeat", new Vector3(0, 0.45f, 1.3f), new Vector3(0.7f, 0.06f, 0.7f), doorMat, false);
        MakeBox(office.transform, "ChairBack", new Vector3(0, 0.8f, 1.65f), new Vector3(0.7f, 0.7f, 0.06f), doorMat, false);

        // Fan
        MakeBox(office.transform, "FanBase", new Vector3(2.0f, 0.82f, -0.2f), new Vector3(0.3f, 0.05f, 0.3f), tileMat, false);
        GameObject fanHead = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        fanHead.name = "FanHead";
        fanHead.transform.parent = office.transform;
        fanHead.transform.localPosition = new Vector3(2.0f, 1.05f, -0.2f);
        fanHead.transform.localScale = new Vector3(0.25f, 0.25f, 0.15f);
        fanHead.GetComponent<Renderer>().material = tileMat;

        // Posters
        MakePoster(office.transform, "PosterRules", new Vector3(-2.5f, 2.0f, -2.38f),
                   new Vector3(0.7f, 0.9f, 0.02f), "Assets/Textures/poster_rules.png");
        MakePoster(office.transform, "PosterCaution", new Vector3(2.5f, 1.8f, -2.38f),
                   new Vector3(0.6f, 0.6f, 0.02f), "Assets/Textures/poster_caution.png");
        MakePoster(office.transform, "PosterCelebrate", new Vector3(-0.5f, 2.5f, -2.38f),
                   new Vector3(0.5f, 0.65f, 0.02f), "Assets/Textures/poster_celebrate.png");
    }

    static void MakePoster(Transform parent, string name, Vector3 pos, Vector3 scale, string texturePath)
    {
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
        if (tex == null) return;

        Material mat = new Material(Shader.Find("Standard"));
        mat.mainTexture = tex;
        mat.SetFloat("_Glossiness", 0.1f);

        GameObject poster = GameObject.CreatePrimitive(PrimitiveType.Cube);
        poster.name = name;
        poster.transform.parent = parent;
        poster.transform.localPosition = pos;
        poster.transform.localScale = scale;
        poster.GetComponent<Renderer>().material = mat;
        Object.DestroyImmediate(poster.GetComponent<Collider>());
    }

    static GameObject MakeBox(Transform parent, string name, Vector3 pos, Vector3 scale, Material mat, bool isStatic)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = name;
        obj.transform.parent = parent;
        obj.transform.localPosition = pos;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().material = mat;
        obj.isStatic = isStatic;
        return obj;
    }

    static GameObject MakeBox(Transform parent, string name, Vector3 pos, Vector3 scale, Color color, bool isStatic)
    {
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        mat.SetFloat("_Glossiness", 0.15f);
        return MakeBox(parent, name, pos, scale, mat, isStatic);
    }

    static void BuildHallways()
    {
        float hallW = 2.5f;
        float hallH = 3.5f;
        float wallT = 0.2f;
        float h2 = hallH / 2f;
        float lx = -5.25f;
        float rx = 5.25f;

        // Left hallway
        GameObject leftHall = new GameObject("LeftHallway");
        MakeBox(leftHall.transform, "Floor", new Vector3(lx, 0, -5f), new Vector3(hallW, 0.1f, 14f), floorMat, true);
        MakeBox(leftHall.transform, "Ceiling", new Vector3(lx, hallH, -5f), new Vector3(hallW, 0.1f, 14f), ceilingMat, true);
        MakeBox(leftHall.transform, "OuterWall", new Vector3(lx - hallW/2, h2, -5f), new Vector3(wallT, hallH, 14f), wallMat, true);
        MakeBox(leftHall.transform, "InnerWall_Front", new Vector3(lx + hallW/2, h2, 2.75f), new Vector3(wallT, hallH, 2.5f), wallMat, true);
        MakeBox(leftHall.transform, "InnerWall_Back", new Vector3(lx + hallW/2, h2, -6f), new Vector3(wallT, hallH, 6f), wallMat, true);

        // Right hallway
        GameObject rightHall = new GameObject("RightHallway");
        MakeBox(rightHall.transform, "Floor", new Vector3(rx, 0, -5f), new Vector3(hallW, 0.1f, 14f), floorMat, true);
        MakeBox(rightHall.transform, "Ceiling", new Vector3(rx, hallH, -5f), new Vector3(hallW, 0.1f, 14f), ceilingMat, true);
        MakeBox(rightHall.transform, "OuterWall", new Vector3(rx + hallW/2, h2, -5f), new Vector3(wallT, hallH, 14f), wallMat, true);
        MakeBox(rightHall.transform, "InnerWall_Front", new Vector3(rx - hallW/2, h2, 2.75f), new Vector3(wallT, hallH, 2.5f), wallMat, true);
        MakeBox(rightHall.transform, "InnerWall_Back", new Vector3(rx - hallW/2, h2, -6f), new Vector3(wallT, hallH, 6f), wallMat, true);

        // Back corridor connecting both hallways
        GameObject corridor = new GameObject("BackCorridor");
        MakeBox(corridor.transform, "Floor", new Vector3(0, 0, -11f), new Vector3(14f, 0.1f, 2.5f), floorMat, true);
        MakeBox(corridor.transform, "Ceiling", new Vector3(0, hallH, -11f), new Vector3(14f, 0.1f, 2.5f), ceilingMat, true);
        MakeBox(corridor.transform, "BackWall", new Vector3(0, h2, -12.25f), new Vector3(14f, hallH, wallT), wallMat, true);
        MakeBox(corridor.transform, "LeftWall", new Vector3(-7f, h2, -11f), new Vector3(wallT, hallH, 2.5f), wallMat, true);
        MakeBox(corridor.transform, "RightWall", new Vector3(7f, h2, -11f), new Vector3(wallT, hallH, 2.5f), wallMat, true);
    }

    static Material doorMetalMat, doorWarnMat;

    static void BuildDoors()
    {
        doorMetalMat = CreateMat("DoorMetalMaterial", new Color(0.25f, 0.25f, 0.28f));
        doorWarnMat = new Material(Shader.Find("Standard"));
        doorWarnMat.color = new Color(0.8f, 0.6f, 0.1f);
        doorWarnMat.SetFloat("_Glossiness", 0.4f);

        BuildSingleDoor("LeftDoor", new Vector3(-4f, 0, 0), KeyCode.E, 1f);
        BuildSingleDoor("RightDoor", new Vector3(4f, 0, 0), KeyCode.Q, -1f);
    }

    static void BuildSingleDoor(string name, Vector3 pos, KeyCode key, float facing)
    {
        GameObject door = new GameObject(name);
        door.transform.position = pos;

        GameObject panel = new GameObject("DoorPanel");
        panel.transform.parent = door.transform;
        panel.transform.localPosition = new Vector3(0, 1.3f, 0);

        GameObject slab = GameObject.CreatePrimitive(PrimitiveType.Cube);
        slab.name = "Slab";
        slab.transform.parent = panel.transform;
        slab.transform.localPosition = Vector3.zero;
        slab.transform.localScale = new Vector3(0.12f, 2.6f, 2.9f);
        slab.GetComponent<Renderer>().material = doorMetalMat;

        float d = 0.07f * facing;

        // Reinforcement bars on both sides
        for (int i = -1; i <= 1; i++)
        {
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar.name = "Bar" + i;
            bar.transform.parent = panel.transform;
            bar.transform.localPosition = new Vector3(d, i * 0.8f, 0);
            bar.transform.localScale = new Vector3(0.03f, 0.08f, 2.9f);
            bar.GetComponent<Renderer>().material = darkMat;
            Object.DestroyImmediate(bar.GetComponent<Collider>());

            GameObject bar2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bar2.name = "BarBack" + i;
            bar2.transform.parent = panel.transform;
            bar2.transform.localPosition = new Vector3(-d, i * 0.8f, 0);
            bar2.transform.localScale = new Vector3(0.03f, 0.08f, 2.9f);
            bar2.GetComponent<Renderer>().material = darkMat;
            Object.DestroyImmediate(bar2.GetComponent<Collider>());
        }

        // Warning stripes
        GameObject stripe = GameObject.CreatePrimitive(PrimitiveType.Cube);
        stripe.name = "WarningStripe";
        stripe.transform.parent = panel.transform;
        stripe.transform.localPosition = new Vector3(d, -1.1f, 0);
        stripe.transform.localScale = new Vector3(0.02f, 0.2f, 2.9f);
        stripe.GetComponent<Renderer>().material = doorWarnMat;
        Object.DestroyImmediate(stripe.GetComponent<Collider>());

        GameObject stripe2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        stripe2.name = "WarningStripeBack";
        stripe2.transform.parent = panel.transform;
        stripe2.transform.localPosition = new Vector3(-d, -1.1f, 0);
        stripe2.transform.localScale = new Vector3(0.02f, 0.2f, 2.9f);
        stripe2.GetComponent<Renderer>().material = doorWarnMat;
        Object.DestroyImmediate(stripe2.GetComponent<Collider>());

        // Windows
        Material windowMat = new Material(Shader.Find("Standard"));
        windowMat.color = new Color(0.1f, 0.15f, 0.2f);
        windowMat.SetFloat("_Glossiness", 0.8f);

        GameObject window = GameObject.CreatePrimitive(PrimitiveType.Cube);
        window.name = "Window";
        window.transform.parent = panel.transform;
        window.transform.localPosition = new Vector3(d, 0.5f, 0);
        window.transform.localScale = new Vector3(0.02f, 0.4f, 0.6f);
        window.GetComponent<Renderer>().material = windowMat;
        Object.DestroyImmediate(window.GetComponent<Collider>());

        GameObject window2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        window2.name = "WindowBack";
        window2.transform.parent = panel.transform;
        window2.transform.localPosition = new Vector3(-d, 0.5f, 0);
        window2.transform.localScale = new Vector3(0.02f, 0.4f, 0.6f);
        window2.GetComponent<Renderer>().material = windowMat;
        Object.DestroyImmediate(window2.GetComponent<Collider>());

        DoorController dc = door.AddComponent<DoorController>();
        dc.toggleKey = key;
        dc.isClosed = false;

        NavMeshObstacle obs = door.AddComponent<NavMeshObstacle>();
        obs.carving = true;
        obs.size = new Vector3(0.5f, 3f, 3f);
        obs.center = new Vector3(0, 1.5f, 0);
        obs.enabled = false;
        dc.doorBlocker = obs;

        PrefabUtility.SaveAsPrefabAsset(door, "Assets/Prefabs/" + name + ".prefab");
    }

    static void BuildEnemy()
    {
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        enemy.name = "Animatronic";
        enemy.transform.position = new Vector3(0, 1, -11f);
        enemy.transform.localScale = new Vector3(0.8f, 1.2f, 0.8f);
        enemy.GetComponent<Renderer>().material = doorMat;

        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        head.name = "Head";
        head.transform.parent = enemy.transform;
        head.transform.localPosition = new Vector3(0, 0.85f, 0);
        head.transform.localScale = new Vector3(0.7f, 0.5f, 0.6f);
        head.GetComponent<Renderer>().material = doorMat;
        Object.DestroyImmediate(head.GetComponent<Collider>());

        // Ears
        for (int i = 0; i < 2; i++)
        {
            GameObject ear = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ear.name = "Ear" + i;
            ear.transform.parent = head.transform;
            float xPos = (i == 0) ? -0.45f : 0.45f;
            ear.transform.localPosition = new Vector3(xPos, 0.4f, 0);
            ear.transform.localScale = new Vector3(0.25f, 0.35f, 0.25f);
            ear.GetComponent<Renderer>().material = doorMat;
            Object.DestroyImmediate(ear.GetComponent<Collider>());
        }

        // Eyes
        for (int i = 0; i < 2; i++)
        {
            GameObject eye = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            eye.name = "Eye" + i;
            eye.transform.parent = head.transform;
            float xPos = (i == 0) ? -0.22f : 0.22f;
            eye.transform.localPosition = new Vector3(xPos, 0.05f, 0.7f);
            eye.transform.localScale = new Vector3(0.2f, 0.2f, 0.1f);

            Material eyeMat = new Material(Shader.Find("Standard"));
            eyeMat.color = Color.red;
            eyeMat.EnableKeyword("_EMISSION");
            eyeMat.SetColor("_EmissionColor", Color.red * 3f);
            eye.GetComponent<Renderer>().material = eyeMat;
            Object.DestroyImmediate(eye.GetComponent<Collider>());
        }

        // Mouth
        GameObject mouth = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mouth.name = "Mouth";
        mouth.transform.parent = head.transform;
        mouth.transform.localPosition = new Vector3(0, -0.25f, 0.7f);
        mouth.transform.localScale = new Vector3(0.4f, 0.15f, 0.1f);
        Material mouthMat = new Material(Shader.Find("Standard"));
        mouthMat.color = new Color(0.15f, 0.02f, 0.02f);
        mouth.GetComponent<Renderer>().material = mouthMat;
        Object.DestroyImmediate(mouth.GetComponent<Collider>());

        // Enemy glow light
        GameObject enemyLight = new GameObject("EnemyGlow");
        enemyLight.transform.parent = enemy.transform;
        enemyLight.transform.localPosition = new Vector3(0, 0.5f, 0);
        Light glow = enemyLight.AddComponent<Light>();
        glow.type = LightType.Point;
        glow.color = new Color(1f, 0.2f, 0.1f);
        glow.intensity = 1.2f;
        glow.range = 5f;

        NavMeshAgent agent = enemy.AddComponent<NavMeshAgent>();
        agent.speed = 2f;
        agent.stoppingDistance = 0.5f;
        agent.radius = 0.4f;
        agent.height = 2f;

        EnemyAI ai = enemy.AddComponent<EnemyAI>();
        ai.moveSpeed = 2f;
        ai.doorWaitTime = 1.5f;

        GameObject centerPt = new GameObject("CenterPoint");
        centerPt.transform.position = new Vector3(0, 0.5f, -11f);

        GameObject leftDoorPt = new GameObject("LeftDoorPoint");
        leftDoorPt.transform.position = new Vector3(-5.25f, 0.5f, 0);

        GameObject rightDoorPt = new GameObject("RightDoorPoint");
        rightDoorPt.transform.position = new Vector3(5.25f, 0.5f, 0);

        ai.centerPoint = centerPt.transform;
        ai.leftDoorPoint = leftDoorPt.transform;
        ai.rightDoorPoint = rightDoorPt.transform;

        DoorController[] doors = Object.FindObjectsByType<DoorController>(FindObjectsSortMode.None);
        foreach (DoorController dc in doors)
        {
            if (dc.transform.position.x < 0) ai.leftDoor = dc;
            if (dc.transform.position.x > 0) ai.rightDoor = dc;
        }

        PrefabUtility.SaveAsPrefabAsset(enemy, "Assets/Prefabs/Animatronic.prefab");
    }

    static void BuildLighting()
    {
        // Remove default directional light
        Light[] allLights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light l in allLights)
        {
            if (l.type == LightType.Directional)
                Object.DestroyImmediate(l.gameObject);
        }

        CreateLight("OfficeLight", new Vector3(0, 3.2f, 0), new Color(1f, 0.9f, 0.75f), 1.2f, 12f);
        CreateLight("MonitorGlow", new Vector3(0, 1.3f, -0.3f), new Color(0.3f, 1f, 0.3f), 0.4f, 4f);
        CreateLight("LeftDoorLight", new Vector3(-3.5f, 2.5f, 0), new Color(1f, 0.2f, 0.2f), 1.5f, 5f);
        CreateLight("RightDoorLight", new Vector3(3.5f, 2.5f, 0), new Color(1f, 0.2f, 0.2f), 1.5f, 5f);

        CreateLight("LeftHallLight1", new Vector3(-5.25f, 2.5f, -1f), new Color(0.3f, 0.4f, 1f), 1.0f, 8f);
        CreateLight("LeftHallLight2", new Vector3(-5.25f, 2.5f, -4f), new Color(0.3f, 0.4f, 1f), 0.8f, 8f);
        CreateLight("LeftHallLight3", new Vector3(-5.25f, 2.5f, -7f), new Color(0.3f, 0.4f, 1f), 0.6f, 8f);

        CreateLight("RightHallLight1", new Vector3(5.25f, 2.5f, -1f), new Color(0.3f, 0.4f, 1f), 1.0f, 8f);
        CreateLight("RightHallLight2", new Vector3(5.25f, 2.5f, -4f), new Color(0.3f, 0.4f, 1f), 0.8f, 8f);
        CreateLight("RightHallLight3", new Vector3(5.25f, 2.5f, -7f), new Color(0.3f, 0.4f, 1f), 0.6f, 8f);

        CreateLight("StageLight", new Vector3(-5.25f, 3f, -10f), new Color(1f, 0.8f, 0.4f), 1.5f, 8f);

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.04f, 0.04f, 0.06f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.01f, 0.01f, 0.02f);
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.025f;

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.transform.position = new Vector3(0, 1.6f, 1.8f);
            mainCam.transform.rotation = Quaternion.Euler(5, 180, 0);
            mainCam.fieldOfView = 90f;
            mainCam.backgroundColor = new Color(0.005f, 0.005f, 0.01f);
            mainCam.nearClipPlane = 0.1f;
        }
    }

    static void CreateLight(string lightName, Vector3 pos, Color color, float intensity, float range)
    {
        GameObject lightObj = new GameObject(lightName);
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = color;
        light.intensity = intensity;
        light.range = range;
        light.shadows = LightShadows.Soft;
        lightObj.transform.position = pos;
    }

    static void BuildNavMesh()
    {
        GameObject navObj = new GameObject("NavMeshSurface");
        NavMeshSurface surface = navObj.AddComponent<NavMeshSurface>();
        surface.collectObjects = CollectObjects.All;
        surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        surface.BuildNavMesh();
    }

    static void BuildGameManager()
    {
        GameObject gm = new GameObject("GameManager");
        GameManager manager = gm.AddComponent<GameManager>();

        EnemyAI enemy = Object.FindFirstObjectByType<EnemyAI>();
        if (enemy != null)
            enemy.gameManager = manager;
    }

    static void TestNavMesh()
    {
        Vector3 office = new Vector3(0, 0.5f, 0);
        Vector3 leftHall = new Vector3(-5.25f, 0.5f, -5f);
        Vector3 rightHall = new Vector3(5.25f, 0.5f, -5f);
        Vector3 leftDoor = new Vector3(-4f, 0.5f, 0);
        Vector3 rightDoor = new Vector3(4f, 0.5f, 0);
        Vector3 stage = new Vector3(-5.25f, 0.5f, -10f);
        Vector3 corridor = new Vector3(0, 0.5f, -11f);

        TestPath("Office -> Left Door", office, leftDoor);
        TestPath("Office -> Right Door", office, rightDoor);
        TestPath("Left Hall -> Left Door", leftHall, leftDoor);
        TestPath("Right Hall -> Right Door", rightHall, rightDoor);
        TestPath("Left Hall -> Right Hall", leftHall, rightHall);
        TestPath("Left Hall -> Corridor", leftHall, corridor);
        TestPath("Right Hall -> Corridor", rightHall, corridor);
        TestPath("Stage -> Left Door", stage, leftDoor);
        TestPath("Stage -> Right Door", stage, rightDoor);
    }

    static void TestPath(string name, Vector3 from, Vector3 to)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);
    }
}
