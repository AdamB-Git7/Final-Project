using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

// =====================================================================
//  PAUSE MENU
// =====================================================================
//  Press ESC to pause the game. Shows a panel with 3 buttons:
//    - Resume: continues the game
//    - Restart: reloads the current scene
//    - Quit: exits the application
//
//  Pausing freezes the game by setting Time.timeScale = 0.
//  Resuming restores Time.timeScale = 1.
// =====================================================================
public class PauseMenu : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.Escape;

    bool isPaused;
    GameObject panel;   // the pause menu UI panel

    // =================================================================
    //  Start() — build the pause menu panel (hidden by default)
    // =================================================================
    void Start()
    {
        BuildMenu();
        panel.SetActive(false);   // hide on startup
    }

    // =================================================================
    //  Update() — listen for ESC key
    // =================================================================
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused) Resume();
            else          Pause();
        }
    }

    // =================================================================
    //  Pause() — freeze the game and show the menu
    // =================================================================
    public void Pause()
    {
        Time.timeScale = 0f;     // 0 = frozen, 1 = normal speed
        panel.SetActive(true);   // show the menu
        isPaused = true;
    }

    // =================================================================
    //  Resume() — unfreeze and hide the menu
    // =================================================================
    public void Resume()
    {
        Time.timeScale = 1f;
        panel.SetActive(false);
        isPaused = false;
    }

    // =================================================================
    //  Restart() — reload the current scene
    // =================================================================
    public void Restart()
    {
        Time.timeScale = 1f;     // make sure time runs normally after reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // =================================================================
    //  Quit() — exit the application
    // =================================================================
    public void Quit()
    {
        // In the editor we can't really quit, so stop play mode
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // =================================================================
    //  BuildMenu() — create the UI panel from code
    // =================================================================
    void BuildMenu()
    {
        // Make sure an EventSystem exists — Unity needs it to detect UI clicks
        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)
        {
            GameObject es = new GameObject("EventSystem");
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        }

        // Create a Canvas (overlay UI)
        GameObject canvasObj = new GameObject("PauseMenuCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 50;   // above gameplay UI but below jumpscare
        var scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        canvasObj.AddComponent<GraphicRaycaster>();

        // Dark semi-transparent background
        panel = new GameObject("PausePanel");
        panel.transform.SetParent(canvasObj.transform, false);
        Image bg = panel.AddComponent<Image>();
        bg.color = new Color(0f, 0f, 0f, 0.85f);
        var bgRT = bg.rectTransform;
        bgRT.anchorMin = Vector2.zero;
        bgRT.anchorMax = Vector2.one;
        bgRT.offsetMin = Vector2.zero;
        bgRT.offsetMax = Vector2.zero;

        // Title
        AddText(panel.transform, "PausedTitle", "PAUSED", 150,
                new Color(1f, 0.3f, 0.3f), new Vector2(0, 250), new Vector2(800, 200));

        // 3 buttons stacked vertically
        AddButton(panel.transform, "ResumeBtn",  "RESUME",  new Vector2(0,  50), Resume);
        AddButton(panel.transform, "RestartBtn", "RESTART", new Vector2(0, -50), Restart);
        AddButton(panel.transform, "QuitBtn",    "QUIT",    new Vector2(0, -150), Quit);

        // Hint at the bottom
        AddText(panel.transform, "Hint", "Press ESC to resume", 32,
                new Color(0.7f, 0.7f, 0.7f), new Vector2(0, -300), new Vector2(800, 50));
    }

    // -----------------------------------------------------------------
    //  Helper: add centered text to a parent
    // -----------------------------------------------------------------
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

    // -----------------------------------------------------------------
    //  Helper: add a clickable button to the panel
    // -----------------------------------------------------------------
    void AddButton(Transform parent, string name, string label,
                   Vector2 pos, UnityEngine.Events.UnityAction onClick)
    {
        // Button background
        GameObject btnObj = new GameObject(name);
        btnObj.transform.SetParent(parent, false);
        Image btnBg = btnObj.AddComponent<Image>();
        btnBg.color = new Color(0.3f, 0.05f, 0.05f);
        var btnRT = btnBg.rectTransform;
        btnRT.anchorMin = new Vector2(0.5f, 0.5f);
        btnRT.anchorMax = new Vector2(0.5f, 0.5f);
        btnRT.pivot = new Vector2(0.5f, 0.5f);
        btnRT.anchoredPosition = pos;
        btnRT.sizeDelta = new Vector2(400, 80);

        // Button component handles click events
        Button btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = btnBg;
        btn.onClick.AddListener(onClick);   // call our method when clicked

        // Button label text (child of the button)
        GameObject labelObj = new GameObject("Label");
        labelObj.transform.SetParent(btnObj.transform, false);
        var lbl = labelObj.AddComponent<TextMeshProUGUI>();
        lbl.text = label;
        lbl.fontSize = 50;
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
