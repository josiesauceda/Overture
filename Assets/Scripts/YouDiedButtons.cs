using UnityEngine;

public class YouDiedButtons : MonoBehaviour
{
    public void RetryLevel()
    {
        GameManager.instance.RestartLevel();
    }

    public void MainMenu()
    {
        GameManager.instance.LoadMainMenu();
    }
}
