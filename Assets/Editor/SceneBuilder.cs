
// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEditor namespace.
using UnityEditor;

// Imports the UnityEditor.SceneManagement namespace.
using UnityEditor.SceneManagement;

// Imports the UnityEngine.AI namespace.
using UnityEngine.AI;

// Imports the UnityEngine.UI namespace.
using UnityEngine.UI;

// Imports the Unity.AI.Navigation namespace.
using Unity.AI.Navigation;

// Imports the TMPro namespace.
using TMPro;


// Declares the class named SceneBuilder.
public class SceneBuilder : EditorWindow

// Opens a new code block.
{


    // Applies the MenuItem("Night Shift/View Map") attribute.
    [MenuItem("Night Shift/View Map")]

    // Declares the method named ViewMap.
    static void ViewMap()

    // Opens a new code block.
    {

        // Calls a method.
        EditorSceneManager.OpenScene("Assets/scene.unity");

    // Closes the current code block.
    }



    // Applies the MenuItem("Night Shift/View Menu") attribute.
    [MenuItem("Night Shift/View Menu")]

    // Declares the method named ViewMenu.
    static void ViewMenu()

    // Opens a new code block.
    {

        // Calls a method.
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");

    // Closes the current code block.
    }



    // Applies the MenuItem("Night Shift/Build") attribute.
    [MenuItem("Night Shift/Build")]

    // Declares the method named Build.
    static void Build()

    // Opens a new code block.
    {

        // Calls a method.
        if (!EditorUtility.DisplayDialog("Build Everything",

            // Executes this statement.
            "This will rebuild and save BOTH scenes:\n" +

            // Executes this statement.
            " • Main Menu (Assets/Scenes/MainMenu.unity)\n" +

            // Executes this statement.
            " • Game World (Assets/scene.unity)\n\nContinue?",

            // Executes this statement.
            "Build", "Cancel")) return;



        // Declares the variable menuScene and initializes it.
        var menuScene = EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");

        // Calls a method.
        BuildMainMenuScene();

        // Calls a method.
        EditorSceneManager.SaveScene(menuScene);



        // Declares the variable gameScene and initializes it.
        var gameScene = EditorSceneManager.OpenScene("Assets/scene.unity");

        // Calls a method.
        BuildGameScene();

        // Calls a method.
        EditorSceneManager.SaveScene(gameScene);



        // Calls a method.
        EditorSceneManager.OpenScene("Assets/Scenes/MainMenu.unity");


        // Calls a method.
        Debug.Log("Build complete. Hit Play to test.");

    // Closes the current code block.
    }


    // Declares the method named BuildMainMenuScene.
    static void BuildMainMenuScene()

    // Opens a new code block.
    {


        // Iterates through each item in the collection.
        foreach (GameObject obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))

        // Opens a new code block.
        {

            // Checks the condition and runs the inline statement when it is true.
            if (obj == null) continue;

            // Checks the condition and runs the inline statement when it is true.
            if (obj.transform.parent != null) continue;

            // Calls a method.
            Object.DestroyImmediate(obj);

        // Closes the current code block.
        }



        // Declares the variable menu and initializes it.
        GameObject menu = new GameObject("MainMenu");

        // Calls a method.
        menu.AddComponent<SimpleMainMenu>();

    // Closes the current code block.
    }


    // Declares the method named BuildGameScene.
    static void BuildGameScene()

    // Opens a new code block.
    {

        // Calls a method.
        ClearScene();

        // Calls a method.
        CreateMaterials();

        // Calls a method.
        BuildOffice();

        // Calls a method.
        BuildHallways();

        // Calls a method.
        BuildLighting();

        // Calls a method.
        BuildNavMesh();

        // Calls a method.
        BuildDoors();

        // Calls a method.
        BuildSpots();

        // Calls a method.
        BuildEnemy();

        // Calls a method.
        BuildClown();

        // Calls a method.
        BuildMonitorClock();

        // Calls a method.
        BuildGameManager();

        // Calls a method.
        TestNavMesh();

    // Closes the current code block.
    }


    // Declares the method named BuildSpots.
    static void BuildSpots()

    // Opens a new code block.
    {

        // Declares the variable parent and initializes it.
        GameObject parent = new GameObject("AISpots");

        // Calls a method.
        MakeNavSpot(parent, "Spot_Corridor",      new Vector3( 0f,    0.5f, -19f));

        // Calls a method.
        MakeNavSpot(parent, "Spot_LeftAlcove",    new Vector3(-7.25f, 0.5f, -7f));

        // Calls a method.
        MakeNavSpot(parent, "Spot_RightAlcove",   new Vector3( 7.25f, 0.5f, -7f));

        // Calls a method.
        MakeNavSpot(parent, "Spot_Stage",         new Vector3( 0f,    0.5f, -23f));

        // Calls a method.
        MakeNavSpot(parent, "Spot_Classroom",     new Vector3(-9.5f,  0.5f, -12f));

        // Calls a method.
        MakeNavSpot(parent, "Spot_Bathroom",      new Vector3( 9.5f,  0.5f, -12f));

        // Calls a method.
        MakeNavSpot(parent, "Spot_LeftDoor",      new Vector3(-5.25f, 0.5f,  0f));

        // Calls a method.
        MakeNavSpot(parent, "Spot_RightDoor",     new Vector3( 5.25f, 0.5f,  0f));

        // Calls a method.
        MakeNavSpot(parent, "Spot_OfficeCenter",  new Vector3( 0f,    0.5f,  1f));

    // Closes the current code block.
    }


    // Declares the method named MakeNavSpot.
    static void MakeNavSpot(GameObject parent, string name, Vector3 pos)

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject(name);

        // Updates an existing value.
        obj.transform.parent = parent.transform;

        // Updates an existing value.
        obj.transform.position = pos;

    // Closes the current code block.
    }


    // Declares the method named BuildClown.
    static void BuildClown()

    // Opens a new code block.
    {

        // Declares the variable tmp and initializes it.
        GameObject tmp = new GameObject("TempClownBuilder");

        // Declares the variable builder and initializes it.
        var builder = tmp.AddComponent<WorldBuilder>();

        // Calls a method.
        builder.BuildClownEnemy(new Vector3(5.25f, 1f, -15f));

        // Calls a method.
        Object.DestroyImmediate(tmp);

    // Closes the current code block.
    }


    // Declares the method named ClearScene.
    static void ClearScene()

    // Opens a new code block.
    {

        // Declares the variable allObjects and initializes it.
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        // Iterates through each item in the collection.
        foreach (GameObject obj in allObjects)

        // Opens a new code block.
        {

            // Checks the condition and runs the inline statement when it is true.
            if (obj == null) continue;

            // Checks the condition and runs the inline statement when it is true.
            if (obj.transform.parent != null) continue;

            // Checks the condition and runs the inline statement when it is true.
            if (obj.GetComponent<Camera>() != null) continue;

            // Calls a method.
            Object.DestroyImmediate(obj);

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Executes this statement.
    static Material wallMat, floorMat, ceilingMat, doorMat, darkMat, tileMat;


    // Declares the method named CreateMaterials.
    static void CreateMaterials()

    // Opens a new code block.
    {

        // Updates an existing value.
        wallMat = CreateMat("WallMaterial", new Color(0.18f, 0.16f, 0.22f));

        // Updates an existing value.
        floorMat = CreateMat("FloorMaterial", new Color(0.08f, 0.08f, 0.1f));

        // Updates an existing value.
        ceilingMat = CreateMat("CeilingMaterial", new Color(0.06f, 0.06f, 0.08f));

        // Updates an existing value.
        doorMat = CreateMat("DoorMaterial", new Color(0.35f, 0.1f, 0.1f));

        // Updates an existing value.
        darkMat = CreateMat("DarkMaterial", new Color(0.04f, 0.04f, 0.06f));

        // Updates an existing value.
        tileMat = CreateMat("TileMaterial", new Color(0.12f, 0.12f, 0.16f));

    // Closes the current code block.
    }


    // Declares the method named CreateMat.
    static Material CreateMat(string matName, Color color)

    // Opens a new code block.
    {

        // Declares the variable path and initializes it.
        string path = "Assets/Art/Materials/Environment/" + matName + ".mat";

        // Declares the variable mat and initializes it.
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

        // Checks whether the condition is true.
        if (mat == null)

        // Opens a new code block.
        {

            // Updates an existing value.
            mat = new Material(Shader.Find("Standard"));

            // Updates an existing value.
            mat.color = color;

            // Calls a method.
            mat.SetFloat("_Glossiness", 0.15f);

            // Calls a method.
            AssetDatabase.CreateAsset(mat, path);

        // Closes the current code block.
        }

        // Runs the fallback branch when earlier conditions were false.
        else

        // Opens a new code block.
        {

            // Updates an existing value.
            mat.color = color;

        // Closes the current code block.
        }

        // Returns the specified value.
        return mat;

    // Closes the current code block.
    }


    // Declares the method named BuildOffice.
    static void BuildOffice()

    // Opens a new code block.
    {

        // Declares the variable office and initializes it.
        GameObject office = new GameObject("SecurityOffice");


        // Declares the variable roomW and initializes it.
        float roomW = 8f;

        // Declares the variable roomD and initializes it.
        float roomD = 5f;

        // Declares the variable roomH and initializes it.
        float roomH = 3.5f;

        // Declares the variable wallT and initializes it.
        float wallT = 0.2f;


        // Calls a method.
        MakeBox(office.transform, "Floor", Vector3.zero, new Vector3(roomW, 0.1f, roomD), floorMat, true);

        // Calls a method.
        MakeBox(office.transform, "Ceiling", new Vector3(0, roomH, 0), new Vector3(roomW, 0.1f, roomD), ceilingMat, true);

        // Calls a method.
        MakeBox(office.transform, "BackWall", new Vector3(0, roomH / 2, -roomD / 2), new Vector3(roomW, roomH, wallT), wallMat, true);

        // Calls a method.
        MakeBox(office.transform, "FrontWall", new Vector3(0, roomH / 2, roomD / 2), new Vector3(roomW, roomH, wallT), wallMat, true);


        // Declares the variable doorGap and initializes it.
        float doorGap = 1.5f;



        // Calls a method.
        MakeBox(office.transform, "LeftWallFront", new Vector3(-roomW/2, roomH/2, doorGap + 0.5f), new Vector3(wallT, roomH, roomD/2 - doorGap), wallMat, true);

        // Calls a method.
        MakeBox(office.transform, "LeftWallBack", new Vector3(-roomW/2, roomH/2, -(doorGap + 0.5f)), new Vector3(wallT, roomH, roomD/2 - doorGap), wallMat, true);



        // Declares the variable lft and initializes it.
        var lft = MakeBox(office.transform, "LeftFrameTop", new Vector3(-roomW/2, 2.6f, 0), new Vector3(0.3f, 0.15f, doorGap * 2 + 0.1f), darkMat, false);

        // Calls a method.
        Object.DestroyImmediate(lft.GetComponent<Collider>());

        // Declares the variable lfl and initializes it.
        var lfl = MakeBox(office.transform, "LeftFrameL", new Vector3(-roomW/2, roomH/2, doorGap), new Vector3(0.3f, roomH, 0.1f), darkMat, false);

        // Calls a method.
        Object.DestroyImmediate(lfl.GetComponent<Collider>());

        // Declares the variable lfr and initializes it.
        var lfr = MakeBox(office.transform, "LeftFrameR", new Vector3(-roomW/2, roomH/2, -doorGap), new Vector3(0.3f, roomH, 0.1f), darkMat, false);

        // Calls a method.
        Object.DestroyImmediate(lfr.GetComponent<Collider>());



        // Calls a method.
        MakeBox(office.transform, "RightWallFront", new Vector3(roomW/2, roomH/2, doorGap + 0.5f), new Vector3(wallT, roomH, roomD/2 - doorGap), wallMat, true);

        // Calls a method.
        MakeBox(office.transform, "RightWallBack", new Vector3(roomW/2, roomH/2, -(doorGap + 0.5f)), new Vector3(wallT, roomH, roomD/2 - doorGap), wallMat, true);



        // Declares the variable rft and initializes it.
        var rft = MakeBox(office.transform, "RightFrameTop", new Vector3(roomW/2, 2.6f, 0), new Vector3(0.3f, 0.15f, doorGap * 2 + 0.1f), darkMat, false);

        // Calls a method.
        Object.DestroyImmediate(rft.GetComponent<Collider>());

        // Declares the variable rfl and initializes it.
        var rfl = MakeBox(office.transform, "RightFrameL", new Vector3(roomW/2, roomH/2, doorGap), new Vector3(0.3f, roomH, 0.1f), darkMat, false);

        // Calls a method.
        Object.DestroyImmediate(rfl.GetComponent<Collider>());

        // Declares the variable rfr and initializes it.
        var rfr = MakeBox(office.transform, "RightFrameR", new Vector3(roomW/2, roomH/2, -doorGap), new Vector3(0.3f, roomH, 0.1f), darkMat, false);

        // Calls a method.
        Object.DestroyImmediate(rfr.GetComponent<Collider>());



        // Calls a method.
        MakeBox(office.transform, "LeftBridgeFloor", new Vector3(-roomW/2, 0, 0), new Vector3(1.5f, 0.1f, doorGap * 2), floorMat, true);

        // Calls a method.
        MakeBox(office.transform, "RightBridgeFloor", new Vector3(roomW/2, 0, 0), new Vector3(1.5f, 0.1f, doorGap * 2), floorMat, true);



        // Calls a method.
        MakeBox(office.transform, "Desk", new Vector3(0, 0.75f, -0.2f), new Vector3(5f, 0.08f, 1.4f), darkMat, true);


        // Declares the variable deskFront and initializes it.
        var deskFront = MakeBox(office.transform, "DeskFront", new Vector3(0, 0.38f, 0.5f), new Vector3(5f, 0.75f, 0.06f), darkMat, false);

        // Calls a method.
        Object.DestroyImmediate(deskFront.GetComponent<Collider>());


        // Declares the variable legX and initializes it.
        float[] legX = { -2.3f, 2.3f, -2.3f, 2.3f };

        // Declares the variable legZ and initializes it.
        float[] legZ = { -0.8f, -0.8f, 0.45f, 0.45f };

        // Starts a for loop.
        for (int i = 0; i < 4; i++)

            // Calls a method.
            MakeBox(office.transform, "DeskLeg" + i, new Vector3(legX[i], 0.35f, legZ[i]), new Vector3(0.08f, 0.7f, 0.08f), darkMat, true);



        // Starts a for loop.
        for (int i = -1; i <= 1; i++)

        // Opens a new code block.
        {

            // Calls a method.
            MakeBox(office.transform, "Monitor" + (i + 2), new Vector3(i * 1.2f, 1.15f, -0.5f), new Vector3(0.9f, 0.6f, 0.05f), darkMat, false);


            // Declares the variable screen and initializes it.
            GameObject screen = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Updates an existing value.
            screen.name = "Screen" + (i + 2);

            // Updates an existing value.
            screen.transform.parent = office.transform;

            // Updates an existing value.
            screen.transform.localPosition = new Vector3(i * 1.2f, 1.15f, -0.48f);

            // Updates an existing value.
            screen.transform.localScale = new Vector3(0.8f, 0.5f, 0.01f);

            // Declares the variable screenMat and initializes it.
            Material screenMat = new Material(Shader.Find("Standard"));

            // Updates an existing value.
            screenMat.color = new Color(0.05f, 0.15f, 0.05f);

            // Calls a method.
            screenMat.EnableKeyword("_EMISSION");

            // Calls a method.
            screenMat.SetColor("_EmissionColor", new Color(0.02f, 0.06f, 0.02f));

            // Calls a method.
            screen.GetComponent<Renderer>().material = screenMat;

        // Closes the current code block.
        }



        // Calls a method.
        MakeBox(office.transform, "ChairSeat", new Vector3(0, 0.45f, 1.3f), new Vector3(0.7f, 0.06f, 0.7f), doorMat, false);

        // Calls a method.
        MakeBox(office.transform, "ChairBack", new Vector3(0, 0.8f, 1.65f), new Vector3(0.7f, 0.7f, 0.06f), doorMat, false);



        // Calls a method.
        MakeBox(office.transform, "FanBase", new Vector3(2.0f, 0.82f, -0.2f), new Vector3(0.3f, 0.05f, 0.3f), tileMat, false);

        // Declares the variable fanHead and initializes it.
        GameObject fanHead = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        fanHead.name = "FanHead";

        // Updates an existing value.
        fanHead.transform.parent = office.transform;

        // Updates an existing value.
        fanHead.transform.localPosition = new Vector3(2.0f, 1.05f, -0.2f);

        // Updates an existing value.
        fanHead.transform.localScale = new Vector3(0.25f, 0.25f, 0.15f);

        // Calls a method.
        fanHead.GetComponent<Renderer>().material = tileMat;



        // Calls a method.
        MakePoster(office.transform, "PosterRules", new Vector3(-2.5f, 2.0f, -2.38f),

                   // Executes this statement.
                   new Vector3(0.7f, 0.9f, 0.02f), "Assets/Art/Textures/poster_rules.png");

        // Calls a method.
        MakePoster(office.transform, "PosterCaution", new Vector3(2.5f, 1.8f, -2.38f),

                   // Executes this statement.
                   new Vector3(0.6f, 0.6f, 0.02f), "Assets/Art/Textures/poster_caution.png");

        // Calls a method.
        MakePoster(office.transform, "PosterCelebrate", new Vector3(-0.5f, 2.5f, -2.38f),

                   // Executes this statement.
                   new Vector3(0.5f, 0.65f, 0.02f), "Assets/Art/Textures/poster_celebrate.png");

    // Closes the current code block.
    }


    // Declares the method named MakePoster.
    static void MakePoster(Transform parent, string name, Vector3 pos, Vector3 scale, string texturePath)

    // Opens a new code block.
    {

        // Declares the variable tex and initializes it.
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);

        // Checks the condition and runs the inline statement when it is true.
        if (tex == null) return;


        // Declares the variable mat and initializes it.
        Material mat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        mat.mainTexture = tex;

        // Calls a method.
        mat.SetFloat("_Glossiness", 0.1f);


        // Declares the variable poster and initializes it.
        GameObject poster = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        poster.name = name;

        // Updates an existing value.
        poster.transform.parent = parent;

        // Updates an existing value.
        poster.transform.localPosition = pos;

        // Updates an existing value.
        poster.transform.localScale = scale;

        // Calls a method.
        poster.GetComponent<Renderer>().material = mat;

        // Calls a method.
        Object.DestroyImmediate(poster.GetComponent<Collider>());

    // Closes the current code block.
    }


    // Declares the method named MakeBox.
    static GameObject MakeBox(Transform parent, string name, Vector3 pos, Vector3 scale, Material mat, bool isStatic)

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        obj.name = name;

        // Updates an existing value.
        obj.transform.parent = parent;

        // Updates an existing value.
        obj.transform.localPosition = pos;

        // Updates an existing value.
        obj.transform.localScale = scale;

        // Calls a method.
        obj.GetComponent<Renderer>().material = mat;

        // Updates an existing value.
        obj.isStatic = isStatic;

        // Returns the specified value.
        return obj;

    // Closes the current code block.
    }


    // Declares the method named MakeBox.
    static GameObject MakeBox(Transform parent, string name, Vector3 pos, Vector3 scale, Color color, bool isStatic)

    // Opens a new code block.
    {

        // Declares the variable mat and initializes it.
        Material mat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        mat.color = color;

        // Calls a method.
        mat.SetFloat("_Glossiness", 0.15f);

        // Returns the specified value.
        return MakeBox(parent, name, pos, scale, mat, isStatic);

    // Closes the current code block.
    }


    // Declares the method named BuildHallways.
    static void BuildHallways()

    // Opens a new code block.
    {

        // Declares the variable hallW and initializes it.
        float hallW = 2.5f;

        // Declares the variable hallH and initializes it.
        float hallH = 3.5f;

        // Declares the variable wallT and initializes it.
        float wallT = 0.2f;

        // Declares the variable h2 and initializes it.
        float h2 = hallH / 2f;

        // Declares the variable lx and initializes it.
        float lx = -5.25f;

        // Declares the variable rx and initializes it.
        float rx = 5.25f;

        // Declares the variable hallLen and initializes it.
        float hallLen = 22f;

        // Declares the variable hallCenter and initializes it.
        float hallCenter = -9f;



        // Declares the variable leftHall and initializes it.
        GameObject leftHall = new GameObject("LeftHallway");

        // Calls a method.
        MakeBox(leftHall.transform, "Floor", new Vector3(lx, 0, hallCenter), new Vector3(hallW, 0.1f, hallLen), floorMat, true);

        // Calls a method.
        MakeBox(leftHall.transform, "Ceiling", new Vector3(lx, hallH, hallCenter), new Vector3(hallW, 0.1f, hallLen), ceilingMat, true);


        // Calls a method.
        MakeBox(leftHall.transform, "OuterWall_A", new Vector3(lx - hallW/2, h2, -2f), new Vector3(wallT, hallH, 8f), wallMat, true);

        // Calls a method.
        MakeBox(leftHall.transform, "OuterWall_B", new Vector3(lx - hallW/2, h2, -17.5f), new Vector3(wallT, hallH, 5f), wallMat, true);


        // Calls a method.
        MakeBox(leftHall.transform, "InnerWall_Front", new Vector3(lx + hallW/2, h2, 2.75f), new Vector3(wallT, hallH, 2.5f), wallMat, true);

        // Calls a method.
        MakeBox(leftHall.transform, "InnerWall_Back", new Vector3(lx + hallW/2, h2, -10f), new Vector3(wallT, hallH, 14f), wallMat, true);



        // Calls a method.
        BuildAlcove(leftHall.transform, "LeftAlcove", new Vector3(lx - hallW/2 - 1f, 0, -7f), 2f, 2f, hallH, true);



        // Declares the variable rightHall and initializes it.
        GameObject rightHall = new GameObject("RightHallway");

        // Calls a method.
        MakeBox(rightHall.transform, "Floor", new Vector3(rx, 0, hallCenter), new Vector3(hallW, 0.1f, hallLen), floorMat, true);

        // Calls a method.
        MakeBox(rightHall.transform, "Ceiling", new Vector3(rx, hallH, hallCenter), new Vector3(hallW, 0.1f, hallLen), ceilingMat, true);

        // Calls a method.
        MakeBox(rightHall.transform, "OuterWall_A", new Vector3(rx + hallW/2, h2, -2f), new Vector3(wallT, hallH, 8f), wallMat, true);

        // Calls a method.
        MakeBox(rightHall.transform, "OuterWall_B", new Vector3(rx + hallW/2, h2, -17.5f), new Vector3(wallT, hallH, 5f), wallMat, true);

        // Calls a method.
        MakeBox(rightHall.transform, "InnerWall_Front", new Vector3(rx - hallW/2, h2, 2.75f), new Vector3(wallT, hallH, 2.5f), wallMat, true);

        // Calls a method.
        MakeBox(rightHall.transform, "InnerWall_Back", new Vector3(rx - hallW/2, h2, -10f), new Vector3(wallT, hallH, 14f), wallMat, true);


        // Calls a method.
        BuildAlcove(rightHall.transform, "RightAlcove", new Vector3(rx + hallW/2 + 1f, 0, -7f), 2f, 2f, hallH, false);



        // Declares the variable corridor and initializes it.
        GameObject corridor = new GameObject("BackCorridor");

        // Declares the variable corrZ and initializes it.
        float corrZ = -19f;

        // Calls a method.
        MakeBox(corridor.transform, "Floor", new Vector3(0, 0, corrZ), new Vector3(18f, 0.1f, 3.5f), floorMat, true);

        // Calls a method.
        MakeBox(corridor.transform, "Ceiling", new Vector3(0, hallH, corrZ), new Vector3(18f, 0.1f, 3.5f), ceilingMat, true);


        // Calls a method.
        MakeBox(corridor.transform, "FarWall_L", new Vector3(-5.5f, h2, corrZ - 1.75f), new Vector3(7f, hallH, wallT), wallMat, true);

        // Calls a method.
        MakeBox(corridor.transform, "FarWall_R", new Vector3(5.5f, h2, corrZ - 1.75f), new Vector3(7f, hallH, wallT), wallMat, true);


        // Calls a method.
        MakeBox(corridor.transform, "LeftWall", new Vector3(-9f, h2, corrZ), new Vector3(wallT, hallH, 3.5f), wallMat, true);

        // Calls a method.
        MakeBox(corridor.transform, "RightWall", new Vector3(9f, h2, corrZ), new Vector3(wallT, hallH, 3.5f), wallMat, true);



        // Declares the variable stage and initializes it.
        GameObject stage = new GameObject("StageRoom");

        // Declares the variable stageZ and initializes it.
        float stageZ = -23f;

        // Calls a method.
        MakeBox(stage.transform, "Floor", new Vector3(0, 0, stageZ), new Vector3(8f, 0.1f, 5f), tileMat, true);

        // Calls a method.
        MakeBox(stage.transform, "Ceiling", new Vector3(0, hallH, stageZ), new Vector3(8f, 0.1f, 5f), ceilingMat, true);

        // Calls a method.
        MakeBox(stage.transform, "BackWall", new Vector3(0, h2, stageZ - 2.5f), new Vector3(8f, hallH, wallT), wallMat, true);

        // Calls a method.
        MakeBox(stage.transform, "LeftWall", new Vector3(-4f, h2, stageZ), new Vector3(wallT, hallH, 5f), wallMat, true);

        // Calls a method.
        MakeBox(stage.transform, "RightWall", new Vector3(4f, h2, stageZ), new Vector3(wallT, hallH, 5f), wallMat, true);


        // Calls a method.
        MakeBox(stage.transform, "Platform", new Vector3(0, 0.2f, stageZ - 1.5f), new Vector3(5f, 0.4f, 1.5f), darkMat, true);


        // Calls a method.
        MakeBox(stage.transform, "CurtainL", new Vector3(-2.5f, h2, stageZ - 0.5f), new Vector3(0.2f, hallH - 0.3f, 0.05f), doorMat, true);

        // Calls a method.
        MakeBox(stage.transform, "CurtainR", new Vector3(2.5f, h2, stageZ - 0.5f), new Vector3(0.2f, hallH - 0.3f, 0.05f), doorMat, true);



        // Executes this statement.
        Material[] signColors = {

            // Calls a method.
            EmissiveMat(new Color(1f, 0.2f, 0.2f)),

            // Calls a method.
            EmissiveMat(new Color(1f, 0.8f, 0.1f)),

            // Calls a method.
            EmissiveMat(new Color(0.2f, 0.9f, 0.3f)),

            // Calls a method.
            EmissiveMat(new Color(0.3f, 0.5f, 1f)),

            // Calls a method.
            EmissiveMat(new Color(0.9f, 0.2f, 0.9f)),

            // Calls a method.
            EmissiveMat(new Color(1f, 0.5f, 0.1f)),

            // Calls a method.
            EmissiveMat(new Color(0.4f, 1f, 0.9f))

        // Executes this statement.
        };


        // Starts a for loop.
        for (int i = 0; i < 7; i++)

        // Opens a new code block.
        {

            // Declares the variable letter and initializes it.
            GameObject letter = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            // Updates an existing value.
            letter.name = "SignLetter" + i;

            // Updates an existing value.
            letter.transform.parent = stage.transform;

            // Updates an existing value.
            letter.transform.localPosition = new Vector3(-1.8f + i * 0.6f, 2.6f, stageZ - 2.35f);

            // Updates an existing value.
            letter.transform.localRotation = Quaternion.Euler(90, 0, 0);

            // Updates an existing value.
            letter.transform.localScale = new Vector3(0.18f, 0.04f, 0.18f);

            // Calls a method.
            letter.GetComponent<Renderer>().material = signColors[i];

            // Calls a method.
            Object.DestroyImmediate(letter.GetComponent<Collider>());



            // Declares the variable neonLight and initializes it.
            GameObject neonLight = new GameObject("NeonLight" + i);

            // Updates an existing value.
            neonLight.transform.parent = letter.transform;

            // Updates an existing value.
            neonLight.transform.localPosition = Vector3.zero;

            // Declares the variable nl and initializes it.
            Light nl = neonLight.AddComponent<Light>();

            // Updates an existing value.
            nl.type = LightType.Point;

            // Updates an existing value.
            nl.color = signColors[i].color;

            // Updates an existing value.
            nl.intensity = 0.8f;

            // Updates an existing value.
            nl.range = 2f;

        // Closes the current code block.
        }


        // Declares the variable spot and initializes it.
        GameObject spot = new GameObject("StageSpot");

        // Updates an existing value.
        spot.transform.parent = stage.transform;

        // Updates an existing value.
        spot.transform.position = new Vector3(0, 3.2f, stageZ - 1.5f);

        // Updates an existing value.
        spot.transform.rotation = Quaternion.Euler(60, 0, 0);

        // Declares the variable sl and initializes it.
        Light sl = spot.AddComponent<Light>();

        // Updates an existing value.
        sl.type = LightType.Spot;

        // Updates an existing value.
        sl.color = new Color(1f, 0.85f, 0.4f);

        // Updates an existing value.
        sl.intensity = 4f;

        // Updates an existing value.
        sl.range = 8f;

        // Updates an existing value.
        sl.spotAngle = 50f;


        // Declares the variable drumMat and initializes it.
        Material drumMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        drumMat.color = new Color(0.9f, 0.1f, 0.1f);

        // Calls a method.
        drumMat.SetFloat("_Glossiness", 0.5f);

        // Declares the variable drum and initializes it.
        GameObject drum = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Updates an existing value.
        drum.name = "Drum";

        // Updates an existing value.
        drum.transform.parent = stage.transform;

        // Updates an existing value.
        drum.transform.localPosition = new Vector3(1.5f, 0.7f, stageZ - 1f);

        // Updates an existing value.
        drum.transform.localScale = new Vector3(0.5f, 0.3f, 0.5f);

        // Calls a method.
        drum.GetComponent<Renderer>().material = drumMat;


        // Declares the variable micMat and initializes it.
        Material micMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        micMat.color = new Color(0.4f, 0.4f, 0.45f);

        // Calls a method.
        micMat.SetFloat("_Glossiness", 0.7f);

        // Calls a method.
        MakeBox(stage.transform, "MicStand", new Vector3(-1.2f, 0.7f, stageZ - 1f), new Vector3(0.05f, 1.4f, 0.05f), micMat, true);

        // Declares the variable mic and initializes it.
        GameObject mic = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        mic.name = "Mic";

        // Updates an existing value.
        mic.transform.parent = stage.transform;

        // Updates an existing value.
        mic.transform.localPosition = new Vector3(-1.2f, 1.5f, stageZ - 1f);

        // Updates an existing value.
        mic.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);

        // Calls a method.
        mic.GetComponent<Renderer>().material = micMat;



        // Declares the variable greenBoardMat and initializes it.
        Material greenBoardMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        greenBoardMat.color = new Color(0.1f, 0.4f, 0.2f);

        // Declares the variable woodMat and initializes it.
        Material woodMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        woodMat.color = new Color(0.5f, 0.32f, 0.18f);

        // Declares the variable redMat and initializes it.
        Material redMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        redMat.color = new Color(0.85f, 0.15f, 0.15f);


        // Declares the variable classroom and initializes it.
        GameObject classroom = new GameObject("Classroom");

        // Declares the variable classZ and initializes it.
        float classZ = -12f;

        // Calls a method.
        MakeBox(classroom.transform, "Floor", new Vector3(-9.5f, 0, classZ), new Vector3(5f, 0.1f, 5f), floorMat, true);

        // Calls a method.
        MakeBox(classroom.transform, "Ceiling", new Vector3(-9.5f, hallH, classZ), new Vector3(5f, 0.1f, 5f), ceilingMat, true);

        // Calls a method.
        MakeBox(classroom.transform, "FarWall", new Vector3(-12f, h2, classZ), new Vector3(wallT, hallH, 5f), wallMat, true);

        // Calls a method.
        MakeBox(classroom.transform, "BackWall", new Vector3(-9.5f, h2, classZ - 2.5f), new Vector3(5f, hallH, wallT), wallMat, true);

        // Calls a method.
        MakeBox(classroom.transform, "FrontWall", new Vector3(-9.5f, h2, classZ + 2.5f), new Vector3(5f, hallH, wallT), wallMat, true);



        // Calls a method.
        MakeBox(classroom.transform, "BoardFrame", new Vector3(-11.88f, 1.7f, classZ), new Vector3(0.04f, 1.6f, 3.4f), woodMat, true);

        // Calls a method.
        MakeBox(classroom.transform, "Greenboard", new Vector3(-11.85f, 1.7f, classZ), new Vector3(0.05f, 1.4f, 3f), greenBoardMat, true);



        // Calls a method.
        MakeBox(classroom.transform, "TeacherDesk", new Vector3(-11f, 0.75f, classZ), new Vector3(0.08f, 1.6f, 1.8f), woodMat, true);

        // Calls a method.
        MakeBox(classroom.transform, "TeacherDeskFront", new Vector3(-10.65f, 0.4f, classZ), new Vector3(0.7f, 0.8f, 1.8f), woodMat, true);


        // Declares the variable globe and initializes it.
        GameObject globe = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        globe.name = "Globe";

        // Updates an existing value.
        globe.transform.parent = classroom.transform;

        // Updates an existing value.
        globe.transform.localPosition = new Vector3(-10.85f, 1f, classZ - 0.6f);

        // Updates an existing value.
        globe.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        // Declares the variable globeMat and initializes it.
        Material globeMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        globeMat.color = new Color(0.2f, 0.5f, 0.9f);

        // Calls a method.
        globe.GetComponent<Renderer>().material = globeMat;


        // Declares the variable apple and initializes it.
        GameObject apple = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        apple.name = "Apple";

        // Updates an existing value.
        apple.transform.parent = classroom.transform;

        // Updates an existing value.
        apple.transform.localPosition = new Vector3(-10.85f, 0.92f, classZ + 0.5f);

        // Updates an existing value.
        apple.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

        // Calls a method.
        apple.GetComponent<Renderer>().material = redMat;



        // Starts a for loop.
        for (int row = 0; row < 2; row++)

            // Starts a for loop.
            for (int col = 0; col < 2; col++)

            // Opens a new code block.
            {

                // Declares the variable x and initializes it.
                float x = -9.5f + col * 1.4f;

                // Declares the variable z and initializes it.
                float z = classZ - 0.7f + row * 1.4f;


                // Calls a method.
                MakeBox(classroom.transform, "StuDesk_" + row + "_" + col, new Vector3(x, 0.62f, z), new Vector3(0.8f, 0.06f, 0.5f), woodMat, true);


                // Starts a for loop.
                for (int leg = 0; leg < 4; leg++)

                // Opens a new code block.
                {

                    // Declares the variable lx2 and initializes it.
                    float lx2 = (leg % 2 == 0) ? -0.35f : 0.35f;

                    // Declares the variable lz and initializes it.
                    float lz = (leg < 2) ? -0.2f : 0.2f;

                    // Declares the variable legObj and initializes it.
                    var legObj = MakeBox(classroom.transform, "DeskLeg_" + row + col + leg, new Vector3(x + lx2, 0.3f, z + lz), new Vector3(0.04f, 0.6f, 0.04f), darkMat, false);

                    // Calls a method.
                    Object.DestroyImmediate(legObj.GetComponent<Collider>());

                // Closes the current code block.
                }


                // Calls a method.
                MakeBox(classroom.transform, "StuChairSeat_" + row + col, new Vector3(x, 0.42f, z + 0.55f), new Vector3(0.45f, 0.05f, 0.45f), darkMat, true);


                // Calls a method.
                MakeBox(classroom.transform, "StuChairBack_" + row + col, new Vector3(x, 0.7f, z + 0.75f), new Vector3(0.45f, 0.5f, 0.04f), darkMat, true);

            // Closes the current code block.
            }

        // Calls a method.
        MakeBox(classroom.transform, "BridgeFloor", new Vector3(-7f, 0, classZ), new Vector3(2f, 0.1f, 2.5f), floorMat, true);



        // Declares the variable whiteTileMat and initializes it.
        Material whiteTileMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        whiteTileMat.color = new Color(0.92f, 0.93f, 0.95f);

        // Calls a method.
        whiteTileMat.SetFloat("_Glossiness", 0.4f);

        // Declares the variable mirrorMat and initializes it.
        Material mirrorMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        mirrorMat.color = new Color(0.7f, 0.85f, 0.95f);

        // Calls a method.
        mirrorMat.SetFloat("_Glossiness", 0.95f);

        // Calls a method.
        mirrorMat.SetFloat("_Metallic", 0.9f);


        // Declares the variable bathroom and initializes it.
        GameObject bathroom = new GameObject("Bathroom");

        // Declares the variable bathZ and initializes it.
        float bathZ = -12f;

        // Calls a method.
        MakeBox(bathroom.transform, "Floor", new Vector3(9.5f, 0, bathZ), new Vector3(5f, 0.1f, 5f), tileMat, true);

        // Calls a method.
        MakeBox(bathroom.transform, "Ceiling", new Vector3(9.5f, hallH, bathZ), new Vector3(5f, 0.1f, 5f), ceilingMat, true);

        // Calls a method.
        MakeBox(bathroom.transform, "FarWall", new Vector3(12f, h2, bathZ), new Vector3(wallT, hallH, 5f), wallMat, true);

        // Calls a method.
        MakeBox(bathroom.transform, "BackWall", new Vector3(9.5f, h2, bathZ - 2.5f), new Vector3(5f, hallH, wallT), wallMat, true);

        // Calls a method.
        MakeBox(bathroom.transform, "FrontWall", new Vector3(9.5f, h2, bathZ + 2.5f), new Vector3(5f, hallH, wallT), wallMat, true);



        // Calls a method.
        MakeBox(bathroom.transform, "TileWall", new Vector3(9.5f, 1.4f, bathZ + 2.45f), new Vector3(5f, 1.6f, 0.05f), whiteTileMat, true);



        // Declares the variable stallDoorMat and initializes it.
        Material stallDoorMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        stallDoorMat.color = new Color(0.3f, 0.5f, 0.7f);

        // Starts a for loop.
        for (int i = 0; i < 3; i++)

        // Opens a new code block.
        {

            // Calls a method.
            MakeBox(bathroom.transform, "StallWall" + i, new Vector3(8.5f + i * 1.0f, h2 - 0.3f, bathZ - 1f), new Vector3(0.05f, 2.2f, 1.5f), whiteTileMat, true);

            // Calls a method.
            MakeBox(bathroom.transform, "StallDoor" + i, new Vector3(9f + i * 1.0f, h2 - 0.3f, bathZ - 0.25f), new Vector3(0.95f, 2.0f, 0.05f), stallDoorMat, true);

        // Closes the current code block.
        }


        // Declares the variable chromeMat and initializes it.
        Material chromeMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        chromeMat.color = new Color(0.7f, 0.72f, 0.75f);

        // Calls a method.
        chromeMat.SetFloat("_Glossiness", 0.9f);

        // Calls a method.
        chromeMat.SetFloat("_Metallic", 0.95f);

        // Starts a for loop.
        for (int i = 0; i < 2; i++)

        // Opens a new code block.
        {

            // Declares the variable sx and initializes it.
            float sx = 10.5f + i * 1.0f;


            // Calls a method.
            MakeBox(bathroom.transform, "Sink" + i, new Vector3(sx, 0.9f, bathZ + 2.3f), new Vector3(0.7f, 0.2f, 0.4f), whiteTileMat, true);

            // Calls a method.
            MakeBox(bathroom.transform, "SinkRim" + i, new Vector3(sx, 1.0f, bathZ + 2.28f), new Vector3(0.72f, 0.04f, 0.4f), chromeMat, true);


            // Declares the variable mFrame and initializes it.
            var mFrame = MakeBox(bathroom.transform, "MirrorFrame" + i, new Vector3(sx, 1.7f, bathZ + 2.43f), new Vector3(0.7f, 0.8f, 0.04f), chromeMat, false);

            // Calls a method.
            Object.DestroyImmediate(mFrame.GetComponent<Collider>());

            // Declares the variable mGlass and initializes it.
            var mGlass = MakeBox(bathroom.transform, "Mirror" + i, new Vector3(sx, 1.7f, bathZ + 2.41f), new Vector3(0.6f, 0.7f, 0.03f), mirrorMat, false);

            // Calls a method.
            Object.DestroyImmediate(mGlass.GetComponent<Collider>());


            // Declares the variable faucetBase and initializes it.
            var faucetBase = MakeBox(bathroom.transform, "FaucetBase" + i, new Vector3(sx, 1.04f, bathZ + 2.18f), new Vector3(0.08f, 0.1f, 0.08f), chromeMat, false);

            // Calls a method.
            Object.DestroyImmediate(faucetBase.GetComponent<Collider>());

            // Declares the variable spout and initializes it.
            GameObject spout = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            // Updates an existing value.
            spout.name = "FaucetSpout" + i;

            // Updates an existing value.
            spout.transform.parent = bathroom.transform;

            // Updates an existing value.
            spout.transform.localPosition = new Vector3(sx, 1.15f, bathZ + 2.22f);

            // Updates an existing value.
            spout.transform.localRotation = Quaternion.Euler(45, 0, 0);

            // Updates an existing value.
            spout.transform.localScale = new Vector3(0.04f, 0.12f, 0.04f);

            // Calls a method.
            spout.GetComponent<Renderer>().material = chromeMat;

            // Calls a method.
            Object.DestroyImmediate(spout.GetComponent<Collider>());

        // Closes the current code block.
        }


        // Executes this statement.
        Material[] soapColors = {

            // Calls a method.
            EmissiveMat(new Color(0.8f, 0.4f, 0.9f)),

            // Calls a method.
            EmissiveMat(new Color(0.4f, 0.8f, 0.4f))

        // Executes this statement.
        };

        // Starts a for loop.
        for (int i = 0; i < 2; i++)

        // Opens a new code block.
        {

            // Declares the variable soap and initializes it.
            GameObject soap = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            // Updates an existing value.
            soap.name = "Soap" + i;

            // Updates an existing value.
            soap.transform.parent = bathroom.transform;

            // Updates an existing value.
            soap.transform.localPosition = new Vector3(10.8f + i * 0.4f, 1.15f, bathZ + 2.3f);

            // Updates an existing value.
            soap.transform.localScale = new Vector3(0.06f, 0.1f, 0.06f);

            // Calls a method.
            soap.GetComponent<Renderer>().material = soapColors[i];

        // Closes the current code block.
        }


        // Declares the variable orangeMat and initializes it.
        Material orangeMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        orangeMat.color = new Color(1f, 0.6f, 0.1f);

        // Calls a method.
        MakeBox(bathroom.transform, "SignOOO", new Vector3(10f, 1.7f, bathZ - 0.22f), new Vector3(0.4f, 0.3f, 0.02f), orangeMat, true);


        // Calls a method.
        MakeBox(bathroom.transform, "BridgeFloor", new Vector3(7f, 0, bathZ), new Vector3(2f, 0.1f, 2.5f), floorMat, true);




        // Declares the variable lockerHandleMat and initializes it.
        Material lockerHandleMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        lockerHandleMat.color = new Color(0.7f, 0.7f, 0.75f);

        // Calls a method.
        lockerHandleMat.SetFloat("_Glossiness", 0.8f);

        // Calls a method.
        lockerHandleMat.SetFloat("_Metallic", 0.7f);

        // Starts a for loop.
        for (int i = 0; i < 8; i++)

        // Opens a new code block.
        {

            // Declares the variable lockerMat and initializes it.
            Material lockerMat = new Material(Shader.Find("Standard"));

            // Updates an existing value.
            lockerMat.color = (i % 2 == 0) ? new Color(0.18f, 0.32f, 0.55f) : new Color(0.55f, 0.25f, 0.28f);

            // Calls a method.
            lockerMat.SetFloat("_Glossiness", 0.5f);

            // Declares the variable lx2 and initializes it.
            float lx2 = -7f + i * 2f;


            // Calls a method.
            MakeBox(corridor.transform, "Locker" + i, new Vector3(lx2, 1.1f, corrZ - 1.6f), new Vector3(1.0f, 2.2f, 0.4f), lockerMat, true);


            // Declares the variable door and initializes it.
            var door = MakeBox(corridor.transform, "LockerDoor" + i, new Vector3(lx2, 1.1f, corrZ - 1.42f), new Vector3(0.9f, 2.0f, 0.05f), lockerMat, true);

            // Calls a method.
            Object.DestroyImmediate(door.GetComponent<Collider>());


            // Starts a for loop.
            for (int v = 0; v < 3; v++)

            // Opens a new code block.
            {

                // Declares the variable slit and initializes it.
                var slit = MakeBox(corridor.transform, "Vent" + i + "_" + v, new Vector3(lx2, 1.95f - v * 0.08f, corrZ - 1.39f), new Vector3(0.5f, 0.03f, 0.02f), darkMat, false);

                // Calls a method.
                Object.DestroyImmediate(slit.GetComponent<Collider>());

            // Closes the current code block.
            }


            // Declares the variable handle and initializes it.
            GameObject handle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            // Updates an existing value.
            handle.name = "Handle" + i;

            // Updates an existing value.
            handle.transform.parent = corridor.transform;

            // Updates an existing value.
            handle.transform.localPosition = new Vector3(lx2 + 0.3f, 1.1f, corrZ - 1.39f);

            // Updates an existing value.
            handle.transform.localRotation = Quaternion.Euler(90, 0, 0);

            // Updates an existing value.
            handle.transform.localScale = new Vector3(0.08f, 0.04f, 0.08f);

            // Calls a method.
            handle.GetComponent<Renderer>().material = lockerHandleMat;

            // Calls a method.
            Object.DestroyImmediate(handle.GetComponent<Collider>());

        // Closes the current code block.
        }


        // Declares the variable exitMat and initializes it.
        Material exitMat = EmissiveMat(new Color(1f, 0.2f, 0.2f));

        // Calls a method.
        MakeBox(corridor.transform, "ExitSign", new Vector3(0, 2.7f, corrZ - 1.7f), new Vector3(1.2f, 0.4f, 0.05f), exitMat, true);

    // Closes the current code block.
    }


    // Declares the method named EmissiveMat.
    static Material EmissiveMat(Color c)

    // Opens a new code block.
    {

        // Declares the variable m and initializes it.
        Material m = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        m.color = c;

        // Calls a method.
        m.EnableKeyword("_EMISSION");

        // Calls a method.
        m.SetColor("_EmissionColor", c * 1.5f);

        // Returns the specified value.
        return m;

    // Closes the current code block.
    }


    // Declares the method named BuildAlcove.
    static void BuildAlcove(Transform parent, string name, Vector3 pos, float width, float depth, float height, bool leftSide)

    // Opens a new code block.
    {


        // Calls a method.
        MakeBox(parent, name + "_Floor", pos, new Vector3(depth, 0.1f, width), floorMat, true);

        // Calls a method.
        MakeBox(parent, name + "_Ceiling", pos + new Vector3(0, height, 0), new Vector3(depth, 0.1f, width), ceilingMat, true);


        // Declares the variable side and initializes it.
        float side = leftSide ? -1f : 1f;

        // Calls a method.
        MakeBox(parent, name + "_BackWall", pos + new Vector3(side * depth/2, height/2, 0), new Vector3(0.2f, height, width), wallMat, true);


        // Calls a method.
        MakeBox(parent, name + "_SideA", pos + new Vector3(0, height/2, width/2), new Vector3(depth, height, 0.2f), wallMat, true);

        // Calls a method.
        MakeBox(parent, name + "_SideB", pos + new Vector3(0, height/2, -width/2), new Vector3(depth, height, 0.2f), wallMat, true);

    // Closes the current code block.
    }


    // Executes this statement.
    static Material doorMetalMat, doorWarnMat;


    // Declares the method named BuildDoors.
    static void BuildDoors()

    // Opens a new code block.
    {

        // Updates an existing value.
        doorMetalMat = CreateMat("DoorMetalMaterial", new Color(0.25f, 0.25f, 0.28f));

        // Updates an existing value.
        doorWarnMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        doorWarnMat.color = new Color(0.8f, 0.6f, 0.1f);

        // Calls a method.
        doorWarnMat.SetFloat("_Glossiness", 0.4f);


        // Calls a method.
        BuildSingleDoor("LeftDoor", new Vector3(-4f, 0, 0), KeyCode.E, 1f);

        // Calls a method.
        BuildSingleDoor("RightDoor", new Vector3(4f, 0, 0), KeyCode.Q, -1f);

    // Closes the current code block.
    }


    // Declares the method named BuildSingleDoor.
    static void BuildSingleDoor(string name, Vector3 pos, KeyCode key, float facing)

    // Opens a new code block.
    {

        // Declares the variable door and initializes it.
        GameObject door = new GameObject(name);

        // Updates an existing value.
        door.transform.position = pos;


        // Declares the variable panel and initializes it.
        GameObject panel = new GameObject("DoorPanel");

        // Updates an existing value.
        panel.transform.parent = door.transform;

        // Updates an existing value.
        panel.transform.localPosition = new Vector3(0, 1.3f, 0);


        // Declares the variable slab and initializes it.
        GameObject slab = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        slab.name = "Slab";

        // Updates an existing value.
        slab.transform.parent = panel.transform;

        // Updates an existing value.
        slab.transform.localPosition = Vector3.zero;

        // Updates an existing value.
        slab.transform.localScale = new Vector3(0.12f, 2.6f, 2.9f);

        // Calls a method.
        slab.GetComponent<Renderer>().material = doorMetalMat;


        // Declares the variable d and initializes it.
        float d = 0.07f * facing;



        // Starts a for loop.
        for (int i = -1; i <= 1; i++)

        // Opens a new code block.
        {

            // Declares the variable bar and initializes it.
            GameObject bar = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Updates an existing value.
            bar.name = "Bar" + i;

            // Updates an existing value.
            bar.transform.parent = panel.transform;

            // Updates an existing value.
            bar.transform.localPosition = new Vector3(d, i * 0.8f, 0);

            // Updates an existing value.
            bar.transform.localScale = new Vector3(0.03f, 0.08f, 2.9f);

            // Calls a method.
            bar.GetComponent<Renderer>().material = darkMat;

            // Calls a method.
            Object.DestroyImmediate(bar.GetComponent<Collider>());


            // Declares the variable bar2 and initializes it.
            GameObject bar2 = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Updates an existing value.
            bar2.name = "BarBack" + i;

            // Updates an existing value.
            bar2.transform.parent = panel.transform;

            // Updates an existing value.
            bar2.transform.localPosition = new Vector3(-d, i * 0.8f, 0);

            // Updates an existing value.
            bar2.transform.localScale = new Vector3(0.03f, 0.08f, 2.9f);

            // Calls a method.
            bar2.GetComponent<Renderer>().material = darkMat;

            // Calls a method.
            Object.DestroyImmediate(bar2.GetComponent<Collider>());

        // Closes the current code block.
        }



        // Declares the variable stripe and initializes it.
        GameObject stripe = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        stripe.name = "WarningStripe";

        // Updates an existing value.
        stripe.transform.parent = panel.transform;

        // Updates an existing value.
        stripe.transform.localPosition = new Vector3(d, -1.1f, 0);

        // Updates an existing value.
        stripe.transform.localScale = new Vector3(0.02f, 0.2f, 2.9f);

        // Calls a method.
        stripe.GetComponent<Renderer>().material = doorWarnMat;

        // Calls a method.
        Object.DestroyImmediate(stripe.GetComponent<Collider>());


        // Declares the variable stripe2 and initializes it.
        GameObject stripe2 = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        stripe2.name = "WarningStripeBack";

        // Updates an existing value.
        stripe2.transform.parent = panel.transform;

        // Updates an existing value.
        stripe2.transform.localPosition = new Vector3(-d, -1.1f, 0);

        // Updates an existing value.
        stripe2.transform.localScale = new Vector3(0.02f, 0.2f, 2.9f);

        // Calls a method.
        stripe2.GetComponent<Renderer>().material = doorWarnMat;

        // Calls a method.
        Object.DestroyImmediate(stripe2.GetComponent<Collider>());



        // Declares the variable windowMat and initializes it.
        Material windowMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        windowMat.color = new Color(0.1f, 0.15f, 0.2f);

        // Calls a method.
        windowMat.SetFloat("_Glossiness", 0.8f);


        // Declares the variable window and initializes it.
        GameObject window = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        window.name = "Window";

        // Updates an existing value.
        window.transform.parent = panel.transform;

        // Updates an existing value.
        window.transform.localPosition = new Vector3(d, 0.5f, 0);

        // Updates an existing value.
        window.transform.localScale = new Vector3(0.02f, 0.4f, 0.6f);

        // Calls a method.
        window.GetComponent<Renderer>().material = windowMat;

        // Calls a method.
        Object.DestroyImmediate(window.GetComponent<Collider>());


        // Declares the variable window2 and initializes it.
        GameObject window2 = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        window2.name = "WindowBack";

        // Updates an existing value.
        window2.transform.parent = panel.transform;

        // Updates an existing value.
        window2.transform.localPosition = new Vector3(-d, 0.5f, 0);

        // Updates an existing value.
        window2.transform.localScale = new Vector3(0.02f, 0.4f, 0.6f);

        // Calls a method.
        window2.GetComponent<Renderer>().material = windowMat;

        // Calls a method.
        Object.DestroyImmediate(window2.GetComponent<Collider>());


        // Declares the variable dc and initializes it.
        DoorController dc = door.AddComponent<DoorController>();

        // Updates an existing value.
        dc.toggleKey = key;

        // Updates an existing value.
        dc.isClosed = false;


        // Declares the variable obs and initializes it.
        NavMeshObstacle obs = door.AddComponent<NavMeshObstacle>();

        // Updates an existing value.
        obs.carving = true;

        // Updates an existing value.
        obs.size = new Vector3(0.5f, 3f, 3f);

        // Updates an existing value.
        obs.center = new Vector3(0, 1.5f, 0);

        // Updates an existing value.
        obs.enabled = false;

        // Updates an existing value.
        dc.doorBlocker = obs;


        // Calls a method.
        PrefabUtility.SaveAsPrefabAsset(door, "Assets/Art/Prefabs/" + name + ".prefab");

    // Closes the current code block.
    }


    // Declares the method named BuildEnemy.
    static void BuildEnemy()

    // Opens a new code block.
    {

        // Declares the variable enemy and initializes it.
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        // Updates an existing value.
        enemy.name = "Animatronic";

        // Updates an existing value.
        enemy.transform.position = new Vector3(0, 1, -11f);

        // Updates an existing value.
        enemy.transform.localScale = new Vector3(0.8f, 1.2f, 0.8f);

        // Calls a method.
        enemy.GetComponent<Renderer>().material = doorMat;


        // Declares the variable head and initializes it.
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        head.name = "Head";

        // Updates an existing value.
        head.transform.parent = enemy.transform;

        // Updates an existing value.
        head.transform.localPosition = new Vector3(0, 0.85f, 0);

        // Updates an existing value.
        head.transform.localScale = new Vector3(0.7f, 0.5f, 0.6f);

        // Calls a method.
        head.GetComponent<Renderer>().material = doorMat;

        // Calls a method.
        Object.DestroyImmediate(head.GetComponent<Collider>());



        // Starts a for loop.
        for (int i = 0; i < 2; i++)

        // Opens a new code block.
        {

            // Declares the variable ear and initializes it.
            GameObject ear = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Updates an existing value.
            ear.name = "Ear" + i;

            // Updates an existing value.
            ear.transform.parent = head.transform;

            // Declares the variable xPos and initializes it.
            float xPos = (i == 0) ? -0.45f : 0.45f;

            // Updates an existing value.
            ear.transform.localPosition = new Vector3(xPos, 0.4f, 0);

            // Updates an existing value.
            ear.transform.localScale = new Vector3(0.25f, 0.35f, 0.25f);

            // Calls a method.
            ear.GetComponent<Renderer>().material = doorMat;

            // Calls a method.
            Object.DestroyImmediate(ear.GetComponent<Collider>());

        // Closes the current code block.
        }



        // Starts a for loop.
        for (int i = 0; i < 2; i++)

        // Opens a new code block.
        {

            // Declares the variable eye and initializes it.
            GameObject eye = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Updates an existing value.
            eye.name = "Eye" + i;

            // Updates an existing value.
            eye.transform.parent = head.transform;

            // Declares the variable xPos and initializes it.
            float xPos = (i == 0) ? -0.22f : 0.22f;

            // Updates an existing value.
            eye.transform.localPosition = new Vector3(xPos, 0.05f, 0.7f);

            // Updates an existing value.
            eye.transform.localScale = new Vector3(0.2f, 0.2f, 0.1f);


            // Declares the variable eyeMat and initializes it.
            Material eyeMat = new Material(Shader.Find("Standard"));

            // Updates an existing value.
            eyeMat.color = Color.red;

            // Calls a method.
            eyeMat.EnableKeyword("_EMISSION");

            // Calls a method.
            eyeMat.SetColor("_EmissionColor", Color.red * 3f);

            // Calls a method.
            eye.GetComponent<Renderer>().material = eyeMat;

            // Calls a method.
            Object.DestroyImmediate(eye.GetComponent<Collider>());

        // Closes the current code block.
        }



        // Declares the variable mouth and initializes it.
        GameObject mouth = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        mouth.name = "Mouth";

        // Updates an existing value.
        mouth.transform.parent = head.transform;

        // Updates an existing value.
        mouth.transform.localPosition = new Vector3(0, -0.25f, 0.7f);

        // Updates an existing value.
        mouth.transform.localScale = new Vector3(0.4f, 0.15f, 0.1f);

        // Declares the variable mouthMat and initializes it.
        Material mouthMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        mouthMat.color = new Color(0.15f, 0.02f, 0.02f);

        // Calls a method.
        mouth.GetComponent<Renderer>().material = mouthMat;

        // Calls a method.
        Object.DestroyImmediate(mouth.GetComponent<Collider>());



        // Declares the variable enemyLight and initializes it.
        GameObject enemyLight = new GameObject("EnemyGlow");

        // Updates an existing value.
        enemyLight.transform.parent = enemy.transform;

        // Updates an existing value.
        enemyLight.transform.localPosition = new Vector3(0, 0.5f, 0);

        // Declares the variable glow and initializes it.
        Light glow = enemyLight.AddComponent<Light>();

        // Updates an existing value.
        glow.type = LightType.Point;

        // Updates an existing value.
        glow.color = new Color(1f, 0.2f, 0.1f);

        // Updates an existing value.
        glow.intensity = 1.2f;

        // Updates an existing value.
        glow.range = 5f;


        // Declares the variable agent and initializes it.
        NavMeshAgent agent = enemy.AddComponent<NavMeshAgent>();

        // Updates an existing value.
        agent.speed = 2f;

        // Updates an existing value.
        agent.stoppingDistance = 0.5f;

        // Updates an existing value.
        agent.radius = 0.4f;

        // Updates an existing value.
        agent.height = 2f;


        // Declares the variable ai and initializes it.
        EnemyAI ai = enemy.AddComponent<EnemyAI>();

        // Updates an existing value.
        ai.moveSpeed = 5f;


        // Declares the variable doors and initializes it.
        DoorController[] doors = Object.FindObjectsByType<DoorController>(FindObjectsSortMode.None);

        // Iterates through each item in the collection.
        foreach (DoorController dc in doors)

        // Opens a new code block.
        {

            // Checks the condition and runs the inline statement when it is true.
            if (dc.transform.position.x < 0) ai.leftDoor = dc;

            // Checks the condition and runs the inline statement when it is true.
            if (dc.transform.position.x > 0) ai.rightDoor = dc;

        // Closes the current code block.
        }


        // Calls a method.
        PrefabUtility.SaveAsPrefabAsset(enemy, "Assets/Art/Prefabs/Animatronic.prefab");

    // Closes the current code block.
    }


    // Declares the method named BuildLighting.
    static void BuildLighting()

    // Opens a new code block.
    {


        // Declares the variable allLights and initializes it.
        Light[] allLights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);

        // Iterates through each item in the collection.
        foreach (Light l in allLights)

        // Opens a new code block.
        {

            // Checks whether the condition is true.
            if (l.type == LightType.Directional)

                // Calls a method.
                Object.DestroyImmediate(l.gameObject);

        // Closes the current code block.
        }


        // Calls a method.
        CreateLight("OfficeLight", new Vector3(0, 3.2f, 0), new Color(1f, 0.9f, 0.75f), 1.2f, 12f);

        // Calls a method.
        CreateLight("MonitorGlow", new Vector3(0, 1.3f, -0.3f), new Color(0.3f, 1f, 0.3f), 0.4f, 4f);

        // Calls a method.
        CreateLight("LeftDoorLight", new Vector3(-3.5f, 2.5f, 0), new Color(1f, 0.2f, 0.2f), 1.5f, 5f);

        // Calls a method.
        CreateLight("RightDoorLight", new Vector3(3.5f, 2.5f, 0), new Color(1f, 0.2f, 0.2f), 1.5f, 5f);


        // Calls a method.
        CreateLight("LeftHallLight1", new Vector3(-5.25f, 2.5f, -2f), new Color(0.3f, 0.4f, 1f), 1.0f, 8f);

        // Calls a method.
        CreateLight("LeftHallLight2", new Vector3(-5.25f, 2.5f, -8f), new Color(0.3f, 0.4f, 1f), 0.7f, 8f);

        // Calls a method.
        CreateLight("LeftHallLight3", new Vector3(-5.25f, 2.5f, -14f), new Color(0.3f, 0.4f, 1f), 0.5f, 8f);


        // Calls a method.
        CreateLight("RightHallLight1", new Vector3(5.25f, 2.5f, -2f), new Color(0.3f, 0.4f, 1f), 1.0f, 8f);

        // Calls a method.
        CreateLight("RightHallLight2", new Vector3(5.25f, 2.5f, -8f), new Color(0.3f, 0.4f, 1f), 0.7f, 8f);

        // Calls a method.
        CreateLight("RightHallLight3", new Vector3(5.25f, 2.5f, -14f), new Color(0.3f, 0.4f, 1f), 0.5f, 8f);


        // Calls a method.
        CreateLight("CorridorLight", new Vector3(0, 2.5f, -19f), new Color(0.5f, 0.5f, 0.7f), 0.6f, 12f);

        // Calls a method.
        CreateLight("StageLight", new Vector3(0, 3f, -23f), new Color(1f, 0.8f, 0.4f), 1.2f, 8f);



        // Calls a method.
        CreateLight("ClassroomLight1", new Vector3(-9.5f, 3.2f, -10.5f), new Color(0.85f, 0.95f, 1f), 2.5f, 10f);

        // Calls a method.
        CreateLight("ClassroomLight2", new Vector3(-9.5f, 3.2f, -13.5f), new Color(0.85f, 0.95f, 1f), 2.5f, 10f);


        // Calls a method.
        CreateLight("BathroomLight1", new Vector3(9.5f, 3.2f, -10.5f), new Color(0.95f, 0.95f, 1f), 2.5f, 10f);

        // Calls a method.
        CreateLight("BathroomLight2", new Vector3(9.5f, 3.2f, -13.5f), new Color(0.95f, 0.95f, 1f), 2.5f, 10f);


        // Calls a method.
        CreateLight("StageRedLight",  new Vector3(-2.5f, 2.8f, -23f), new Color(1f, 0.2f, 0.3f),  1.5f, 8f);

        // Calls a method.
        CreateLight("StageBlueLight", new Vector3(2.5f, 2.8f, -23f),  new Color(0.3f, 0.3f, 1f),  1.5f, 8f);


        // Calls a method.
        CreateLight("CorridorLight2", new Vector3(-5f, 2.8f, -20f), new Color(0.7f, 0.7f, 0.85f), 1.2f, 10f);

        // Calls a method.
        CreateLight("CorridorLight3", new Vector3(5f, 2.8f, -20f),  new Color(0.7f, 0.7f, 0.85f), 1.2f, 10f);


        // Updates an existing value.
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

        // Updates an existing value.
        RenderSettings.ambientLight = new Color(0.04f, 0.04f, 0.06f);

        // Updates an existing value.
        RenderSettings.fog = true;

        // Updates an existing value.
        RenderSettings.fogColor = new Color(0.02f, 0.02f, 0.03f);

        // Updates an existing value.
        RenderSettings.fogMode = FogMode.Exponential;

        // Updates an existing value.
        RenderSettings.fogDensity = 0.015f;


        // Declares the variable mainCam and initializes it.
        Camera mainCam = Camera.main;

        // Checks whether the condition is true.
        if (mainCam != null)

        // Opens a new code block.
        {

            // Updates an existing value.
            mainCam.transform.position = new Vector3(0, 1.6f, 1.8f);

            // Updates an existing value.
            mainCam.transform.rotation = Quaternion.Euler(5, 180, 0);

            // Updates an existing value.
            mainCam.fieldOfView = 90f;

            // Updates an existing value.
            mainCam.backgroundColor = new Color(0.005f, 0.005f, 0.01f);

            // Updates an existing value.
            mainCam.nearClipPlane = 0.1f;

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Declares the method named CreateLight.
    static void CreateLight(string lightName, Vector3 pos, Color color, float intensity, float range)

    // Opens a new code block.
    {

        // Declares the variable lightObj and initializes it.
        GameObject lightObj = new GameObject(lightName);

        // Declares the variable light and initializes it.
        Light light = lightObj.AddComponent<Light>();

        // Updates an existing value.
        light.type = LightType.Point;

        // Updates an existing value.
        light.color = color;

        // Updates an existing value.
        light.intensity = intensity;

        // Updates an existing value.
        light.range = range;

        // Updates an existing value.
        light.shadows = LightShadows.Soft;

        // Updates an existing value.
        lightObj.transform.position = pos;

    // Closes the current code block.
    }


    // Declares the method named BuildNavMesh.
    static void BuildNavMesh()

    // Opens a new code block.
    {

        // Declares the variable navObj and initializes it.
        GameObject navObj = new GameObject("NavMeshSurface");

        // Declares the variable surface and initializes it.
        NavMeshSurface surface = navObj.AddComponent<NavMeshSurface>();

        // Updates an existing value.
        surface.collectObjects = CollectObjects.All;

        // Updates an existing value.
        surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;

        // Calls a method.
        surface.BuildNavMesh();

    // Closes the current code block.
    }


    // Declares the method named BuildGameManager.
    static void BuildGameManager()

    // Opens a new code block.
    {

        // Declares the variable gm and initializes it.
        GameObject gm = new GameObject("GameManager");

        // Declares the variable manager and initializes it.
        GameManager manager = gm.AddComponent<GameManager>();

        // Updates an existing value.
        manager.nightDuration = 120f;

        // Updates an existing value.
        manager.currentNight = 1;


        // Declares the variable enemy and initializes it.
        EnemyAI enemy = Object.FindFirstObjectByType<EnemyAI>();

        // Checks whether the condition is true.
        if (enemy != null)

            // Updates an existing value.
            enemy.gameManager = manager;



        // Declares the variable clockObj and initializes it.
        GameObject clockObj = GameObject.Find("MonitorClockText");

        // Checks whether the condition is true.
        if (clockObj != null)

        // Opens a new code block.
        {

            // Declares the variable tmp and initializes it.
            TMP_Text tmp = clockObj.GetComponent<TMP_Text>();

            // Updates an existing value.
            manager.clockText = tmp;

            // Calls a method.
            Debug.Log("[SceneBuilder] Found MonitorClockText, TMP_Text component: " + (tmp != null ? "OK" : "NULL"));

        // Closes the current code block.
        }

        // Runs the fallback branch when earlier conditions were false.
        else

        // Opens a new code block.
        {

            // Calls a method.
            Debug.LogError("[SceneBuilder] MonitorClockText GameObject not found!");

        // Closes the current code block.
        }


    // Closes the current code block.
    }


    // Declares the method named BuildMonitorClock.
    static void BuildMonitorClock()

    // Opens a new code block.
    {

        // Declares the variable clockObj and initializes it.
        GameObject clockObj = new GameObject("MonitorClockText");

        // Updates an existing value.
        clockObj.transform.position = new Vector3(1.2f, 1.15f, -0.46f);


        // Updates an existing value.
        clockObj.transform.rotation = Quaternion.Euler(0, 180, 0);


        // Declares the variable clock and initializes it.
        TextMeshPro clock = clockObj.AddComponent<TextMeshPro>();

        // Updates an existing value.
        clock.text = "12 AM";

        // Updates an existing value.
        clock.fontSize = 1.2f;

        // Updates an existing value.
        clock.color = new Color(0.3f, 1f, 0.3f);

        // Updates an existing value.
        clock.alignment = TextAlignmentOptions.Center;

        // Updates an existing value.
        clock.fontStyle = FontStyles.Bold;


        // Declares the variable rt and initializes it.
        RectTransform rt = clock.rectTransform;

        // Updates an existing value.
        rt.sizeDelta = new Vector2(0.7f, 0.4f);


        // Calls a method.
        Debug.Log("[SceneBuilder] Created MonitorClockText at " + clockObj.transform.position);

    // Closes the current code block.
    }


    // Declares the method named BuildHUD.
    static void BuildHUD()

    // Opens a new code block.
    {


        // Declares the variable canvasObj and initializes it.
        GameObject canvasObj = new GameObject("HUDCanvas");

        // Declares the variable canvas and initializes it.
        Canvas canvas = canvasObj.AddComponent<Canvas>();

        // Updates an existing value.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Calls a method.
        canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // Calls a method.
        canvasObj.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);

        // Calls a method.
        canvasObj.AddComponent<GraphicRaycaster>();



        // Declares the variable clockObj and initializes it.
        GameObject clockObj = new GameObject("ClockText");

        // Calls a method.
        clockObj.transform.SetParent(canvasObj.transform, false);

        // Declares the variable clock and initializes it.
        TMP_Text clock = clockObj.AddComponent<TextMeshProUGUI>();

        // Updates an existing value.
        clock.text = "12 AM";

        // Updates an existing value.
        clock.fontSize = 80;

        // Updates an existing value.
        clock.color = new Color(1f, 0.3f, 0.3f);

        // Updates an existing value.
        clock.alignment = TextAlignmentOptions.Right;

        // Updates an existing value.
        clock.fontStyle = FontStyles.Bold;

        // Declares the variable clockRT and initializes it.
        RectTransform clockRT = clock.rectTransform;

        // Updates an existing value.
        clockRT.anchorMin = new Vector2(1, 1);

        // Updates an existing value.
        clockRT.anchorMax = new Vector2(1, 1);

        // Updates an existing value.
        clockRT.pivot = new Vector2(1, 1);

        // Updates an existing value.
        clockRT.anchoredPosition = new Vector2(-40, -40);

        // Updates an existing value.
        clockRT.sizeDelta = new Vector2(300, 100);



        // Declares the variable nightObj and initializes it.
        GameObject nightObj = new GameObject("NightText");

        // Calls a method.
        nightObj.transform.SetParent(canvasObj.transform, false);

        // Declares the variable night and initializes it.
        TMP_Text night = nightObj.AddComponent<TextMeshProUGUI>();

        // Updates an existing value.
        night.text = "Night 1";

        // Updates an existing value.
        night.fontSize = 50;

        // Updates an existing value.
        night.color = new Color(0.9f, 0.9f, 0.9f);

        // Updates an existing value.
        night.alignment = TextAlignmentOptions.Left;

        // Updates an existing value.
        night.fontStyle = FontStyles.Bold;

        // Declares the variable nightRT and initializes it.
        RectTransform nightRT = night.rectTransform;

        // Updates an existing value.
        nightRT.anchorMin = new Vector2(0, 1);

        // Updates an existing value.
        nightRT.anchorMax = new Vector2(0, 1);

        // Updates an existing value.
        nightRT.pivot = new Vector2(0, 1);

        // Updates an existing value.
        nightRT.anchoredPosition = new Vector2(40, -40);

        // Updates an existing value.
        nightRT.sizeDelta = new Vector2(300, 70);



        // Declares the variable winPanel and initializes it.
        GameObject winPanel = new GameObject("WinPanel");

        // Calls a method.
        winPanel.transform.SetParent(canvasObj.transform, false);

        // Declares the variable winBG and initializes it.
        Image winBG = winPanel.AddComponent<Image>();

        // Updates an existing value.
        winBG.color = new Color(0f, 0f, 0f, 0.85f);

        // Declares the variable winRT and initializes it.
        RectTransform winRT = winBG.rectTransform;

        // Updates an existing value.
        winRT.anchorMin = Vector2.zero;

        // Updates an existing value.
        winRT.anchorMax = Vector2.one;

        // Updates an existing value.
        winRT.offsetMin = Vector2.zero;

        // Updates an existing value.
        winRT.offsetMax = Vector2.zero;



        // Declares the variable winTitleObj and initializes it.
        GameObject winTitleObj = new GameObject("WinTitle");

        // Calls a method.
        winTitleObj.transform.SetParent(winPanel.transform, false);

        // Declares the variable winTitle and initializes it.
        TMP_Text winTitle = winTitleObj.AddComponent<TextMeshProUGUI>();

        // Updates an existing value.
        winTitle.text = "6 AM";

        // Updates an existing value.
        winTitle.fontSize = 200;

        // Updates an existing value.
        winTitle.color = new Color(0.9f, 0.6f, 0.2f);

        // Updates an existing value.
        winTitle.alignment = TextAlignmentOptions.Center;

        // Updates an existing value.
        winTitle.fontStyle = FontStyles.Bold;

        // Declares the variable winTitleRT and initializes it.
        RectTransform winTitleRT = winTitle.rectTransform;

        // Updates an existing value.
        winTitleRT.anchorMin = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        winTitleRT.anchorMax = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        winTitleRT.pivot = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        winTitleRT.anchoredPosition = new Vector2(0, 100);

        // Updates an existing value.
        winTitleRT.sizeDelta = new Vector2(800, 250);



        // Declares the variable winSubObj and initializes it.
        GameObject winSubObj = new GameObject("WinSubtitle");

        // Calls a method.
        winSubObj.transform.SetParent(winPanel.transform, false);

        // Declares the variable winSub and initializes it.
        TMP_Text winSub = winSubObj.AddComponent<TextMeshProUGUI>();

        // Updates an existing value.
        winSub.text = "YOU SURVIVED THE NIGHT";

        // Updates an existing value.
        winSub.fontSize = 60;

        // Updates an existing value.
        winSub.color = new Color(0.95f, 0.95f, 0.95f);

        // Updates an existing value.
        winSub.alignment = TextAlignmentOptions.Center;

        // Declares the variable winSubRT and initializes it.
        RectTransform winSubRT = winSub.rectTransform;

        // Updates an existing value.
        winSubRT.anchorMin = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        winSubRT.anchorMax = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        winSubRT.pivot = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        winSubRT.anchoredPosition = new Vector2(0, -50);

        // Updates an existing value.
        winSubRT.sizeDelta = new Vector2(1200, 100);


        // Calls a method.
        winPanel.SetActive(false);

    // Closes the current code block.
    }


    // Declares the method named TestNavMesh.
    static void TestNavMesh()

    // Opens a new code block.
    {

        // Declares the variable office and initializes it.
        Vector3 office = new Vector3(0, 0.5f, 0);

        // Declares the variable leftHall and initializes it.
        Vector3 leftHall = new Vector3(-5.25f, 0.5f, -5f);

        // Declares the variable rightHall and initializes it.
        Vector3 rightHall = new Vector3(5.25f, 0.5f, -5f);

        // Declares the variable leftDoor and initializes it.
        Vector3 leftDoor = new Vector3(-4f, 0.5f, 0);

        // Declares the variable rightDoor and initializes it.
        Vector3 rightDoor = new Vector3(4f, 0.5f, 0);

        // Declares the variable stage and initializes it.
        Vector3 stage = new Vector3(-5.25f, 0.5f, -10f);

        // Declares the variable corridor and initializes it.
        Vector3 corridor = new Vector3(0, 0.5f, -11f);


        // Calls a method.
        TestPath("Office -> Left Door", office, leftDoor);

        // Calls a method.
        TestPath("Office -> Right Door", office, rightDoor);

        // Calls a method.
        TestPath("Left Hall -> Left Door", leftHall, leftDoor);

        // Calls a method.
        TestPath("Right Hall -> Right Door", rightHall, rightDoor);

        // Calls a method.
        TestPath("Left Hall -> Right Hall", leftHall, rightHall);

        // Calls a method.
        TestPath("Left Hall -> Corridor", leftHall, corridor);

        // Calls a method.
        TestPath("Right Hall -> Corridor", rightHall, corridor);

        // Calls a method.
        TestPath("Stage -> Left Door", stage, leftDoor);

        // Calls a method.
        TestPath("Stage -> Right Door", stage, rightDoor);

    // Closes the current code block.
    }


    // Declares the method named TestPath.
    static void TestPath(string name, Vector3 from, Vector3 to)

    // Opens a new code block.
    {

        // Declares the variable path and initializes it.
        NavMeshPath path = new NavMeshPath();

        // Calls a method.
        NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path);

    // Closes the current code block.
    }

// Closes the current code block.
}
