using UnityEngine;

public class LevelSetup : MonoBehaviour
{
    public int totalEnemies = 1;

    void Start()
    {
        if (GameManager.instance != null)
            GameManager.instance.totalEnemies = totalEnemies;
    }
}