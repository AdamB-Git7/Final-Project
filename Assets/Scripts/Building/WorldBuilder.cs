
// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.AI namespace.
using UnityEngine.AI;

// Imports the Unity.AI.Navigation namespace.
using Unity.AI.Navigation;

// Imports the TMPro namespace.
using TMPro;


// Declares the class named WorldBuilder.
public class WorldBuilder : MonoBehaviour

// Opens a new code block.
{

    // Executes this statement.
    Material wallMat, floorMat, ceilingMat, doorMat, darkMat, tileMat, doorMetalMat, doorWarnMat;


    // Declares the method named BuildAll.
    public void BuildAll()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (GameObject.Find("SecurityOffice") != null) return;


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

    // Closes the current code block.
    }



    // Executes this statement.
    public static GameObject Corridor, LeftAlcove, RightAlcove, Stage, Classroom, Bathroom;

    // Executes this statement.
    public static GameObject LeftDoorSpot, RightDoorSpot, OfficeCenter;


    // Declares the method named BuildSpots.
    void BuildSpots()

    // Opens a new code block.
    {


        // Declares the variable parent and initializes it.
        GameObject parent = new GameObject("AISpots");

        // Updates an existing value.
        Corridor      = MakeSpot(parent, "Spot_Corridor",      new Vector3( 0f,    0.5f, -19f));

        // Updates an existing value.
        LeftAlcove    = MakeSpot(parent, "Spot_LeftAlcove",    new Vector3(-7.25f, 0.5f, -7f));

        // Updates an existing value.
        RightAlcove   = MakeSpot(parent, "Spot_RightAlcove",   new Vector3( 7.25f, 0.5f, -7f));

        // Updates an existing value.
        Stage         = MakeSpot(parent, "Spot_Stage",         new Vector3( 0f,    0.5f, -23f));

        // Updates an existing value.
        Classroom     = MakeSpot(parent, "Spot_Classroom",     new Vector3(-9.5f,  0.5f, -12f));

        // Updates an existing value.
        Bathroom      = MakeSpot(parent, "Spot_Bathroom",      new Vector3( 9.5f,  0.5f, -12f));

        // Updates an existing value.
        LeftDoorSpot  = MakeSpot(parent, "Spot_LeftDoor",      new Vector3(-5.25f, 0.5f,  0f));

        // Updates an existing value.
        RightDoorSpot = MakeSpot(parent, "Spot_RightDoor",     new Vector3( 5.25f, 0.5f,  0f));

        // Updates an existing value.
        OfficeCenter  = MakeSpot(parent, "Spot_OfficeCenter",  new Vector3( 0f,    0.5f,  1f));

    // Closes the current code block.
    }


    // Declares the method named MakeSpot.
    GameObject MakeSpot(GameObject parent, string name, Vector3 pos)

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject(name);

        // Updates an existing value.
        obj.transform.parent = parent.transform;

        // Updates an existing value.
        obj.transform.position = pos;

        // Returns the specified value.
        return obj;

    // Closes the current code block.
    }


    // Declares the method named WireAI.
    public static void WireAI(EnemyAI ai)

    // Opens a new code block.
    {



        // Checks the condition and runs the inline statement when it is true.
        if (Corridor != null)      ai.corridor      = Corridor.transform;

        // Checks the condition and runs the inline statement when it is true.
        if (LeftAlcove != null)    ai.leftAlcove    = LeftAlcove.transform;

        // Checks the condition and runs the inline statement when it is true.
        if (RightAlcove != null)   ai.rightAlcove   = RightAlcove.transform;

        // Checks the condition and runs the inline statement when it is true.
        if (Stage != null)         ai.stage         = Stage.transform;

        // Checks the condition and runs the inline statement when it is true.
        if (Classroom != null)     ai.classroom     = Classroom.transform;

        // Checks the condition and runs the inline statement when it is true.
        if (Bathroom != null)      ai.bathroom      = Bathroom.transform;

        // Checks the condition and runs the inline statement when it is true.
        if (LeftDoorSpot != null)  ai.leftDoorSpot  = LeftDoorSpot.transform;

        // Checks the condition and runs the inline statement when it is true.
        if (RightDoorSpot != null) ai.rightDoorSpot = RightDoorSpot.transform;

        // Checks the condition and runs the inline statement when it is true.
        if (OfficeCenter != null)  ai.officeCenter  = OfficeCenter.transform;

    // Closes the current code block.
    }


    // Declares the method named CreateMaterials.
    void CreateMaterials()

    // Opens a new code block.
    {

        // Updates an existing value.
        wallMat = MakeMat(new Color(0.18f, 0.16f, 0.22f));

        // Updates an existing value.
        floorMat = MakeMat(new Color(0.08f, 0.08f, 0.1f));

        // Updates an existing value.
        ceilingMat = MakeMat(new Color(0.06f, 0.06f, 0.08f));

        // Updates an existing value.
        doorMat = MakeMat(new Color(0.35f, 0.1f, 0.1f));

        // Updates an existing value.
        darkMat = MakeMat(new Color(0.04f, 0.04f, 0.06f));

        // Updates an existing value.
        tileMat = MakeMat(new Color(0.12f, 0.12f, 0.16f));

        // Updates an existing value.
        doorMetalMat = MakeMat(new Color(0.25f, 0.25f, 0.28f));

        // Updates an existing value.
        doorWarnMat = MakeMat(new Color(0.8f, 0.6f, 0.1f));

    // Closes the current code block.
    }


    // Declares the method named MakeMat.
    Material MakeMat(Color c)

    // Opens a new code block.
    {

        // Declares the variable m and initializes it.
        Material m = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        m.color = c;

        // Calls a method.
        m.SetFloat("_Glossiness", 0.15f);

        // Returns the specified value.
        return m;

    // Closes the current code block.
    }


    // Declares the method named MakeBox.
    GameObject MakeBox(Transform parent, string n, Vector3 pos, Vector3 scale, Material mat)

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        obj.name = n;

        // Checks the condition and runs the inline statement when it is true.
        if (parent != null) obj.transform.parent = parent;

        // Updates an existing value.
        obj.transform.localPosition = pos;

        // Updates an existing value.
        obj.transform.localScale = scale;

        // Calls a method.
        obj.GetComponent<Renderer>().material = mat;

        // Returns the specified value.
        return obj;

    // Closes the current code block.
    }


    // Declares the method named BuildOffice.
    void BuildOffice()

    // Opens a new code block.
    {

        // Declares the variable office and initializes it.
        GameObject office = new GameObject("SecurityOffice");

        // Declares the variable roomW and initializes it.
        float roomW = 8f, roomD = 5f, roomH = 3.5f, wallT = 0.2f;

        // Declares the variable doorGap and initializes it.
        float doorGap = 1.5f;


        // Calls a method.
        MakeBox(office.transform, "Floor", Vector3.zero, new Vector3(roomW, 0.1f, roomD), floorMat);

        // Calls a method.
        MakeBox(office.transform, "Ceiling", new Vector3(0, roomH, 0), new Vector3(roomW, 0.1f, roomD), ceilingMat);

        // Calls a method.
        MakeBox(office.transform, "BackWall", new Vector3(0, roomH/2, -roomD/2), new Vector3(roomW, roomH, wallT), wallMat);

        // Calls a method.
        MakeBox(office.transform, "FrontWall", new Vector3(0, roomH/2, roomD/2), new Vector3(roomW, roomH, wallT), wallMat);



        // Calls a method.
        BuildWallWithWindow(office.transform, "LeftWall", -roomW/2, roomH, wallT, doorGap, roomD);

        // Calls a method.
        BuildWallWithWindow(office.transform, "RightWall", roomW/2, roomH, wallT, doorGap, roomD);



        // Calls a method.
        MakeBox(office.transform, "LeftBridge", new Vector3(-roomW/2, 0, 0), new Vector3(1.5f, 0.1f, doorGap*2), floorMat);

        // Calls a method.
        MakeBox(office.transform, "RightBridge", new Vector3(roomW/2, 0, 0), new Vector3(1.5f, 0.1f, doorGap*2), floorMat);



        // Calls a method.
        MakeBox(office.transform, "Desk", new Vector3(0, 0.75f, -0.2f), new Vector3(5f, 0.08f, 1.4f), darkMat);

        // Declares the variable deskFront and initializes it.
        var deskFront = MakeBox(office.transform, "DeskFront", new Vector3(0, 0.38f, 0.5f), new Vector3(5f, 0.75f, 0.06f), darkMat);

        // Calls a method.
        Object.DestroyImmediate(deskFront.GetComponent<Collider>());



        // Starts a for loop.
        for (int i = -1; i <= 1; i++)

        // Opens a new code block.
        {

            // Calls a method.
            MakeBox(office.transform, "Monitor" + (i + 2), new Vector3(i * 1.2f, 1.15f, -0.5f), new Vector3(0.9f, 0.6f, 0.05f), darkMat);

            // Declares the variable screen and initializes it.
            GameObject screen = MakeBox(office.transform, "Screen" + (i + 2), new Vector3(i * 1.2f, 1.15f, -0.48f), new Vector3(0.8f, 0.5f, 0.01f), darkMat);

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
        MakeBox(office.transform, "ChairSeat", new Vector3(0, 0.45f, 1.3f), new Vector3(0.7f, 0.06f, 0.7f), doorMat);

        // Calls a method.
        MakeBox(office.transform, "ChairBack", new Vector3(0, 0.8f, 1.65f), new Vector3(0.7f, 0.7f, 0.06f), doorMat);

    // Closes the current code block.
    }


    // Declares the method named BuildHallways.
    void BuildHallways()

    // Opens a new code block.
    {

        // Declares the variable hallW and initializes it.
        float hallW = 2.5f, hallH = 3.5f, wallT = 0.2f, h2 = hallH/2f;

        // Declares the variable lx and initializes it.
        float lx = -5.25f, rx = 5.25f;

        // Declares the variable hallLen and initializes it.
        float hallLen = 22f, hallCenter = -9f;



        // Declares the variable leftHall and initializes it.
        GameObject leftHall = new GameObject("LeftHallway");

        // Calls a method.
        MakeBox(leftHall.transform, "Floor", new Vector3(lx, 0, hallCenter), new Vector3(hallW, 0.1f, hallLen), floorMat);

        // Calls a method.
        MakeBox(leftHall.transform, "Ceiling", new Vector3(lx, hallH, hallCenter), new Vector3(hallW, 0.1f, hallLen), ceilingMat);

        // Calls a method.
        MakeBox(leftHall.transform, "OuterWall_A", new Vector3(lx - hallW/2, h2, -2f), new Vector3(wallT, hallH, 8f), wallMat);

        // Calls a method.
        MakeBox(leftHall.transform, "OuterWall_B", new Vector3(lx - hallW/2, h2, -17.5f), new Vector3(wallT, hallH, 5f), wallMat);

        // Calls a method.
        MakeBox(leftHall.transform, "InnerWall_Front", new Vector3(lx + hallW/2, h2, 2.75f), new Vector3(wallT, hallH, 2.5f), wallMat);

        // Calls a method.
        MakeBox(leftHall.transform, "InnerWall_Back", new Vector3(lx + hallW/2, h2, -10f), new Vector3(wallT, hallH, 14f), wallMat);

        // Calls a method.
        BuildAlcove(leftHall.transform, "LeftAlcove", new Vector3(lx - hallW/2 - 1f, 0, -7f), 2f, 2f, hallH, true);



        // Declares the variable rightHall and initializes it.
        GameObject rightHall = new GameObject("RightHallway");

        // Calls a method.
        MakeBox(rightHall.transform, "Floor", new Vector3(rx, 0, hallCenter), new Vector3(hallW, 0.1f, hallLen), floorMat);

        // Calls a method.
        MakeBox(rightHall.transform, "Ceiling", new Vector3(rx, hallH, hallCenter), new Vector3(hallW, 0.1f, hallLen), ceilingMat);

        // Calls a method.
        MakeBox(rightHall.transform, "OuterWall_A", new Vector3(rx + hallW/2, h2, -2f), new Vector3(wallT, hallH, 8f), wallMat);

        // Calls a method.
        MakeBox(rightHall.transform, "OuterWall_B", new Vector3(rx + hallW/2, h2, -17.5f), new Vector3(wallT, hallH, 5f), wallMat);

        // Calls a method.
        MakeBox(rightHall.transform, "InnerWall_Front", new Vector3(rx - hallW/2, h2, 2.75f), new Vector3(wallT, hallH, 2.5f), wallMat);

        // Calls a method.
        MakeBox(rightHall.transform, "InnerWall_Back", new Vector3(rx - hallW/2, h2, -10f), new Vector3(wallT, hallH, 14f), wallMat);

        // Calls a method.
        BuildAlcove(rightHall.transform, "RightAlcove", new Vector3(rx + hallW/2 + 1f, 0, -7f), 2f, 2f, hallH, false);



        // Declares the variable corridor and initializes it.
        GameObject corridor = new GameObject("BackCorridor");

        // Declares the variable corrZ and initializes it.
        float corrZ = -19f;

        // Calls a method.
        MakeBox(corridor.transform, "Floor", new Vector3(0, 0, corrZ), new Vector3(18f, 0.1f, 3.5f), floorMat);

        // Calls a method.
        MakeBox(corridor.transform, "Ceiling", new Vector3(0, hallH, corrZ), new Vector3(18f, 0.1f, 3.5f), ceilingMat);

        // Calls a method.
        MakeBox(corridor.transform, "FarWall_L", new Vector3(-5.5f, h2, corrZ - 1.75f), new Vector3(7f, hallH, wallT), wallMat);

        // Calls a method.
        MakeBox(corridor.transform, "FarWall_R", new Vector3(5.5f, h2, corrZ - 1.75f), new Vector3(7f, hallH, wallT), wallMat);

        // Calls a method.
        MakeBox(corridor.transform, "LeftWall", new Vector3(-9f, h2, corrZ), new Vector3(wallT, hallH, 3.5f), wallMat);

        // Calls a method.
        MakeBox(corridor.transform, "RightWall", new Vector3(9f, h2, corrZ), new Vector3(wallT, hallH, 3.5f), wallMat);



        // Declares the variable stage and initializes it.
        GameObject stage = new GameObject("StageRoom");

        // Declares the variable stageZ and initializes it.
        float stageZ = -23f;

        // Calls a method.
        MakeBox(stage.transform, "Floor", new Vector3(0, 0, stageZ), new Vector3(8f, 0.1f, 5f), tileMat);

        // Calls a method.
        MakeBox(stage.transform, "Ceiling", new Vector3(0, hallH, stageZ), new Vector3(8f, 0.1f, 5f), ceilingMat);

        // Calls a method.
        MakeBox(stage.transform, "BackWall", new Vector3(0, h2, stageZ - 2.5f), new Vector3(8f, hallH, wallT), wallMat);

        // Calls a method.
        MakeBox(stage.transform, "LeftWall", new Vector3(-4f, h2, stageZ), new Vector3(wallT, hallH, 5f), wallMat);

        // Calls a method.
        MakeBox(stage.transform, "RightWall", new Vector3(4f, h2, stageZ), new Vector3(wallT, hallH, 5f), wallMat);

        // Calls a method.
        MakeBox(stage.transform, "Platform", new Vector3(0, 0.2f, stageZ - 1.5f), new Vector3(5f, 0.4f, 1.5f), darkMat);



        // Declares the variable classroom and initializes it.
        GameObject classroom = new GameObject("Classroom");

        // Declares the variable classZ and initializes it.
        float classZ = -12f;

        // Calls a method.
        MakeBox(classroom.transform, "Floor", new Vector3(-9.5f, 0, classZ), new Vector3(5f, 0.1f, 5f), floorMat);

        // Calls a method.
        MakeBox(classroom.transform, "Ceiling", new Vector3(-9.5f, hallH, classZ), new Vector3(5f, 0.1f, 5f), ceilingMat);

        // Calls a method.
        MakeBox(classroom.transform, "FarWall", new Vector3(-12f, h2, classZ), new Vector3(wallT, hallH, 5f), wallMat);

        // Calls a method.
        MakeBox(classroom.transform, "BackWall", new Vector3(-9.5f, h2, classZ - 2.5f), new Vector3(5f, hallH, wallT), wallMat);

        // Calls a method.
        MakeBox(classroom.transform, "FrontWall", new Vector3(-9.5f, h2, classZ + 2.5f), new Vector3(5f, hallH, wallT), wallMat);


        // Starts a for loop.
        for (int i = 0; i < 3; i++)

            // Calls a method.
            MakeBox(classroom.transform, "Desk" + i, new Vector3(-10.5f + i * 1.0f, 0.6f, classZ), new Vector3(0.8f, 0.08f, 0.6f), darkMat);

        // Calls a method.
        MakeBox(classroom.transform, "Chalkboard", new Vector3(-11.85f, 1.8f, classZ), new Vector3(0.05f, 1.4f, 3f), darkMat);



        // Declares the variable bathroom and initializes it.
        GameObject bathroom = new GameObject("Bathroom");

        // Declares the variable bathZ and initializes it.
        float bathZ = -12f;

        // Declares the variable whiteMat and initializes it.
        Material whiteMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        whiteMat.color = new Color(0.85f, 0.85f, 0.88f);

        // Calls a method.
        MakeBox(bathroom.transform, "Floor", new Vector3(9.5f, 0, bathZ), new Vector3(5f, 0.1f, 5f), tileMat);

        // Calls a method.
        MakeBox(bathroom.transform, "Ceiling", new Vector3(9.5f, hallH, bathZ), new Vector3(5f, 0.1f, 5f), ceilingMat);

        // Calls a method.
        MakeBox(bathroom.transform, "FarWall", new Vector3(12f, h2, bathZ), new Vector3(wallT, hallH, 5f), wallMat);

        // Calls a method.
        MakeBox(bathroom.transform, "BackWall", new Vector3(9.5f, h2, bathZ - 2.5f), new Vector3(5f, hallH, wallT), wallMat);

        // Calls a method.
        MakeBox(bathroom.transform, "FrontWall", new Vector3(9.5f, h2, bathZ + 2.5f), new Vector3(5f, hallH, wallT), wallMat);


        // Starts a for loop.
        for (int i = 0; i < 3; i++)

            // Calls a method.
            MakeBox(bathroom.transform, "StallWall" + i, new Vector3(8.5f + i * 1.0f, h2 - 0.3f, bathZ - 1f), new Vector3(0.05f, 2.2f, 1.5f), whiteMat);

        // Starts a for loop.
        for (int i = 0; i < 2; i++)

            // Calls a method.
            MakeBox(bathroom.transform, "Sink" + i, new Vector3(10.5f + i * 1.0f, 0.9f, bathZ + 2.3f), new Vector3(0.7f, 0.2f, 0.4f), whiteMat);






        // Calls a method.
        MakeBox(classroom.transform, "BridgeFloor", new Vector3(-7f, 0, classZ), new Vector3(2f, 0.1f, 2.5f), floorMat);

        // Calls a method.
        MakeBox(bathroom.transform, "BridgeFloor", new Vector3(7f, 0, bathZ), new Vector3(2f, 0.1f, 2.5f), floorMat);

    // Closes the current code block.
    }


    // Declares the method named BuildWallWithWindow.
    void BuildWallWithWindow(Transform parent, string name, float x, float roomH, float wallT, float doorGap, float roomD)

    // Opens a new code block.
    {


        // Declares the variable winBot and initializes it.
        float winBot = 1.4f;

        // Declares the variable winTop and initializes it.
        float winTop = 2.1f;



        // Declares the variable frontZ and initializes it.
        float frontZ = doorGap + 0.5f;

        // Declares the variable frontDepth and initializes it.
        float frontDepth = roomD/2 - doorGap;



        // Calls a method.
        MakeBox(parent, name + "_FrontBottom", new Vector3(x, winBot/2, frontZ), new Vector3(wallT, winBot, frontDepth), wallMat);


        // Calls a method.
        MakeBox(parent, name + "_FrontTop", new Vector3(x, (winTop + roomH)/2, frontZ), new Vector3(wallT, roomH - winTop, frontDepth), wallMat);





        // Declares the variable backZ and initializes it.
        float backZ = -(doorGap + 0.5f);

        // Calls a method.
        MakeBox(parent, name + "_BackBottom", new Vector3(x, winBot/2, backZ), new Vector3(wallT, winBot, frontDepth), wallMat);

        // Calls a method.
        MakeBox(parent, name + "_BackTop", new Vector3(x, (winTop + roomH)/2, backZ), new Vector3(wallT, roomH - winTop, frontDepth), wallMat);



        // Declares the variable glassMat and initializes it.
        Material glassMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        glassMat.color = new Color(0.4f, 0.5f, 0.6f, 0.15f);

        // Calls a method.
        glassMat.SetFloat("_Mode", 3);

        // Calls a method.
        glassMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);

        // Calls a method.
        glassMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);

        // Calls a method.
        glassMat.SetInt("_ZWrite", 0);

        // Calls a method.
        glassMat.DisableKeyword("_ALPHATEST_ON");

        // Calls a method.
        glassMat.EnableKeyword("_ALPHABLEND_ON");

        // Updates an existing value.
        glassMat.renderQueue = 3000;


        // Declares the variable glassFront and initializes it.
        var glassFront = MakeBox(parent, name + "_GlassFront", new Vector3(x, (winBot + winTop)/2, frontZ), new Vector3(0.05f, winTop - winBot, frontDepth), glassMat);

        // Calls a method.
        Object.DestroyImmediate(glassFront.GetComponent<Collider>());

        // Declares the variable glassBack and initializes it.
        var glassBack = MakeBox(parent, name + "_GlassBack", new Vector3(x, (winBot + winTop)/2, backZ), new Vector3(0.05f, winTop - winBot, frontDepth), glassMat);

        // Calls a method.
        Object.DestroyImmediate(glassBack.GetComponent<Collider>());

    // Closes the current code block.
    }


    // Declares the method named BuildAlcove.
    void BuildAlcove(Transform parent, string name, Vector3 pos, float width, float depth, float height, bool leftSide)

    // Opens a new code block.
    {

        // Calls a method.
        MakeBox(parent, name + "_Floor", pos, new Vector3(depth, 0.1f, width), floorMat);

        // Calls a method.
        MakeBox(parent, name + "_Ceiling", pos + new Vector3(0, height, 0), new Vector3(depth, 0.1f, width), ceilingMat);

        // Declares the variable side and initializes it.
        float side = leftSide ? -1f : 1f;

        // Calls a method.
        MakeBox(parent, name + "_BackWall", pos + new Vector3(side * depth/2, height/2, 0), new Vector3(0.2f, height, width), wallMat);

        // Calls a method.
        MakeBox(parent, name + "_SideA", pos + new Vector3(0, height/2, width/2), new Vector3(depth, height, 0.2f), wallMat);

        // Calls a method.
        MakeBox(parent, name + "_SideB", pos + new Vector3(0, height/2, -width/2), new Vector3(depth, height, 0.2f), wallMat);

    // Closes the current code block.
    }


    // Declares the method named BuildLighting.
    void BuildLighting()

    // Opens a new code block.
    {


        // Declares the variable allLights and initializes it.
        Light[] allLights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);

        // Iterates through each item in the collection.
        foreach (Light l in allLights)

            // Checks whether the condition is true.
            if (l.type == LightType.Directional)

                // Calls a method.
                Object.DestroyImmediate(l.gameObject);


        // Calls a method.
        AddLight("OfficeLight", new Vector3(0, 3.2f, 0), new Color(1f, 0.9f, 0.75f), 1.2f, 12f);

        // Calls a method.
        AddLight("LeftDoorLight", new Vector3(-3.5f, 2.5f, 0), new Color(1f, 0.2f, 0.2f), 1.5f, 5f);

        // Calls a method.
        AddLight("RightDoorLight", new Vector3(3.5f, 2.5f, 0), new Color(1f, 0.2f, 0.2f), 1.5f, 5f);


        // Calls a method.
        AddLight("LeftHallLight1", new Vector3(-5.25f, 2.5f, -2f), new Color(0.3f, 0.4f, 1f), 1.0f, 8f);

        // Calls a method.
        AddLight("LeftHallLight2", new Vector3(-5.25f, 2.5f, -8f), new Color(0.3f, 0.4f, 1f), 0.7f, 8f);

        // Calls a method.
        AddLight("LeftHallLight3", new Vector3(-5.25f, 2.5f, -14f), new Color(0.3f, 0.4f, 1f), 0.5f, 8f);


        // Calls a method.
        AddLight("RightHallLight1", new Vector3(5.25f, 2.5f, -2f), new Color(0.3f, 0.4f, 1f), 1.0f, 8f);

        // Calls a method.
        AddLight("RightHallLight2", new Vector3(5.25f, 2.5f, -8f), new Color(0.3f, 0.4f, 1f), 0.7f, 8f);

        // Calls a method.
        AddLight("RightHallLight3", new Vector3(5.25f, 2.5f, -14f), new Color(0.3f, 0.4f, 1f), 0.5f, 8f);


        // Calls a method.
        AddLight("CorridorLight", new Vector3(0, 2.5f, -19f), new Color(0.5f, 0.5f, 0.7f), 0.6f, 12f);

        // Calls a method.
        AddLight("StageLight", new Vector3(0, 3f, -23f), new Color(1f, 0.8f, 0.4f), 1.2f, 8f);


        // Updates an existing value.
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

        // Updates an existing value.
        RenderSettings.ambientLight = new Color(0.04f, 0.04f, 0.06f);

        // Updates an existing value.
        RenderSettings.fog = true;

        // Updates an existing value.
        RenderSettings.fogColor = new Color(0.01f, 0.01f, 0.02f);

        // Updates an existing value.
        RenderSettings.fogMode = FogMode.Exponential;

        // Updates an existing value.
        RenderSettings.fogDensity = 0.025f;



        // Declares the variable mainCam and initializes it.
        Camera mainCam = Camera.main;

        // Checks whether the condition is true.
        if (mainCam == null)

        // Opens a new code block.
        {

            // Declares the variable camObj and initializes it.
            GameObject camObj = new GameObject("Main Camera");

            // Updates an existing value.
            camObj.tag = "MainCamera";

            // Updates an existing value.
            mainCam = camObj.AddComponent<Camera>();

            // Calls a method.
            camObj.AddComponent<AudioListener>();

        // Closes the current code block.
        }

        // Updates an existing value.
        mainCam.transform.position = new Vector3(0, 1.6f, 1.8f);

        // Updates an existing value.
        mainCam.transform.rotation = Quaternion.Euler(5, 180, 0);

        // Updates an existing value.
        mainCam.fieldOfView = 90f;

        // Updates an existing value.
        mainCam.backgroundColor = new Color(0.005f, 0.005f, 0.01f);

        // Updates an existing value.
        mainCam.clearFlags = CameraClearFlags.SolidColor;

        // Updates an existing value.
        mainCam.nearClipPlane = 0.1f;

    // Closes the current code block.
    }


    // Declares the method named AddLight.
    void AddLight(string n, Vector3 pos, Color c, float intensity, float range)

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject(n);

        // Updates an existing value.
        obj.transform.position = pos;

        // Declares the variable light and initializes it.
        Light light = obj.AddComponent<Light>();

        // Updates an existing value.
        light.type = LightType.Point;

        // Updates an existing value.
        light.color = c;

        // Updates an existing value.
        light.intensity = intensity;

        // Updates an existing value.
        light.range = range;

        // Updates an existing value.
        light.shadows = LightShadows.Soft;

    // Closes the current code block.
    }


    // Declares the method named BuildNavMesh.
    void BuildNavMesh()

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


    // Declares the method named BuildDoors.
    void BuildDoors()

    // Opens a new code block.
    {

        // Calls a method.
        BuildSingleDoor("LeftDoor", new Vector3(-4f, 0, 0), KeyCode.E, 1f);

        // Calls a method.
        BuildSingleDoor("RightDoor", new Vector3(4f, 0, 0), KeyCode.Q, -1f);

    // Closes the current code block.
    }


    // Declares the method named BuildSingleDoor.
    void BuildSingleDoor(string name, Vector3 pos, KeyCode key, float facing)

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

    // Closes the current code block.
    }


    // Declares the method named BuildClownEnemy.
    public EnemyAI BuildClownEnemy(Vector3 position)

    // Opens a new code block.
    {

        // Declares the variable whiteSkin and initializes it.
        Material whiteSkin = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        whiteSkin.color = new Color(0.95f, 0.92f, 0.88f);


        // Declares the variable redMat and initializes it.
        Material redMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        redMat.color = new Color(0.85f, 0.1f, 0.15f);


        // Declares the variable yellowMat and initializes it.
        Material yellowMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        yellowMat.color = new Color(1f, 0.85f, 0.1f);


        // Declares the variable purpleMat and initializes it.
        Material purpleMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        purpleMat.color = new Color(0.4f, 0.1f, 0.5f);


        // Declares the variable blackMat and initializes it.
        Material blackMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        blackMat.color = new Color(0.05f, 0.05f, 0.05f);



        // Declares the variable clown and initializes it.
        GameObject clown = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        // Updates an existing value.
        clown.name = "Clown";

        // Updates an existing value.
        clown.transform.position = position;

        // Updates an existing value.
        clown.transform.localScale = new Vector3(1.0f, 1.0f, 0.85f);

        // Calls a method.
        clown.GetComponent<Renderer>().material = purpleMat;



        // Starts a for loop.
        for (int i = 0; i < 3; i++)

        // Opens a new code block.
        {

            // Declares the variable button and initializes it.
            GameObject button = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Updates an existing value.
            button.name = "Button" + i;

            // Updates an existing value.
            button.transform.parent = clown.transform;

            // Updates an existing value.
            button.transform.localPosition = new Vector3(0, 0.3f - i * 0.3f, 0.45f);

            // Updates an existing value.
            button.transform.localScale = new Vector3(0.18f, 0.18f, 0.1f);

            // Calls a method.
            button.GetComponent<Renderer>().material = yellowMat;

            // Calls a method.
            Object.DestroyImmediate(button.GetComponent<Collider>());

        // Closes the current code block.
        }



        // Declares the variable collar and initializes it.
        GameObject collar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Updates an existing value.
        collar.name = "Collar";

        // Updates an existing value.
        collar.transform.parent = clown.transform;

        // Updates an existing value.
        collar.transform.localPosition = new Vector3(0, 0.55f, 0);

        // Updates an existing value.
        collar.transform.localScale = new Vector3(0.85f, 0.08f, 0.85f);

        // Calls a method.
        collar.GetComponent<Renderer>().material = whiteSkin;

        // Calls a method.
        Object.DestroyImmediate(collar.GetComponent<Collider>());



        // Declares the variable head and initializes it.
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        head.name = "Head";

        // Updates an existing value.
        head.transform.parent = clown.transform;

        // Updates an existing value.
        head.transform.localPosition = new Vector3(0, 0.95f, 0);

        // Updates an existing value.
        head.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);

        // Calls a method.
        head.GetComponent<Renderer>().material = whiteSkin;

        // Calls a method.
        Object.DestroyImmediate(head.GetComponent<Collider>());



        // Declares the variable nose and initializes it.
        GameObject nose = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        nose.name = "Nose";

        // Updates an existing value.
        nose.transform.parent = head.transform;

        // Updates an existing value.
        nose.transform.localPosition = new Vector3(0, 0, 0.55f);

        // Updates an existing value.
        nose.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        // Calls a method.
        nose.GetComponent<Renderer>().material = redMat;

        // Calls a method.
        Object.DestroyImmediate(nose.GetComponent<Collider>());



        // Declares the variable mouth and initializes it.
        GameObject mouth = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        mouth.name = "Mouth";

        // Updates an existing value.
        mouth.transform.parent = head.transform;

        // Updates an existing value.
        mouth.transform.localPosition = new Vector3(0, -0.3f, 0.42f);

        // Updates an existing value.
        mouth.transform.localScale = new Vector3(0.6f, 0.12f, 0.1f);

        // Calls a method.
        mouth.GetComponent<Renderer>().material = redMat;

        // Calls a method.
        Object.DestroyImmediate(mouth.GetComponent<Collider>());



        // Starts a for loop.
        for (int i = -2; i <= 2; i++)

        // Opens a new code block.
        {

            // Declares the variable tooth and initializes it.
            GameObject tooth = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Updates an existing value.
            tooth.name = "Tooth" + i;

            // Updates an existing value.
            tooth.transform.parent = mouth.transform;

            // Updates an existing value.
            tooth.transform.localPosition = new Vector3(i * 0.18f, 0.2f, 0.5f);

            // Updates an existing value.
            tooth.transform.localScale = new Vector3(0.13f, 0.6f, 0.4f);

            // Calls a method.
            tooth.GetComponent<Renderer>().material = whiteSkin;

            // Calls a method.
            Object.DestroyImmediate(tooth.GetComponent<Collider>());

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
            eye.transform.localPosition = new Vector3(xPos, 0.15f, 0.42f);

            // Updates an existing value.
            eye.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            // Calls a method.
            eye.GetComponent<Renderer>().material = whiteSkin;

            // Calls a method.
            Object.DestroyImmediate(eye.GetComponent<Collider>());



            // Starts a for loop.
            for (int j = 0; j < 2; j++)

            // Opens a new code block.
            {

                // Declares the variable xPart and initializes it.
                GameObject xPart = GameObject.CreatePrimitive(PrimitiveType.Cube);

                // Updates an existing value.
                xPart.name = "XPart" + j;

                // Updates an existing value.
                xPart.transform.parent = eye.transform;

                // Updates an existing value.
                xPart.transform.localPosition = new Vector3(0, 0, 0.45f);

                // Updates an existing value.
                xPart.transform.localRotation = Quaternion.Euler(0, 0, j * 90f + 45f);

                // Updates an existing value.
                xPart.transform.localScale = new Vector3(0.7f, 0.15f, 0.15f);

                // Calls a method.
                xPart.GetComponent<Renderer>().material = blackMat;

                // Calls a method.
                Object.DestroyImmediate(xPart.GetComponent<Collider>());

            // Closes the current code block.
            }

        // Closes the current code block.
        }



        // Starts a for loop.
        for (int i = 0; i < 2; i++)

        // Opens a new code block.
        {

            // Declares the variable xPos and initializes it.
            float xPos = (i == 0) ? -0.22f : 0.22f;

            // Declares the variable paint and initializes it.
            GameObject paint = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Updates an existing value.
            paint.name = "EyePaint" + i;

            // Updates an existing value.
            paint.transform.parent = head.transform;

            // Updates an existing value.
            paint.transform.localPosition = new Vector3(xPos, 0.4f, 0.45f);

            // Updates an existing value.
            paint.transform.localRotation = Quaternion.Euler(0, 0, 45f);

            // Updates an existing value.
            paint.transform.localScale = new Vector3(0.15f, 0.15f, 0.05f);

            // Calls a method.
            paint.GetComponent<Renderer>().material = redMat;

            // Calls a method.
            Object.DestroyImmediate(paint.GetComponent<Collider>());

        // Closes the current code block.
        }



        // Starts a for loop.
        for (int side = 0; side < 2; side++)

        // Opens a new code block.
        {

            // Starts a for loop.
            for (int i = 0; i < 4; i++)

            // Opens a new code block.
            {

                // Declares the variable tuft and initializes it.
                GameObject tuft = GameObject.CreatePrimitive(PrimitiveType.Sphere);

                // Updates an existing value.
                tuft.name = "Hair_" + side + "_" + i;

                // Updates an existing value.
                tuft.transform.parent = head.transform;

                // Declares the variable baseX and initializes it.
                float baseX = (side == 0) ? -0.5f : 0.5f;

                // Updates an existing value.
                tuft.transform.localPosition = new Vector3(

                    // Executes this statement.
                    baseX + Random.Range(-0.15f, 0.15f),

                    // Executes this statement.
                    0.3f + i * 0.12f,

                    // Calls a method.
                    Random.Range(-0.2f, 0.2f)

                // Executes this statement.
                );

                // Updates an existing value.
                tuft.transform.localScale = new Vector3(0.3f, 0.25f, 0.3f);

                // Calls a method.
                tuft.GetComponent<Renderer>().material = (i % 2 == 0) ? redMat : yellowMat;

                // Calls a method.
                Object.DestroyImmediate(tuft.GetComponent<Collider>());

            // Closes the current code block.
            }

        // Closes the current code block.
        }



        // Declares the variable hat and initializes it.
        GameObject hat = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Updates an existing value.
        hat.name = "Hat";

        // Updates an existing value.
        hat.transform.parent = head.transform;

        // Updates an existing value.
        hat.transform.localPosition = new Vector3(0, 0.6f, -0.05f);

        // Updates an existing value.
        hat.transform.localScale = new Vector3(0.15f, 0.25f, 0.15f);

        // Calls a method.
        hat.GetComponent<Renderer>().material = purpleMat;

        // Calls a method.
        Object.DestroyImmediate(hat.GetComponent<Collider>());



        // Declares the variable glow and initializes it.
        GameObject glow = new GameObject("ClownGlow");

        // Updates an existing value.
        glow.transform.parent = clown.transform;

        // Updates an existing value.
        glow.transform.localPosition = new Vector3(0, 0.5f, 0);

        // Declares the variable gl and initializes it.
        Light gl = glow.AddComponent<Light>();

        // Updates an existing value.
        gl.type = LightType.Point;

        // Updates an existing value.
        gl.color = new Color(0.9f, 0.2f, 0.9f);

        // Updates an existing value.
        gl.intensity = 1.2f;

        // Updates an existing value.
        gl.range = 5f;



        // Declares the variable agent and initializes it.
        NavMeshAgent agent = clown.AddComponent<NavMeshAgent>();

        // Updates an existing value.
        agent.speed = 5f;

        // Updates an existing value.
        agent.stoppingDistance = 0.5f;

        // Updates an existing value.
        agent.radius = 0.4f;

        // Updates an existing value.
        agent.height = 2f;


        // Declares the variable ai and initializes it.
        EnemyAI ai = clown.AddComponent<EnemyAI>();

        // Calls a method.
        WireAI(ai);


        // Declares the variable doors and initializes it.
        DoorController[] doors = Object.FindObjectsByType<DoorController>(FindObjectsSortMode.None);

        // Iterates through each item in the collection.
        foreach (var d in doors)

        // Opens a new code block.
        {

            // Checks the condition and runs the inline statement when it is true.
            if (d.transform.position.x < 0) ai.leftDoor = d;

            // Checks the condition and runs the inline statement when it is true.
            if (d.transform.position.x > 0) ai.rightDoor = d;

        // Closes the current code block.
        }


        // Returns the specified value.
        return ai;

    // Closes the current code block.
    }


    // Declares the method named BuildEnemy.
    void BuildEnemy()

    // Opens a new code block.
    {


        // Declares the variable furMat and initializes it.
        Material furMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        furMat.color = new Color(0.32f, 0.18f, 0.1f);

        // Calls a method.
        furMat.SetFloat("_Glossiness", 0.05f);


        // Declares the variable snoutMat and initializes it.
        Material snoutMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        snoutMat.color = new Color(0.55f, 0.4f, 0.25f);

        // Calls a method.
        snoutMat.SetFloat("_Glossiness", 0.1f);


        // Declares the variable hatMat and initializes it.
        Material hatMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        hatMat.color = new Color(0.05f, 0.05f, 0.05f);

        // Calls a method.
        hatMat.SetFloat("_Glossiness", 0.3f);


        // Declares the variable bowMat and initializes it.
        Material bowMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        bowMat.color = new Color(0.05f, 0.05f, 0.05f);


        // Declares the variable whiteMat and initializes it.
        Material whiteMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        whiteMat.color = new Color(0.9f, 0.9f, 0.85f);



        // Declares the variable enemy and initializes it.
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);

        // Updates an existing value.
        enemy.name = "Animatronic";

        // Updates an existing value.
        enemy.transform.position = new Vector3(-5.25f, 1, -15f);

        // Updates an existing value.
        enemy.transform.localScale = new Vector3(1.1f, 1.0f, 0.85f);

        // Calls a method.
        enemy.GetComponent<Renderer>().material = furMat;



        // Declares the variable belly and initializes it.
        GameObject belly = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        belly.name = "Belly";

        // Updates an existing value.
        belly.transform.parent = enemy.transform;

        // Updates an existing value.
        belly.transform.localPosition = new Vector3(0, -0.1f, 0.45f);

        // Updates an existing value.
        belly.transform.localScale = new Vector3(0.7f, 0.65f, 0.4f);

        // Calls a method.
        belly.GetComponent<Renderer>().material = snoutMat;

        // Calls a method.
        Object.DestroyImmediate(belly.GetComponent<Collider>());



        // Declares the variable bow and initializes it.
        GameObject bow = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        bow.name = "BowTie";

        // Updates an existing value.
        bow.transform.parent = enemy.transform;

        // Updates an existing value.
        bow.transform.localPosition = new Vector3(0, 0.4f, 0.5f);

        // Updates an existing value.
        bow.transform.localScale = new Vector3(0.5f, 0.18f, 0.1f);

        // Calls a method.
        bow.GetComponent<Renderer>().material = bowMat;

        // Calls a method.
        Object.DestroyImmediate(bow.GetComponent<Collider>());



        // Declares the variable head and initializes it.
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        head.name = "Head";

        // Updates an existing value.
        head.transform.parent = enemy.transform;

        // Updates an existing value.
        head.transform.localPosition = new Vector3(0, 0.95f, 0.05f);

        // Updates an existing value.
        head.transform.localScale = new Vector3(0.85f, 0.75f, 0.85f);

        // Calls a method.
        head.GetComponent<Renderer>().material = furMat;

        // Calls a method.
        Object.DestroyImmediate(head.GetComponent<Collider>());



        // Declares the variable snout and initializes it.
        GameObject snout = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        snout.name = "Snout";

        // Updates an existing value.
        snout.transform.parent = head.transform;

        // Updates an existing value.
        snout.transform.localPosition = new Vector3(0, -0.15f, 0.55f);

        // Updates an existing value.
        snout.transform.localScale = new Vector3(0.55f, 0.5f, 0.45f);

        // Calls a method.
        snout.GetComponent<Renderer>().material = snoutMat;

        // Calls a method.
        Object.DestroyImmediate(snout.GetComponent<Collider>());



        // Declares the variable nose and initializes it.
        GameObject nose = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        nose.name = "Nose";

        // Updates an existing value.
        nose.transform.parent = snout.transform;

        // Updates an existing value.
        nose.transform.localPosition = new Vector3(0, 0.25f, 0.6f);

        // Updates an existing value.
        nose.transform.localScale = new Vector3(0.35f, 0.25f, 0.35f);

        // Calls a method.
        nose.GetComponent<Renderer>().material = bowMat;

        // Calls a method.
        Object.DestroyImmediate(nose.GetComponent<Collider>());



        // Declares the variable mouth and initializes it.
        GameObject mouth = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Updates an existing value.
        mouth.name = "Mouth";

        // Updates an existing value.
        mouth.transform.parent = snout.transform;

        // Updates an existing value.
        mouth.transform.localPosition = new Vector3(0, -0.4f, 0.4f);

        // Updates an existing value.
        mouth.transform.localScale = new Vector3(0.7f, 0.3f, 0.3f);

        // Declares the variable mouthMat and initializes it.
        Material mouthMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        mouthMat.color = new Color(0.05f, 0.02f, 0.02f);

        // Calls a method.
        mouth.GetComponent<Renderer>().material = mouthMat;

        // Calls a method.
        Object.DestroyImmediate(mouth.GetComponent<Collider>());



        // Starts a for loop.
        for (int i = -2; i <= 2; i++)

        // Opens a new code block.
        {

            // Declares the variable tooth and initializes it.
            GameObject tooth = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Updates an existing value.
            tooth.name = "Tooth" + i;

            // Updates an existing value.
            tooth.transform.parent = mouth.transform;

            // Updates an existing value.
            tooth.transform.localPosition = new Vector3(i * 0.18f, 0.3f, 0f);

            // Updates an existing value.
            tooth.transform.localScale = new Vector3(0.13f, 0.5f, 0.5f);

            // Calls a method.
            tooth.GetComponent<Renderer>().material = whiteMat;

            // Calls a method.
            Object.DestroyImmediate(tooth.GetComponent<Collider>());

        // Closes the current code block.
        }



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
            ear.transform.localPosition = new Vector3(xPos, 0.55f, 0);

            // Updates an existing value.
            ear.transform.localScale = new Vector3(0.35f, 0.35f, 0.3f);

            // Calls a method.
            ear.GetComponent<Renderer>().material = furMat;

            // Calls a method.
            Object.DestroyImmediate(ear.GetComponent<Collider>());



            // Declares the variable earInner and initializes it.
            GameObject earInner = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Updates an existing value.
            earInner.name = "EarInner" + i;

            // Updates an existing value.
            earInner.transform.parent = ear.transform;

            // Updates an existing value.
            earInner.transform.localPosition = new Vector3(0, 0, -0.1f);

            // Updates an existing value.
            earInner.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

            // Calls a method.
            earInner.GetComponent<Renderer>().material = snoutMat;

            // Calls a method.
            Object.DestroyImmediate(earInner.GetComponent<Collider>());

        // Closes the current code block.
        }



        // Starts a for loop.
        for (int i = 0; i < 2; i++)

        // Opens a new code block.
        {

            // Declares the variable cheek and initializes it.
            GameObject cheek = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Updates an existing value.
            cheek.name = "Cheek" + i;

            // Updates an existing value.
            cheek.transform.parent = head.transform;

            // Declares the variable xPos and initializes it.
            float xPos = (i == 0) ? -0.45f : 0.45f;

            // Updates an existing value.
            cheek.transform.localPosition = new Vector3(xPos, -0.15f, 0.25f);

            // Updates an existing value.
            cheek.transform.localScale = new Vector3(0.3f, 0.25f, 0.3f);

            // Calls a method.
            cheek.GetComponent<Renderer>().material = furMat;

            // Calls a method.
            Object.DestroyImmediate(cheek.GetComponent<Collider>());

        // Closes the current code block.
        }



        // Starts a for loop.
        for (int i = 0; i < 2; i++)

        // Opens a new code block.
        {

            // Declares the variable brow and initializes it.
            GameObject brow = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Updates an existing value.
            brow.name = "Brow" + i;

            // Updates an existing value.
            brow.transform.parent = head.transform;

            // Declares the variable xPos and initializes it.
            float xPos = (i == 0) ? -0.22f : 0.22f;

            // Updates an existing value.
            brow.transform.localPosition = new Vector3(xPos, 0.32f, 0.45f);

            // Updates an existing value.
            brow.transform.localRotation = Quaternion.Euler(0, 0, (i == 0) ? -20f : 20f);

            // Updates an existing value.
            brow.transform.localScale = new Vector3(0.28f, 0.07f, 0.05f);

            // Calls a method.
            brow.GetComponent<Renderer>().material = bowMat;

            // Calls a method.
            Object.DestroyImmediate(brow.GetComponent<Collider>());

        // Closes the current code block.
        }



        // Declares the variable hatBrim and initializes it.
        GameObject hatBrim = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Updates an existing value.
        hatBrim.name = "HatBrim";

        // Updates an existing value.
        hatBrim.transform.parent = head.transform;

        // Updates an existing value.
        hatBrim.transform.localPosition = new Vector3(0, 0.65f, -0.05f);

        // Updates an existing value.
        hatBrim.transform.localScale = new Vector3(0.55f, 0.04f, 0.55f);

        // Calls a method.
        hatBrim.GetComponent<Renderer>().material = hatMat;

        // Calls a method.
        Object.DestroyImmediate(hatBrim.GetComponent<Collider>());


        // Declares the variable hatTop and initializes it.
        GameObject hatTop = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Updates an existing value.
        hatTop.name = "HatTop";

        // Updates an existing value.
        hatTop.transform.parent = head.transform;

        // Updates an existing value.
        hatTop.transform.localPosition = new Vector3(0, 0.85f, -0.05f);

        // Updates an existing value.
        hatTop.transform.localScale = new Vector3(0.4f, 0.2f, 0.4f);

        // Calls a method.
        hatTop.GetComponent<Renderer>().material = hatMat;

        // Calls a method.
        Object.DestroyImmediate(hatTop.GetComponent<Collider>());



        // Declares the variable goldMat and initializes it.
        Material goldMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        goldMat.color = new Color(0.85f, 0.65f, 0.15f);

        // Calls a method.
        goldMat.SetFloat("_Glossiness", 0.6f);

        // Declares the variable hatBand and initializes it.
        GameObject hatBand = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Updates an existing value.
        hatBand.name = "HatBand";

        // Updates an existing value.
        hatBand.transform.parent = head.transform;

        // Updates an existing value.
        hatBand.transform.localPosition = new Vector3(0, 0.7f, -0.05f);

        // Updates an existing value.
        hatBand.transform.localScale = new Vector3(0.42f, 0.04f, 0.42f);

        // Calls a method.
        hatBand.GetComponent<Renderer>().material = goldMat;

        // Calls a method.
        Object.DestroyImmediate(hatBand.GetComponent<Collider>());



        // Starts a for loop.
        for (int i = 0; i < 2; i++)

        // Opens a new code block.
        {

            // Declares the variable side and initializes it.
            float side = (i == 0) ? -1f : 1f;



            // Declares the variable shoulder and initializes it.
            GameObject shoulder = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Updates an existing value.
            shoulder.name = "Shoulder" + i;

            // Updates an existing value.
            shoulder.transform.parent = enemy.transform;

            // Updates an existing value.
            shoulder.transform.localPosition = new Vector3(side * 0.55f, 0.35f, 0);

            // Updates an existing value.
            shoulder.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            // Calls a method.
            shoulder.GetComponent<Renderer>().material = furMat;

            // Calls a method.
            Object.DestroyImmediate(shoulder.GetComponent<Collider>());



            // Declares the variable upperArm and initializes it.
            GameObject upperArm = GameObject.CreatePrimitive(PrimitiveType.Capsule);

            // Updates an existing value.
            upperArm.name = "UpperArm" + i;

            // Updates an existing value.
            upperArm.transform.parent = enemy.transform;

            // Updates an existing value.
            upperArm.transform.localPosition = new Vector3(side * 0.6f, -0.05f, 0);

            // Updates an existing value.
            upperArm.transform.localScale = new Vector3(0.25f, 0.4f, 0.25f);

            // Calls a method.
            upperArm.GetComponent<Renderer>().material = furMat;

            // Calls a method.
            Object.DestroyImmediate(upperArm.GetComponent<Collider>());



            // Declares the variable hand and initializes it.
            GameObject hand = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Updates an existing value.
            hand.name = "Hand" + i;

            // Updates an existing value.
            hand.transform.parent = enemy.transform;

            // Updates an existing value.
            hand.transform.localPosition = new Vector3(side * 0.65f, -0.55f, 0.05f);

            // Updates an existing value.
            hand.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            // Calls a method.
            hand.GetComponent<Renderer>().material = furMat;

            // Calls a method.
            Object.DestroyImmediate(hand.GetComponent<Collider>());

        // Closes the current code block.
        }



        // Declares the variable micHandle and initializes it.
        GameObject micHandle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Updates an existing value.
        micHandle.name = "MicHandle";

        // Updates an existing value.
        micHandle.transform.parent = enemy.transform;

        // Updates an existing value.
        micHandle.transform.localPosition = new Vector3(0.65f, -0.7f, 0.05f);

        // Updates an existing value.
        micHandle.transform.localScale = new Vector3(0.05f, 0.15f, 0.05f);

        // Calls a method.
        micHandle.GetComponent<Renderer>().material = bowMat;

        // Calls a method.
        Object.DestroyImmediate(micHandle.GetComponent<Collider>());


        // Declares the variable micHead and initializes it.
        GameObject micHead = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        // Updates an existing value.
        micHead.name = "MicHead";

        // Updates an existing value.
        micHead.transform.parent = enemy.transform;

        // Updates an existing value.
        micHead.transform.localPosition = new Vector3(0.65f, -0.5f, 0.05f);

        // Updates an existing value.
        micHead.transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);

        // Declares the variable micMat and initializes it.
        Material micMat = new Material(Shader.Find("Standard"));

        // Updates an existing value.
        micMat.color = new Color(0.4f, 0.4f, 0.45f);

        // Calls a method.
        micMat.SetFloat("_Glossiness", 0.7f);

        // Calls a method.
        micHead.GetComponent<Renderer>().material = micMat;

        // Calls a method.
        Object.DestroyImmediate(micHead.GetComponent<Collider>());



        // Starts a for loop.
        for (int i = 0; i < 2; i++)

        // Opens a new code block.
        {

            // Declares the variable side and initializes it.
            float side = (i == 0) ? -1f : 1f;

            // Declares the variable bolt and initializes it.
            GameObject bolt = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

            // Updates an existing value.
            bolt.name = "Bolt" + i;

            // Updates an existing value.
            bolt.transform.parent = head.transform;

            // Updates an existing value.
            bolt.transform.localPosition = new Vector3(side * 0.62f, 0, 0);

            // Updates an existing value.
            bolt.transform.localRotation = Quaternion.Euler(0, 0, 90);

            // Updates an existing value.
            bolt.transform.localScale = new Vector3(0.06f, 0.04f, 0.06f);

            // Calls a method.
            bolt.GetComponent<Renderer>().material = micMat;

            // Calls a method.
            Object.DestroyImmediate(bolt.GetComponent<Collider>());

        // Closes the current code block.
        }



        // Starts a for loop.
        for (int i = 0; i < 2; i++)

        // Opens a new code block.
        {

            // Declares the variable eyeWhite and initializes it.
            GameObject eyeWhite = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Updates an existing value.
            eyeWhite.name = "EyeWhite" + i;

            // Updates an existing value.
            eyeWhite.transform.parent = head.transform;

            // Declares the variable xPos and initializes it.
            float xPos = (i == 0) ? -0.22f : 0.22f;

            // Updates an existing value.
            eyeWhite.transform.localPosition = new Vector3(xPos, 0.1f, 0.45f);

            // Updates an existing value.
            eyeWhite.transform.localScale = new Vector3(0.22f, 0.22f, 0.22f);

            // Calls a method.
            eyeWhite.GetComponent<Renderer>().material = whiteMat;

            // Calls a method.
            Object.DestroyImmediate(eyeWhite.GetComponent<Collider>());


            // Declares the variable pupil and initializes it.
            GameObject pupil = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            // Updates an existing value.
            pupil.name = "Pupil" + i;

            // Updates an existing value.
            pupil.transform.parent = eyeWhite.transform;

            // Updates an existing value.
            pupil.transform.localPosition = new Vector3(0, 0, 0.6f);

            // Updates an existing value.
            pupil.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            // Declares the variable pupilMat and initializes it.
            Material pupilMat = new Material(Shader.Find("Standard"));

            // Updates an existing value.
            pupilMat.color = Color.red;

            // Calls a method.
            pupilMat.EnableKeyword("_EMISSION");

            // Calls a method.
            pupilMat.SetColor("_EmissionColor", Color.red * 4f);

            // Calls a method.
            pupil.GetComponent<Renderer>().material = pupilMat;

            // Calls a method.
            Object.DestroyImmediate(pupil.GetComponent<Collider>());

        // Closes the current code block.
        }



        // Declares the variable glow and initializes it.
        GameObject glow = new GameObject("EnemyGlow");

        // Updates an existing value.
        glow.transform.parent = enemy.transform;

        // Updates an existing value.
        glow.transform.localPosition = new Vector3(0, 0.5f, 0);

        // Declares the variable gl and initializes it.
        Light gl = glow.AddComponent<Light>();

        // Updates an existing value.
        gl.type = LightType.Point;

        // Updates an existing value.
        gl.color = new Color(1f, 0.2f, 0.1f);

        // Updates an existing value.
        gl.intensity = 1.2f;

        // Updates an existing value.
        gl.range = 5f;



        // Declares the variable agent and initializes it.
        NavMeshAgent agent = enemy.AddComponent<NavMeshAgent>();

        // Updates an existing value.
        agent.speed = 5f;

        // Updates an existing value.
        agent.stoppingDistance = 0.5f;

        // Updates an existing value.
        agent.radius = 0.4f;

        // Updates an existing value.
        agent.height = 2f;


        // Declares the variable ai and initializes it.
        EnemyAI ai = enemy.AddComponent<EnemyAI>();

        // Calls a method.
        WireAI(ai);



        // Declares the variable doors and initializes it.
        DoorController[] doors = Object.FindObjectsByType<DoorController>(FindObjectsSortMode.None);

        // Iterates through each item in the collection.
        foreach (var d in doors)

        // Opens a new code block.
        {

            // Checks the condition and runs the inline statement when it is true.
            if (d.transform.position.x < 0) ai.leftDoor = d;

            // Checks the condition and runs the inline statement when it is true.
            if (d.transform.position.x > 0) ai.rightDoor = d;

        // Closes the current code block.
        }

    // Closes the current code block.
    }

// Closes the current code block.
}
