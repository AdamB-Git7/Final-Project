using UnityEngine;
using UnityEngine.UI;
using TMPro;

public static class SceneSetup
{
    public static void EnsureSceneIsBuilt(GameManager manager)
    {
        EnsureMainCamera();
        EnsureSceneGeometry();
        EnsureSecurityCamera();
        EnsureHallwayLights();
        EnsureAudioManager();
        EnsureDeskAnimations();
        EnsureSecondAnimatronic(manager);
        EnsureParticleSystems();
        EnsurePauseMenu();
    }

    static void EnsurePauseMenu()
    {
        if (Object.FindFirstObjectByType<PauseMenu>() != null) return;
        new GameObject("PauseMenu").AddComponent<PauseMenu>();
    }

    static void EnsureDeskAnimations()
    {
        if (Object.FindFirstObjectByType<DeskAnimations>() != null) return;
        new GameObject("DeskAnimations").AddComponent<DeskAnimations>();
    }

    static void EnsureParticleSystems()
    {
        if (Object.FindFirstObjectByType<ParticleSystems>() != null) return;
        new GameObject("ParticleSystems").AddComponent<ParticleSystems>();
    }

    static void EnsureAudioManager()
    {
        if (Object.FindFirstObjectByType<AudioManager>() != null) return;
        new GameObject("AudioManager").AddComponent<AudioManager>();
    }

    static void EnsureMainCamera()
    {
        if (Camera.main != null) return;

        GameObject obj = new GameObject("Main Camera");
        obj.tag = "MainCamera";
        Camera cam = obj.AddComponent<Camera>();
        obj.AddComponent<AudioListener>();
        cam.transform.position = new Vector3(0, 1.6f, 1.8f);
        cam.transform.rotation = Quaternion.Euler(5, 180, 0);
        cam.fieldOfView = 90f;
        cam.backgroundColor = new Color(0.005f, 0.005f, 0.01f);
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.nearClipPlane = 0.1f;
    }

    static void EnsureSceneGeometry()
    {
        if (GameObject.Find("SecurityOffice") != null) return;

        GameObject obj = new GameObject("WorldBuilder");
        var builder = obj.AddComponent<WorldBuilder>();
        builder.BuildAll();
    }

    static void EnsureSecurityCamera()
    {
        if (Object.FindFirstObjectByType<SecurityCamera>() != null) return;
        new GameObject("SecurityCameraSystem").AddComponent<SecurityCamera>();
    }

    static void EnsureHallwayLights()
    {
        if (Object.FindFirstObjectByType<HallwayLightSystem>() != null) return;
        new GameObject("HallwayLightSystem").AddComponent<HallwayLightSystem>();
    }

    static void EnsureSecondAnimatronic(GameManager manager)
    {
        EnemyAI[] enemies = Object.FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        if (enemies.Length != 1) return;

        var builder = Object.FindFirstObjectByType<WorldBuilder>();
        if (builder == null)
            builder = new GameObject("TempBuilder").AddComponent<WorldBuilder>();

        EnemyAI clown = builder.BuildClownEnemy(new Vector3(5.25f, 1f, -15f));
        if (clown != null) clown.gameManager = manager;

        // Make all animatronics pass through each other and add walking animation
        foreach (var ai in Object.FindObjectsByType<EnemyAI>(FindObjectsSortMode.None))
        {
            DisablePhysicsCollision(ai.gameObject);
            if (ai.GetComponent<AnimatronicAnimator>() == null)
                ai.gameObject.AddComponent<AnimatronicAnimator>();
        }
    }

    static void DisablePhysicsCollision(GameObject obj)
    {
        // Remove the capsule collider so they pass through each other physically
        var col = obj.GetComponent<Collider>();
        if (col != null) Object.Destroy(col);

        // Tell the NavMeshAgent to ignore other agents
        var agent = obj.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
            agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
    }

