using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Controls the main menu UI and atmospheric effects for the scary office starting screen.
/// </summary>
public class MainMenuController : MonoBehaviour
{
    [Header("Scene References")]
    public UIDocument uiDocument;
    public Light flickerLight;
    public AudioSource ambientAudioSource;
    public AudioSource staticAudioSource;

    [Header("Flicker Settings")]
    public float flickerMinInterval = 0.05f;
    public float flickerMaxInterval = 0.3f;
    public float flickerMinIntensity = 0.2f;
    public float flickerMaxIntensity = 2.5f;

    [Header("Game Scene")]
    public string gameSceneName = "GameScene";

    private Button _startButton;
    private Button _quitButton;
    private VisualElement _root;
    private VisualElement _staticOverlay;
    private Coroutine _flickerCoroutine;
    private Coroutine _staticCoroutine;

    private float _baseIntensity;

    void OnEnable()
    {
        _root = uiDocument.rootVisualElement;

        _startButton = _root.Q<Button>("StartButton");
        _quitButton = _root.Q<Button>("QuitButton");
        _staticOverlay = _root.Q<VisualElement>("StaticOverlay");

        if (_startButton != null)
            _startButton.clicked += OnStartClicked;

        if (_quitButton != null)
            _quitButton.clicked += OnQuitClicked;
    }

    void OnDisable()
    {
        if (_startButton != null)
            _startButton.clicked -= OnStartClicked;

        if (_quitButton != null)
            _quitButton.clicked -= OnQuitClicked;
    }

    void Start()
    {
        if (flickerLight != null)
        {
            _baseIntensity = flickerLight.intensity;
            _flickerCoroutine = StartCoroutine(FlickerRoutine());
        }

        _staticCoroutine = StartCoroutine(StaticOverlayRoutine());
    }

    void OnDestroy()
    {
        if (_flickerCoroutine != null) StopCoroutine(_flickerCoroutine);
        if (_staticCoroutine != null) StopCoroutine(_staticCoroutine);
    }

    /// <summary>
    /// Starts the game by loading the main game scene.
    /// </summary>
    public void OnStartClicked()
    {
        StartCoroutine(LoadGameWithDelay());
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void OnQuitClicked()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator LoadGameWithDelay()
    {
        if (_startButton != null)
            _startButton.SetEnabled(false);

        if (staticAudioSource != null)
        {
            staticAudioSource.volume = 1f;
        }

        // Flicker rapidly before loading
        for (int i = 0; i < 8; i++)
        {
            if (flickerLight != null)
                flickerLight.enabled = !flickerLight.enabled;
            yield return new WaitForSeconds(0.05f);
        }

        if (flickerLight != null)
            flickerLight.enabled = false;

        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(gameSceneName);
    }

    private IEnumerator FlickerRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(flickerMinInterval, flickerMaxInterval);
            yield return new WaitForSeconds(waitTime);

            // Occasional full blackout
            if (Random.value < 0.08f)
            {
                flickerLight.enabled = false;
                yield return new WaitForSeconds(Random.Range(0.05f, 0.25f));
                flickerLight.enabled = true;
            }
            else
            {
                flickerLight.intensity = Random.Range(flickerMinIntensity, flickerMaxIntensity);
            }
        }
    }

    private IEnumerator StaticOverlayRoutine()
    {
        if (_staticOverlay == null) yield break;

        while (true)
        {
            float opacity = Random.Range(0.02f, 0.09f);
            _staticOverlay.style.opacity = opacity;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        }
    }
}
