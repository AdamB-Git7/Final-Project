
// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.UI namespace.
using UnityEngine.UI;

// Imports the TMPro namespace.
using TMPro;


// Declares the class named JumpscareEffect.
public class JumpscareEffect : MonoBehaviour

// Opens a new code block.
{

    // Declares the variable duration and initializes it.
    public float duration = 1.2f;


    // Declares the variable redFlash.
    Image redFlash;

    // Declares the variable faceCircle.
    Image faceCircle;

    // Declares the variable eyes and initializes it.
    Image[] eyes = new Image[2];

    // Declares the variable screamText.
    TMP_Text screamText;

    // Declares the variable timer.
    float timer;

    // Declares the variable finished.
    bool finished;

    // Executes this statement.
    System.Action onComplete;


    // Declares the method named Play.
    public void Play(System.Action callback)

    // Opens a new code block.
    {

        // Updates an existing value.
        onComplete = callback;

        // Calls a method.
        BuildUI();



        // Checks whether the condition is true.
        if (AudioManager.Instance != null)

            // Calls a method.
            AudioManager.Instance.PlayJumpscare();

    // Closes the current code block.
    }


    // Declares the method named BuildUI.
    void BuildUI()

    // Opens a new code block.
    {

        // Declares the variable canvas and initializes it.
        Canvas canvas = gameObject.AddComponent<Canvas>();

        // Updates an existing value.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Updates an existing value.
        canvas.sortingOrder = 200;

        // Declares the variable scaler and initializes it.
        var scaler = gameObject.AddComponent<CanvasScaler>();

        // Updates an existing value.
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        // Updates an existing value.
        scaler.referenceResolution = new Vector2(1920, 1080);

        // Calls a method.
        gameObject.AddComponent<GraphicRaycaster>();



        // Declares the variable flashObj and initializes it.
        GameObject flashObj = new GameObject("Flash");

        // Calls a method.
        flashObj.transform.SetParent(transform, false);

        // Updates an existing value.
        redFlash = flashObj.AddComponent<Image>();

        // Updates an existing value.
        redFlash.color = new Color(0.6f, 0f, 0f, 1f);

        // Declares the variable flashRT and initializes it.
        var flashRT = redFlash.rectTransform;

        // Updates an existing value.
        flashRT.anchorMin = Vector2.zero;

        // Updates an existing value.
        flashRT.anchorMax = Vector2.one;

        // Updates an existing value.
        flashRT.offsetMin = Vector2.zero;

        // Updates an existing value.
        flashRT.offsetMax = Vector2.zero;



        // Declares the variable faceObj and initializes it.
        GameObject faceObj = new GameObject("Face");

        // Calls a method.
        faceObj.transform.SetParent(transform, false);

        // Updates an existing value.
        faceCircle = faceObj.AddComponent<Image>();

        // Updates an existing value.
        faceCircle.color = new Color(0.05f, 0.02f, 0.02f);

        // Updates an existing value.
        faceCircle.sprite = MakeCircleSprite();

        // Declares the variable faceRT and initializes it.
        var faceRT = faceCircle.rectTransform;

        // Updates an existing value.
        faceRT.anchorMin = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        faceRT.anchorMax = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        faceRT.pivot = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        faceRT.sizeDelta = new Vector2(50, 50);



        // Starts a for loop.
        for (int i = 0; i < 2; i++)

        // Opens a new code block.
        {

            // Declares the variable eyeObj and initializes it.
            GameObject eyeObj = new GameObject("Eye" + i);

            // Calls a method.
            eyeObj.transform.SetParent(faceObj.transform, false);

            // Updates an existing value.
            eyes[i] = eyeObj.AddComponent<Image>();

            // Updates an existing value.
            eyes[i].color = new Color(1f, 0.1f, 0.1f);

            // Updates an existing value.
            eyes[i].sprite = faceCircle.sprite;

            // Declares the variable eyeRT and initializes it.
            var eyeRT = eyes[i].rectTransform;

            // Declares the variable xOff and initializes it.
            float xOff = i == 0 ? -120 : 120;

            // Updates an existing value.
            eyeRT.anchorMin = new Vector2(0.5f, 0.5f);

            // Updates an existing value.
            eyeRT.anchorMax = new Vector2(0.5f, 0.5f);

            // Updates an existing value.
            eyeRT.pivot = new Vector2(0.5f, 0.5f);

            // Updates an existing value.
            eyeRT.anchoredPosition = new Vector2(xOff, 60);

            // Updates an existing value.
            eyeRT.sizeDelta = new Vector2(120, 120);

        // Closes the current code block.
        }



        // Declares the variable screamObj and initializes it.
        GameObject screamObj = new GameObject("Scream");

        // Calls a method.
        screamObj.transform.SetParent(transform, false);

        // Updates an existing value.
        screamText = screamObj.AddComponent<TextMeshProUGUI>();

        // Updates an existing value.
        screamText.text = "RAAAAH!";

        // Updates an existing value.
        screamText.fontSize = 250;

        // Updates an existing value.
        screamText.color = Color.white;

        // Updates an existing value.
        screamText.alignment = TextAlignmentOptions.Center;

        // Updates an existing value.
        screamText.fontStyle = FontStyles.Bold;

        // Declares the variable screamRT and initializes it.
        var screamRT = screamText.rectTransform;

        // Updates an existing value.
        screamRT.anchorMin = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        screamRT.anchorMax = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        screamRT.pivot = new Vector2(0.5f, 0.5f);

        // Updates an existing value.
        screamRT.anchoredPosition = new Vector2(0, -350);

        // Updates an existing value.
        screamRT.sizeDelta = new Vector2(1500, 300);

    // Closes the current code block.
    }


    // Declares the variable camOriginalPos.
    Vector3 camOriginalPos;

    // Declares the variable camOriginalRot.
    Quaternion camOriginalRot;

    // Declares the variable camCaptured.
    bool camCaptured;


    // Declares the method named Update.
    void Update()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (finished) return;


        // Updates an existing value.
        timer += Time.deltaTime;

        // Declares the variable t and initializes it.
        float t = timer / duration;



        // Checks whether the condition is true.
        if (!camCaptured && Camera.main != null)

        // Opens a new code block.
        {

            // Updates an existing value.
            camOriginalPos = Camera.main.transform.localPosition;

            // Updates an existing value.
            camOriginalRot = Camera.main.transform.localRotation;

            // Updates an existing value.
            camCaptured = true;

        // Closes the current code block.
        }



        // Declares the variable scale and initializes it.
        float scale = Mathf.Lerp(50f, 1400f, Mathf.Pow(t, 0.4f));

        // Checks the condition and runs the inline statement when it is true.
        if (faceCircle != null) faceCircle.rectTransform.sizeDelta = new Vector2(scale, scale);



        // Checks whether the condition is true.
        if (Camera.main != null)

        // Opens a new code block.
        {


            // Declares the variable tiltAngle and initializes it.
            float tiltAngle = Mathf.Lerp(0f, 65f, Mathf.Pow(t, 0.6f));

            // Updates an existing value.
            Camera.main.transform.localRotation = camOriginalRot * Quaternion.Euler(tiltAngle, 0, Mathf.Sin(t * 8f) * 15f);



            // Declares the variable drop and initializes it.
            float drop = Mathf.Lerp(0f, -0.6f, t);



            // Declares the variable shake and initializes it.
            float shake = Mathf.Lerp(0.2f, 0f, t);

            // Declares the variable shakeOffset and initializes it.
            Vector3 shakeOffset = (Vector3)Random.insideUnitCircle * shake;


            // Updates an existing value.
            Camera.main.transform.localPosition = camOriginalPos + new Vector3(0, drop, 0) + shakeOffset;

        // Closes the current code block.
        }



        // Checks whether the condition is true.
        if (redFlash != null)

        // Opens a new code block.
        {

            // Declares the variable a and initializes it.
            float a = Mathf.PingPong(timer * 12f, 1f);

            // Updates an existing value.
            redFlash.color = new Color(0.6f, 0f, 0f, 0.6f + a * 0.4f);

        // Closes the current code block.
        }


        // Checks whether the condition is true.
        if (timer >= duration)

        // Opens a new code block.
        {

            // Updates an existing value.
            finished = true;

            // Calls a method.
            Destroy(gameObject);

            // Executes this statement.
            onComplete?.Invoke();

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Declares the variable cachedCircle.
    static Sprite cachedCircle;

    // Declares the method named MakeCircleSprite.
    static Sprite MakeCircleSprite()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (cachedCircle != null) return cachedCircle;


        // Declares the variable size and initializes it.
        int size = 128;

        // Declares the variable tex and initializes it.
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);

        // Declares the variable c and initializes it.
        Vector2 c = new Vector2(size / 2f, size / 2f);

        // Declares the variable r and initializes it.
        float r = size / 2f - 1f;

        // Starts a for loop.
        for (int y = 0; y < size; y++)

            // Starts a for loop.
            for (int x = 0; x < size; x++)

            // Opens a new code block.
            {

                // Declares the variable d and initializes it.
                float d = Vector2.Distance(new Vector2(x, y), c);

                // Calls a method.
                tex.SetPixel(x, y, d <= r ? Color.white : new Color(0, 0, 0, 0));

            // Closes the current code block.
            }

        // Calls a method.
        tex.Apply();

        // Updates an existing value.
        cachedCircle = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));

        // Returns the specified value.
        return cachedCircle;

    // Closes the current code block.
    }

// Closes the current code block.
}
