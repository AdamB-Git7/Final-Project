
// Imports the UnityEngine namespace.
using UnityEngine;

// Imports the UnityEngine.SceneManagement namespace.
using UnityEngine.SceneManagement;

// Imports the TMPro namespace.
using TMPro;













// Declares the class named GameManager.
public class GameManager : MonoBehaviour

// Opens a new code block.
{

    // Applies the Header("Night Settings") attribute.
    [Header("Night Settings")]

    // Declares the variable nightDuration and initializes it.
    public float nightDuration = 120f;


    // Applies the Header("UI References") attribute.
    [Header("UI References")]

    // Declares the variable clockText.
    public TMP_Text clockText;

    // Declares the variable nightText.
    public TMP_Text nightText;




    // Declares the variable currentNight and initializes it.
    public int currentNight = 1;


    // Declares the variable timer.
    float timer;

    // Declares the variable isGameOver.
    bool isGameOver;

    // Declares the variable hasWon.
    bool hasWon;





    // Declares the method named Start.
    void Start()

    // Opens a new code block.
    {


        // Updates an existing value.
        currentNight = PlayerPrefs.GetInt("CurrentNight", 1);

        // Checks the condition and runs the inline statement when it is true.
        if (currentNight < 1 || currentNight > 3) currentNight = 1;



        // Calls a method.
        SceneSetup.EnsureSceneIsBuilt(this);



        // Updates an existing value.
        timer = 0f;

        // Updates an existing value.
        isGameOver = false;

        // Updates an existing value.
        hasWon = false;



        // Checks whether the condition is true.
        if (clockText == null)

            // Updates an existing value.
            clockText = SceneSetup.FindOrCreateClockText();



        // Checks the condition and runs the inline statement when it is true.
        if (nightText != null) nightText.text = "Night " + currentNight;



        // Calls a method.
        ApplyDifficulty();



        // Updates an existing value.
        Cursor.lockState = CursorLockMode.None;

        // Updates an existing value.
        Cursor.visible = true;

    // Closes the current code block.
    }





    // Declares the method named ApplyDifficulty.
    void ApplyDifficulty()

    // Opens a new code block.
    {

        // Declares the variable enemies and initializes it.
        EnemyAI[] enemies = Object.FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        // Iterates through each item in the collection.
        foreach (EnemyAI ai in enemies)

        // Opens a new code block.
        {

            // Checks whether the condition is true.
            if (currentNight == 1)

            // Opens a new code block.
            {

                // Updates an existing value.
                ai.moveSpeed = 2.5f;

                // Updates an existing value.
                ai.aggressionGrowth = 0.03f;

            // Closes the current code block.
            }

            // Checks the next condition when earlier conditions were false.
            else if (currentNight == 2)

            // Opens a new code block.
            {

                // Updates an existing value.
                ai.moveSpeed = 3.5f;

                // Updates an existing value.
                ai.aggressionGrowth = 0.05f;

            // Closes the current code block.
            }

            // Runs the fallback branch when earlier conditions were false.
            else

            // Opens a new code block.
            {

                // Updates an existing value.
                ai.moveSpeed = 5f;

                // Updates an existing value.
                ai.aggressionGrowth = 0.08f;

            // Closes the current code block.
            }

        // Closes the current code block.
        }

    // Closes the current code block.
    }





    // Declares the method named Update.
    void Update()

    // Opens a new code block.
    {


        // Checks whether the condition is true.
        if (isGameOver || hasWon)

        // Opens a new code block.
        {

            // Checks the condition and runs the inline statement when it is true.
            if (Input.GetKeyDown(KeyCode.R)) RestartGame();


            // Checks the condition and runs the inline statement when it is true.
            if (hasWon && Input.GetKeyDown(KeyCode.N)) LoadNextNight();

            // Returns from the current method.
            return;

        // Closes the current code block.
        }



        // Updates an existing value.
        timer += Time.deltaTime;

        // Calls a method.
        UpdateClock();



        // Checks the condition and runs the inline statement when it is true.
        if (timer >= nightDuration) WinNight();

    // Closes the current code block.
    }





    // Declares the method named UpdateClock.
    void UpdateClock()

    // Opens a new code block.
    {

        // Declares the variable progress and initializes it.
        float progress = timer / nightDuration;

        // Declares the variable totalMinutes and initializes it.
        int totalMinutes = Mathf.FloorToInt(progress * 360f);

        // Declares the variable snappedMinutes and initializes it.
        int snappedMinutes = (totalMinutes / 30) * 30;

        // Declares the variable hour and initializes it.
        int hour = snappedMinutes / 60;

        // Declares the variable minute and initializes it.
        int minute = snappedMinutes % 60;

        // Declares the variable displayHour and initializes it.
        int displayHour = (hour == 0) ? 12 : hour;

        // Checks whether the condition is true.
        if (clockText != null)

            // Updates an existing value.
            clockText.text = displayHour + ":" + minute.ToString("00") + " AM";

    // Closes the current code block.
    }





    // Declares the method named TriggerGameOver.
    public void TriggerGameOver()

    // Opens a new code block.
    {

        // Checks the condition and runs the inline statement when it is true.
        if (isGameOver || hasWon) return;

        // Updates an existing value.
        isGameOver = true;



        // Calls a method.
        PlayerPrefs.SetInt("CurrentNight", 1);



        // Declares the variable jumpObj and initializes it.
        GameObject jumpObj = new GameObject("JumpscareEffect");

        // Declares the variable jump and initializes it.
        JumpscareEffect jump = jumpObj.AddComponent<JumpscareEffect>();

        // Calls a method.
        jump.Play(SceneSetup.ShowGameOverScreen);

    // Closes the current code block.
    }





    // Declares the method named WinNight.
    void WinNight()

    // Opens a new code block.
    {

        // Updates an existing value.
        hasWon = true;

        // Checks the condition and runs the inline statement when it is true.
        if (clockText != null) clockText.text = "6 AM";



        // Checks whether the condition is true.
        if (currentNight >= 3)

        // Opens a new code block.
        {

            // Calls a method.
            SceneSetup.ShowFinalWinScreen();

            // Calls a method.
            PlayerPrefs.SetInt("CurrentNight", 1);

        // Closes the current code block.
        }

        // Runs the fallback branch when earlier conditions were false.
        else

        // Opens a new code block.
        {


            // Calls a method.
            SceneSetup.ShowWinScreen(currentNight);

        // Closes the current code block.
        }

    // Closes the current code block.
    }





    // Declares the method named RestartGame.
    public void RestartGame()

    // Opens a new code block.
    {

        // Calls a method.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    // Closes the current code block.
    }





    // Declares the method named LoadNextNight.
    public void LoadNextNight()

    // Opens a new code block.
    {

        // Calls a method.
        PlayerPrefs.SetInt("CurrentNight", currentNight + 1);

        // Calls a method.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    // Closes the current code block.
    }





    // Declares the method named IsGameActive.
    public bool IsGameActive()

    // Opens a new code block.
    {

        // Returns the specified value.
        return !isGameOver && !hasWon;

    // Closes the current code block.
    }

// Closes the current code block.
}
