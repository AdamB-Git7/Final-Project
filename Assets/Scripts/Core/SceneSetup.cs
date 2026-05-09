
// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.UI namespace.
using UnityEngine.UI;

// Imports the TMPro namespace.
using TMPro;


// Declares the class named SceneSetup.
public static class SceneSetup

// Opens a new code block.
{

    // Declares the method named EnsureSceneIsBuilt.
    public static void EnsureSceneIsBuilt(GameManager manager)

    // Opens a new code block.
    {

        // Calls a method.
        EnsureMainCamera();

        // Calls a method.
        EnsureSceneGeometry();

        // Calls a method.
        EnsureSecurityCamera();

        // Calls a method.
        EnsureHallwayLights();

        // Calls a method.
        EnsureAudioManager();

        // Calls a method.
        EnsureDeskAnimations();

        // Calls a method.
        EnsureSecondAnimatronic(manager);

        // Calls a method.
        EnsureParticleSystems();

        // Calls a method.
        EnsurePauseMenu();

    // Closes the current code block.
    }


    // Declares the method named EnsurePauseMenu.
    static void EnsurePauseMenu()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (Object.FindFirstObjectByType<PauseMenu>() != null) return;

        // Executes this statement.
        new GameObject("PauseMenu").AddComponent<PauseMenu>();

    // Closes the current code block.
    }


    // Declares the method named EnsureDeskAnimations.
    static void EnsureDeskAnimations()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (Object.FindFirstObjectByType<DeskAnimations>() != null) return;

        // Executes this statement.
        new GameObject("DeskAnimations").AddComponent<DeskAnimations>();

    // Closes the current code block.
    }


    // Declares the method named EnsureParticleSystems.
    static void EnsureParticleSystems()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (Object.FindFirstObjectByType<ParticleSystems>() != null) return;

        // Executes this statement.
        new GameObject("ParticleSystems").AddComponent<ParticleSystems>();

    // Closes the current code block.
    }


    // Declares the method named EnsureAudioManager.
    static void EnsureAudioManager()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (Object.FindFirstObjectByType<AudioManager>() != null) return;

        // Executes this statement.
        new GameObject("AudioManager").AddComponent<AudioManager>();

    // Closes the current code block.
    }


    // Declares the method named EnsureMainCamera.
    static void EnsureMainCamera()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (Camera.main != null) return;


        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject("Main Camera");

        // Updates an existing value.
        obj.tag = "MainCamera";

        // Declares the variable cam and initializes it.
        Camera cam = obj.AddComponent<Camera>();

        // Calls a method.
        obj.AddComponent<AudioListener>();

        // Updates an existing value.
        cam.transform.position = new Vector3(0, 1.6f, 1.8f);

        // Updates an existing value.
        cam.transform.rotation = Quaternion.Euler(5, 180, 0);

        // Updates an existing value.
        cam.fieldOfView = 90f;

        // Updates an existing value.
        cam.backgroundColor = new Color(0.005f, 0.005f, 0.01f);

        // Updates an existing value.
        cam.clearFlags = CameraClearFlags.SolidColor;

        // Updates an existing value.
        cam.nearClipPlane = 0.1f;

    // Closes the current code block.
    }


    // Declares the method named EnsureSceneGeometry.
    static void EnsureSceneGeometry()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (GameObject.Find("SecurityOffice") != null) return;


        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject("WorldBuilder");

        // Declares the variable builder and initializes it.
        var builder = obj.AddComponent<WorldBuilder>();

        // Calls a method.
        builder.BuildAll();

    // Closes the current code block.
    }


    // Declares the method named EnsureSecurityCamera.
    static void EnsureSecurityCamera()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (Object.FindFirstObjectByType<SecurityCamera>() != null) return;

        // Executes this statement.
        new GameObject("SecurityCameraSystem").AddComponent<SecurityCamera>();

    // Closes the current code block.
    }


    // Declares the method named EnsureHallwayLights.
    static void EnsureHallwayLights()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (Object.FindFirstObjectByType<HallwayLightSystem>() != null) return;

        // Executes this statement.
        new GameObject("HallwayLightSystem").AddComponent<HallwayLightSystem>();

    // Closes the current code block.
    }


    // Declares the method named EnsureSecondAnimatronic.
    static void EnsureSecondAnimatronic(GameManager manager)

    // Opens a new code block.
    {

        // Declares the variable enemies and initializes it.
        EnemyAI[] enemies = Object.FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        // Checks the condition and runs the inline statement when it is true.
        if (enemies.Length != 1) return;


        // Declares the variable builder and initializes it.
        var builder = Object.FindFirstObjectByType<WorldBuilder>();

        // Checks whether the condition is true.
        if (builder == null)

            // Updates an existing value.
            builder = new GameObject("TempBuilder").AddComponent<WorldBuilder>();


        // Declares the variable clown and initializes it.
        EnemyAI clown = builder.BuildClownEnemy(new Vector3(5.25f, 1f, -15f));

        // Checks the condition and runs the inline statement when it is true.
        if (clown != null) clown.gameManager = manager;



        // Iterates through each item in the collection.
        foreach (var ai in Object.FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))

        // Opens a new code block.
        {

            // Calls a method.
            DisablePhysicsCollision(ai.gameObject);

            // Checks whether the condition is true.
            if (ai.GetComponent<AnimatronicAnimator>() == null)

                // Calls a method.
                ai.gameObject.AddComponent<AnimatronicAnimator>();

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Declares the method named DisablePhysicsCollision.
    static void DisablePhysicsCollision(GameObject obj)

    // Opens a new code block.
    {


        // Declares the variable col and initializes it.
        var col = obj.GetComponent<Collider>();

        // Checks the condition and runs the inline statement when it is true.
        if (col != null) Object.Destroy(col);



        // Declares the variable agent and initializes it.
        var agent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();

        // Checks whether the condition is true.
        if (agent != null)

            // Updates an existing value.
            agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

    // Closes the current code block.
    }


    // Declares the method named FindOrCreateClockText.
    public static TMP_Text FindOrCreateClockText()

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = GameObject.Find("MonitorClockText");

        // Checks whether the condition is true.
        if (obj != null)

            // Returns the specified value.
            return obj.GetComponent<TMP_Text>();


        // Updates an existing value.
        obj = new GameObject("MonitorClockText");

        // Updates an existing value.
        obj.transform.position = new Vector3(1.2f, 1.15f, -0.46f);

        // Updates an existing value.
        obj.transform.rotation = Quaternion.Euler(0, 180, 0);


        // Declares the variable tmp and initializes it.
        TextMeshPro tmp = obj.AddComponent<TextMeshPro>();

        // Updates an existing value.
        tmp.text = "12:00 AM";

        // Updates an existing value.
        tmp.fontSize = 1.2f;

        // Updates an existing value.
        tmp.color = new Color(0.3f, 1f, 0.3f);

        // Updates an existing value.
        tmp.alignment = TextAlignmentOptions.Center;

        // Updates an existing value.
        tmp.fontStyle = FontStyles.Bold;

        // Updates an existing value.
        tmp.rectTransform.sizeDelta = new Vector2(0.7f, 0.4f);

        // Returns the specified value.
        return tmp;

    // Closes the current code block.
    }


    // Declares the method named ShowGameOverScreen.
    public static void ShowGameOverScreen()

    // Opens a new code block.
    {

        // Declares the variable canvasObj and initializes it.
        GameObject canvasObj = new GameObject("RuntimeGameOverCanvas");

        // Declares the variable canvas and initializes it.
        Canvas canvas = canvasObj.AddComponent<Canvas>();

        // Updates an existing value.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Updates an existing value.
        canvas.sortingOrder = 100;

        // Declares the variable scaler and initializes it.
        var scaler = canvasObj.AddComponent<CanvasScaler>();

        // Updates an existing value.
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // Updates an existing value.
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Calls a method.
        canvasObj.AddComponent<GraphicRaycaster>();


        // Calls a method.
        AddFullscreenImage(canvasObj.transform, "Background", new Color(0.4f, 0f, 0f, 0.9f));

        // Calls a method.
        AddCenteredText(canvasObj.transform, "Title", "YOU DIED", 200, Color.white, new Vector2(0, 50), new Vector2(1200, 250));

        // Calls a method.
        AddCenteredText(canvasObj.transform, "Hint", "Press R to restart", 50, new Color(0.9f, 0.9f, 0.9f), new Vector2(0, -100), new Vector2(1000, 100));

    // Closes the current code block.
    }


    // Declares the method named ShowWinScreen.
    public static void ShowWinScreen(int nightCompleted)

    // Opens a new code block.
    {

        // Declares the variable canvasObj and initializes it.
        GameObject canvasObj = new GameObject("RuntimeWinCanvas");

        // Declares the variable canvas and initializes it.
        Canvas canvas = canvasObj.AddComponent<Canvas>();

        // Updates an existing value.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Updates an existing value.
        canvas.sortingOrder = 100;

        // Declares the variable scaler and initializes it.
        var scaler = canvasObj.AddComponent<CanvasScaler>();

        // Updates an existing value.
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // Updates an existing value.
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Calls a method.
        canvasObj.AddComponent<GraphicRaycaster>();



        // Calls a method.
        AddFullscreenImage(canvasObj.transform, "Background", new Color(0.9f, 0.5f, 0.15f, 0.9f));



        // Calls a method.
        AddCenteredText(canvasObj.transform, "Time", "6 AM", 280, Color.white,

                        // Executes this statement.
                        new Vector2(0, 250), new Vector2(1200, 300));



        // Calls a method.
        AddCenteredText(canvasObj.transform, "Title",

                        // Executes this statement.
                        "NIGHT " + nightCompleted + " COMPLETE", 80,

                        // Executes this statement.
                        Color.white, new Vector2(0, 50), new Vector2(1500, 150));



        // Calls a method.
        AddCenteredText(canvasObj.transform, "NextHint",

                        // Executes this statement.
                        "Press N for next night", 55,

                        // Executes this statement.
                        Color.white, new Vector2(0, -100), new Vector2(1200, 100));

        // Calls a method.
        AddCenteredText(canvasObj.transform, "RestartHint",

                        // Executes this statement.
                        "Press R to replay this night", 40,

                        // Executes this statement.
                        new Color(0.9f, 0.9f, 0.9f), new Vector2(0, -200),

                        // Executes this statement.
                        new Vector2(1200, 80));

    // Closes the current code block.
    }


    // Declares the method named ShowFinalWinScreen.
    public static void ShowFinalWinScreen()

    // Opens a new code block.
    {

        // Declares the variable canvasObj and initializes it.
        GameObject canvasObj = new GameObject("RuntimeFinalWinCanvas");

        // Declares the variable canvas and initializes it.
        Canvas canvas = canvasObj.AddComponent<Canvas>();

        // Updates an existing value.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Updates an existing value.
        canvas.sortingOrder = 100;

        // Declares the variable scaler and initializes it.
        var scaler = canvasObj.AddComponent<CanvasScaler>();

        // Updates an existing value.
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // Updates an existing value.
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Calls a method.
        canvasObj.AddComponent<GraphicRaycaster>();



        // Calls a method.
        AddFullscreenImage(canvasObj.transform, "Background", new Color(0.95f, 0.7f, 0.2f, 0.95f));


        // Calls a method.
        AddCenteredText(canvasObj.transform, "Title", "YOU BEAT THE GAME", 150,

                        // Executes this statement.
                        new Color(0.2f, 0.05f, 0.05f),

                        // Executes this statement.
                        new Vector2(0, 200), new Vector2(1800, 250));


        // Calls a method.
        AddCenteredText(canvasObj.transform, "Sub", "All 3 nights survived",

                        // Executes this statement.
                        70, new Color(0.2f, 0.05f, 0.05f),

                        // Executes this statement.
                        new Vector2(0, 50), new Vector2(1500, 100));


        // Calls a method.
        AddCenteredText(canvasObj.transform, "Credits",

                        // Executes this statement.
                        "Made by Luca & Adam", 50,

                        // Executes this statement.
                        new Color(0.2f, 0.05f, 0.05f),

                        // Executes this statement.
                        new Vector2(0, -100), new Vector2(1200, 80));


        // Calls a method.
        AddCenteredText(canvasObj.transform, "Hint", "Press R to play again", 45,

                        // Executes this statement.
                        new Color(0.2f, 0.05f, 0.05f),

                        // Executes this statement.
                        new Vector2(0, -250), new Vector2(1200, 80));

    // Closes the current code block.
    }


    // Declares the method named AddFullscreenImage.
    static void AddFullscreenImage(Transform parent, string name, Color color)

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject(name);

        // Calls a method.
        obj.transform.SetParent(parent, false);

        // Declares the variable img and initializes it.
        var img = obj.AddComponent<Image>();

        // Updates an existing value.
        img.color = color;

        // Declares the variable rt and initializes it.
        var rt = img.rectTransform;

        // Updates an existing value.
        rt.anchorMin = Vector2.zero;

        // Updates an existing value.
        rt.anchorMax = Vector2.one;

        // Updates an existing value.
        rt.offsetMin = Vector2.zero;

        // Updates an existing value.
        rt.offsetMax = Vector2.zero;

    // Closes the current code block.
    }


    // Declares the method named AddCenteredText.
    static void AddCenteredText(Transform parent, string name, string text, int fontSize, Color color, Vector2 anchored, Vector2 size)

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject(name);

        // Calls a method.
        obj.transform.SetParent(parent, false);

        // Declares the variable tmp and initializes it.
        var tmp = obj.AddComponent<TextMeshProUGUI>();

        // Updates an existing value.
        tmp.text = text;

        // Updates an existing value.
        tmp.fontSize = fontSize;

        // Updates an existing value.
        tmp.color = color;

        // Updates an existing value.
        tmp.alignment = TextAlignmentOptions.Center;

        // Updates an existing value.
        tmp.fontStyle = FontStyles.Bold;

        // Declares the variable rt and initializes it.
        var rt = tmp.rectTransform;

        // Updates an existing value.
        rt.anchorMin = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        rt.anchorMax = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        rt.pivot = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        rt.anchoredPosition = anchored;

        // Updates an existing value.
        rt.sizeDelta = size;

    // Closes the current code block.
    }

// Closes the current code block.
}
