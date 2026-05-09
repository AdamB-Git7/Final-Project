
// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.UI namespace.
using UnityEngine.UI;

// Imports the TMPro namespace.
using TMPro;


// Declares the class named SecurityCamera.
public class SecurityCamera : MonoBehaviour

// Opens a new code block.
{

    // Applies the Header("Controls") attribute.
    [Header("Controls")]

    // Declares the variable toggleKey and initializes it.
    public KeyCode toggleKey = KeyCode.C;


    // Applies the Header("Battery") attribute.
    [Header("Battery")]

    // Declares the variable maxBattery and initializes it.
    public float maxBattery = 100f;

    // Declares the variable drainPerSecond and initializes it.
    public float drainPerSecond = 8f;

    // Declares the variable doorDrainPerSecond and initializes it.
    public float doorDrainPerSecond = 3f;

    // Declares the variable rechargePerSecond and initializes it.
    public float rechargePerSecond = 12f;

    // Declares the variable rechargeKey and initializes it.
    public KeyCode rechargeKey = KeyCode.F;


    // Applies the Header("Animation") attribute.
    [Header("Animation")]

    // Declares the variable fadeSpeed and initializes it.
    public float fadeSpeed = 6f;


    // Declares the variable securityCameras.
    Camera[] securityCameras;

    // Declares the variable cameraNames.
    string[] cameraNames;

    // Declares the variable monitorDisplay.
    RawImage monitorDisplay;

    // Declares the variable cameraNameText.
    TMP_Text cameraNameText;

    // Declares the variable batteryText.
    TMP_Text batteryText;

    // Declares the variable middleScreenText.
    TMP_Text middleScreenText;

    // Declares the variable monitorPanel.
    GameObject monitorPanel;

    // Declares the variable panelGroup.
    CanvasGroup panelGroup;


    // Declares the variable currentCamera.
    int currentCamera;

    // Declares the variable monitorActive.
    bool monitorActive;

    // Declares the variable battery.
    float battery;

    // Declares the variable panelAlpha.
    float panelAlpha;

    // Declares the variable renderTexture.
    RenderTexture renderTexture;


    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {

        // Updates an existing value.
        battery = maxBattery;

        // Calls a method.
        BuildCameras();

        // Calls a method.
        BuildUI();

        // Calls a method.
        BuildBatteryDisplay();

        // Calls a method.
        BuildMiddleScreenText();


        // Updates an existing value.
        currentCamera = 0;

        // Updates an existing value.
        monitorActive = false;

        // Updates an existing value.
        panelAlpha = 0f;


        // Updates an existing value.
        renderTexture = new RenderTexture(800, 500, 16);


        // Iterates through each item in the collection.
        foreach (Camera cam in securityCameras)

            // Updates an existing value.
            cam.enabled = false;


        // Calls a method.
        monitorPanel.SetActive(false);

    // Closes the current code block.
    }


    // Declares the method named Update.
    void Update()

    // Opens a new code block.
    {

        // Checks whether the condition is true.
        if (Input.GetKeyDown(toggleKey))

            // Calls a method.
            ToggleMonitor();


        // Calls a method.
        DrainBattery();

        // Calls a method.
        UpdateBatteryDisplay();

        // Calls a method.
        UpdateMiddleScreen();

        // Calls a method.
        AnimatePanel();


        // Checks the condition and runs the inline statement when it is true.
        if (!monitorActive) return;



        // Starts a for loop.
        for (int i = 0; i < 6; i++)

        // Opens a new code block.
        {

            // Checks whether the condition is true.
            if (Input.GetKeyDown(KeyCode.Alpha1 + i) || Input.GetKeyDown(KeyCode.Keypad1 + i))

                // Calls a method.
                SwitchToCamera(i);

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Declares the method named ToggleMonitor.
    void ToggleMonitor()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (!monitorActive && battery <= 0f) return;


        // Updates an existing value.
        monitorActive = !monitorActive;

        // Checks whether the condition is true.
        if (monitorActive)

            // Calls a method.
            monitorPanel.SetActive(true);

        // Calls a method.
        UpdateCamera();

        // Calls a method.
        HighlightCurrentCam();


        // Checks whether the condition is true.
        if (AudioManager.Instance != null)

            // Calls a method.
            AudioManager.Instance.PlayCameraClick();

    // Closes the current code block.
    }


    // Declares the method named AnimatePanel.
    void AnimatePanel()

    // Opens a new code block.
    {

        // Declares the variable target and initializes it.
        float target = monitorActive ? 1f : 0f;

        // Updates an existing value.
        panelAlpha = Mathf.MoveTowards(panelAlpha, target, fadeSpeed * Time.deltaTime);


        // Checks whether the condition is true.
        if (panelGroup != null)

            // Updates an existing value.
            panelGroup.alpha = panelAlpha;



        // Checks whether the condition is true.
        if (!monitorActive && panelAlpha <= 0.01f && monitorPanel.activeSelf)

            // Calls a method.
            monitorPanel.SetActive(false);

    // Closes the current code block.
    }


    // Declares the method named DrainBatteryExternal.
    public void DrainBatteryExternal(float amount)

    // Opens a new code block.
    {

        // Updates an existing value.
        battery -= amount;

        // Checks the condition and runs the inline statement when it is true.
        if (battery < 0f) battery = 0f;

    // Closes the current code block.
    }


    // Declares the method named DrainBattery.
    void DrainBattery()

    // Opens a new code block.
    {


        // Checks whether the condition is true.
        if (!monitorActive && Input.GetKey(rechargeKey))

        // Opens a new code block.
        {

            // Updates an existing value.
            battery += rechargePerSecond * Time.deltaTime;

            // Checks the condition and runs the inline statement when it is true.
            if (battery > maxBattery) battery = maxBattery;

            // Returns from the current method.
            return;

        // Closes the current code block.
        }


        // Declares the variable drain and initializes it.
        float drain = 0f;

        // Checks the condition and runs the inline statement when it is true.
        if (monitorActive) drain += drainPerSecond;


        // Declares the variable doors and initializes it.
        DoorController[] doors = Object.FindObjectsByType<DoorController>(FindObjectsSortMode.None);

        // Iterates through each item in the collection.
        foreach (var d in doors)

            // Checks the condition and runs the inline statement when it is true.
            if (d.isClosed) drain += doorDrainPerSecond;


        // Checks the condition and runs the inline statement when it is true.
        if (drain <= 0f) return;


        // Updates an existing value.
        battery -= drain * Time.deltaTime;

        // Checks whether the condition is true.
        if (battery <= 0f)

        // Opens a new code block.
        {

            // Updates an existing value.
            battery = 0f;

            // Updates an existing value.
            monitorActive = false;

            // Iterates through each item in the collection.
            foreach (var d in doors)

                // Checks the condition and runs the inline statement when it is true.
                if (d.isClosed) d.OpenDoor();

            // Calls a method.
            UpdateCamera();

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Declares the method named UpdateBatteryDisplay.
    void UpdateBatteryDisplay()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (batteryText == null) return;

        // Updates an existing value.
        batteryText.text = "BATT\n" + Mathf.CeilToInt(battery) + "%";


        // Checks whether the condition is true.
        if (battery > 50f)

            // Updates an existing value.
            batteryText.color = new Color(0.3f, 1f, 0.3f);

        // Checks the next condition when earlier conditions were false.
        else if (battery > 20f)

            // Updates an existing value.
            batteryText.color = new Color(1f, 0.8f, 0.2f);

        // Runs the fallback branch when earlier conditions were false.
        else

            // Updates an existing value.
            batteryText.color = new Color(1f, 0.2f, 0.2f);

    // Closes the current code block.
    }


    // Declares the method named UpdateMiddleScreen.
    void UpdateMiddleScreen()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (middleScreenText == null) return;

        // Checks whether the condition is true.
        if (monitorActive)

            // Updates an existing value.
            middleScreenText.text = "VIEWING\nCAMERA";

        // Checks the next condition when earlier conditions were false.
        else if (Input.GetKey(rechargeKey))

            // Updates an existing value.
            middleScreenText.text = "CHARGING\n+12%/s";

        // Checks the next condition when earlier conditions were false.
        else if (battery <= 0f)

            // Updates an existing value.
            middleScreenText.text = "HOLD F\nTO CHARGE";

        // Runs the fallback branch when earlier conditions were false.
        else

            // Updates an existing value.
            middleScreenText.text = "C: CAMS\nF: CHARGE";

    // Closes the current code block.
    }


    // Declares the method named BuildCameras.
    void BuildCameras()

    // Opens a new code block.
    {

        // Executes this statement.
        Vector3[] positions = {

            // Executes this statement.
            new Vector3(-5.25f, 2.8f, 1.5f),

            // Executes this statement.
            new Vector3(5.25f, 2.8f, 1.5f),

            // Executes this statement.
            new Vector3(0, 2.8f, -19f),

            // Executes this statement.
            new Vector3(0, 2.8f, -20.5f),

            // Executes this statement.
            new Vector3(-7.5f, 2.8f, -12f),

            // Declares the method named Vector3.
            new Vector3(7.5f, 2.8f, -12f)

        // Executes this statement.
        };

        // Executes this statement.
        Vector3[] rotations = {

            // Executes this statement.
            new Vector3(20, 180, 0),

            // Executes this statement.
            new Vector3(20, 180, 0),

            // Executes this statement.
            new Vector3(15, 180, 0),

            // Executes this statement.
            new Vector3(15, 180, 0),

            // Executes this statement.
            new Vector3(15, -90, 0),

            // Declares the method named Vector3.
            new Vector3(15, 90, 0)

        // Executes this statement.
        };

        // Updates an existing value.
        cameraNames = new[] {

            // Executes this statement.
            "CAM 01 - LEFT HALL",

            // Executes this statement.
            "CAM 02 - RIGHT HALL",

            // Executes this statement.
            "CAM 03 - CORRIDOR",

            // Executes this statement.
            "CAM 04 - STAGE",

            // Executes this statement.
            "CAM 05 - CLASSROOM",

            // Executes this statement.
            "CAM 06 - BATHROOM"

        // Executes this statement.
        };


        // Updates an existing value.
        securityCameras = new Camera[positions.Length];

        // Starts a for loop.
        for (int i = 0; i < positions.Length; i++)

        // Opens a new code block.
        {

            // Declares the variable camObj and initializes it.
            GameObject camObj = new GameObject("SecurityCam_" + i);

            // Updates an existing value.
            camObj.transform.position = positions[i];

            // Updates an existing value.
            camObj.transform.rotation = Quaternion.Euler(rotations[i]);


            // Declares the variable cam and initializes it.
            Camera cam = camObj.AddComponent<Camera>();

            // Updates an existing value.
            cam.fieldOfView = 70f;

            // Updates an existing value.
            cam.farClipPlane = 30f;

            // Updates an existing value.
            cam.backgroundColor = Color.black;

            // Updates an existing value.
            cam.clearFlags = CameraClearFlags.SolidColor;


            // Updates an existing value.
            securityCameras[i] = cam;

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Declares the method named BuildUI.
    void BuildUI()

    // Opens a new code block.
    {

        // Declares the variable canvasObj and initializes it.
        GameObject canvasObj = new GameObject("CameraMonitorCanvas");

        // Declares the variable canvas and initializes it.
        Canvas canvas = canvasObj.AddComponent<Canvas>();

        // Updates an existing value.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Updates an existing value.
        canvas.sortingOrder = 5;

        // Declares the variable scaler and initializes it.
        var scaler = canvasObj.AddComponent<CanvasScaler>();

        // Updates an existing value.
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // Updates an existing value.
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Calls a method.
        canvasObj.AddComponent<GraphicRaycaster>();


        // Updates an existing value.
        monitorPanel = new GameObject("MonitorPanel");

        // Calls a method.
        monitorPanel.transform.SetParent(canvasObj.transform, false);

        // Updates an existing value.
        panelGroup = monitorPanel.AddComponent<CanvasGroup>();

        // Declares the variable bg and initializes it.
        Image bg = monitorPanel.AddComponent<Image>();

        // Updates an existing value.
        bg.color = new Color(0f, 0f, 0f, 0.95f);

        // Declares the variable bgRT and initializes it.
        RectTransform bgRT = bg.rectTransform;

        // Updates an existing value.
        bgRT.anchorMin = Vector2.zero;

        // Updates an existing value.
        bgRT.anchorMax = Vector2.one;

        // Updates an existing value.
        bgRT.offsetMin = Vector2.zero;

        // Updates an existing value.
        bgRT.offsetMax = Vector2.zero;


        // Declares the variable frameObj and initializes it.
        GameObject frameObj = new GameObject("MonitorFrame");

        // Calls a method.
        frameObj.transform.SetParent(monitorPanel.transform, false);

        // Declares the variable frame and initializes it.
        Image frame = frameObj.AddComponent<Image>();

        // Updates an existing value.
        frame.color = new Color(0.2f, 0.2f, 0.2f);

        // Declares the variable frameRT and initializes it.
        RectTransform frameRT = frame.rectTransform;

        // Updates an existing value.
        frameRT.anchorMin = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        frameRT.anchorMax = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        frameRT.pivot = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        frameRT.sizeDelta = new Vector2(1340, 820);

        // Updates an existing value.
        frameRT.anchoredPosition = Vector2.zero;


        // Declares the variable feedObj and initializes it.
        GameObject feedObj = new GameObject("CameraFeed");

        // Calls a method.
        feedObj.transform.SetParent(frameObj.transform, false);

        // Updates an existing value.
        monitorDisplay = feedObj.AddComponent<RawImage>();

        // Declares the variable feedRT and initializes it.
        RectTransform feedRT = monitorDisplay.rectTransform;

        // Updates an existing value.
        feedRT.anchorMin = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        feedRT.anchorMax = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        feedRT.pivot = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        feedRT.sizeDelta = new Vector2(1280, 720);

        // Updates an existing value.
        feedRT.anchoredPosition = Vector2.zero;


        // Declares the variable nameObj and initializes it.
        GameObject nameObj = new GameObject("CameraNameText");

        // Calls a method.
        nameObj.transform.SetParent(frameObj.transform, false);

        // Updates an existing value.
        cameraNameText = nameObj.AddComponent<TextMeshProUGUI>();

        // Updates an existing value.
        cameraNameText.text = "CAM 01";

        // Updates an existing value.
        cameraNameText.fontSize = 36;

        // Updates an existing value.
        cameraNameText.color = new Color(1f, 0.3f, 0.3f);

        // Updates an existing value.
        cameraNameText.alignment = TextAlignmentOptions.Center;

        // Updates an existing value.
        cameraNameText.fontStyle = FontStyles.Bold;

        // Declares the variable nameRT and initializes it.
        RectTransform nameRT = cameraNameText.rectTransform;

        // Updates an existing value.
        nameRT.anchorMin = new Vector2(0.5f, 1f);

        // Updates an existing value.
        nameRT.anchorMax = new Vector2(0.5f, 1f);

        // Updates an existing value.
        nameRT.pivot = new Vector2(0.5f, 1f);

        // Updates an existing value.
        nameRT.anchoredPosition = new Vector2(0, -10);

        // Updates an existing value.
        nameRT.sizeDelta = new Vector2(800, 60);


        // Declares the variable hintObj and initializes it.
        GameObject hintObj = new GameObject("HintText");

        // Calls a method.
        hintObj.transform.SetParent(frameObj.transform, false);

        // Declares the variable hint and initializes it.
        var hint = hintObj.AddComponent<TextMeshProUGUI>();

        // Updates an existing value.
        hint.text = "[1-6] SWITCH CAM   [C] CLOSE";

        // Updates an existing value.
        hint.fontSize = 24;

        // Updates an existing value.
        hint.color = new Color(0.7f, 0.7f, 0.7f);

        // Updates an existing value.
        hint.alignment = TextAlignmentOptions.Center;

        // Declares the variable hintRT and initializes it.
        RectTransform hintRT = hint.rectTransform;

        // Updates an existing value.
        hintRT.anchorMin = new Vector2(0.5f, 0f);

        // Updates an existing value.
        hintRT.anchorMax = new Vector2(0.5f, 0f);

        // Updates an existing value.
        hintRT.pivot = new Vector2(0.5f, 0f);

        // Updates an existing value.
        hintRT.anchoredPosition = new Vector2(0, 10);

        // Updates an existing value.
        hintRT.sizeDelta = new Vector2(800, 40);


        // Calls a method.
        BuildCameraMap(frameObj.transform);

    // Closes the current code block.
    }


    // Declares the variable mapButtons.
    Button[] mapButtons;


    // Declares the method named BuildCameraMap.
    void BuildCameraMap(Transform parent)

    // Opens a new code block.
    {


        // Declares the variable mapObj and initializes it.
        GameObject mapObj = new GameObject("CameraMap");

        // Calls a method.
        mapObj.transform.SetParent(parent, false);

        // Declares the variable mapBg and initializes it.
        var mapBg = mapObj.AddComponent<Image>();

        // Updates an existing value.
        mapBg.color = new Color(0.1f, 0.1f, 0.1f, 0.85f);

        // Declares the variable mapRT and initializes it.
        var mapRT = mapBg.rectTransform;

        // Updates an existing value.
        mapRT.anchorMin = new Vector2(1f, 0f);

        // Updates an existing value.
        mapRT.anchorMax = new Vector2(1f, 0f);

        // Updates an existing value.
        mapRT.pivot = new Vector2(1f, 0f);

        // Updates an existing value.
        mapRT.anchoredPosition = new Vector2(-20, 60);

        // Updates an existing value.
        mapRT.sizeDelta = new Vector2(280, 200);







        // Declares the variable labels and initializes it.
        string[] labels = { "1", "2", "3", "4", "5", "6" };

        // Declares the variable subs and initializes it.
        string[] subs = { "L HALL", "R HALL", "CORR", "STAGE", "CLASS", "BATH" };


        // Updates an existing value.
        mapButtons = new Button[6];

        // Starts a for loop.
        for (int i = 0; i < 6; i++)

        // Opens a new code block.
        {

            // Declares the variable idx and initializes it.
            int idx = i;

            // Declares the variable row and initializes it.
            int row = i / 3;

            // Declares the variable col and initializes it.
            int col = i % 3;


            // Declares the variable btnObj and initializes it.
            GameObject btnObj = new GameObject("MapBtn_" + i);

            // Calls a method.
            btnObj.transform.SetParent(mapObj.transform, false);

            // Declares the variable btnImg and initializes it.
            var btnImg = btnObj.AddComponent<Image>();

            // Updates an existing value.
            btnImg.color = new Color(0.2f, 0.2f, 0.25f);

            // Declares the variable btn and initializes it.
            var btn = btnObj.AddComponent<Button>();

            // Updates an existing value.
            btn.targetGraphic = btnImg;

            // Calls a method.
            btn.onClick.AddListener(() => SwitchToCamera(idx));


            // Declares the variable bRT and initializes it.
            var bRT = btnImg.rectTransform;

            // Updates an existing value.
            bRT.anchorMin = new Vector2(0, 1);

            // Updates an existing value.
            bRT.anchorMax = new Vector2(0, 1);

            // Updates an existing value.
            bRT.pivot = new Vector2(0, 1);

            // Updates an existing value.
            bRT.anchoredPosition = new Vector2(15 + col * 85, -15 - row * 90);

            // Updates an existing value.
            bRT.sizeDelta = new Vector2(75, 75);



            // Declares the variable numObj and initializes it.
            GameObject numObj = new GameObject("Num");

            // Calls a method.
            numObj.transform.SetParent(btnObj.transform, false);

            // Declares the variable num and initializes it.
            var num = numObj.AddComponent<TextMeshProUGUI>();

            // Updates an existing value.
            num.text = labels[i];

            // Updates an existing value.
            num.fontSize = 32;

            // Updates an existing value.
            num.fontStyle = FontStyles.Bold;

            // Updates an existing value.
            num.color = new Color(1f, 0.3f, 0.3f);

            // Updates an existing value.
            num.alignment = TextAlignmentOptions.Center;

            // Declares the variable nRT and initializes it.
            var nRT = num.rectTransform;

            // Updates an existing value.
            nRT.anchorMin = Vector2.zero;

            // Updates an existing value.
            nRT.anchorMax = Vector2.one;

            // Updates an existing value.
            nRT.offsetMin = new Vector2(0, 18);

            // Updates an existing value.
            nRT.offsetMax = Vector2.zero;



            // Declares the variable subObj and initializes it.
            GameObject subObj = new GameObject("Sub");

            // Calls a method.
            subObj.transform.SetParent(btnObj.transform, false);

            // Declares the variable sub and initializes it.
            var sub = subObj.AddComponent<TextMeshProUGUI>();

            // Updates an existing value.
            sub.text = subs[i];

            // Updates an existing value.
            sub.fontSize = 13;

            // Updates an existing value.
            sub.color = new Color(0.8f, 0.8f, 0.8f);

            // Updates an existing value.
            sub.alignment = TextAlignmentOptions.Center;

            // Declares the variable sRT and initializes it.
            var sRT = sub.rectTransform;

            // Updates an existing value.
            sRT.anchorMin = Vector2.zero;

            // Updates an existing value.
            sRT.anchorMax = Vector2.one;

            // Updates an existing value.
            sRT.offsetMin = Vector2.zero;

            // Updates an existing value.
            sRT.offsetMax = new Vector2(0, -50);


            // Updates an existing value.
            mapButtons[i] = btn;

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Declares the method named SwitchToCamera.
    void SwitchToCamera(int index)

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (index < 0 || index >= securityCameras.Length) return;

        // Updates an existing value.
        currentCamera = index;

        // Calls a method.
        UpdateCamera();

        // Checks the condition and runs the inline statement when it is true.
        if (AudioManager.Instance != null) AudioManager.Instance.PlayCameraClick();

        // Calls a method.
        HighlightCurrentCam();

    // Closes the current code block.
    }


    // Declares the method named HighlightCurrentCam.
    void HighlightCurrentCam()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (mapButtons == null) return;

        // Starts a for loop.
        for (int i = 0; i < mapButtons.Length; i++)

        // Opens a new code block.
        {

            // Declares the variable img and initializes it.
            var img = mapButtons[i].GetComponent<Image>();

            // Checks whether the condition is true.
            if (img != null)

                // Updates an existing value.
                img.color = (i == currentCamera) ? new Color(0.6f, 0.15f, 0.15f) : new Color(0.2f, 0.2f, 0.25f);

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Declares the method named BuildBatteryDisplay.
    void BuildBatteryDisplay()

    // Opens a new code block.
    {

        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject("BatteryText");

        // Updates an existing value.
        obj.transform.position = new Vector3(-1.2f, 1.15f, -0.46f);

        // Updates an existing value.
        obj.transform.rotation = Quaternion.Euler(0, 180, 0);


        // Declares the variable tmp and initializes it.
        TextMeshPro tmp = obj.AddComponent<TextMeshPro>();

        // Updates an existing value.
        tmp.text = "BATT\n100%";

        // Updates an existing value.
        tmp.fontSize = 0.9f;

        // Updates an existing value.
        tmp.color = new Color(0.3f, 1f, 0.3f);

        // Updates an existing value.
        tmp.alignment = TextAlignmentOptions.Center;

        // Updates an existing value.
        tmp.fontStyle = FontStyles.Bold;

        // Updates an existing value.
        tmp.rectTransform.sizeDelta = new Vector2(0.7f, 0.4f);

        // Updates an existing value.
        batteryText = tmp;

    // Closes the current code block.
    }


    // Declares the method named BuildMiddleScreenText.
    void BuildMiddleScreenText()

    // Opens a new code block.
    {


        // Declares the variable obj and initializes it.
        GameObject obj = new GameObject("MiddleScreenText");

        // Updates an existing value.
        obj.transform.position = new Vector3(0f, 1.15f, -0.46f);

        // Updates an existing value.
        obj.transform.rotation = Quaternion.Euler(0, 180, 0);


        // Declares the variable tmp and initializes it.
        TextMeshPro tmp = obj.AddComponent<TextMeshPro>();

        // Updates an existing value.
        tmp.text = "PRESS C\nFOR CAMS";

        // Updates an existing value.
        tmp.fontSize = 0.9f;

        // Updates an existing value.
        tmp.color = new Color(1f, 0.3f, 0.3f);

        // Updates an existing value.
        tmp.alignment = TextAlignmentOptions.Center;

        // Updates an existing value.
        tmp.fontStyle = FontStyles.Bold;

        // Updates an existing value.
        tmp.rectTransform.sizeDelta = new Vector2(0.7f, 0.4f);

        // Updates an existing value.
        middleScreenText = tmp;

    // Closes the current code block.
    }


    // Declares the method named UpdateCamera.
    void UpdateCamera()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (securityCameras == null || securityCameras.Length == 0) return;


        // Iterates through each item in the collection.
        foreach (Camera cam in securityCameras)

        // Opens a new code block.
        {

            // Updates an existing value.
            cam.targetTexture = null;

            // Updates an existing value.
            cam.enabled = false;

        // Closes the current code block.
        }


        // Checks the condition and runs the inline statement when it is true.
        if (!monitorActive) return;


        // Declares the variable activeCam and initializes it.
        Camera activeCam = securityCameras[currentCamera];

        // Updates an existing value.
        activeCam.targetTexture = renderTexture;

        // Updates an existing value.
        activeCam.enabled = true;


        // Checks whether the condition is true.
        if (monitorDisplay != null)

            // Updates an existing value.
            monitorDisplay.texture = renderTexture;


        // Checks whether the condition is true.
        if (cameraNameText != null && cameraNames.Length > currentCamera)

            // Updates an existing value.
            cameraNameText.text = cameraNames[currentCamera];

    // Closes the current code block.
    }


    // Declares the method named OnDestroy.
    void OnDestroy()

    // Opens a new code block.
    {

        // Checks whether the condition is true.
        if (renderTexture != null)

            // Calls a method.
            renderTexture.Release();

    // Closes the current code block.
    }

// Closes the current code block.
}
