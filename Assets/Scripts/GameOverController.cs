using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

/// <summary>
/// Controls the scary game over overlay triggered when the enemy reaches the player.
/// </summary>
public class GameOverController : MonoBehaviour
{
    [Header("References")]
    public UIDocument uiDocument;

    [Header("Settings")]
    public int currentNight = 1;
    public string mainMenuSceneName = "MainMenu";

    private VisualElement _root;
    private VisualElement _bloodOverlay;
    private VisualElement _staticOverlay;
    private Label _nightLabel;
    private Button _restartButton;
    private Button _menuButton;

    private Coroutine _staticCoroutine;

    private const string VisibleClass = "visible";

    void OnEnable()
    {
        _root = uiDocument.rootVisualElement.Q<VisualElement>("Root");
        _bloodOverlay = _root?.Q<VisualElement>("BloodOverlay");
        _staticOverlay = _root?.Q<VisualElement>("StaticOverlay");
        _nightLabel = _root?.Q<Label>("NightLabel");
        _restartButton = _root?.Q<Button>("RestartButton");
        _menuButton = _root?.Q<Button>("MenuButton");

        if (_restartButton != null)
            _restartButton.clicked += OnRestartClicked;

        if (_menuButton != null)
            _menuButton.clicked += OnMenuClicked;

        if (_nightLabel != null)
            _nightLabel.text = $"INCIDENT REPORT — NIGHT {currentNight}";
    }

    void OnDisable()
    {
        if (_restartButton != null)
            _restartButton.clicked -= OnRestartClicked;

        if (_menuButton != null)
            _menuButton.clicked -= OnMenuClicked;
    }

    /// <summary>
    /// Shows the game over screen with a blood flash effect. Called by GameManager.
    /// </summary>
    public void Show()
    {
        if (_root == null) return;

        _root.AddToClassList(VisibleClass);
        StartCoroutine(BloodFlashRoutine());
        _staticCoroutine = StartCoroutine(StaticRoutine());
    }

    private IEnumerator BloodFlashRoutine()
    {
        if (_bloodOverlay == null) yield break;

        // Slam red in
        float elapsed = 0f;
        float fadeInDuration = 0.15f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeInDuration);
            _bloodOverlay.style.backgroundColor = new StyleColor(new Color(0.5f, 0f, 0f, t * 0.75f));
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // Fade back to dark
        elapsed = 0f;
        float fadeOutDuration = 1.2f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = 1f - Mathf.Clamp01(elapsed / fadeOutDuration);
            _bloodOverlay.style.backgroundColor = new StyleColor(new Color(0.5f, 0f, 0f, t * 0.75f));
            yield return null;
        }

        _bloodOverlay.style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0f));
    }

    private IEnumerator StaticRoutine()
    {
        if (_staticOverlay == null) yield break;

        while (true)
        {
            float opacity = Random.Range(0.02f, 0.08f);
            _staticOverlay.style.opacity = opacity;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.14f));
        }
    }

    private void OnRestartClicked()
    {
        if (_staticCoroutine != null)
            StopCoroutine(_staticCoroutine);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnMenuClicked()
    {
        if (_staticCoroutine != null)
            StopCoroutine(_staticCoroutine);

        SceneManager.LoadScene(mainMenuSceneName);
    }
}
