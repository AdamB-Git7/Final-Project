
// Imports the System.Collections namespace.
using System.Collections;

// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.SceneManagement namespace.
using UnityEngine.SceneManagement;

// Imports the UnityEngine.UIElements namespace.
using UnityEngine.UIElements;





// Declares the class named GameOverController.
public class GameOverController : MonoBehaviour

// Opens a new code block.
{

    // Applies the Header("References") attribute.
    [Header("References")]

    // Declares the variable uiDocument.
    public UIDocument uiDocument;


    // Applies the Header("Settings") attribute.
    [Header("Settings")]

    // Declares the variable currentNight and initializes it.
    public int currentNight = 1;

    // Declares the variable mainMenuSceneName and initializes it.
    public string mainMenuSceneName = "MainMenu";


    // Declares the variable _root.
    private VisualElement _root;

    // Declares the variable _bloodOverlay.
    private VisualElement _bloodOverlay;

    // Declares the variable _staticOverlay.
    private VisualElement _staticOverlay;

    // Declares the variable _nightLabel.
    private Label _nightLabel;

    // Declares the variable _restartButton.
    private Button _restartButton;

    // Declares the variable _menuButton.
    private Button _menuButton;


    // Declares the variable _staticCoroutine.
    private Coroutine _staticCoroutine;


    // Executes this statement.
    private const string VisibleClass = "visible";


    // Declares the method named OnEnable.
    void OnEnable()

    // Opens a new code block.
    {

        // Updates an existing value.
        _root = uiDocument.rootVisualElement.Q<VisualElement>("Root");

        // Updates an existing value.
        _bloodOverlay = _root?.Q<VisualElement>("BloodOverlay");

        // Updates an existing value.
        _staticOverlay = _root?.Q<VisualElement>("StaticOverlay");

        // Updates an existing value.
        _nightLabel = _root?.Q<Label>("NightLabel");

        // Updates an existing value.
        _restartButton = _root?.Q<Button>("RestartButton");

        // Updates an existing value.
        _menuButton = _root?.Q<Button>("MenuButton");


        // Checks whether the condition is true.
        if (_restartButton != null)

            // Updates an existing value.
            _restartButton.clicked += OnRestartClicked;


        // Checks whether the condition is true.
        if (_menuButton != null)

            // Updates an existing value.
            _menuButton.clicked += OnMenuClicked;


        // Checks whether the condition is true.
        if (_nightLabel != null)

            // Updates an existing value.
            _nightLabel.text = $"INCIDENT REPORT — NIGHT {currentNight}";

    // Closes the current code block.
    }


    // Declares the method named OnDisable.
    void OnDisable()

    // Opens a new code block.
    {

        // Checks whether the condition is true.
        if (_restartButton != null)

            // Updates an existing value.
            _restartButton.clicked -= OnRestartClicked;


        // Checks whether the condition is true.
        if (_menuButton != null)

            // Updates an existing value.
            _menuButton.clicked -= OnMenuClicked;

    // Closes the current code block.
    }





    // Declares the method named Show.
    public void Show()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (_root == null) return;


        // Calls a method.
        _root.AddToClassList(VisibleClass);

        // Calls a method.
        StartCoroutine(BloodFlashRoutine());

        // Updates an existing value.
        _staticCoroutine = StartCoroutine(StaticRoutine());

    // Closes the current code block.
    }


    // Declares the method named BloodFlashRoutine.
    private IEnumerator BloodFlashRoutine()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (_bloodOverlay == null) yield break;



        // Declares the variable elapsed and initializes it.
        float elapsed = 0f;

        // Declares the variable fadeInDuration and initializes it.
        float fadeInDuration = 0.15f;

        // Repeats the loop while the condition stays true.
        while (elapsed < fadeInDuration)

        // Opens a new code block.
        {

            // Updates an existing value.
            elapsed += Time.deltaTime;

            // Declares the variable t and initializes it.
            float t = Mathf.Clamp01(elapsed / fadeInDuration);

            // Updates an existing value.
            _bloodOverlay.style.backgroundColor = new StyleColor(new Color(0.5f, 0f, 0f, t * 0.75f));

            // Controls iterator execution.
            yield return null;

        // Closes the current code block.
        }


        // Controls iterator execution.
        yield return new WaitForSeconds(0.3f);



        // Updates an existing value.
        elapsed = 0f;

        // Declares the variable fadeOutDuration and initializes it.
        float fadeOutDuration = 1.2f;

        // Repeats the loop while the condition stays true.
        while (elapsed < fadeOutDuration)

        // Opens a new code block.
        {

            // Updates an existing value.
            elapsed += Time.deltaTime;

            // Declares the variable t and initializes it.
            float t = 1f - Mathf.Clamp01(elapsed / fadeOutDuration);

            // Updates an existing value.
            _bloodOverlay.style.backgroundColor = new StyleColor(new Color(0.5f, 0f, 0f, t * 0.75f));

            // Controls iterator execution.
            yield return null;

        // Closes the current code block.
        }


        // Updates an existing value.
        _bloodOverlay.style.backgroundColor = new StyleColor(new Color(0f, 0f, 0f, 0f));

    // Closes the current code block.
    }


    // Declares the method named StaticRoutine.
    private IEnumerator StaticRoutine()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (_staticOverlay == null) yield break;


        // Repeats the loop while the condition stays true.
        while (true)

        // Opens a new code block.
        {

            // Declares the variable opacity and initializes it.
            float opacity = Random.Range(0.02f, 0.08f);

            // Updates an existing value.
            _staticOverlay.style.opacity = opacity;

            // Controls iterator execution.
            yield return new WaitForSeconds(Random.Range(0.05f, 0.14f));

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Declares the method named OnRestartClicked.
    private void OnRestartClicked()

    // Opens a new code block.
    {

        // Checks whether the condition is true.
        if (_staticCoroutine != null)

            // Calls a method.
            StopCoroutine(_staticCoroutine);


        // Calls a method.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    // Closes the current code block.
    }


    // Declares the method named OnMenuClicked.
    private void OnMenuClicked()

    // Opens a new code block.
    {

        // Checks whether the condition is true.
        if (_staticCoroutine != null)

            // Calls a method.
            StopCoroutine(_staticCoroutine);


        // Calls a method.
        SceneManager.LoadScene(mainMenuSceneName);

    // Closes the current code block.
    }

// Closes the current code block.
}
