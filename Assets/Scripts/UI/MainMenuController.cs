
// Imports the System.Collections namespace.
using System.Collections;

// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.SceneManagement namespace.
using UnityEngine.SceneManagement;

// Imports the UnityEngine.UIElements namespace.
using UnityEngine.UIElements;





// Declares the class named MainMenuController.
public class MainMenuController : MonoBehaviour

// Opens a new code block.
{

    // Applies the Header("Scene References") attribute.
    [Header("Scene References")]

    // Declares the variable uiDocument.
    public UIDocument uiDocument;

    // Declares the variable flickerLight.
    public Light flickerLight;

    // Declares the variable ambientAudioSource.
    public AudioSource ambientAudioSource;

    // Declares the variable staticAudioSource.
    public AudioSource staticAudioSource;


    // Applies the Header("Flicker Settings") attribute.
    [Header("Flicker Settings")]

    // Declares the variable flickerMinInterval and initializes it.
    public float flickerMinInterval = 0.05f;

    // Declares the variable flickerMaxInterval and initializes it.
    public float flickerMaxInterval = 0.3f;

    // Declares the variable flickerMinIntensity and initializes it.
    public float flickerMinIntensity = 0.2f;

    // Declares the variable flickerMaxIntensity and initializes it.
    public float flickerMaxIntensity = 2.5f;


    // Applies the Header("Game Scene") attribute.
    [Header("Game Scene")]

    // Declares the variable gameSceneName and initializes it.
    public string gameSceneName = "GameScene";


    // Declares the variable _startButton.
    private Button _startButton;

    // Declares the variable _quitButton.
    private Button _quitButton;

    // Declares the variable _root.
    private VisualElement _root;

    // Declares the variable _staticOverlay.
    private VisualElement _staticOverlay;

    // Declares the variable _flickerCoroutine.
    private Coroutine _flickerCoroutine;

    // Declares the variable _staticCoroutine.
    private Coroutine _staticCoroutine;


    // Declares the variable _baseIntensity.
    private float _baseIntensity;


    // Declares the method named OnEnable.
    void OnEnable()

    // Opens a new code block.
    {

        // Updates an existing value.
        _root = uiDocument.rootVisualElement;


        // Updates an existing value.
        _startButton = _root.Q<Button>("StartButton");

        // Updates an existing value.
        _quitButton = _root.Q<Button>("QuitButton");

        // Updates an existing value.
        _staticOverlay = _root.Q<VisualElement>("StaticOverlay");


        // Checks whether the condition is true.
        if (_startButton != null)

            // Updates an existing value.
            _startButton.clicked += OnStartClicked;


        // Checks whether the condition is true.
        if (_quitButton != null)

            // Updates an existing value.
            _quitButton.clicked += OnQuitClicked;

    // Closes the current code block.
    }


    // Declares the method named OnDisable.
    void OnDisable()

    // Opens a new code block.
    {

        // Checks whether the condition is true.
        if (_startButton != null)

            // Updates an existing value.
            _startButton.clicked -= OnStartClicked;


        // Checks whether the condition is true.
        if (_quitButton != null)

            // Updates an existing value.
            _quitButton.clicked -= OnQuitClicked;

    // Closes the current code block.
    }


    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {

        // Checks whether the condition is true.
        if (flickerLight != null)

        // Opens a new code block.
        {

            // Updates an existing value.
            _baseIntensity = flickerLight.intensity;

            // Updates an existing value.
            _flickerCoroutine = StartCoroutine(FlickerRoutine());

        // Closes the current code block.
        }


        // Updates an existing value.
        _staticCoroutine = StartCoroutine(StaticOverlayRoutine());

    // Closes the current code block.
    }


    // Declares the method named OnDestroy.
    void OnDestroy()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (_flickerCoroutine != null) StopCoroutine(_flickerCoroutine);

        // Checks the condition and runs the inline statement when it is true.
        if (_staticCoroutine != null) StopCoroutine(_staticCoroutine);

    // Closes the current code block.
    }





    // Declares the method named OnStartClicked.
    public void OnStartClicked()

    // Opens a new code block.
    {

        // Calls a method.
        StartCoroutine(LoadGameWithDelay());

    // Closes the current code block.
    }





    // Declares the method named OnQuitClicked.
    public void OnQuitClicked()

    // Opens a new code block.
    {

        // Calls a method.
        Application.Quit();

// Executes this statement.
#if UNITY_EDITOR

        // Updates an existing value.
        UnityEditor.EditorApplication.isPlaying = false;

// Executes this statement.
#endif

    // Closes the current code block.
    }


    // Declares the method named LoadGameWithDelay.
    private IEnumerator LoadGameWithDelay()

    // Opens a new code block.
    {

        // Checks whether the condition is true.
        if (_startButton != null)

            // Calls a method.
            _startButton.SetEnabled(false);


        // Checks whether the condition is true.
        if (staticAudioSource != null)

        // Opens a new code block.
        {

            // Updates an existing value.
            staticAudioSource.volume = 1f;

        // Closes the current code block.
        }



        // Starts a for loop.
        for (int i = 0; i < 8; i++)

        // Opens a new code block.
        {

            // Checks whether the condition is true.
            if (flickerLight != null)

                // Updates an existing value.
                flickerLight.enabled = !flickerLight.enabled;

            // Controls iterator execution.
            yield return new WaitForSeconds(0.05f);

        // Closes the current code block.
        }


        // Checks whether the condition is true.
        if (flickerLight != null)

            // Updates an existing value.
            flickerLight.enabled = false;


        // Controls iterator execution.
        yield return new WaitForSeconds(0.3f);

        // Calls a method.
        SceneManager.LoadScene(gameSceneName);

    // Closes the current code block.
    }


    // Declares the method named FlickerRoutine.
    private IEnumerator FlickerRoutine()

    // Opens a new code block.
    {

        // Repeats the loop while the condition stays true.
        while (true)

        // Opens a new code block.
        {

            // Declares the variable waitTime and initializes it.
            float waitTime = Random.Range(flickerMinInterval, flickerMaxInterval);

            // Controls iterator execution.
            yield return new WaitForSeconds(waitTime);



            // Checks whether the condition is true.
            if (Random.value < 0.08f)

            // Opens a new code block.
            {

                // Updates an existing value.
                flickerLight.enabled = false;

                // Controls iterator execution.
                yield return new WaitForSeconds(Random.Range(0.05f, 0.25f));

                // Updates an existing value.
                flickerLight.enabled = true;

            // Closes the current code block.
            }

            // Runs the fallback branch when earlier conditions were false.
            else

            // Opens a new code block.
            {

                // Updates an existing value.
                flickerLight.intensity = Random.Range(flickerMinIntensity, flickerMaxIntensity);

            // Closes the current code block.
            }

        // Closes the current code block.
        }

    // Closes the current code block.
    }


    // Declares the method named StaticOverlayRoutine.
    private IEnumerator StaticOverlayRoutine()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (_staticOverlay == null) yield break;


        // Repeats the loop while the condition stays true.
        while (true)

        // Opens a new code block.
        {

            // Declares the variable opacity and initializes it.
            float opacity = Random.Range(0.02f, 0.09f);

            // Updates an existing value.
            _staticOverlay.style.opacity = opacity;

            // Controls iterator execution.
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));

        // Closes the current code block.
        }

    // Closes the current code block.
    }

// Closes the current code block.
}
