
using UnityEngine;
using TMPro;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI difficultyText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI levelUpText;

    public ItemGenerator generator;

    public float time = 30f;

    public AudioClip levelUpSound;
    private AudioSource audioSource;

    private int score = 0;
    private int level = 0;
    private bool gameOver = false;
    private Coroutine levelUpCoroutine;

    void Start()
    {
        Time.timeScale = 1f;

        levelUpText.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();

        SetLevel(1);
        UpdateUI();
    }

    void Update()
    {
        if (gameOver) return;

        time -= Time.deltaTime;
        time = Mathf.Max(0f, time);

        if (time <= 10f && level != 3)
        {
            SetLevel(3);
        }
        else if (time <= 20f && time > 10f && level != 2)
        {
            SetLevel(2);
        }

        UpdateUI();

        if (time <= 0f)
        {
            gameOver = true;
            difficultyText.text = "GAME OVER";

            if (generator != null)
                generator.enabled = false;

            if (levelUpCoroutine != null)
                StopCoroutine(levelUpCoroutine);

            levelUpText.gameObject.SetActive(false);

            Time.timeScale = 0f;
        }
    }

    void SetLevel(int newLevel)
    {
        level = newLevel;
        levelText.text = "LEVEL " + level;

        if (level == 1)
        {
            difficultyText.text = "EASY";
            generator.interval = 1f;
            generator.dropSpeed = 1f;
            generator.bombRate = 20;
        }
        else if (level == 2)
        {
            difficultyText.text = "HARDER!";
            generator.interval = 0.8f;
            generator.dropSpeed = 1.5f;
            generator.bombRate = 40;

            ShowLevelUp("LEVEL 2\nLEVEL UP!");
        }
        else if (level == 3)
        {
            difficultyText.text = "VERY HARD!";
            generator.interval = 0.5f;
            generator.dropSpeed = 2f;
            generator.bombRate = 60;

            ShowLevelUp("LEVEL 3\nDANGER!");
        }

        Debug.Log("進入第 " + level + " 關");
    }

    void ShowLevelUp(string message)
    {
        if (audioSource != null && levelUpSound != null)
        {
            audioSource.PlayOneShot(levelUpSound);
        }
        if (levelUpCoroutine != null)
            StopCoroutine(levelUpCoroutine);

        levelUpCoroutine = StartCoroutine(
            LevelUpAnimation(message)
        );
    }

    IEnumerator LevelUpAnimation(string message)
    {
        levelUpText.text = message;
        levelUpText.gameObject.SetActive(true);

        levelUpText.transform.localScale = Vector3.zero;

        float elapsed = 0f;
        float duration = 0.4f;

        // 文字從小變大
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float progress =
                Mathf.Clamp01(elapsed / duration);

            levelUpText.transform.localScale =
                Vector3.one * progress;

            yield return null;
        }

        levelUpText.transform.localScale = Vector3.one;

        // 停留 2 秒
        yield return new WaitForSeconds(2f);

        levelUpText.gameObject.SetActive(false);
        levelUpCoroutine = null;
    }

    void UpdateUI()
    {
        timeText.text = "TIME " + time.ToString("F1");
        scoreText.text = "SCORE " + score;
    }

    public void GetApple()
    {
        if (gameOver) return;

        score += 100;
        UpdateUI();
    }

    public void GetBomb()
    {
        if (gameOver) return;

        score /= 2;
        UpdateUI();
    }
}
