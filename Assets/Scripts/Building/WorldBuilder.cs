using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using TMPro;

public class WorldBuilder : MonoBehaviour
{
    Material wallMat, floorMat, ceilingMat, doorMat, darkMat, tileMat, doorMetalMat, doorWarnMat;

    public void BuildAll()
    {
        if (GameObject.Find("SecurityOffice") != null) return;

        CreateMaterials();
        BuildOffice();
        BuildHallways();
        BuildLighting();
        BuildNavMesh();
        BuildDoors();
        BuildSpots();
        BuildEnemy();
    }

    // The named locations the AI walks between. EnemyAI references these by name
    public static GameObject Corridor, LeftAlcove, RightAlcove, Stage, Classroom, Bathroom;
    public static GameObject LeftDoorSpot, RightDoorSpot, OfficeCenter;

    void BuildSpots()
    {
        // Create empty markers grouped under one parent — these are AI navigation targets
        GameObject parent = new GameObject("AISpots");
        Corridor      = MakeSpot(parent, "Spot_Corridor",      new Vector3( 0f,    0.5f, -19f));
        LeftAlcove    = MakeSpot(parent, "Spot_LeftAlcove",    new Vector3(-7.25f, 0.5f, -7f));
        RightAlcove   = MakeSpot(parent, "Spot_RightAlcove",   new Vector3( 7.25f, 0.5f, -7f));
        Stage         = MakeSpot(parent, "Spot_Stage",         new Vector3( 0f,    0.5f, -23f));
        Classroom     = MakeSpot(parent, "Spot_Classroom",     new Vector3(-9.5f,  0.5f, -12f));
        Bathroom      = MakeSpot(parent, "Spot_Bathroom",      new Vector3( 9.5f,  0.5f, -12f));
        LeftDoorSpot  = MakeSpot(parent, "Spot_LeftDoor",      new Vector3(-5.25f, 0.5f,  0f));
        RightDoorSpot = MakeSpot(parent, "Spot_RightDoor",     new Vector3( 5.25f, 0.5f,  0f));
        OfficeCenter  = MakeSpot(parent, "Spot_OfficeCenter",  new Vector3( 0f,    0.5f,  1f));
    }

