using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SecurityCamera : MonoBehaviour
{
    [Header("Controls")]
    public KeyCode toggleKey = KeyCode.C;

    [Header("Battery")]
    public float maxBattery = 100f;
    public float drainPerSecond = 8f;
    public float doorDrainPerSecond = 3f;
    public float rechargePerSecond = 12f;
    public KeyCode rechargeKey = KeyCode.F;

    [Header("Animation")]
    public float fadeSpeed = 6f;

    Camera[] securityCameras;
    string[] cameraNames;
    RawImage monitorDisplay;
    TMP_Text cameraNameText;
    TMP_Text batteryText;
    TMP_Text middleScreenText;
    GameObject monitorPanel;
    CanvasGroup panelGroup;

    int currentCamera;
    bool monitorActive;
    float battery;
    float panelAlpha;
    RenderTexture renderTexture;

    void Start()
    {
        battery = maxBattery;
        BuildCameras();
        BuildUI();
        BuildBatteryDisplay();
        BuildMiddleScreenText();

        currentCamera = 0;
        monitorActive = false;
        panelAlpha = 0f;

        renderTexture = new RenderTexture(800, 500, 16);

        foreach (Camera cam in securityCameras)
            cam.enabled = false;

        monitorPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
            ToggleMonitor();

        DrainBattery();
        UpdateBatteryDisplay();
        UpdateMiddleScreen();
        AnimatePanel();

        if (!monitorActive) return;

        // Number keys 1-6 switch cameras
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))
                SwitchToCamera(i);
        }
    }

    void ToggleMonitor()
    {
        if (!monitorActive && battery <= 0f) return;

        monitorActive = !monitorActive;
        if (monitorActive)
            monitorPanel.SetActive(true);
        UpdateCamera();
        HighlightCurrentCam();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayCameraClick();
    }

    void AnimatePanel()
    {
        float target = monitorActive ? 1f : 0f;
        panelAlpha = Mathf.MoveTowards(panelAlpha, target, fadeSpeed * Time.deltaTime);

        if (panelGroup != null)
            panelGroup.alpha = panelAlpha;

        // Hide panel object once fully faded out
        if (!monitorActive && panelAlpha <= 0.01f && monitorPanel.activeSelf)
            monitorPanel.SetActive(false);
    }

    public void DrainBatteryExternal(float amount)
    {
        battery -= amount;
        if (battery < 0f) battery = 0f;
    }

    void DrainBattery()
    {
        // Recharge while holding F (only when camera is closed)
        if (!monitorActive && Input.GetKey(rechargeKey))
        {
            battery += rechargePerSecond * Time.deltaTime;
            if (battery > maxBattery) battery = maxBattery;
            return;
        }

        float drain = 0f;
        if (monitorActive) drain += drainPerSecond;

        DoorController[] doors = Object.FindObjectsByType<DoorController>(FindObjectsSortMode.None);
        foreach (var d in doors)
            if (d.isClosed) drain += doorDrainPerSecond;

        if (drain <= 0f) return;

        battery -= drain * Time.deltaTime;
        if (battery <= 0f)
        {
            battery = 0f;
            monitorActive = false;
            foreach (var d in doors)
                if (d.isClosed) d.OpenDoor();
            UpdateCamera();
        }
    }

    void UpdateBatteryDisplay()
    {
        if (batteryText == null) return;
        batteryText.text = "BATT\n" + Mathf.CeilToInt(battery) + "%";

        if (battery > 50f)
            batteryText.color = new Color(0.3f, 1f, 0.3f);
        else if (battery > 20f)
            batteryText.color = new Color(1f, 0.8f, 0.2f);
        else
            batteryText.color = new Color(1f, 0.2f, 0.2f);
    }

    void UpdateMiddleScreen()
    {
        if (middleScreenText == null) return;
        if (monitorActive)
            middleScreenText.text = "VIEWING\nCAMERA";
        else if (Input.GetKey(rechargeKey))
            middleScreenText.text = "CHARGING\n+12%/s";
        else if (battery <= 0f)
            middleScreenText.text = "HOLD F\nTO CHARGE";
        else
            middleScreenText.text = "C: CAMS\nF: CHARGE";
    }

    void BuildCameras()
    {
        Vector3[] positions = {
            new Vector3(-5.25f, 2.8f, 1.5f),    // top of left hallway, looking back
            new Vector3(5.25f, 2.8f, 1.5f),     // top of right hallway, looking back
            new Vector3(0, 2.8f, -19f),         // corridor center, looking at lockers
            new Vector3(0, 2.8f, -20.5f),       // back of corridor, looking at stage sign
            new Vector3(-7.5f, 2.8f, -12f),     // classroom corner, looking across
            new Vector3(7.5f, 2.8f, -12f)       // bathroom corner, looking across
        };
        Vector3[] rotations = {
            new Vector3(20, 180, 0),     // CAM01 looking -Z (down hallway)
            new Vector3(20, 180, 0),     // CAM02 looking -Z
            new Vector3(15, 180, 0),     // CAM03 looking -Z toward lockers/exit sign
            new Vector3(15, 180, 0),     // CAM04 looking -Z at FREDDY'S sign
            new Vector3(15, -90, 0),     // CAM05 looking -X into classroom
            new Vector3(15, 90, 0)       // CAM06 looking +X into bathroom
        };
        cameraNames = new[] {
            "CAM 01 - LEFT HALL",
            "CAM 02 - RIGHT HALL",
            "CAM 03 - CORRIDOR",
            "CAM 04 - STAGE",
            "CAM 05 - CLASSROOM",
            "CAM 06 - BATHROOM"
        };

        securityCameras = new Camera[positions.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            GameObject camObj = new GameObject("SecurityCam_" + i);
            camObj.transform.position = positions[i];
            camObj.transform.rotation = Quaternion.Euler(rotations[i]);

            Camera cam = camObj.AddComponent<Camera>();
            cam.fieldOfView = 70f;
            cam.farClipPlane = 30f;
            cam.backgroundColor = Color.black;
            cam.clearFlags = CameraClearFlags.SolidColor;

            securityCameras[i] = cam;
        }
    }

    void BuildUI()
    {
        GameObject canvasObj = new GameObject("CameraMonitorCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 5;
        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        monitorPanel = new GameObject("MonitorPanel");
        monitorPanel.transform.SetParent(canvasObj.transform, false);
        panelGroup = monitorPanel.AddComponent<CanvasGroup>();
        Image bg = monitorPanel.AddComponent<Image>();
        bg.color = new Color(0f, 0f, 0f, 0.95f);
        RectTransform bgRT = bg.rectTransform;
        bgRT.anchorMin = Vector2.zero;
        bgRT.anchorMax = Vector2.one;
        bgRT.offsetMin = Vector2.zero;
        bgRT.offsetMax = Vector2.zero;

        GameObject frameObj = new GameObject("MonitorFrame");
        frameObj.transform.SetParent(monitorPanel.transform, false);
        Image frame = frameObj.AddComponent<Image>();
        frame.color = new Color(0.2f, 0.2f, 0.2f);
        RectTransform frameRT = frame.rectTransform;
        frameRT.anchorMin = new Vector2(0.5f, 0.5f);
        frameRT.anchorMax = new Vector2(0.5f, 0.5f);
        frameRT.pivot = new Vector2(0.5f, 0.5f);
        frameRT.sizeDelta = new Vector2(1340, 820);
        frameRT.anchoredPosition = Vector2.zero;

        GameObject feedObj = new GameObject("CameraFeed");
        feedObj.transform.SetParent(frameObj.transform, false);
        monitorDisplay = feedObj.AddComponent<RawImage>();
        RectTransform feedRT = monitorDisplay.rectTransform;
        feedRT.anchorMin = new Vector2(0.5f, 0.5f);
        feedRT.anchorMax = new Vector2(0.5f, 0.5f);
        feedRT.pivot = new Vector2(0.5f, 0.5f);
        feedRT.sizeDelta = new Vector2(1280, 720);
        feedRT.anchoredPosition = Vector2.zero;

        GameObject nameObj = new GameObject("CameraNameText");
        nameObj.transform.SetParent(frameObj.transform, false);
        cameraNameText = nameObj.AddComponent<TextMeshProUGUI>();
        cameraNameText.text = "CAM 01";
        cameraNameText.fontSize = 36;
        cameraNameText.color = new Color(1f, 0.3f, 0.3f);
        cameraNameText.alignment = TextAlignmentOptions.Center;
        cameraNameText.fontStyle = FontStyles.Bold;
        RectTransform nameRT = cameraNameText.rectTransform;
        nameRT.anchorMin = new Vector2(0.5f, 1f);
        nameRT.anchorMax = new Vector2(0.5f, 1f);
        nameRT.pivot = new Vector2(0.5f, 1f);
        nameRT.anchoredPosition = new Vector2(0, -10);
        nameRT.sizeDelta = new Vector2(800, 60);

        GameObject hintObj = new GameObject("HintText");
        hintObj.transform.SetParent(frameObj.transform, false);
        var hint = hintObj.AddComponent<TextMeshProUGUI>();
        hint.text = "[1-6] SWITCH CAM   [C] CLOSE";
        hint.fontSize = 24;
        hint.color = new Color(0.7f, 0.7f, 0.7f);
        hint.alignment = TextAlignmentOptions.Center;
        RectTransform hintRT = hint.rectTransform;
        hintRT.anchorMin = new Vector2(0.5f, 0f);
        hintRT.anchorMax = new Vector2(0.5f, 0f);
        hintRT.pivot = new Vector2(0.5f, 0f);
        hintRT.anchoredPosition = new Vector2(0, 10);
        hintRT.sizeDelta = new Vector2(800, 40);

        BuildCameraMap(frameObj.transform);
    }

    Button[] mapButtons;

    void BuildCameraMap(Transform parent)
    {
        // Camera map panel in the bottom-right corner
        GameObject mapObj = new GameObject("CameraMap");
        mapObj.transform.SetParent(parent, false);
        var mapBg = mapObj.AddComponent<Image>();
        mapBg.color = new Color(0.1f, 0.1f, 0.1f, 0.85f);
        var mapRT = mapBg.rectTransform;
        mapRT.anchorMin = new Vector2(1f, 0f);
        mapRT.anchorMax = new Vector2(1f, 0f);
        mapRT.pivot = new Vector2(1f, 0f);
        mapRT.anchoredPosition = new Vector2(-20, 60);
        mapRT.sizeDelta = new Vector2(280, 200);

        // Layout: 2 rows x 3 cols, mapped to room positions roughly
        // Row 1: CAM 03 (Corridor), CAM 04 (Stage), [empty]
        // Row 2: CAM 05 (Classroom), [middle], CAM 06 (Bathroom)
        // Row 3: CAM 01 (Left Hall), CAM 02 (Right Hall), [empty]
        // Simplified: a 2x3 grid showing all 6 cams
        string[] labels = { "1", "2", "3", "4", "5", "6" };
        string[] subs = { "L HALL", "R HALL", "CORR", "STAGE", "CLASS", "BATH" };

        mapButtons = new Button[6];
        for (int i = 0; i < 6; i++)
        {
            int idx = i;  // capture for closure
            int row = i / 3;
            int col = i % 3;

            GameObject btnObj = new GameObject("MapBtn_" + i);
            btnObj.transform.SetParent(mapObj.transform, false);
            var btnImg = btnObj.AddComponent<Image>();
            btnImg.color = new Color(0.2f, 0.2f, 0.25f);
            var btn = btnObj.AddComponent<Button>();
            btn.targetGraphic = btnImg;
            btn.onClick.AddListener(() => SwitchToCamera(idx));

            var bRT = btnImg.rectTransform;
            bRT.anchorMin = new Vector2(0, 1);
            bRT.anchorMax = new Vector2(0, 1);
            bRT.pivot = new Vector2(0, 1);
            bRT.anchoredPosition = new Vector2(15 + col * 85, -15 - row * 90);
            bRT.sizeDelta = new Vector2(75, 75);

            // Number label
            GameObject numObj = new GameObject("Num");
            numObj.transform.SetParent(btnObj.transform, false);
            var num = numObj.AddComponent<TextMeshProUGUI>();
            num.text = labels[i];
            num.fontSize = 32;
            num.fontStyle = FontStyles.Bold;
            num.color = new Color(1f, 0.3f, 0.3f);
            num.alignment = TextAlignmentOptions.Center;
            var nRT = num.rectTransform;
            nRT.anchorMin = Vector2.zero;
            nRT.anchorMax = Vector2.one;
            nRT.offsetMin = new Vector2(0, 18);
            nRT.offsetMax = Vector2.zero;

            // Sub label
            GameObject subObj = new GameObject("Sub");
            subObj.transform.SetParent(btnObj.transform, false);
            var sub = subObj.AddComponent<TextMeshProUGUI>();
            sub.text = subs[i];
            sub.fontSize = 13;
            sub.color = new Color(0.8f, 0.8f, 0.8f);
            sub.alignment = TextAlignmentOptions.Center;
            var sRT = sub.rectTransform;
            sRT.anchorMin = Vector2.zero;
            sRT.anchorMax = Vector2.one;
            sRT.offsetMin = Vector2.zero;
            sRT.offsetMax = new Vector2(0, -50);

            mapButtons[i] = btn;
        }
    }

    void SwitchToCamera(int index)
    {
        if (index < 0 || index >= securityCameras.Length) return;
        currentCamera = index;
        UpdateCamera();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayCameraClick();
        HighlightCurrentCam();
    }

    void HighlightCurrentCam()
    {
        if (mapButtons == null) return;
        for (int i = 0; i < mapButtons.Length; i++)
        {
            var img = mapButtons[i].GetComponent<Image>();
            if (img != null)
                img.color = (i == currentCamera) ? new Color(0.6f, 0.15f, 0.15f) : new Color(0.2f, 0.2f, 0.25f);
        }
    }

    void BuildBatteryDisplay()
    {
        GameObject obj = new GameObject("BatteryText");
        obj.transform.position = new Vector3(-1.2f, 1.15f, -0.46f);
        obj.transform.rotation = Quaternion.Euler(0, 180, 0);

        TextMeshPro tmp = obj.AddComponent<TextMeshPro>();
        tmp.text = "BATT\n100%";
        tmp.fontSize = 0.9f;
        tmp.color = new Color(0.3f, 1f, 0.3f);
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        tmp.rectTransform.sizeDelta = new Vector2(0.7f, 0.4f);
        batteryText = tmp;
    }

    void BuildMiddleScreenText()
    {
        // 3D text on the middle monitor screen
        GameObject obj = new GameObject("MiddleScreenText");
        obj.transform.position = new Vector3(0f, 1.15f, -0.46f);
        obj.transform.rotation = Quaternion.Euler(0, 180, 0);

        TextMeshPro tmp = obj.AddComponent<TextMeshPro>();
        tmp.text = "PRESS C\nFOR CAMS";
        tmp.fontSize = 0.9f;
        tmp.color = new Color(1f, 0.3f, 0.3f);
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.fontStyle = FontStyles.Bold;
        tmp.rectTransform.sizeDelta = new Vector2(0.7f, 0.4f);
        middleScreenText = tmp;
    }

    void UpdateCamera()
    {
        if (securityCameras == null || securityCameras.Length == 0) return;

        foreach (Camera cam in securityCameras)
        {
            cam.targetTexture = null;
            cam.enabled = false;
        }

        if (!monitorActive) return;

        Camera activeCam = securityCameras[currentCamera];
        activeCam.targetTexture = renderTexture;
        activeCam.enabled = true;

        if (monitorDisplay != null)
            monitorDisplay.texture = renderTexture;

        if (cameraNameText != null && cameraNames.Length > currentCamera)
            cameraNameText.text = cameraNames[currentCamera];
    }

    void OnDestroy()
    {
        if (renderTexture != null)
            renderTexture.Release();
    }
}
