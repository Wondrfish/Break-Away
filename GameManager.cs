using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text livesText;
    public TMP_Text timerText;
    public TMP_Text finalScoreText;
    public GameObject gameOverPanel;

    [Header("Gameplay")]
    public int startingLives = 3;
    public float gameDuration = 180f; // 180 seconds
    public float damageCooldown = 1.5f; 

    private int score = 0;
    private int lives;
    private float remainingTime;
    private bool isGameOver = false;
    private bool canTakeDamage = true;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        lives = startingLives;
        remainingTime = gameDuration;
        UpdateScoreUI();
        UpdateLivesUI();
        UpdateTimerUI();

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        remainingTime -= Time.deltaTime;
        UpdateTimerUI();

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            GameOver();
        }
    }

    // --- SCORE ---
    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    // --- DAMAGE ---
    public void DamagePlayer()
    {
        if (isGameOver || !canTakeDamage) return;

        canTakeDamage = false;
        lives--;
        UpdateLivesUI();

        if (lives <= 0)
            GameOver();
        else
            StartCoroutine(DamageCooldown());
    }

    private IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = $"Lives: {lives}";
    }

    //  SLOW TIME POWERUP FEATURE
    public void SlowTime(float duration, float timeScale)
    {
        if (isGameOver) return;

        Time.timeScale = timeScale;
        CancelInvoke(nameof(ResetTime));
        Invoke(nameof(ResetTime), duration * timeScale);
    }

    private void ResetTime()
    {
        Time.timeScale = 1f;
    }

    // --- TIMER ---
    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = $"Time: {minutes:00}:{seconds:00}";
        }
    }

    // --- GAME OVER ---
    private void GameOver()
    {
        isGameOver = true;
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (finalScoreText != null)
            finalScoreText.text = $"Final Score: {score}";

        Debug.Log("Game Over!");
    }

    // --- RESTART / MAIN MENU ---
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}