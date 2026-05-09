using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;
using Unity.AI.Navigation;
using TMPro;

public class SceneBuilder : EditorWindow
{
    // Quick scene switch — see the map without playing
    [MenuItem("Night Shift/View Map")]
    static void ViewMap()
    {
        EditorSceneManager.OpenScene("Assets/scene.unity");
    }

    // Quick scene switch — back to the menu
    [MenuItem("Night Shift/View Menu")]
    static void ViewMenu()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
    }

    // ONE button that builds & saves both scenes (menu + game)
    [MenuItem("Night Shift/Build")]
    static void Build()
    {
        if (!EditorUtility.DisplayDialog("Build Everything",
            "This will rebuild and save BOTH scenes:\n" +
            " • Main Menu (Assets/Scenes/MainMenu.unity)\n" +
            " • Game World (Assets/scene.unity)\n\nContinue?",
            "Build", "Cancel")) return;

        // Step 1: build the main menu and save it
        var menuScene = EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");
        BuildMainMenuScene();
        EditorSceneManager.SaveScene(menuScene);

        // Step 2: build the game scene and save it
        var gameScene = EditorSceneManager.OpenScene("Assets/scene.unity");
        BuildGameScene();
        EditorSceneManager.SaveScene(gameScene);

        // Step 3: leave the user on the menu so they can hit Play
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");

        Debug.Log("Build complete. Hit Play to test.");
    }

    static void BuildMainMenuScene()
    {
        // Clear everything in the scene
        foreach (GameObject obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (obj == null) continue;
            if (obj.transform.parent != null) continue;
            Object.DestroyImmediate(obj);
        }

        // Spawn the SimpleMainMenu — it builds the UI in Start()
        GameObject menu = new GameObject("MainMenu");
        menu.AddComponent<SimpleMainMenu>();
    }

    static void BuildGameScene()
    {
        ClearScene();
        CreateMaterials();
        BuildOffice();
        BuildHallways();
        BuildLighting();
        BuildNavMesh();
        BuildDoors();
        BuildSpots();
        BuildEnemy();
        BuildClown();
        BuildMonitorClock();
        BuildGameManager();
        TestNavMesh();
    }

    static void BuildSpots()
    {
        GameObject parent = new GameObject("AISpots");
        MakeNavSpot(parent, "Spot_Corridor",      new Vector3( 0f,    0.5f, -19f));
        MakeNavSpot(parent, "Spot_LeftAlcove",    new Vector3(-7.25f, 0.5f, -7f));
        MakeNavSpot(parent, "Spot_RightAlcove",   new Vector3( 7.25f, 0.5f, -7f));
        MakeNavSpot(parent, "Spot_Stage",         new Vector3( 0f,    0.5f, -23f));
        MakeNavSpot(parent, "Spot_Classroom",     new Vector3(-9.5f,  0.5f, -12f));
        MakeNavSpot(parent, "Spot_Bathroom",      new Vector3( 9.5f,  0.5f, -12f));
        MakeNavSpot(parent, "Spot_LeftDoor",      new Vector3(-5.25f, 0.5f,  0f));
        MakeNavSpot(parent, "Spot_RightDoor",     new Vector3( 5.25f, 0.5f,  0f));
        MakeNavSpot(parent, "Spot_OfficeCenter",  new Vector3( 0f,    0.5f,  1f));
    }

    static void MakeNavSpot(GameObject parent, string name, Vector3 pos)
    {
        GameObject obj = new GameObject(name);
        obj.transform.parent = parent.transform;
        obj.transform.position = pos;
    }

    static void BuildClown()
    {
        GameObject tmp = new GameObject("TempClownBuilder");
        var builder = tmp.AddComponent<WorldBuilder>();
        builder.BuildClownEnemy(new Vector3(5.25f, 1f, -15f));
        Object.DestroyImmediate(tmp);
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
        string path = "Assets/Art/Materials/Environment/" + matName + ".mat";
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
                   new Vector3(0.7f, 0.9f, 0.02f), "Assets/Art/Textures/poster_rules.png");
        MakePoster(office.transform, "PosterCaution", new Vector3(2.5f, 1.8f, -2.38f),
                   new Vector3(0.6f, 0.6f, 0.02f), "Assets/Art/Textures/poster_caution.png");
        MakePoster(office.transform, "PosterCelebrate", new Vector3(-0.5f, 2.5f, -2.38f),
                   new Vector3(0.5f, 0.65f, 0.02f), "Assets/Art/Textures/poster_celebrate.png");
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
        float hallLen = 22f;     // longer hallways (was 14)
        float hallCenter = -9f;  // center of hallway (was -5)

        // ---- LEFT HALLWAY ----
        GameObject leftHall = new GameObject("LeftHallway");
        MakeBox(leftHall.transform, "Floor", new Vector3(lx, 0, hallCenter), new Vector3(hallW, 0.1f, hallLen), floorMat, true);
        MakeBox(leftHall.transform, "Ceiling", new Vector3(lx, hallH, hallCenter), new Vector3(hallW, 0.1f, hallLen), ceilingMat, true);
        // Outer wall split for an alcove at z=-7 (alcove from z=-8 to z=-6, depth 1.5 outward)
        MakeBox(leftHall.transform, "OuterWall_A", new Vector3(lx - hallW/2, h2, -2f), new Vector3(wallT, hallH, 8f), wallMat, true);
        MakeBox(leftHall.transform, "OuterWall_B", new Vector3(lx - hallW/2, h2, -17.5f), new Vector3(wallT, hallH, 5f), wallMat, true);
        // Inner wall (gap for office door at z=-1.5 to 1.5)
        MakeBox(leftHall.transform, "InnerWall_Front", new Vector3(lx + hallW/2, h2, 2.75f), new Vector3(wallT, hallH, 2.5f), wallMat, true);
        MakeBox(leftHall.transform, "InnerWall_Back", new Vector3(lx + hallW/2, h2, -10f), new Vector3(wallT, hallH, 14f), wallMat, true);

        // Left alcove (hiding spot - small recess in outer wall at z=-7)
        BuildAlcove(leftHall.transform, "LeftAlcove", new Vector3(lx - hallW/2 - 1f, 0, -7f), 2f, 2f, hallH, true);

        // ---- RIGHT HALLWAY ----
        GameObject rightHall = new GameObject("RightHallway");
        MakeBox(rightHall.transform, "Floor", new Vector3(rx, 0, hallCenter), new Vector3(hallW, 0.1f, hallLen), floorMat, true);
        MakeBox(rightHall.transform, "Ceiling", new Vector3(rx, hallH, hallCenter), new Vector3(hallW, 0.1f, hallLen), ceilingMat, true);
        MakeBox(rightHall.transform, "OuterWall_A", new Vector3(rx + hallW/2, h2, -2f), new Vector3(wallT, hallH, 8f), wallMat, true);
        MakeBox(rightHall.transform, "OuterWall_B", new Vector3(rx + hallW/2, h2, -17.5f), new Vector3(wallT, hallH, 5f), wallMat, true);
        MakeBox(rightHall.transform, "InnerWall_Front", new Vector3(rx - hallW/2, h2, 2.75f), new Vector3(wallT, hallH, 2.5f), wallMat, true);
        MakeBox(rightHall.transform, "InnerWall_Back", new Vector3(rx - hallW/2, h2, -10f), new Vector3(wallT, hallH, 14f), wallMat, true);

        BuildAlcove(rightHall.transform, "RightAlcove", new Vector3(rx + hallW/2 + 1f, 0, -7f), 2f, 2f, hallH, false);

        // ---- BACK CORRIDOR (wider, with stage room behind) ----
        GameObject corridor = new GameObject("BackCorridor");
        float corrZ = -19f;
        MakeBox(corridor.transform, "Floor", new Vector3(0, 0, corrZ), new Vector3(18f, 0.1f, 3.5f), floorMat, true);
        MakeBox(corridor.transform, "Ceiling", new Vector3(0, hallH, corrZ), new Vector3(18f, 0.1f, 3.5f), ceilingMat, true);
        // Far wall has a gap in the middle (entry to stage room)
        MakeBox(corridor.transform, "FarWall_L", new Vector3(-5.5f, h2, corrZ - 1.75f), new Vector3(7f, hallH, wallT), wallMat, true);
        MakeBox(corridor.transform, "FarWall_R", new Vector3(5.5f, h2, corrZ - 1.75f), new Vector3(7f, hallH, wallT), wallMat, true);
        // Side walls
        MakeBox(corridor.transform, "LeftWall", new Vector3(-9f, h2, corrZ), new Vector3(wallT, hallH, 3.5f), wallMat, true);
        MakeBox(corridor.transform, "RightWall", new Vector3(9f, h2, corrZ), new Vector3(wallT, hallH, 3.5f), wallMat, true);

        // ---- STAGE ROOM (hiding spot behind corridor) ----
        GameObject stage = new GameObject("StageRoom");
        float stageZ = -23f;
        MakeBox(stage.transform, "Floor", new Vector3(0, 0, stageZ), new Vector3(8f, 0.1f, 5f), tileMat, true);
        MakeBox(stage.transform, "Ceiling", new Vector3(0, hallH, stageZ), new Vector3(8f, 0.1f, 5f), ceilingMat, true);
        MakeBox(stage.transform, "BackWall", new Vector3(0, h2, stageZ - 2.5f), new Vector3(8f, hallH, wallT), wallMat, true);
        MakeBox(stage.transform, "LeftWall", new Vector3(-4f, h2, stageZ), new Vector3(wallT, hallH, 5f), wallMat, true);
        MakeBox(stage.transform, "RightWall", new Vector3(4f, h2, stageZ), new Vector3(wallT, hallH, 5f), wallMat, true);
        // Stage platform
        MakeBox(stage.transform, "Platform", new Vector3(0, 0.2f, stageZ - 1.5f), new Vector3(5f, 0.4f, 1.5f), darkMat, true);
        // Curtains as visual cover
        MakeBox(stage.transform, "CurtainL", new Vector3(-2.5f, h2, stageZ - 0.5f), new Vector3(0.2f, hallH - 0.3f, 0.05f), doorMat, true);
        MakeBox(stage.transform, "CurtainR", new Vector3(2.5f, h2, stageZ - 0.5f), new Vector3(0.2f, hallH - 0.3f, 0.05f), doorMat, true);

        // Stage decoration: colorful "FREDDY'S" sign with emissive letters
        Material[] signColors = {
            EmissiveMat(new Color(1f, 0.2f, 0.2f)),
            EmissiveMat(new Color(1f, 0.8f, 0.1f)),
            EmissiveMat(new Color(0.2f, 0.9f, 0.3f)),
            EmissiveMat(new Color(0.3f, 0.5f, 1f)),
            EmissiveMat(new Color(0.9f, 0.2f, 0.9f)),
            EmissiveMat(new Color(1f, 0.5f, 0.1f)),
            EmissiveMat(new Color(0.4f, 1f, 0.9f))
        };
        // Sign letters as neon tubes (cylinders rotated to face camera)
        for (int i = 0; i < 7; i++)
        {
            GameObject letter = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            letter.name = "SignLetter" + i;
            letter.transform.parent = stage.transform;
            letter.transform.localPosition = new Vector3(-1.8f + i * 0.6f, 2.6f, stageZ - 2.35f);
            letter.transform.localRotation = Quaternion.Euler(90, 0, 0);
            letter.transform.localScale = new Vector3(0.18f, 0.04f, 0.18f);
            letter.GetComponent<Renderer>().material = signColors[i];
            Object.DestroyImmediate(letter.GetComponent<Collider>());

            // Small point light per letter for neon glow
            GameObject neonLight = new GameObject("NeonLight" + i);
            neonLight.transform.parent = letter.transform;
            neonLight.transform.localPosition = Vector3.zero;
            Light nl = neonLight.AddComponent<Light>();
            nl.type = LightType.Point;
            nl.color = signColors[i].color;
            nl.intensity = 0.8f;
            nl.range = 2f;
        }
        // Spotlight on stage
        GameObject spot = new GameObject("StageSpot");
        spot.transform.parent = stage.transform;
        spot.transform.position = new Vector3(0, 3.2f, stageZ - 1.5f);
        spot.transform.rotation = Quaternion.Euler(60, 0, 0);
        Light sl = spot.AddComponent<Light>();
        sl.type = LightType.Spot;
        sl.color = new Color(1f, 0.85f, 0.4f);
        sl.intensity = 4f;
        sl.range = 8f;
        sl.spotAngle = 50f;
        // Drum kit (cylinder)
        Material drumMat = new Material(Shader.Find("Standard"));
        drumMat.color = new Color(0.9f, 0.1f, 0.1f);
        drumMat.SetFloat("_Glossiness", 0.5f);
        GameObject drum = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        drum.name = "Drum";
        drum.transform.parent = stage.transform;
        drum.transform.localPosition = new Vector3(1.5f, 0.7f, stageZ - 1f);
        drum.transform.localScale = new Vector3(0.5f, 0.3f, 0.5f);
        drum.GetComponent<Renderer>().material = drumMat;
        // Microphone stand on stage
        Material micMat = new Material(Shader.Find("Standard"));
        micMat.color = new Color(0.4f, 0.4f, 0.45f);
        micMat.SetFloat("_Glossiness", 0.7f);
        MakeBox(stage.transform, "MicStand", new Vector3(-1.2f, 0.7f, stageZ - 1f), new Vector3(0.05f, 1.4f, 0.05f), micMat, true);
        GameObject mic = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mic.name = "Mic";
        mic.transform.parent = stage.transform;
        mic.transform.localPosition = new Vector3(-1.2f, 1.5f, stageZ - 1f);
        mic.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
        mic.GetComponent<Renderer>().material = micMat;

        // ---- CLASSROOM ----
        Material greenBoardMat = new Material(Shader.Find("Standard"));
        greenBoardMat.color = new Color(0.1f, 0.4f, 0.2f);
        Material woodMat = new Material(Shader.Find("Standard"));
        woodMat.color = new Color(0.5f, 0.32f, 0.18f);
        Material redMat = new Material(Shader.Find("Standard"));
        redMat.color = new Color(0.85f, 0.15f, 0.15f);

        GameObject classroom = new GameObject("Classroom");
        float classZ = -12f;
        MakeBox(classroom.transform, "Floor", new Vector3(-9.5f, 0, classZ), new Vector3(5f, 0.1f, 5f), floorMat, true);
        MakeBox(classroom.transform, "Ceiling", new Vector3(-9.5f, hallH, classZ), new Vector3(5f, 0.1f, 5f), ceilingMat, true);
        MakeBox(classroom.transform, "FarWall", new Vector3(-12f, h2, classZ), new Vector3(wallT, hallH, 5f), wallMat, true);
        MakeBox(classroom.transform, "BackWall", new Vector3(-9.5f, h2, classZ - 2.5f), new Vector3(5f, hallH, wallT), wallMat, true);
        MakeBox(classroom.transform, "FrontWall", new Vector3(-9.5f, h2, classZ + 2.5f), new Vector3(5f, hallH, wallT), wallMat, true);

        // Green chalkboard with white frame
        MakeBox(classroom.transform, "BoardFrame", new Vector3(-11.88f, 1.7f, classZ), new Vector3(0.04f, 1.6f, 3.4f), woodMat, true);
        MakeBox(classroom.transform, "Greenboard", new Vector3(-11.85f, 1.7f, classZ), new Vector3(0.05f, 1.4f, 3f), greenBoardMat, true);

        // Teacher desk (bigger, in front of board)
        MakeBox(classroom.transform, "TeacherDesk", new Vector3(-11f, 0.75f, classZ), new Vector3(0.08f, 1.6f, 1.8f), woodMat, true);
        MakeBox(classroom.transform, "TeacherDeskFront", new Vector3(-10.65f, 0.4f, classZ), new Vector3(0.7f, 0.8f, 1.8f), woodMat, true);
        // Globe on teacher desk
        GameObject globe = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        globe.name = "Globe";
        globe.transform.parent = classroom.transform;
        globe.transform.localPosition = new Vector3(-10.85f, 1f, classZ - 0.6f);
        globe.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        Material globeMat = new Material(Shader.Find("Standard"));
        globeMat.color = new Color(0.2f, 0.5f, 0.9f);
        globe.GetComponent<Renderer>().material = globeMat;
        // Apple on teacher desk
        GameObject apple = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        apple.name = "Apple";
        apple.transform.parent = classroom.transform;
        apple.transform.localPosition = new Vector3(-10.85f, 0.92f, classZ + 0.5f);
        apple.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        apple.GetComponent<Renderer>().material = redMat;

        // Student desks (2 rows of 2) with legs and back-rest chairs
        for (int row = 0; row < 2; row++)
            for (int col = 0; col < 2; col++)
            {
                float x = -9.5f + col * 1.4f;
                float z = classZ - 0.7f + row * 1.4f;
                // Desk top
                MakeBox(classroom.transform, "StuDesk_" + row + "_" + col, new Vector3(x, 0.62f, z), new Vector3(0.8f, 0.06f, 0.5f), woodMat, true);
                // 4 legs
                for (int leg = 0; leg < 4; leg++)
                {
                    float lx2 = (leg % 2 == 0) ? -0.35f : 0.35f;
                    float lz = (leg < 2) ? -0.2f : 0.2f;
                    var legObj = MakeBox(classroom.transform, "DeskLeg_" + row + col + leg, new Vector3(x + lx2, 0.3f, z + lz), new Vector3(0.04f, 0.6f, 0.04f), darkMat, false);
                    Object.DestroyImmediate(legObj.GetComponent<Collider>());
                }
                // Chair seat
                MakeBox(classroom.transform, "StuChairSeat_" + row + col, new Vector3(x, 0.42f, z + 0.55f), new Vector3(0.45f, 0.05f, 0.45f), darkMat, true);
                // Chair back
                MakeBox(classroom.transform, "StuChairBack_" + row + col, new Vector3(x, 0.7f, z + 0.75f), new Vector3(0.45f, 0.5f, 0.04f), darkMat, true);
            }
        MakeBox(classroom.transform, "BridgeFloor", new Vector3(-7f, 0, classZ), new Vector3(2f, 0.1f, 2.5f), floorMat, true);

        // ---- BATHROOM ----
        Material whiteTileMat = new Material(Shader.Find("Standard"));
        whiteTileMat.color = new Color(0.92f, 0.93f, 0.95f);
        whiteTileMat.SetFloat("_Glossiness", 0.4f);
        Material mirrorMat = new Material(Shader.Find("Standard"));
        mirrorMat.color = new Color(0.7f, 0.85f, 0.95f);
        mirrorMat.SetFloat("_Glossiness", 0.95f);
        mirrorMat.SetFloat("_Metallic", 0.9f);

        GameObject bathroom = new GameObject("Bathroom");
        float bathZ = -12f;
        MakeBox(bathroom.transform, "Floor", new Vector3(9.5f, 0, bathZ), new Vector3(5f, 0.1f, 5f), tileMat, true);
        MakeBox(bathroom.transform, "Ceiling", new Vector3(9.5f, hallH, bathZ), new Vector3(5f, 0.1f, 5f), ceilingMat, true);
        MakeBox(bathroom.transform, "FarWall", new Vector3(12f, h2, bathZ), new Vector3(wallT, hallH, 5f), wallMat, true);
        MakeBox(bathroom.transform, "BackWall", new Vector3(9.5f, h2, bathZ - 2.5f), new Vector3(5f, hallH, wallT), wallMat, true);
        MakeBox(bathroom.transform, "FrontWall", new Vector3(9.5f, h2, bathZ + 2.5f), new Vector3(5f, hallH, wallT), wallMat, true);

        // White tile wall behind sinks
        MakeBox(bathroom.transform, "TileWall", new Vector3(9.5f, 1.4f, bathZ + 2.45f), new Vector3(5f, 1.6f, 0.05f), whiteTileMat, true);

        // 3 stalls with tinted blue doors
        Material stallDoorMat = new Material(Shader.Find("Standard"));
        stallDoorMat.color = new Color(0.3f, 0.5f, 0.7f);
        for (int i = 0; i < 3; i++)
        {
            MakeBox(bathroom.transform, "StallWall" + i, new Vector3(8.5f + i * 1.0f, h2 - 0.3f, bathZ - 1f), new Vector3(0.05f, 2.2f, 1.5f), whiteTileMat, true);
            MakeBox(bathroom.transform, "StallDoor" + i, new Vector3(9f + i * 1.0f, h2 - 0.3f, bathZ - 0.25f), new Vector3(0.95f, 2.0f, 0.05f), stallDoorMat, true);
        }
        // Sinks with mirrors and metal frames
        Material chromeMat = new Material(Shader.Find("Standard"));
        chromeMat.color = new Color(0.7f, 0.72f, 0.75f);
        chromeMat.SetFloat("_Glossiness", 0.9f);
        chromeMat.SetFloat("_Metallic", 0.95f);
        for (int i = 0; i < 2; i++)
        {
            float sx = 10.5f + i * 1.0f;
            // Sink basin (rounded look using sphere half)
            MakeBox(bathroom.transform, "Sink" + i, new Vector3(sx, 0.9f, bathZ + 2.3f), new Vector3(0.7f, 0.2f, 0.4f), whiteTileMat, true);
            MakeBox(bathroom.transform, "SinkRim" + i, new Vector3(sx, 1.0f, bathZ + 2.28f), new Vector3(0.72f, 0.04f, 0.4f), chromeMat, true);
            // Mirror frame (chrome) + mirror
            var mFrame = MakeBox(bathroom.transform, "MirrorFrame" + i, new Vector3(sx, 1.7f, bathZ + 2.43f), new Vector3(0.7f, 0.8f, 0.04f), chromeMat, false);
            Object.DestroyImmediate(mFrame.GetComponent<Collider>());
            var mGlass = MakeBox(bathroom.transform, "Mirror" + i, new Vector3(sx, 1.7f, bathZ + 2.41f), new Vector3(0.6f, 0.7f, 0.03f), mirrorMat, false);
            Object.DestroyImmediate(mGlass.GetComponent<Collider>());
            // Faucet
            var faucetBase = MakeBox(bathroom.transform, "FaucetBase" + i, new Vector3(sx, 1.04f, bathZ + 2.18f), new Vector3(0.08f, 0.1f, 0.08f), chromeMat, false);
            Object.DestroyImmediate(faucetBase.GetComponent<Collider>());
            GameObject spout = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            spout.name = "FaucetSpout" + i;
            spout.transform.parent = bathroom.transform;
            spout.transform.localPosition = new Vector3(sx, 1.15f, bathZ + 2.22f);
            spout.transform.localRotation = Quaternion.Euler(45, 0, 0);
            spout.transform.localScale = new Vector3(0.04f, 0.12f, 0.04f);
            spout.GetComponent<Renderer>().material = chromeMat;
            Object.DestroyImmediate(spout.GetComponent<Collider>());
        }
        // Soap dispensers (small colorful bottles)
        Material[] soapColors = {
            EmissiveMat(new Color(0.8f, 0.4f, 0.9f)),
            EmissiveMat(new Color(0.4f, 0.8f, 0.4f))
        };
        for (int i = 0; i < 2; i++)
        {
            GameObject soap = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            soap.name = "Soap" + i;
            soap.transform.parent = bathroom.transform;
            soap.transform.localPosition = new Vector3(10.8f + i * 0.4f, 1.15f, bathZ + 2.3f);
            soap.transform.localScale = new Vector3(0.06f, 0.1f, 0.06f);
            soap.GetComponent<Renderer>().material = soapColors[i];
        }
        // OUT OF ORDER sign on one stall
        Material orangeMat = new Material(Shader.Find("Standard"));
        orangeMat.color = new Color(1f, 0.6f, 0.1f);
        MakeBox(bathroom.transform, "SignOOO", new Vector3(10f, 1.7f, bathZ - 0.22f), new Vector3(0.4f, 0.3f, 0.02f), orangeMat, true);

        MakeBox(bathroom.transform, "BridgeFloor", new Vector3(7f, 0, bathZ), new Vector3(2f, 0.1f, 2.5f), floorMat, true);

        // ---- BACK CORRIDOR DECORATIONS ----
        // Lockers along the far wall — body + door + handle, alternating colors
        Material lockerHandleMat = new Material(Shader.Find("Standard"));
        lockerHandleMat.color = new Color(0.7f, 0.7f, 0.75f);
        lockerHandleMat.SetFloat("_Glossiness", 0.8f);
        lockerHandleMat.SetFloat("_Metallic", 0.7f);
        for (int i = 0; i < 8; i++)
        {
            Material lockerMat = new Material(Shader.Find("Standard"));
            lockerMat.color = (i % 2 == 0) ? new Color(0.18f, 0.32f, 0.55f) : new Color(0.55f, 0.25f, 0.28f);
            lockerMat.SetFloat("_Glossiness", 0.5f);
            float lx2 = -7f + i * 2f;
            // Body (recessed)
            MakeBox(corridor.transform, "Locker" + i, new Vector3(lx2, 1.1f, corrZ - 1.6f), new Vector3(1.0f, 2.2f, 0.4f), lockerMat, true);
            // Door (slightly in front so it pops out)
            var door = MakeBox(corridor.transform, "LockerDoor" + i, new Vector3(lx2, 1.1f, corrZ - 1.42f), new Vector3(0.9f, 2.0f, 0.05f), lockerMat, true);
            Object.DestroyImmediate(door.GetComponent<Collider>());
            // Vent slits at top
            for (int v = 0; v < 3; v++)
            {
                var slit = MakeBox(corridor.transform, "Vent" + i + "_" + v, new Vector3(lx2, 1.95f - v * 0.08f, corrZ - 1.39f), new Vector3(0.5f, 0.03f, 0.02f), darkMat, false);
                Object.DestroyImmediate(slit.GetComponent<Collider>());
            }
            // Handle
            GameObject handle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            handle.name = "Handle" + i;
            handle.transform.parent = corridor.transform;
            handle.transform.localPosition = new Vector3(lx2 + 0.3f, 1.1f, corrZ - 1.39f);
            handle.transform.localRotation = Quaternion.Euler(90, 0, 0);
            handle.transform.localScale = new Vector3(0.08f, 0.04f, 0.08f);
            handle.GetComponent<Renderer>().material = lockerHandleMat;
            Object.DestroyImmediate(handle.GetComponent<Collider>());
        }
        // Emergency exit sign (glowing red)
        Material exitMat = EmissiveMat(new Color(1f, 0.2f, 0.2f));
        MakeBox(corridor.transform, "ExitSign", new Vector3(0, 2.7f, corrZ - 1.7f), new Vector3(1.2f, 0.4f, 0.05f), exitMat, true);
    }

    static Material EmissiveMat(Color c)
    {
        Material m = new Material(Shader.Find("Standard"));
        m.color = c;
        m.EnableKeyword("_EMISSION");
        m.SetColor("_EmissionColor", c * 1.5f);
        return m;
    }

    static void BuildAlcove(Transform parent, string name, Vector3 pos, float width, float depth, float height, bool leftSide)
    {
        // Floor of alcove
        MakeBox(parent, name + "_Floor", pos, new Vector3(depth, 0.1f, width), floorMat, true);
        MakeBox(parent, name + "_Ceiling", pos + new Vector3(0, height, 0), new Vector3(depth, 0.1f, width), ceilingMat, true);
        // Outer (back) wall of alcove
        float side = leftSide ? -1f : 1f;
        MakeBox(parent, name + "_BackWall", pos + new Vector3(side * depth/2, height/2, 0), new Vector3(0.2f, height, width), wallMat, true);
        // Front of alcove side walls
        MakeBox(parent, name + "_SideA", pos + new Vector3(0, height/2, width/2), new Vector3(depth, height, 0.2f), wallMat, true);
        MakeBox(parent, name + "_SideB", pos + new Vector3(0, height/2, -width/2), new Vector3(depth, height, 0.2f), wallMat, true);
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

        PrefabUtility.SaveAsPrefabAsset(door, "Assets/Art/Prefabs/" + name + ".prefab");
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
        ai.moveSpeed = 5f;

        DoorController[] doors = Object.FindObjectsByType<DoorController>(FindObjectsSortMode.None);
        foreach (DoorController dc in doors)
        {
            if (dc.transform.position.x < 0) ai.leftDoor = dc;
            if (dc.transform.position.x > 0) ai.rightDoor = dc;
        }

        PrefabUtility.SaveAsPrefabAsset(enemy, "Assets/Art/Prefabs/Animatronic.prefab");
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

        CreateLight("LeftHallLight1", new Vector3(-5.25f, 2.5f, -2f), new Color(0.3f, 0.4f, 1f), 1.0f, 8f);
        CreateLight("LeftHallLight2", new Vector3(-5.25f, 2.5f, -8f), new Color(0.3f, 0.4f, 1f), 0.7f, 8f);
        CreateLight("LeftHallLight3", new Vector3(-5.25f, 2.5f, -14f), new Color(0.3f, 0.4f, 1f), 0.5f, 8f);

        CreateLight("RightHallLight1", new Vector3(5.25f, 2.5f, -2f), new Color(0.3f, 0.4f, 1f), 1.0f, 8f);
        CreateLight("RightHallLight2", new Vector3(5.25f, 2.5f, -8f), new Color(0.3f, 0.4f, 1f), 0.7f, 8f);
        CreateLight("RightHallLight3", new Vector3(5.25f, 2.5f, -14f), new Color(0.3f, 0.4f, 1f), 0.5f, 8f);

        CreateLight("CorridorLight", new Vector3(0, 2.5f, -19f), new Color(0.5f, 0.5f, 0.7f), 0.6f, 12f);
        CreateLight("StageLight", new Vector3(0, 3f, -23f), new Color(1f, 0.8f, 0.4f), 1.2f, 8f);

        // Classroom - bright fluorescent (TWO lights to fully cover the room)
        CreateLight("ClassroomLight1", new Vector3(-9.5f, 3.2f, -10.5f), new Color(0.85f, 0.95f, 1f), 2.5f, 10f);
        CreateLight("ClassroomLight2", new Vector3(-9.5f, 3.2f, -13.5f), new Color(0.85f, 0.95f, 1f), 2.5f, 10f);
        // Bathroom - bright cold white
        CreateLight("BathroomLight1", new Vector3(9.5f, 3.2f, -10.5f), new Color(0.95f, 0.95f, 1f), 2.5f, 10f);
        CreateLight("BathroomLight2", new Vector3(9.5f, 3.2f, -13.5f), new Color(0.95f, 0.95f, 1f), 2.5f, 10f);
        // Stage colored mood lights
        CreateLight("StageRedLight",  new Vector3(-2.5f, 2.8f, -23f), new Color(1f, 0.2f, 0.3f),  1.5f, 8f);
        CreateLight("StageBlueLight", new Vector3(2.5f, 2.8f, -23f),  new Color(0.3f, 0.3f, 1f),  1.5f, 8f);
        // Corridor — extra lights so cameras can see lockers and sign
        CreateLight("CorridorLight2", new Vector3(-5f, 2.8f, -20f), new Color(0.7f, 0.7f, 0.85f), 1.2f, 10f);
        CreateLight("CorridorLight3", new Vector3(5f, 2.8f, -20f),  new Color(0.7f, 0.7f, 0.85f), 1.2f, 10f);

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.04f, 0.04f, 0.06f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.02f, 0.02f, 0.03f);
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogDensity = 0.015f;

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
        manager.nightDuration = 120f;
        manager.currentNight = 1;

        EnemyAI enemy = Object.FindFirstObjectByType<EnemyAI>();
        if (enemy != null)
            enemy.gameManager = manager;

        // Wire monitor clock
        GameObject clockObj = GameObject.Find("MonitorClockText");
        if (clockObj != null)
        {
            TMP_Text tmp = clockObj.GetComponent<TMP_Text>();
            manager.clockText = tmp;
            Debug.Log("[SceneBuilder] Found MonitorClockText, TMP_Text component: " + (tmp != null ? "OK" : "NULL"));
        }
        else
        {
            Debug.LogError("[SceneBuilder] MonitorClockText GameObject not found!");
        }

    }

    static void BuildMonitorClock()
    {
        GameObject clockObj = new GameObject("MonitorClockText");
        clockObj.transform.position = new Vector3(1.2f, 1.15f, -0.46f);
        // Camera is rotated 180 around Y, so flip text 180 around Y to compensate
        clockObj.transform.rotation = Quaternion.Euler(0, 180, 0);

        TextMeshPro clock = clockObj.AddComponent<TextMeshPro>();
        clock.text = "12 AM";
        clock.fontSize = 1.2f;
        clock.color = new Color(0.3f, 1f, 0.3f);
        clock.alignment = TextAlignmentOptions.Center;
        clock.fontStyle = FontStyles.Bold;

        RectTransform rt = clock.rectTransform;
        rt.sizeDelta = new Vector2(0.7f, 0.4f);

        Debug.Log("[SceneBuilder] Created MonitorClockText at " + clockObj.transform.position);
    }

    static void BuildHUD()
    {
        // Canvas
        GameObject canvasObj = new GameObject("HUDCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObj.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        // Clock (top right)
        GameObject clockObj = new GameObject("ClockText");
        clockObj.transform.SetParent(canvasObj.transform, false);
        TMP_Text clock = clockObj.AddComponent<TextMeshProUGUI>();
        clock.text = "12 AM";
        clock.fontSize = 80;
        clock.color = new Color(1f, 0.3f, 0.3f);
        clock.alignment = TextAlignmentOptions.Right;
        clock.fontStyle = FontStyles.Bold;
        RectTransform clockRT = clock.rectTransform;
        clockRT.anchorMin = new Vector2(1, 1);
        clockRT.anchorMax = new Vector2(1, 1);
        clockRT.pivot = new Vector2(1, 1);
        clockRT.anchoredPosition = new Vector2(-40, -40);
        clockRT.sizeDelta = new Vector2(300, 100);

        // Night display (top left)
        GameObject nightObj = new GameObject("NightText");
        nightObj.transform.SetParent(canvasObj.transform, false);
        TMP_Text night = nightObj.AddComponent<TextMeshProUGUI>();
        night.text = "Night 1";
        night.fontSize = 50;
        night.color = new Color(0.9f, 0.9f, 0.9f);
        night.alignment = TextAlignmentOptions.Left;
        night.fontStyle = FontStyles.Bold;
        RectTransform nightRT = night.rectTransform;
        nightRT.anchorMin = new Vector2(0, 1);
        nightRT.anchorMax = new Vector2(0, 1);
        nightRT.pivot = new Vector2(0, 1);
        nightRT.anchoredPosition = new Vector2(40, -40);
        nightRT.sizeDelta = new Vector2(300, 70);

        // Win Panel (full screen overlay, hidden by default)
        GameObject winPanel = new GameObject("WinPanel");
        winPanel.transform.SetParent(canvasObj.transform, false);
        Image winBG = winPanel.AddComponent<Image>();
        winBG.color = new Color(0f, 0f, 0f, 0.85f);
        RectTransform winRT = winBG.rectTransform;
        winRT.anchorMin = Vector2.zero;
        winRT.anchorMax = Vector2.one;
        winRT.offsetMin = Vector2.zero;
        winRT.offsetMax = Vector2.zero;

        // Win title
        GameObject winTitleObj = new GameObject("WinTitle");
        winTitleObj.transform.SetParent(winPanel.transform, false);
        TMP_Text winTitle = winTitleObj.AddComponent<TextMeshProUGUI>();
        winTitle.text = "6 AM";
        winTitle.fontSize = 200;
        winTitle.color = new Color(0.9f, 0.6f, 0.2f);
        winTitle.alignment = TextAlignmentOptions.Center;
        winTitle.fontStyle = FontStyles.Bold;
        RectTransform winTitleRT = winTitle.rectTransform;
        winTitleRT.anchorMin = new Vector2(0.5f, 0.5f);
        winTitleRT.anchorMax = new Vector2(0.5f, 0.5f);
        winTitleRT.pivot = new Vector2(0.5f, 0.5f);
        winTitleRT.anchoredPosition = new Vector2(0, 100);
        winTitleRT.sizeDelta = new Vector2(800, 250);

        // Win subtitle
        GameObject winSubObj = new GameObject("WinSubtitle");
        winSubObj.transform.SetParent(winPanel.transform, false);
        TMP_Text winSub = winSubObj.AddComponent<TextMeshProUGUI>();
        winSub.text = "YOU SURVIVED THE NIGHT";
        winSub.fontSize = 60;
        winSub.color = new Color(0.95f, 0.95f, 0.95f);
        winSub.alignment = TextAlignmentOptions.Center;
        RectTransform winSubRT = winSub.rectTransform;
        winSubRT.anchorMin = new Vector2(0.5f, 0.5f);
        winSubRT.anchorMax = new Vector2(0.5f, 0.5f);
        winSubRT.pivot = new Vector2(0.5f, 0.5f);
        winSubRT.anchoredPosition = new Vector2(0, -50);
        winSubRT.sizeDelta = new Vector2(1200, 100);

        winPanel.SetActive(false);
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