    public static TMP_Text FindOrCreateClockText()
    {
        GameObject obj = GameObject.Find("MonitorClockText");
        if (obj != null)
            return obj.GetComponent<TMP_Text>();

        obj = new GameObject("MonitorClockText");
        obj.transform.position = new Vector3(1.2f, 1.15f, -0.46f);
        obj.transform.rotation = Quaternion.Euler(0, 180, 0);

        TextMeshPro tmp = obj.AddComponent<TextMeshPro>();
        tmp.text = "12:00 AM";
        tmp.fontSize = 1.2f;
        tmp.color = new Color(0.3f, 1f, 0.3f);
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        tmp.rectTransform.sizeDelta = new Vector2(0.7f, 0.4f);
        return tmp;
    }

    public static void ShowGameOverScreen()
    {
        GameObject canvasObj = new GameObject("RuntimeGameOverCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        AddFullscreenImage(canvasObj.transform, "Background", new Color(0.4f, 0f, 0f, 0.9f));
        AddCenteredText(canvasObj.transform, "Title", "YOU DIED", 200, Color.white, new Vector2(0, 50), new Vector2(1200, 250));
        AddCenteredText(canvasObj.transform, "Hint", "Press R to restart", 50, new Color(0.9f, 0.9f, 0.9f), new Vector2(0, -100), new Vector2(1000, 100));
    }

    public static void ShowWinScreen(int nightCompleted)
    {
        GameObject canvasObj = new GameObject("RuntimeWinCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        // Warm orange "sunrise" background
        AddFullscreenImage(canvasObj.transform, "Background", new Color(0.9f, 0.5f, 0.15f, 0.9f));

        // Big "6 AM" headline
        AddCenteredText(canvasObj.transform, "Time", "6 AM", 280, Color.white,
                        new Vector2(0, 250), new Vector2(1200, 300));

        // Subtitle showing which night was beaten
        AddCenteredText(canvasObj.transform, "Title",
                        "NIGHT " + nightCompleted + " COMPLETE", 80,
                        Color.white, new Vector2(0, 50), new Vector2(1500, 150));

        // Hints
        AddCenteredText(canvasObj.transform, "NextHint",
                        "Press N for next night", 55,
                        Color.white, new Vector2(0, -100), new Vector2(1200, 100));
        AddCenteredText(canvasObj.transform, "RestartHint",
                        "Press R to replay this night", 40,
                        new Color(0.9f, 0.9f, 0.9f), new Vector2(0, -200),
                        new Vector2(1200, 80));
    }

    public static void ShowFinalWinScreen()
    {
        GameObject canvasObj = new GameObject("RuntimeFinalWinCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        // Golden victory background
        AddFullscreenImage(canvasObj.transform, "Background", new Color(0.95f, 0.7f, 0.2f, 0.95f));

        AddCenteredText(canvasObj.transform, "Title", "YOU BEAT THE GAME", 150,
                        new Color(0.2f, 0.05f, 0.05f),
                        new Vector2(0, 200), new Vector2(1800, 250));

        AddCenteredText(canvasObj.transform, "Sub", "All 3 nights survived",
                        70, new Color(0.2f, 0.05f, 0.05f),
                        new Vector2(0, 50), new Vector2(1500, 100));

        AddCenteredText(canvasObj.transform, "Credits",
                        "Made by Luca & Adam", 50,
                        new Color(0.2f, 0.05f, 0.05f),
                        new Vector2(0, -100), new Vector2(1200, 80));

        AddCenteredText(canvasObj.transform, "Hint", "Press R to play again", 45,
                        new Color(0.2f, 0.05f, 0.05f),
                        new Vector2(0, -250), new Vector2(1200, 80));
    }

    static void AddFullscreenImage(Transform parent, string name, Color color)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        var img = obj.AddComponent<Image>();
        img.color = color;
        var rt = img.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }

    static void AddCenteredText(Transform parent, string name, string text, int fontSize, Color color, Vector2 anchored, Vector2 size)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent, false);
        var tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.color = color;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        var rt = tmp.rectTransform;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = anchored;
        rt.sizeDelta = size;
    }
}
