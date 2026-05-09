
// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.UI namespace.
using UnityEngine.UI;

// Imports the UnityEngine.SceneManagement namespace.
using UnityEngine.SceneManagement;

// Imports the TMPro namespace.
using TMPro;













// Declares the class named PauseMenu.
public class PauseMenu : MonoBehaviour

// Opens a new code block.
{

    // Declares the variable pauseKey and initializes it.
    public KeyCode pauseKey = KeyCode.Escape;


    // Declares the variable isPaused.
    bool isPaused;

    // Declares the variable panel.
    GameObject panel;





    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {

        // Calls a method.
        BuildMenu();

        // Calls a method.
        panel.SetActive(false);

    // Closes the current code block.
    }





    // Declares the method named Update.
    void Update()

    // Opens a new code block.
    {

        // Checks whether the condition is true.
        if (Input.GetKeyDown(pauseKey))

        // Opens a new code block.
        {

            // Checks the condition and runs the inline statement when it is true.
            if (isPaused) Resume();

            // Runs the fallback inline statement.
            else          Pause();

        // Closes the current code block.
        }

    // Closes the current code block.
    }





    // Declares the method named Pause.
    public void Pause()

    // Opens a new code block.
    {

        // Updates an existing value.
        Time.timeScale = 0f;

        // Calls a method.
        panel.SetActive(true);

        // Updates an existing value.
        isPaused = true;

    // Closes the current code block.
    }





    // Declares the method named Resume.
    public void Resume()

    // Opens a new code block.
    {

        // Updates an existing value.
        Time.timeScale = 1f;

        // Calls a method.
        panel.SetActive(false);

        // Updates an existing value.
        isPaused = false;

    // Closes the current code block.
    }





    // Declares the method named Restart.
    public void Restart()

    // Opens a new code block.
    {

        // Updates an existing value.
        Time.timeScale = 1f;

        // Calls a method.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    // Closes the current code block.
    }





    // Declares the method named Quit.
    public void Quit()

    // Opens a new code block.
    {


        // Executes this statement.
        #if UNITY_EDITOR

            // Updates an existing value.
            UnityEditor.EditorApplication.isPlaying = false;

        // Executes this statement.
        #else

            // Calls a method.
            Application.Quit();

        // Executes this statement.
        #endif

    // Closes the current code block.
    }





    // Declares the method named BuildMenu.
    void BuildMenu()

    // Opens a new code block.
    {


        // Checks whether the condition is true.
        if (Object.FindFirstObjectByType<UnityEngine.EventSystems.EventSystem>() == null)

        // Opens a new code block.
        {

            // Declares the variable es and initializes it.
            GameObject es = new GameObject("EventSystem");

            // Calls a method.
            es.AddComponent<UnityEngine.EventSystems.EventSystem>();

            // Calls a method.
            es.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();

        // Closes the current code block.
        }



        // Declares the variable canvasObj and initializes it.
        GameObject canvasObj = new GameObject("PauseMenuCanvas");

        // Declares the variable canvas and initializes it.
        Canvas canvas = canvasObj.AddComponent<Canvas>();

        // Updates an existing value.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Updates an existing value.
        canvas.sortingOrder = 50;

        // Declares the variable scaler and initializes it.
        var scaler = canvasObj.AddComponent<CanvasScaler>();

        // Updates an existing value.
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // Updates an existing value.
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Calls a method.
        canvasObj.AddComponent<GraphicRaycaster>();



        // Updates an existing value.
        panel = new GameObject("PausePanel");

        // Calls a method.
        panel.transform.SetParent(canvasObj.transform, false);

        // Declares the variable bg and initializes it.
        Image bg = panel.AddComponent<Image>();

        // Updates an existing value.
        bg.color = new Color(0f, 0f, 0f, 0.85f);

        // Declares the variable bgRT and initializes it.
        var bgRT = bg.rectTransform;

        // Updates an existing value.
        bgRT.anchorMin = Vector2.zero;

        // Updates an existing value.
        bgRT.anchorMax = Vector2.one;

        // Updates an existing value.
        bgRT.offsetMin = Vector2.zero;

        // Updates an existing value.
        bgRT.offsetMax = Vector2.zero;



        // Calls a method.
        AddText(panel.transform, "PausedTitle", "PAUSED", 150,

                // Executes this statement.
                new Color(1f, 0.3f, 0.3f), new Vector2(0, 250), new Vector2(800, 200));



        // Calls a method.
        AddButton(panel.transform, "ResumeBtn",  "RESUME",  new Vector2(0,  50), Resume);

        // Calls a method.
        AddButton(panel.transform, "RestartBtn", "RESTART", new Vector2(0, -50), Restart);

        // Calls a method.
        AddButton(panel.transform, "QuitBtn",    "QUIT",    new Vector2(0, -150), Quit);



        // Calls a method.
        AddText(panel.transform, "Hint", "Press ESC to resume", 32,

                // Executes this statement.
                new Color(0.7f, 0.7f, 0.7f), new Vector2(0, -300), new Vector2(800, 50));

    // Closes the current code block.
    }





    // Executes this statement.
    void AddText(Transform parent, string name, string text, int fontSize,

                 // Executes this statement.
                 Color color, Vector2 pos, Vector2 size)

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
        rt.anchoredPosition = pos;

        // Updates an existing value.
        rt.sizeDelta = size;

    // Closes the current code block.
    }





    // Executes this statement.
    void AddButton(Transform parent, string name, string label,

                   // Executes this statement.
                   Vector2 pos, UnityEngine.Events.UnityAction onClick)

    // Opens a new code block.
    {


        // Declares the variable btnObj and initializes it.
        GameObject btnObj = new GameObject(name);

        // Calls a method.
        btnObj.transform.SetParent(parent, false);

        // Declares the variable btnBg and initializes it.
        Image btnBg = btnObj.AddComponent<Image>();

        // Updates an existing value.
        btnBg.color = new Color(0.3f, 0.05f, 0.05f);

        // Declares the variable btnRT and initializes it.
        var btnRT = btnBg.rectTransform;

        // Updates an existing value.
        btnRT.anchorMin = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        btnRT.anchorMax = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        btnRT.pivot = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        btnRT.anchoredPosition = pos;

        // Updates an existing value.
        btnRT.sizeDelta = new Vector2(400, 80);



        // Declares the variable btn and initializes it.
        Button btn = btnObj.AddComponent<Button>();

        // Updates an existing value.
        btn.targetGraphic = btnBg;

        // Calls a method.
        btn.onClick.AddListener(onClick);



        // Declares the variable labelObj and initializes it.
        GameObject labelObj = new GameObject("Label");

        // Calls a method.
        labelObj.transform.SetParent(btnObj.transform, false);

        // Declares the variable lbl and initializes it.
        var lbl = labelObj.AddComponent<TextMeshProUGUI>();

        // Updates an existing value.
        lbl.text = label;

        // Updates an existing value.
        lbl.fontSize = 50;

        // Updates an existing value.
        lbl.color = Color.white;

        // Updates an existing value.
        lbl.alignment = TextAlignmentOptions.Center;

        // Updates an existing value.
        lbl.fontStyle = FontStyles.Bold;

        // Declares the variable lblRT and initializes it.
        var lblRT = lbl.rectTransform;

        // Updates an existing value.
        lblRT.anchorMin = Vector2.zero;

        // Updates an existing value.
        lblRT.anchorMax = Vector2.one;

        // Updates an existing value.
        lblRT.offsetMin = Vector2.zero;

        // Updates an existing value.
        lblRT.offsetMax = Vector2.zero;

    // Closes the current code block.
    }

// Closes the current code block.
}