    GameObject MakeSpot(GameObject parent, string name, Vector3 pos)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = parent.transform;
        obj.transform.position = pos;
        return obj;
    }

    public static void WireAI(EnemyAI ai)
    {
        // The classroom GameObject already exists as a room with that name.
        // Use FindWithTag or just use the marker spots we created
        if (Corridor != null)      ai.corridor      = Corridor.transform;
        if (LeftAlcove != null)    ai.leftAlcove    = LeftAlcove.transform;
        if (RightAlcove != null)   ai.rightAlcove   = RightAlcove.transform;
        if (Stage != null)         ai.stage         = Stage.transform;
        if (Classroom != null)     ai.classroom     = Classroom.transform;
        if (Bathroom != null)      ai.bathroom      = Bathroom.transform;
        if (LeftDoorSpot != null)  ai.leftDoorSpot  = LeftDoorSpot.transform;
        if (RightDoorSpot != null) ai.rightDoorSpot = RightDoorSpot.transform;
        if (OfficeCenter != null)  ai.officeCenter  = OfficeCenter.transform;
    }

    void CreateMaterials()
    {
        wallMat = MakeMat(new Color(0.18f, 0.16f, 0.22f));
        floorMat = MakeMat(new Color(0.08f, 0.08f, 0.1f));
        ceilingMat = MakeMat(new Color(0.06f, 0.06f, 0.08f));
        doorMat = MakeMat(new Color(0.35f, 0.1f, 0.1f));
        darkMat = MakeMat(new Color(0.04f, 0.04f, 0.06f));
        tileMat = MakeMat(new Color(0.12f, 0.12f, 0.16f));
        doorMetalMat = MakeMat(new Color(0.25f, 0.25f, 0.28f));
        doorWarnMat = MakeMat(new Color(0.8f, 0.6f, 0.1f));
    }

    Material MakeMat(Color c)
    {
        Material m = new Material(Shader.Find("Standard"));
        m.color = c;
        m.SetFloat("_Glossiness", 0.15f);
        return m;
    }

    GameObject MakeBox(Transform parent, string n, Vector3 pos, Vector3 scale, Material mat)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.name = n;
        if (parent != null) obj.transform.parent = parent;
        obj.transform.localPosition = pos;
        obj.transform.localScale = scale;
        obj.GetComponent<Renderer>().material = mat;
        return obj;
    }

    void BuildOffice()
    {
        GameObject office = new GameObject("SecurityOffice");
        float roomW = 8f, roomD = 5f, roomH = 3.5f, wallT = 0.2f;
        float doorGap = 1.5f;

        MakeBox(office.transform, "Floor", Vector3.zero, new Vector3(roomW, 0.1f, roomD), floorMat);
        MakeBox(office.transform, "Ceiling", new Vector3(0, roomH, 0), new Vector3(roomW, 0.1f, roomD), ceilingMat);
        MakeBox(office.transform, "BackWall", new Vector3(0, roomH/2, -roomD/2), new Vector3(roomW, roomH, wallT), wallMat);
        MakeBox(office.transform, "FrontWall", new Vector3(0, roomH/2, roomD/2), new Vector3(roomW, roomH, wallT), wallMat);

        // Side walls with door gap AND viewing window (window at y=1.4 to y=2.0)
        BuildWallWithWindow(office.transform, "LeftWall", -roomW/2, roomH, wallT, doorGap, roomD);
        BuildWallWithWindow(office.transform, "RightWall", roomW/2, roomH, wallT, doorGap, roomD);

        // Bridge floors
        MakeBox(office.transform, "LeftBridge", new Vector3(-roomW/2, 0, 0), new Vector3(1.5f, 0.1f, doorGap*2), floorMat);
        MakeBox(office.transform, "RightBridge", new Vector3(roomW/2, 0, 0), new Vector3(1.5f, 0.1f, doorGap*2), floorMat);

        // Desk
        MakeBox(office.transform, "Desk", new Vector3(0, 0.75f, -0.2f), new Vector3(5f, 0.08f, 1.4f), darkMat);
        var deskFront = MakeBox(office.transform, "DeskFront", new Vector3(0, 0.38f, 0.5f), new Vector3(5f, 0.75f, 0.06f), darkMat);
        Object.DestroyImmediate(deskFront.GetComponent<Collider>());

        // Monitors
        for (int i = -1; i <= 1; i++)
        {
            MakeBox(office.transform, "Monitor" + (i + 2), new Vector3(i * 1.2f, 1.15f, -0.5f), new Vector3(0.9f, 0.6f, 0.05f), darkMat);
            GameObject screen = MakeBox(office.transform, "Screen" + (i + 2), new Vector3(i * 1.2f, 1.15f, -0.48f), new Vector3(0.8f, 0.5f, 0.01f), darkMat);
            Material screenMat = new Material(Shader.Find("Standard"));
            screenMat.color = new Color(0.05f, 0.15f, 0.05f);
            screenMat.EnableKeyword("_EMISSION");
            screenMat.SetColor("_EmissionColor", new Color(0.02f, 0.06f, 0.02f));
            screen.GetComponent<Renderer>().material = screenMat;
        }

        // Chair
        MakeBox(office.transform, "ChairSeat", new Vector3(0, 0.45f, 1.3f), new Vector3(0.7f, 0.06f, 0.7f), doorMat);
        MakeBox(office.transform, "ChairBack", new Vector3(0, 0.8f, 1.65f), new Vector3(0.7f, 0.7f, 0.06f), doorMat);
    }

    void BuildHallways()
    {
        float hallW = 2.5f, hallH = 3.5f, wallT = 0.2f, h2 = hallH/2f;
        float lx = -5.25f, rx = 5.25f;
        float hallLen = 22f, hallCenter = -9f;

        // Left hallway
        GameObject leftHall = new GameObject("LeftHallway");
        MakeBox(leftHall.transform, "Floor", new Vector3(lx, 0, hallCenter), new Vector3(hallW, 0.1f, hallLen), floorMat);
        MakeBox(leftHall.transform, "Ceiling", new Vector3(lx, hallH, hallCenter), new Vector3(hallW, 0.1f, hallLen), ceilingMat);
        MakeBox(leftHall.transform, "OuterWall_A", new Vector3(lx - hallW/2, h2, -2f), new Vector3(wallT, hallH, 8f), wallMat);
        MakeBox(leftHall.transform, "OuterWall_B", new Vector3(lx - hallW/2, h2, -17.5f), new Vector3(wallT, hallH, 5f), wallMat);
        MakeBox(leftHall.transform, "InnerWall_Front", new Vector3(lx + hallW/2, h2, 2.75f), new Vector3(wallT, hallH, 2.5f), wallMat);
        MakeBox(leftHall.transform, "InnerWall_Back", new Vector3(lx + hallW/2, h2, -10f), new Vector3(wallT, hallH, 14f), wallMat);
        BuildAlcove(leftHall.transform, "LeftAlcove", new Vector3(lx - hallW/2 - 1f, 0, -7f), 2f, 2f, hallH, true);

        // Right hallway
        GameObject rightHall = new GameObject("RightHallway");
        MakeBox(rightHall.transform, "Floor", new Vector3(rx, 0, hallCenter), new Vector3(hallW, 0.1f, hallLen), floorMat);
        MakeBox(rightHall.transform, "Ceiling", new Vector3(rx, hallH, hallCenter), new Vector3(hallW, 0.1f, hallLen), ceilingMat);
        MakeBox(rightHall.transform, "OuterWall_A", new Vector3(rx + hallW/2, h2, -2f), new Vector3(wallT, hallH, 8f), wallMat);
        MakeBox(rightHall.transform, "OuterWall_B", new Vector3(rx + hallW/2, h2, -17.5f), new Vector3(wallT, hallH, 5f), wallMat);
        MakeBox(rightHall.transform, "InnerWall_Front", new Vector3(rx - hallW/2, h2, 2.75f), new Vector3(wallT, hallH, 2.5f), wallMat);
        MakeBox(rightHall.transform, "InnerWall_Back", new Vector3(rx - hallW/2, h2, -10f), new Vector3(wallT, hallH, 14f), wallMat);
        BuildAlcove(rightHall.transform, "RightAlcove", new Vector3(rx + hallW/2 + 1f, 0, -7f), 2f, 2f, hallH, false);

        // Back corridor
        GameObject corridor = new GameObject("BackCorridor");
        float corrZ = -19f;
        MakeBox(corridor.transform, "Floor", new Vector3(0, 0, corrZ), new Vector3(18f, 0.1f, 3.5f), floorMat);
        MakeBox(corridor.transform, "Ceiling", new Vector3(0, hallH, corrZ), new Vector3(18f, 0.1f, 3.5f), ceilingMat);
        MakeBox(corridor.transform, "FarWall_L", new Vector3(-5.5f, h2, corrZ - 1.75f), new Vector3(7f, hallH, wallT), wallMat);
        MakeBox(corridor.transform, "FarWall_R", new Vector3(5.5f, h2, corrZ - 1.75f), new Vector3(7f, hallH, wallT), wallMat);
        MakeBox(corridor.transform, "LeftWall", new Vector3(-9f, h2, corrZ), new Vector3(wallT, hallH, 3.5f), wallMat);
        MakeBox(corridor.transform, "RightWall", new Vector3(9f, h2, corrZ), new Vector3(wallT, hallH, 3.5f), wallMat);

        // Stage room
        GameObject stage = new GameObject("StageRoom");
        float stageZ = -23f;
        MakeBox(stage.transform, "Floor", new Vector3(0, 0, stageZ), new Vector3(8f, 0.1f, 5f), tileMat);
        MakeBox(stage.transform, "Ceiling", new Vector3(0, hallH, stageZ), new Vector3(8f, 0.1f, 5f), ceilingMat);
        MakeBox(stage.transform, "BackWall", new Vector3(0, h2, stageZ - 2.5f), new Vector3(8f, hallH, wallT), wallMat);
        MakeBox(stage.transform, "LeftWall", new Vector3(-4f, h2, stageZ), new Vector3(wallT, hallH, 5f), wallMat);
        MakeBox(stage.transform, "RightWall", new Vector3(4f, h2, stageZ), new Vector3(wallT, hallH, 5f), wallMat);
        MakeBox(stage.transform, "Platform", new Vector3(0, 0.2f, stageZ - 1.5f), new Vector3(5f, 0.4f, 1.5f), darkMat);

        // Classroom (off the left hallway, opens at z=-12)
        GameObject classroom = new GameObject("Classroom");
        float classZ = -12f;
        MakeBox(classroom.transform, "Floor", new Vector3(-9.5f, 0, classZ), new Vector3(5f, 0.1f, 5f), floorMat);
        MakeBox(classroom.transform, "Ceiling", new Vector3(-9.5f, hallH, classZ), new Vector3(5f, 0.1f, 5f), ceilingMat);
        MakeBox(classroom.transform, "FarWall", new Vector3(-12f, h2, classZ), new Vector3(wallT, hallH, 5f), wallMat);
        MakeBox(classroom.transform, "BackWall", new Vector3(-9.5f, h2, classZ - 2.5f), new Vector3(5f, hallH, wallT), wallMat);
        MakeBox(classroom.transform, "FrontWall", new Vector3(-9.5f, h2, classZ + 2.5f), new Vector3(5f, hallH, wallT), wallMat);
        // Desks (visual)
        for (int i = 0; i < 3; i++)
            MakeBox(classroom.transform, "Desk" + i, new Vector3(-10.5f + i * 1.0f, 0.6f, classZ), new Vector3(0.8f, 0.08f, 0.6f), darkMat);
        MakeBox(classroom.transform, "Chalkboard", new Vector3(-11.85f, 1.8f, classZ), new Vector3(0.05f, 1.4f, 3f), darkMat);

        // Bathroom (off the right hallway, opens at z=-12)
        GameObject bathroom = new GameObject("Bathroom");
        float bathZ = -12f;
        Material whiteMat = new Material(Shader.Find("Standard"));
        whiteMat.color = new Color(0.85f, 0.85f, 0.88f);
        MakeBox(bathroom.transform, "Floor", new Vector3(9.5f, 0, bathZ), new Vector3(5f, 0.1f, 5f), tileMat);
        MakeBox(bathroom.transform, "Ceiling", new Vector3(9.5f, hallH, bathZ), new Vector3(5f, 0.1f, 5f), ceilingMat);
        MakeBox(bathroom.transform, "FarWall", new Vector3(12f, h2, bathZ), new Vector3(wallT, hallH, 5f), wallMat);
        MakeBox(bathroom.transform, "BackWall", new Vector3(9.5f, h2, bathZ - 2.5f), new Vector3(5f, hallH, wallT), wallMat);
        MakeBox(bathroom.transform, "FrontWall", new Vector3(9.5f, h2, bathZ + 2.5f), new Vector3(5f, hallH, wallT), wallMat);
        // Stalls and sinks (visual)
        for (int i = 0; i < 3; i++)
            MakeBox(bathroom.transform, "StallWall" + i, new Vector3(8.5f + i * 1.0f, h2 - 0.3f, bathZ - 1f), new Vector3(0.05f, 2.2f, 1.5f), whiteMat);
        for (int i = 0; i < 2; i++)
            MakeBox(bathroom.transform, "Sink" + i, new Vector3(10.5f + i * 1.0f, 0.9f, bathZ + 2.3f), new Vector3(0.7f, 0.2f, 0.4f), whiteMat);

        // Side openings in left/right hallways so the AI can reach the rooms
        // (we replace the alcove walls' OuterWall to leave a 2-unit gap at z=-12)
        // Already handled because OuterWall_A covers z=-2 (range -6 to 2) and OuterWall_B covers z=-16 (range -20 to -12).
        // There's a natural gap from z=-12 to z=-12 -- wait, that's nothing. Let me add explicit floor bridges.
        MakeBox(classroom.transform, "BridgeFloor", new Vector3(-7f, 0, classZ), new Vector3(2f, 0.1f, 2.5f), floorMat);
        MakeBox(bathroom.transform, "BridgeFloor", new Vector3(7f, 0, bathZ), new Vector3(2f, 0.1f, 2.5f), floorMat);
    }

    void BuildWallWithWindow(Transform parent, string name, float x, float roomH, float wallT, float doorGap, float roomD)
    {
        // Window opening from y=1.4 to y=2.1 (height 0.7)
        float winBot = 1.4f;
        float winTop = 2.1f;

        // Front part (positive Z side of door): split into bottom, window section, top
        float frontZ = doorGap + 0.5f;
        float frontDepth = roomD/2 - doorGap;

        // Bottom strip (full)
        MakeBox(parent, name + "_FrontBottom", new Vector3(x, winBot/2, frontZ), new Vector3(wallT, winBot, frontDepth), wallMat);
        // Top strip (full)
        MakeBox(parent, name + "_FrontTop", new Vector3(x, (winTop + roomH)/2, frontZ), new Vector3(wallT, roomH - winTop, frontDepth), wallMat);
        // Frame on outer side (away from door) - keeps the window from being too wide
        // (no extra frame needed - the section near the door already extends to roomD/2)

        // Back part (negative Z side of door): same split
        float backZ = -(doorGap + 0.5f);
        MakeBox(parent, name + "_BackBottom", new Vector3(x, winBot/2, backZ), new Vector3(wallT, winBot, frontDepth), wallMat);
        MakeBox(parent, name + "_BackTop", new Vector3(x, (winTop + roomH)/2, backZ), new Vector3(wallT, roomH - winTop, frontDepth), wallMat);

        // Window glass (subtle blue tint, see-through visually)
        Material glassMat = new Material(Shader.Find("Standard"));
        glassMat.color = new Color(0.4f, 0.5f, 0.6f, 0.15f);
        glassMat.SetFloat("_Mode", 3); // transparent
        glassMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        glassMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        glassMat.SetInt("_ZWrite", 0);
        glassMat.DisableKeyword("_ALPHATEST_ON");
        glassMat.EnableKeyword("_ALPHABLEND_ON");
        glassMat.renderQueue = 3000;

        var glassFront = MakeBox(parent, name + "_GlassFront", new Vector3(x, (winBot + winTop)/2, frontZ), new Vector3(0.05f, winTop - winBot, frontDepth), glassMat);
        Object.DestroyImmediate(glassFront.GetComponent<Collider>());
        var glassBack = MakeBox(parent, name + "_GlassBack", new Vector3(x, (winBot + winTop)/2, backZ), new Vector3(0.05f, winTop - winBot, frontDepth), glassMat);
        Object.DestroyImmediate(glassBack.GetComponent<Collider>());
    }

    void BuildAlcove(Transform parent, string name, Vector3 pos, float width, float depth, float height, bool leftSide)
    {
        MakeBox(parent, name + "_Floor", pos, new Vector3(depth, 0.1f, width), floorMat);
        MakeBox(parent, name + "_Ceiling", pos + new Vector3(0, height, 0), new Vector3(depth, 0.1f, width), ceilingMat);
        float side = leftSide ? -1f : 1f;
        MakeBox(parent, name + "_BackWall", pos + new Vector3(side * depth/2, height/2, 0), new Vector3(0.2f, height, width), wallMat);
        MakeBox(parent, name + "_SideA", pos + new Vector3(0, height/2, width/2), new Vector3(depth, height, 0.2f), wallMat);
        MakeBox(parent, name + "_SideB", pos + new Vector3(0, height/2, -width/2), new Vector3(depth, height, 0.2f), wallMat);
    }

    void BuildLighting()
    {
        // Remove default directional light
        Light[] allLights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
        foreach (Light l in allLights)
            if (l.type == LightType.Directional)
                Object.DestroyImmediate(l.gameObject);

        AddLight("OfficeLight", new Vector3(0, 3.2f, 0), new Color(1f, 0.9f, 0.75f), 1.2f, 12f);
        AddLight("LeftDoorLight", new Vector3(-3.5f, 2.5f, 0), new Color(1f, 0.2f, 0.2f), 1.5f, 5f);
        AddLight("RightDoorLight", new Vector3(3.5f, 2.5f, 0), new Color(1f, 0.2f, 0.2f), 1.5f, 5f);

        AddLight("LeftHallLight1", new Vector3(-5.25f, 2.5f, -2f), new Color(0.3f, 0.4f, 1f), 1.0f, 8f);
        AddLight("LeftHallLight2", new Vector3(-5.25f, 2.5f, -8f), new Color(0.3f, 0.4f, 1f), 0.7f, 8f);
        AddLight("LeftHallLight3", new Vector3(-5.25f, 2.5f, -14f), new Color(0.3f, 0.4f, 1f), 0.5f, 8f);

        AddLight("RightHallLight1", new Vector3(5.25f, 2.5f, -2f), new Color(0.3f, 0.4f, 1f), 1.0f, 8f);
        AddLight("RightHallLight2", new Vector3(5.25f, 2.5f, -8f), new Color(0.3f, 0.4f, 1f), 0.7f, 8f);
        AddLight("RightHallLight3", new Vector3(5.25f, 2.5f, -14f), new Color(0.3f, 0.4f, 1f), 0.5f, 8f);

        AddLight("CorridorLight", new Vector3(0, 2.5f, -19f), new Color(0.5f, 0.5f, 0.7f), 0.6f, 12f);
        AddLight("StageLight", new Vector3(0, 3f, -23f), new Color(1f, 0.8f, 0.4f), 1.2f, 8f);

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.04f, 0.04f, 0.06f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.01f, 0.01f, 0.02f);
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.025f;

        // Position the player camera
        Camera mainCam = Camera.main;
        if (mainCam == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            mainCam = camObj.AddComponent<Camera>();
            camObj.AddComponent<AudioListener>();
        }
        mainCam.transform.position = new Vector3(0, 1.6f, 1.8f);
        mainCam.transform.rotation = Quaternion.Euler(5, 180, 0);
        mainCam.fieldOfView = 90f;
        mainCam.backgroundColor = new Color(0.005f, 0.005f, 0.01f);
        mainCam.clearFlags = CameraClearFlags.SolidColor;
        mainCam.nearClipPlane = 0.1f;
    }

    void AddLight(string n, Vector3 pos, Color c, float intensity, float range)
    {
        GameObject obj = new GameObject(n);
        obj.transform.position = pos;
        Light light = obj.AddComponent<Light>();
        light.type = LightType.Point;
        light.color = c;
        light.intensity = intensity;
        light.range = range;
        light.shadows = LightShadows.Soft;
    }

    void BuildNavMesh()
    {
        GameObject navObj = new GameObject("NavMeshSurface");
        NavMeshSurface surface = navObj.AddComponent<NavMeshSurface>();
        surface.collectObjects = CollectObjects.All;
        surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        surface.BuildNavMesh();
    }

    void BuildDoors()
    {
        BuildSingleDoor("LeftDoor", new Vector3(-4f, 0, 0), KeyCode.E, 1f);
        BuildSingleDoor("RightDoor", new Vector3(4f, 0, 0), KeyCode.Q, -1f);
    }

    void BuildSingleDoor(string name, Vector3 pos, KeyCode key, float facing)
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

        DoorController dc = door.AddComponent<DoorController>();
        dc.toggleKey = key;
        dc.isClosed = false;

        NavMeshObstacle obs = door.AddComponent<NavMeshObstacle>();
        obs.carving = true;
        obs.size = new Vector3(0.5f, 3f, 3f);
        obs.center = new Vector3(0, 1.5f, 0);
        obs.enabled = false;
        dc.doorBlocker = obs;
    }

    public EnemyAI BuildClownEnemy(Vector3 position)
    {
        Material whiteSkin = new Material(Shader.Find("Standard"));
        whiteSkin.color = new Color(0.95f, 0.92f, 0.88f);

        Material redMat = new Material(Shader.Find("Standard"));
        redMat.color = new Color(0.85f, 0.1f, 0.15f);

        Material yellowMat = new Material(Shader.Find("Standard"));
        yellowMat.color = new Color(1f, 0.85f, 0.1f);

        Material purpleMat = new Material(Shader.Find("Standard"));
        purpleMat.color = new Color(0.4f, 0.1f, 0.5f);

        Material blackMat = new Material(Shader.Find("Standard"));
        blackMat.color = new Color(0.05f, 0.05f, 0.05f);

        // Body (purple jumpsuit)
        GameObject clown = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        clown.name = "Clown";
        clown.transform.position = position;
        clown.transform.localScale = new Vector3(1.0f, 1.0f, 0.85f);
        clown.GetComponent<Renderer>().material = purpleMat;

        // Yellow stripes/buttons on body
        for (int i = 0; i < 3; i++)
        {
            GameObject button = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            button.name = "Button" + i;
            button.transform.parent = clown.transform;
            button.transform.localPosition = new Vector3(0, 0.3f - i * 0.3f, 0.45f);
            button.transform.localScale = new Vector3(0.18f, 0.18f, 0.1f);
            button.GetComponent<Renderer>().material = yellowMat;
            Object.DestroyImmediate(button.GetComponent<Collider>());
        }

        // Ruffled collar (white frilly disc)
        GameObject collar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        collar.name = "Collar";
        collar.transform.parent = clown.transform;
        collar.transform.localPosition = new Vector3(0, 0.55f, 0);
        collar.transform.localScale = new Vector3(0.85f, 0.08f, 0.85f);
        collar.GetComponent<Renderer>().material = whiteSkin;
        Object.DestroyImmediate(collar.GetComponent<Collider>());

        // Head (white face)
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        head.name = "Head";
        head.transform.parent = clown.transform;
        head.transform.localPosition = new Vector3(0, 0.95f, 0);
        head.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        head.GetComponent<Renderer>().material = whiteSkin;
        Object.DestroyImmediate(head.GetComponent<Collider>());

        // Big red nose
        GameObject nose = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        nose.name = "Nose";
        nose.transform.parent = head.transform;
        nose.transform.localPosition = new Vector3(0, 0, 0.55f);
        nose.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        nose.GetComponent<Renderer>().material = redMat;
        Object.DestroyImmediate(nose.GetComponent<Collider>());

        // Creepy wide smile (red)
        GameObject mouth = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mouth.name = "Mouth";
        mouth.transform.parent = head.transform;
        mouth.transform.localPosition = new Vector3(0, -0.3f, 0.42f);
        mouth.transform.localScale = new Vector3(0.6f, 0.12f, 0.1f);
        mouth.GetComponent<Renderer>().material = redMat;
        Object.DestroyImmediate(mouth.GetComponent<Collider>());

        // Teeth in mouth
        for (int i = -2; i <= 2; i++)
        {
            GameObject tooth = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tooth.name = "Tooth" + i;
            tooth.transform.parent = mouth.transform;
            tooth.transform.localPosition = new Vector3(i * 0.18f, 0.2f, 0.5f);
            tooth.transform.localScale = new Vector3(0.13f, 0.6f, 0.4f);
            tooth.GetComponent<Renderer>().material = whiteSkin;
            Object.DestroyImmediate(tooth.GetComponent<Collider>());
        }

        // Eyes - white spheres
        for (int i = 0; i < 2; i++)
        {
            GameObject eye = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            eye.name = "Eye" + i;
            eye.transform.parent = head.transform;
            float xPos = (i == 0) ? -0.22f : 0.22f;
            eye.transform.localPosition = new Vector3(xPos, 0.15f, 0.42f);
            eye.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
            eye.GetComponent<Renderer>().material = whiteSkin;
            Object.DestroyImmediate(eye.GetComponent<Collider>());

            // Black X-shaped pupils (using two crossed cubes)
            for (int j = 0; j < 2; j++)
            {
                GameObject xPart = GameObject.CreatePrimitive(PrimitiveType.Cube);
                xPart.name = "XPart" + j;
                xPart.transform.parent = eye.transform;
                xPart.transform.localPosition = new Vector3(0, 0, 0.45f);
                xPart.transform.localRotation = Quaternion.Euler(0, 0, j * 90f + 45f);
                xPart.transform.localScale = new Vector3(0.7f, 0.15f, 0.15f);
                xPart.GetComponent<Renderer>().material = blackMat;
                Object.DestroyImmediate(xPart.GetComponent<Collider>());
            }
        }

        // Painted red triangles around eyes
        for (int i = 0; i < 2; i++)
        {
            float xPos = (i == 0) ? -0.22f : 0.22f;
            GameObject paint = GameObject.CreatePrimitive(PrimitiveType.Cube);
            paint.name = "EyePaint" + i;
            paint.transform.parent = head.transform;
            paint.transform.localPosition = new Vector3(xPos, 0.4f, 0.45f);
            paint.transform.localRotation = Quaternion.Euler(0, 0, 45f);
            paint.transform.localScale = new Vector3(0.15f, 0.15f, 0.05f);
            paint.GetComponent<Renderer>().material = redMat;
            Object.DestroyImmediate(paint.GetComponent<Collider>());
        }

        // Crazy hair tufts (alternating yellow/red on each side)
        for (int side = 0; side < 2; side++)
        {
            for (int i = 0; i < 4; i++)
            {
                GameObject tuft = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                tuft.name = "Hair_" + side + "_" + i;
                tuft.transform.parent = head.transform;
                float baseX = (side == 0) ? -0.5f : 0.5f;
                tuft.transform.localPosition = new Vector3(
                    baseX + Random.Range(-0.15f, 0.15f),
                    0.3f + i * 0.12f,
                    Random.Range(-0.2f, 0.2f)
                );
                tuft.transform.localScale = new Vector3(0.3f, 0.25f, 0.3f);
                tuft.GetComponent<Renderer>().material = (i % 2 == 0) ? redMat : yellowMat;
                Object.DestroyImmediate(tuft.GetComponent<Collider>());
            }
        }

        // Tiny pointy hat
        GameObject hat = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        hat.name = "Hat";
        hat.transform.parent = head.transform;
        hat.transform.localPosition = new Vector3(0, 0.6f, -0.05f);
        hat.transform.localScale = new Vector3(0.15f, 0.25f, 0.15f);
        hat.GetComponent<Renderer>().material = purpleMat;
        Object.DestroyImmediate(hat.GetComponent<Collider>());

        // Glow light (creepy purple)
        GameObject glow = new GameObject("ClownGlow");
        glow.transform.parent = clown.transform;
        glow.transform.localPosition = new Vector3(0, 0.5f, 0);
        Light gl = glow.AddComponent<Light>();
        gl.type = LightType.Point;
        gl.color = new Color(0.9f, 0.2f, 0.9f);
        gl.intensity = 1.2f;
        gl.range = 5f;

        // NavMesh + AI setup (mirror the Freddy enemy)
        NavMeshAgent agent = clown.AddComponent<NavMeshAgent>();
        agent.speed = 5f;
        agent.stoppingDistance = 0.5f;
        agent.radius = 0.4f;
        agent.height = 2f;

        EnemyAI ai = clown.AddComponent<EnemyAI>();
        WireAI(ai);

        DoorController[] doors = Object.FindObjectsByType<DoorController>(FindObjectsSortMode.None);
        foreach (var d in doors)
        {
            if (d.transform.position.x < 0) ai.leftDoor = d;
            if (d.transform.position.x > 0) ai.rightDoor = d;
        }

        return ai;
    }

    void BuildEnemy()
    {
        // Brown bear-style materials
        Material furMat = new Material(Shader.Find("Standard"));
        furMat.color = new Color(0.32f, 0.18f, 0.1f);
        furMat.SetFloat("_Glossiness", 0.05f);

        Material snoutMat = new Material(Shader.Find("Standard"));
        snoutMat.color = new Color(0.55f, 0.4f, 0.25f);
        snoutMat.SetFloat("_Glossiness", 0.1f);

        Material hatMat = new Material(Shader.Find("Standard"));
        hatMat.color = new Color(0.05f, 0.05f, 0.05f);
        hatMat.SetFloat("_Glossiness", 0.3f);

        Material bowMat = new Material(Shader.Find("Standard"));
        bowMat.color = new Color(0.05f, 0.05f, 0.05f);

        Material whiteMat = new Material(Shader.Find("Standard"));
        whiteMat.color = new Color(0.9f, 0.9f, 0.85f);

        // Body (chubbier capsule)
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        enemy.name = "Animatronic";
        enemy.transform.position = new Vector3(-5.25f, 1, -15f);
        enemy.transform.localScale = new Vector3(1.1f, 1.0f, 0.85f);
        enemy.GetComponent<Renderer>().material = furMat;

        // Belly patch (lighter color, in front)
        GameObject belly = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        belly.name = "Belly";
        belly.transform.parent = enemy.transform;
        belly.transform.localPosition = new Vector3(0, -0.1f, 0.45f);
        belly.transform.localScale = new Vector3(0.7f, 0.65f, 0.4f);
        belly.GetComponent<Renderer>().material = snoutMat;
        Object.DestroyImmediate(belly.GetComponent<Collider>());

        // Bow tie (under chin)
        GameObject bow = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bow.name = "BowTie";
        bow.transform.parent = enemy.transform;
        bow.transform.localPosition = new Vector3(0, 0.4f, 0.5f);
        bow.transform.localScale = new Vector3(0.5f, 0.18f, 0.1f);
        bow.GetComponent<Renderer>().material = bowMat;
        Object.DestroyImmediate(bow.GetComponent<Collider>());

        // Head (rounder, bigger)
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        head.name = "Head";
        head.transform.parent = enemy.transform;
        head.transform.localPosition = new Vector3(0, 0.95f, 0.05f);
        head.transform.localScale = new Vector3(0.85f, 0.75f, 0.85f);
        head.GetComponent<Renderer>().material = furMat;
        Object.DestroyImmediate(head.GetComponent<Collider>());

        // Snout (lighter color, protrudes)
        GameObject snout = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        snout.name = "Snout";
        snout.transform.parent = head.transform;
        snout.transform.localPosition = new Vector3(0, -0.15f, 0.55f);
        snout.transform.localScale = new Vector3(0.55f, 0.5f, 0.45f);
        snout.GetComponent<Renderer>().material = snoutMat;
        Object.DestroyImmediate(snout.GetComponent<Collider>());

        // Nose (black, on snout)
        GameObject nose = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        nose.name = "Nose";
        nose.transform.parent = snout.transform;
        nose.transform.localPosition = new Vector3(0, 0.25f, 0.6f);
        nose.transform.localScale = new Vector3(0.35f, 0.25f, 0.35f);
        nose.GetComponent<Renderer>().material = bowMat;
        Object.DestroyImmediate(nose.GetComponent<Collider>());

        // Mouth (open, dark)
        GameObject mouth = GameObject.CreatePrimitive(PrimitiveType.Cube);
        mouth.name = "Mouth";
        mouth.transform.parent = snout.transform;
        mouth.transform.localPosition = new Vector3(0, -0.4f, 0.4f);
        mouth.transform.localScale = new Vector3(0.7f, 0.3f, 0.3f);
        Material mouthMat = new Material(Shader.Find("Standard"));
        mouthMat.color = new Color(0.05f, 0.02f, 0.02f);
        mouth.GetComponent<Renderer>().material = mouthMat;
        Object.DestroyImmediate(mouth.GetComponent<Collider>());

        // Teeth (top row)
        for (int i = -2; i <= 2; i++)
        {
            GameObject tooth = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tooth.name = "Tooth" + i;
            tooth.transform.parent = mouth.transform;
            tooth.transform.localPosition = new Vector3(i * 0.18f, 0.3f, 0f);
            tooth.transform.localScale = new Vector3(0.13f, 0.5f, 0.5f);
            tooth.GetComponent<Renderer>().material = whiteMat;
            Object.DestroyImmediate(tooth.GetComponent<Collider>());
        }

        // Ears (rounder, on top of head)
        for (int i = 0; i < 2; i++)
        {
            GameObject ear = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ear.name = "Ear" + i;
            ear.transform.parent = head.transform;
            float xPos = (i == 0) ? -0.45f : 0.45f;
            ear.transform.localPosition = new Vector3(xPos, 0.55f, 0);
            ear.transform.localScale = new Vector3(0.35f, 0.35f, 0.3f);
            ear.GetComponent<Renderer>().material = furMat;
            Object.DestroyImmediate(ear.GetComponent<Collider>());

            // Ear inner (lighter)
            GameObject earInner = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            earInner.name = "EarInner" + i;
            earInner.transform.parent = ear.transform;
            earInner.transform.localPosition = new Vector3(0, 0, -0.1f);
            earInner.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            earInner.GetComponent<Renderer>().material = snoutMat;
            Object.DestroyImmediate(earInner.GetComponent<Collider>());
        }

        // Cheeks (round tufts on lower side of head)
        for (int i = 0; i < 2; i++)
        {
            GameObject cheek = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cheek.name = "Cheek" + i;
            cheek.transform.parent = head.transform;
            float xPos = (i == 0) ? -0.45f : 0.45f;
            cheek.transform.localPosition = new Vector3(xPos, -0.15f, 0.25f);
            cheek.transform.localScale = new Vector3(0.3f, 0.25f, 0.3f);
            cheek.GetComponent<Renderer>().material = furMat;
            Object.DestroyImmediate(cheek.GetComponent<Collider>());
        }

        // Eyebrows (thick black bars angled menacingly)
        for (int i = 0; i < 2; i++)
        {
            GameObject brow = GameObject.CreatePrimitive(PrimitiveType.Cube);
            brow.name = "Brow" + i;
            brow.transform.parent = head.transform;
            float xPos = (i == 0) ? -0.22f : 0.22f;
            brow.transform.localPosition = new Vector3(xPos, 0.32f, 0.45f);
            brow.transform.localRotation = Quaternion.Euler(0, 0, (i == 0) ? -20f : 20f);
            brow.transform.localScale = new Vector3(0.28f, 0.07f, 0.05f);
            brow.GetComponent<Renderer>().material = bowMat;
            Object.DestroyImmediate(brow.GetComponent<Collider>());
        }

        // Top hat with gold band
        GameObject hatBrim = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        hatBrim.name = "HatBrim";
        hatBrim.transform.parent = head.transform;
        hatBrim.transform.localPosition = new Vector3(0, 0.65f, -0.05f);
        hatBrim.transform.localScale = new Vector3(0.55f, 0.04f, 0.55f);
        hatBrim.GetComponent<Renderer>().material = hatMat;
        Object.DestroyImmediate(hatBrim.GetComponent<Collider>());

        GameObject hatTop = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        hatTop.name = "HatTop";
        hatTop.transform.parent = head.transform;
        hatTop.transform.localPosition = new Vector3(0, 0.85f, -0.05f);
        hatTop.transform.localScale = new Vector3(0.4f, 0.2f, 0.4f);
        hatTop.GetComponent<Renderer>().material = hatMat;
        Object.DestroyImmediate(hatTop.GetComponent<Collider>());

        // Gold hat band
        Material goldMat = new Material(Shader.Find("Standard"));
        goldMat.color = new Color(0.85f, 0.65f, 0.15f);
        goldMat.SetFloat("_Glossiness", 0.6f);
        GameObject hatBand = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        hatBand.name = "HatBand";
        hatBand.transform.parent = head.transform;
        hatBand.transform.localPosition = new Vector3(0, 0.7f, -0.05f);
        hatBand.transform.localScale = new Vector3(0.42f, 0.04f, 0.42f);
        hatBand.GetComponent<Renderer>().material = goldMat;
        Object.DestroyImmediate(hatBand.GetComponent<Collider>());

        // Arms (rounded shoulders + forearms hanging down)
        for (int i = 0; i < 2; i++)
        {
            float side = (i == 0) ? -1f : 1f;

            // Shoulder
            GameObject shoulder = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            shoulder.name = "Shoulder" + i;
            shoulder.transform.parent = enemy.transform;
            shoulder.transform.localPosition = new Vector3(side * 0.55f, 0.35f, 0);
            shoulder.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            shoulder.GetComponent<Renderer>().material = furMat;
            Object.DestroyImmediate(shoulder.GetComponent<Collider>());

            // Upper arm (capsule)
            GameObject upperArm = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            upperArm.name = "UpperArm" + i;
            upperArm.transform.parent = enemy.transform;
            upperArm.transform.localPosition = new Vector3(side * 0.6f, -0.05f, 0);
            upperArm.transform.localScale = new Vector3(0.25f, 0.4f, 0.25f);
            upperArm.GetComponent<Renderer>().material = furMat;
            Object.DestroyImmediate(upperArm.GetComponent<Collider>());

            // Hand (sphere)
            GameObject hand = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            hand.name = "Hand" + i;
            hand.transform.parent = enemy.transform;
            hand.transform.localPosition = new Vector3(side * 0.65f, -0.55f, 0.05f);
            hand.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            hand.GetComponent<Renderer>().material = furMat;
            Object.DestroyImmediate(hand.GetComponent<Collider>());
        }

        // Microphone in right hand
        GameObject micHandle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        micHandle.name = "MicHandle";
        micHandle.transform.parent = enemy.transform;
        micHandle.transform.localPosition = new Vector3(0.65f, -0.7f, 0.05f);
        micHandle.transform.localScale = new Vector3(0.05f, 0.15f, 0.05f);
        micHandle.GetComponent<Renderer>().material = bowMat;
        Object.DestroyImmediate(micHandle.GetComponent<Collider>());

        GameObject micHead = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        micHead.name = "MicHead";
        micHead.transform.parent = enemy.transform;
        micHead.transform.localPosition = new Vector3(0.65f, -0.5f, 0.05f);
        micHead.transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
        Material micMat = new Material(Shader.Find("Standard"));
        micMat.color = new Color(0.4f, 0.4f, 0.45f);
        micMat.SetFloat("_Glossiness", 0.7f);
        micHead.GetComponent<Renderer>().material = micMat;
        Object.DestroyImmediate(micHead.GetComponent<Collider>());

        // Bolts on the side of head (animatronic feel)
        for (int i = 0; i < 2; i++)
        {
            float side = (i == 0) ? -1f : 1f;
            GameObject bolt = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            bolt.name = "Bolt" + i;
            bolt.transform.parent = head.transform;
            bolt.transform.localPosition = new Vector3(side * 0.62f, 0, 0);
            bolt.transform.localRotation = Quaternion.Euler(0, 0, 90);
            bolt.transform.localScale = new Vector3(0.06f, 0.04f, 0.06f);
            bolt.GetComponent<Renderer>().material = micMat;
            Object.DestroyImmediate(bolt.GetComponent<Collider>());
        }

        // Eyes (white sclera + red glowing pupil)
        for (int i = 0; i < 2; i++)
        {
            GameObject eyeWhite = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            eyeWhite.name = "EyeWhite" + i;
            eyeWhite.transform.parent = head.transform;
            float xPos = (i == 0) ? -0.22f : 0.22f;
            eyeWhite.transform.localPosition = new Vector3(xPos, 0.1f, 0.45f);
            eyeWhite.transform.localScale = new Vector3(0.22f, 0.22f, 0.22f);
            eyeWhite.GetComponent<Renderer>().material = whiteMat;
            Object.DestroyImmediate(eyeWhite.GetComponent<Collider>());

            GameObject pupil = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pupil.name = "Pupil" + i;
            pupil.transform.parent = eyeWhite.transform;
            pupil.transform.localPosition = new Vector3(0, 0, 0.6f);
            pupil.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Material pupilMat = new Material(Shader.Find("Standard"));
            pupilMat.color = Color.red;
            pupilMat.EnableKeyword("_EMISSION");
            pupilMat.SetColor("_EmissionColor", Color.red * 4f);
            pupil.GetComponent<Renderer>().material = pupilMat;
            Object.DestroyImmediate(pupil.GetComponent<Collider>());
        }

        // Glow light
        GameObject glow = new GameObject("EnemyGlow");
        glow.transform.parent = enemy.transform;
        glow.transform.localPosition = new Vector3(0, 0.5f, 0);
        Light gl = glow.AddComponent<Light>();
        gl.type = LightType.Point;
        gl.color = new Color(1f, 0.2f, 0.1f);
        gl.intensity = 1.2f;
        gl.range = 5f;

        // NavMesh agent
        NavMeshAgent agent = enemy.AddComponent<NavMeshAgent>();
        agent.speed = 5f;
        agent.stoppingDistance = 0.5f;
        agent.radius = 0.4f;
        agent.height = 2f;

        EnemyAI ai = enemy.AddComponent<EnemyAI>();
        WireAI(ai);

        // Link doors
        DoorController[] doors = Object.FindObjectsByType<DoorController>(FindObjectsSortMode.None);
        foreach (var d in doors)
        {
            if (d.transform.position.x < 0) ai.leftDoor = d;
            if (d.transform.position.x > 0) ai.rightDoor = d;
        }
    }
}
