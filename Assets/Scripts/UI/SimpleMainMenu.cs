using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// =====================================================================
//  SIMPLE MAIN MENU
//  Builds a basic FNAF-style menu from code:
//    - Title "NIGHT SHIFT: FREDDY PROTOCOL"
//    - NEW GAME button (loads the game scene)
//    - QUIT button
// =====================================================================
public class SimpleMainMenu : MonoBehaviour
{
    void Start()
    {
        // Reset the night counter to 1 when entering the menu
        PlayerPrefs.SetInt("CurrentNight", 1);

        BuildMenu();
        PlayAmbient();
    }

    void PlayAmbient()
    {
        // Build the AudioManager so the menu has the horror drone playing
        if (Object.FindFirstObjectByType<AudioManager>() == null)
            new GameObject("AudioManager").AddComponent<AudioManager>();
    }

    void BuildMenu()
    {
        // Make sure an EventSystem exists (needed for button clicks)
        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // Make sure there's a camera in the scene (otherwise everything is black)
        if (Camera.main == null)
        {
            GameObject camObj = new GameObject("Main Camera");
            camObj.tag = "MainCamera";
            Camera cam = camObj.AddComponent<Camera>();
            camObj.AddComponent<AudioListener>();
            cam.backgroundColor = new Color(0.05f, 0.02f, 0.02f);
            cam.clearFlags = CameraClearFlags.SolidColor;
        }

        // Build the canvas
        GameObject canvasObj = new GameObject("MenuCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        // Dark background
        GameObject bg = new GameObject("Background");
        bg.transform.SetParent(canvasObj.transform, false);
        Image bgImg = bg.AddComponent<Image>();
        bgImg.color = new Color(0.05f, 0.02f, 0.02f);
        var bgRT = bgImg.rectTransform;
        bgRT.anchorMin = Vector2.zero;
        bgRT.anchorMax = Vector2.one;
        bgRT.offsetMin = Vector2.zero;
        bgRT.offsetMax = Vector2.zero;

        // Title — big red glowing text
        AddText(canvasObj.transform, "Title", "NIGHT SHIFT", 220,
                new Color(0.9f, 0.1f, 0.1f), new Vector2(0, 280), new Vector2(1800, 280));
        AddText(canvasObj.transform, "Subtitle", "FREDDY PROTOCOL", 100,
                new Color(0.95f, 0.95f, 0.95f), new Vector2(0, 130), new Vector2(1500, 150));

        // Buttons
        AddButton(canvasObj.transform, "NewGameBtn", "NEW GAME",
                  new Vector2(0, -50), NewGame);
        AddButton(canvasObj.transform, "QuitBtn", "QUIT",
                  new Vector2(0, -180), QuitGame);

        // Bottom credit
        AddText(canvasObj.transform, "Credit", "Made by Luca & Adam", 30,
                new Color(0.6f, 0.6f, 0.6f),
                new Vector2(0, -480), new Vector2(800, 50));
    }

    public void NewGame()
    {
        // Load the gameplay scene (build index 1 = scene.unity)
        SceneManager.LoadScene("scene");
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void AddText(Transform parent, string name, string text, int fontSize,
                 Color color, Vector2 pos, Vector2 size)
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
        rt.anchoredPosition = pos;
        rt.sizeDelta = size;
    }

    void AddButton(Transform parent, string name, string label,
                   Vector2 pos, UnityEngine.Events.UnityAction onClick)
    {
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent, false);
        Image btnBg = btnObj.AddComponent<Image>();
        btnBg.color = new Color(0.4f, 0.05f, 0.05f);
        var btnRT = btnBg.rectTransform;
        btnRT.anchorMin = new Vector2(0.5f, 0.5f);
        btnRT.anchorMax = new Vector2(0.5f, 0.5f);
        btnRT.pivot = new Vector2(0.5f, 0.5f);
        btnRT.anchoredPosition = pos;
        btnRT.sizeDelta = new Vector2(500, 100);

        Button btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = btnBg;
        btn.onClick.AddListener(onClick);

        GameObject lblObj = new GameObject("Label");
        lblObj.transform.SetParent(btnObj.transform, false);
        var lbl = lblObj.AddComponent<TextMeshProUGUI>();
        lbl.text = label;
        lbl.fontSize = 60;
        lbl.color = Color.white;
        lbl.alignment = TextAlignmentOptions.Center;
        lbl.fontStyle = FontStyles.Bold;
        var lblRT = lbl.rectTransform;
        lblRT.anchorMin = Vector2.zero;
        lblRT.anchorMax = Vector2.one;
        lblRT.offsetMin = Vector2.zero;
        lblRT.offsetMax = Vector2.zero;
    }
}
