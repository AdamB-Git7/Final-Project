using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// =====================================================================
//  GAME MANAGER — the brain of the game
// =====================================================================
//  Responsibilities:
//    - Track the night timer (12 AM → 6 AM)
//    - Show the clock on the office monitor
//    - Apply difficulty based on the current night (1, 2, or 3)
//    - Win the game when the timer reaches 6 AM
//    - Trigger game over when an animatronic catches you
//    - Restart on R press, advance to next night on N press
// =====================================================================
public class GameManager : MonoBehaviour
{
    [Header("Night Settings")]
    public float nightDuration = 120f;  // how long the night lasts (real seconds)

    [Header("UI References")]
    public TMP_Text clockText;
    public TMP_Text nightText;

    // Public so EnemyAI can read it. Saved in PlayerPrefs so it persists
    // across scene reloads (so beating Night 1 advances to Night 2)
    public int currentNight = 1;

    float timer;
    bool isGameOver;
    bool hasWon;

    // =================================================================
    //  Start() — runs once when the game begins
    // =================================================================
    void Start()
    {
        // Read the saved night number (defaults to 1 if never saved)
        currentNight = PlayerPrefs.GetInt("CurrentNight", 1);
        if (currentNight < 1 || currentNight > 3) currentNight = 1;

        // Build the scene first
        SceneSetup.EnsureSceneIsBuilt(this);

        // Reset state
        timer = 0f;
        isGameOver = false;
        hasWon = false;

        // Auto-find the clock if not wired up
        if (clockText == null)
            clockText = SceneSetup.FindOrCreateClockText();

        // Show the night number on the office display
        if (nightText != null) nightText.text = "Night " + currentNight;

        // Apply the night's difficulty to all animatronics
        ApplyDifficulty();

        // Show the cursor for camera clicks
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // =================================================================
    //  ApplyDifficulty() — make AIs harder each night
    // =================================================================
    void ApplyDifficulty()
    {
        EnemyAI[] enemies = Object.FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
        foreach (EnemyAI ai in enemies)
        {
            if (currentNight == 1)
            {
                ai.moveSpeed = 2.5f;          // slow walk
                ai.aggressionGrowth = 0.03f;  // calm
            }
            else if (currentNight == 2)
            {
                ai.moveSpeed = 3.5f;          // medium
                ai.aggressionGrowth = 0.05f;
            }
            else // night 3
            {
                ai.moveSpeed = 5f;            // fast
                ai.aggressionGrowth = 0.08f;  // brutal
            }
        }
    }

    // =================================================================
    //  Update() — runs every frame
    // =================================================================
    void Update()
    {
        // If the game is over, listen for restart / next-night keys
        if (isGameOver || hasWon)
        {
            if (Input.GetKeyDown(KeyCode.R)) RestartGame();
            // After winning, N goes to the next night (or to credits if night 3)
            if (hasWon && Input.GetKeyDown(KeyCode.N)) LoadNextNight();
            return;
        }

        // Advance the timer
        timer += Time.deltaTime;
        UpdateClock();

        // Hit 6 AM = win
        if (timer >= nightDuration) WinNight();
    }

    // =================================================================
    //  UpdateClock() — show "12:30 AM" style time on the monitor
    // =================================================================
    void UpdateClock()
    {
        float progress = timer / nightDuration;
        int totalMinutes = Mathf.FloorToInt(progress * 360f);
        int snappedMinutes = (totalMinutes / 30) * 30;
        int hour = snappedMinutes / 60;
        int minute = snappedMinutes % 60;
        int displayHour = (hour == 0) ? 12 : hour;
        if (clockText != null)
            clockText.text = displayHour + ":" + minute.ToString("00") + " AM";
    }

    // =================================================================
    //  TriggerGameOver() — called when an AI catches the player
    // =================================================================
    public void TriggerGameOver()
    {
        if (isGameOver || hasWon) return;
        isGameOver = true;

        // Reset to Night 1 on death
        PlayerPrefs.SetInt("CurrentNight", 1);

        // Play jumpscare → YOU DIED screen
        GameObject jumpObj = new GameObject("JumpscareEffect");
        JumpscareEffect jump = jumpObj.AddComponent<JumpscareEffect>();
        jump.Play(SceneSetup.ShowGameOverScreen);
    }

    // =================================================================
    //  WinNight() — runs when the timer reaches 6 AM
    // =================================================================
    void WinNight()
    {
        hasWon = true;
        if (clockText != null) clockText.text = "6 AM";

        // If they beat night 3, show the credits / final win
        if (currentNight >= 3)
        {
            SceneSetup.ShowFinalWinScreen();
            PlayerPrefs.SetInt("CurrentNight", 1);  // reset for next playthrough
        }
        else
        {
            // Otherwise show the regular sunrise screen with "Press N for next night"
            SceneSetup.ShowWinScreen(currentNight);
        }
    }

    // =================================================================
    //  RestartGame() — reload the current scene
    // =================================================================
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // =================================================================
    //  LoadNextNight() — save next night and reload the scene
    // =================================================================
    public void LoadNextNight()
    {
        PlayerPrefs.SetInt("CurrentNight", currentNight + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // =================================================================
    //  IsGameActive() — used by EnemyAI to know if it should pause
    // =================================================================
    public bool IsGameActive()
    {
        return !isGameOver && !hasWon;
    }
}
