using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int totalEnemies = 1;
    private int enemiesDead = 0;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        enemiesDead = 0;
    }

    public void EnemyDied()
    {
        enemiesDead++;
        Debug.Log("Enemies dead: " + enemiesDead + "/" + totalEnemies);
        if (enemiesDead >= totalEnemies)
            Success();
    }

    public void Success()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("CurrentLevel", currentScene);

        if (currentScene == "Level 1")
        {
            PlayerPrefs.SetString("NextLevel", "Level2");
            SceneManager.LoadScene("Next Level!");
        }
        else if (currentScene == "Level2")
        {
            PlayerPrefs.SetString("NextLevel", "Level3");
            SceneManager.LoadScene("Next Level!");
        }
        else if (currentScene == "Level3")
        {
            SceneManager.LoadScene("Cutscene");
        }
    }

    public void GameOver()
    {
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);
        SceneManager.LoadScene("You Died!");
    }

    public void LoadNextLevel()
    {
        string nextLevel = PlayerPrefs.GetString("NextLevel", "Level 1");
        SceneManager.LoadScene(nextLevel);
    }

    public void RestartLevel()
    {
        string currentLevel = PlayerPrefs.GetString("CurrentLevel", "Level 1");
        SceneManager.LoadScene(currentLevel);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}