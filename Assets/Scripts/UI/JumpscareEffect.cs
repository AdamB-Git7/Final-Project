using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JumpscareEffect : MonoBehaviour
{
    public float duration = 1.2f;

    Image redFlash;
    Image faceCircle;
    Image[] eyes = new Image[2];
    TMP_Text screamText;
    float timer;
    bool finished;
    System.Action onComplete;

    public void Play(System.Action callback)
    {
        onComplete = callback;
        BuildUI();

        // Play the jumpscare scream
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayJumpscare();
    }

    void BuildUI()
    {
        Canvas canvas = gameObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 200;
        var scaler = gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        gameObject.AddComponent<GraphicRaycaster>();

        // Red flash background
        GameObject flashObj = new GameObject("Flash");
        flashObj.transform.SetParent(transform, false);
        redFlash = flashObj.AddComponent<Image>();
        redFlash.color = new Color(0.6f, 0f, 0f, 1f);
        var flashRT = redFlash.rectTransform;
        flashRT.anchorMin = Vector2.zero;
        flashRT.anchorMax = Vector2.one;
        flashRT.offsetMin = Vector2.zero;
        flashRT.offsetMax = Vector2.zero;

        // Animatronic face (big dark circle)
        GameObject faceObj = new GameObject("Face");
        faceObj.transform.SetParent(transform, false);
        faceCircle = faceObj.AddComponent<Image>();
        faceCircle.color = new Color(0.05f, 0.02f, 0.02f);
        faceCircle.sprite = MakeCircleSprite();
        var faceRT = faceCircle.rectTransform;
        faceRT.anchorMin = new Vector2(0.5f, 0.5f);
        faceRT.anchorMax = new Vector2(0.5f, 0.5f);
        faceRT.pivot = new Vector2(0.5f, 0.5f);
        faceRT.sizeDelta = new Vector2(50, 50);

        // Eyes
        for (int i = 0; i < 2; i++)
        {
            GameObject eyeObj = new GameObject("Eye" + i);
            eyeObj.transform.SetParent(faceObj.transform, false);
            eyes[i] = eyeObj.AddComponent<Image>();
            eyes[i].color = new Color(1f, 0.1f, 0.1f);
            eyes[i].sprite = faceCircle.sprite;
            var eyeRT = eyes[i].rectTransform;
            float xOff = i == 0 ? -120 : 120;
            eyeRT.anchorMin = new Vector2(0.5f, 0.5f);
            eyeRT.anchorMax = new Vector2(0.5f, 0.5f);
            eyeRT.pivot = new Vector2(0.5f, 0.5f);
            eyeRT.anchoredPosition = new Vector2(xOff, 60);
            eyeRT.sizeDelta = new Vector2(120, 120);
        }

        // Scream text
        GameObject screamObj = new GameObject("Scream");
        screamObj.transform.SetParent(transform, false);
        screamText = screamObj.AddComponent<TextMeshProUGUI>();
        screamText.text = "RAAAAH!";
        screamText.fontSize = 250;
        screamText.color = Color.white;
        screamText.alignment = TextAlignmentOptions.Center;
        screamText.fontStyle = FontStyles.Bold;
        var screamRT = screamText.rectTransform;
        screamRT.anchorMin = new Vector2(0.5f, 0.5f);
        screamRT.anchorMax = new Vector2(0.5f, 0.5f);
        screamRT.pivot = new Vector2(0.5f, 0.5f);
        screamRT.anchoredPosition = new Vector2(0, -350);
        screamRT.sizeDelta = new Vector2(1500, 300);
    }

    Vector3 camOriginalPos;
    Quaternion camOriginalRot;
    bool camCaptured;

    void Update()
    {
        if (finished) return;

        timer += Time.deltaTime;
        float t = timer / duration;

        // Capture camera starting position once
        if (!camCaptured && Camera.main != null)
        {
            camOriginalPos = Camera.main.transform.localPosition;
            camOriginalRot = Camera.main.transform.localRotation;
            camCaptured = true;
        }

        // Face zooms in fast
        float scale = Mathf.Lerp(50f, 1400f, Mathf.Pow(t, 0.4f));
        if (faceCircle != null) faceCircle.rectTransform.sizeDelta = new Vector2(scale, scale);

        // Camera animation: shake + tilt forward like player is falling
        if (Camera.main != null)
        {
            // Tilt forward (player falls/collapses)
            float tiltAngle = Mathf.Lerp(0f, 65f, Mathf.Pow(t, 0.6f));
            Camera.main.transform.localRotation = camOriginalRot * Quaternion.Euler(tiltAngle, 0, Mathf.Sin(t * 8f) * 15f);

            // Drop down slightly
            float drop = Mathf.Lerp(0f, -0.6f, t);

            // Violent shake
            float shake = Mathf.Lerp(0.2f, 0f, t);
            Vector3 shakeOffset = (Vector3)Random.insideUnitCircle * shake;

            Camera.main.transform.localPosition = camOriginalPos + new Vector3(0, drop, 0) + shakeOffset;
        }

        // Flash strobe
        if (redFlash != null)
        {
            float a = Mathf.PingPong(timer * 12f, 1f);
            redFlash.color = new Color(0.6f, 0f, 0f, 0.6f + a * 0.4f);
        }

        if (timer >= duration)
        {
            finished = true;
            Destroy(gameObject);
            onComplete?.Invoke();
        }
    }

    static Sprite cachedCircle;
    static Sprite MakeCircleSprite()
    {
        if (cachedCircle != null) return cachedCircle;

        int size = 128;
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        Vector2 c = new Vector2(size / 2f, size / 2f);
        float r = size / 2f - 1f;
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
            {
                float d = Vector2.Distance(new Vector2(x, y), c);
                tex.SetPixel(x, y, d <= r ? Color.white : new Color(0, 0, 0, 0));
            }
        tex.Apply();
        cachedCircle = Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f));
        return cachedCircle;
    }
}
