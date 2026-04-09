using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Night Settings")]
    public int currentNight = 1;
    public float nightDuration = 120f;

    [Header("UI References")]
    public TMP_Text clockText;
    public TMP_Text nightText;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameOverController gameOverController;

    [Header("Audio")]
    public AudioSource ambientAudio;

    float timer;
    bool isGameOver;
    bool hasWon;

    void Start()
    {
        timer = 0f;
        isGameOver = false;
        hasWon = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
        if (nightText != null) nightText.text = "Night " + currentNight;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (isGameOver || hasWon) return;

        timer += Time.deltaTime;

        UpdateClock();

        if (timer >= nightDuration)
        {
            WinNight();
        }
    }

    void UpdateClock()
    {
        float progress = timer / nightDuration;
        int hour = Mathf.FloorToInt(progress * 6f);

        string timeText;
        if (hour == 0)
            timeText = "12 AM";
        else
            timeText = hour + " AM";

        if (clockText != null)
            clockText.text = timeText;
    }

    public void TriggerGameOver()
    {
        if (isGameOver || hasWon) return;

        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (gameOverController != null)
            gameOverController.Show();

        if (ambientAudio != null)
            ambientAudio.Stop();
    }

    void WinNight()
    {
        hasWon = true;

        if (winPanel != null)
            winPanel.SetActive(true);

        if (clockText != null)
            clockText.text = "6 AM";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextNight()
    {
        currentNight++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public bool IsGameActive()
    {
        return !isGameOver && !hasWon;
    }
}
