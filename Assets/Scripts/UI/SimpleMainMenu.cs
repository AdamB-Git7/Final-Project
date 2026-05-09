
// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.UI namespace.
using UnityEngine.UI;

// Imports the UnityEngine.SceneManagement namespace.
using UnityEngine.SceneManagement;

// Imports the TMPro namespace.
using TMPro;









// Declares the class named SimpleMainMenu.
public class SimpleMainMenu : MonoBehaviour

// Opens a new code block.
{

    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {


        // Calls a method.
        PlayerPrefs.SetInt("CurrentNight", 1);


        // Calls a method.
        BuildMenu();

        // Calls a method.
        PlayAmbient();

    // Closes the current code block.
    }


    // Declares the method named PlayAmbient.
    void PlayAmbient()

    // Opens a new code block.
    {


        // Checks whether the condition is true.
        if (Object.FindFirstObjectByType<AudioManager>() == null)

            // Executes this statement.
            new GameObject("AudioManager").AddComponent<AudioManager>();

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



        // Checks whether the condition is true.
        if (Camera.main == null)

        // Opens a new code block.
        {

            // Declares the variable camObj and initializes it.
            GameObject camObj = new GameObject("Main Camera");

            // Updates an existing value.
            camObj.tag = "MainCamera";

            // Declares the variable cam and initializes it.
            Camera cam = camObj.AddComponent<Camera>();

            // Calls a method.
            camObj.AddComponent<AudioListener>();

            // Updates an existing value.
            cam.backgroundColor = new Color(0.05f, 0.02f, 0.02f);

            // Updates an existing value.
            cam.clearFlags = CameraClearFlags.SolidColor;

        // Closes the current code block.
        }



        // Declares the variable canvasObj and initializes it.
        GameObject canvasObj = new GameObject("MenuCanvas");

        // Declares the variable canvas and initializes it.
        Canvas canvas = canvasObj.AddComponent<Canvas>();

        // Updates an existing value.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Declares the variable scaler and initializes it.
        var scaler = canvasObj.AddComponent<CanvasScaler>();

        // Updates an existing value.
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // Updates an existing value.
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Calls a method.
        canvasObj.AddComponent<GraphicRaycaster>();



        // Declares the variable bg and initializes it.
        GameObject bg = new GameObject("Background");

        // Calls a method.
        bg.transform.SetParent(canvasObj.transform, false);

        // Declares the variable bgImg and initializes it.
        Image bgImg = bg.AddComponent<Image>();

        // Updates an existing value.
        bgImg.color = new Color(0.05f, 0.02f, 0.02f);

        // Declares the variable bgRT and initializes it.
        var bgRT = bgImg.rectTransform;

        // Updates an existing value.
        bgRT.anchorMin = Vector2.zero;

        // Updates an existing value.
        bgRT.anchorMax = Vector2.one;

        // Updates an existing value.
        bgRT.offsetMin = Vector2.zero;

        // Updates an existing value.
        bgRT.offsetMax = Vector2.zero;



        // Calls a method.
        AddText(canvasObj.transform, "Title", "NIGHT SHIFT", 220,

                // Executes this statement.
                new Color(0.9f, 0.1f, 0.1f), new Vector2(0, 280), new Vector2(1800, 280));

        // Calls a method.
        AddText(canvasObj.transform, "Subtitle", "FREDDY PROTOCOL", 100,

                // Executes this statement.
                new Color(0.95f, 0.95f, 0.95f), new Vector2(0, 130), new Vector2(1500, 150));



        // Calls a method.
        AddButton(canvasObj.transform, "NewGameBtn", "NEW GAME",

                  // Executes this statement.
                  new Vector2(0, -50), NewGame);

        // Calls a method.
        AddButton(canvasObj.transform, "QuitBtn", "QUIT",

                  // Executes this statement.
                  new Vector2(0, -180), QuitGame);



        // Calls a method.
        AddText(canvasObj.transform, "Credit", "Made by Luca & Adam", 30,

                // Executes this statement.
                new Color(0.6f, 0.6f, 0.6f),

                // Executes this statement.
                new Vector2(0, -480), new Vector2(800, 50));

    // Closes the current code block.
    }


    // Declares the method named NewGame.
    public void NewGame()

    // Opens a new code block.
    {


        // Calls a method.
        SceneManager.LoadScene("scene");

    // Closes the current code block.
    }


    // Declares the method named QuitGame.
    public void QuitGame()

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
        btnBg.color = new Color(0.4f, 0.05f, 0.05f);

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
        btnRT.sizeDelta = new Vector2(500, 100);


        // Declares the variable btn and initializes it.
        Button btn = btnObj.AddComponent<Button>();

        // Updates an existing value.
        btn.targetGraphic = btnBg;

        // Calls a method.
        btn.onClick.AddListener(onClick);


        // Declares the variable lblObj and initializes it.
        GameObject lblObj = new GameObject("Label");

        // Calls a method.
        lblObj.transform.SetParent(btnObj.transform, false);

        // Declares the variable lbl and initializes it.
        var lbl = lblObj.AddComponent<TextMeshProUGUI>();

        // Updates an existing value.
        lbl.text = label;

        // Updates an existing value.
        lbl.fontSize = 60;

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
