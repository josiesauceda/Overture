using UnityEngine;

public class Corgi : MonoBehaviour
{
    public float attackRange = 20f;
    public float throwForce = 10f;
    public float throwInterval = 2f;
    public GameObject starPrefab;
    public Transform throwPoint;
    public Transform player;
    public int maxLives = 4;
    public int healthPerLife = 10;
    public CorgiCloud corgiCloud;

    [Header("Attack Mode")]
    public bool tripleShot = false;
    public float tripleShotSpread = 15f;

    private int currentLives;
    private int currentHealth;
    private float timer;
    private bool frozen = false;
    public GameObject frozenEffect;
    private GameObject activeFrozenEffect;

    void Start()
    {
        currentLives = maxLives;
        currentHealth = healthPerLife;
        Debug.Log("Corgi initialized - lives: " + currentLives + " health: " + currentHealth);
    }

    void Update()
    {
        if (frozen) return; // stop throwing while frozen
        if (player == null) return;

        timer += Time.deltaTime;
        if (timer >= throwInterval)
        {
            if (tripleShot)
                ShootTriple();
            else
                ShootSingle();
            timer = 0f;
        }
    }

    public void Freeze(float duration)
    {
        if (!frozen)
            StartCoroutine(FreezeCoroutine(duration));
    }

    System.Collections.IEnumerator FreezeCoroutine(float duration)
    {
        frozen = true;
        if (frozenEffect != null)
            activeFrozenEffect = Instantiate(frozenEffect, transform.position, Quaternion.identity, transform);
        yield return new WaitForSeconds(duration);
        frozen = false;
        if (activeFrozenEffect != null)
            Destroy(activeFrozenEffect);
    }

    public void TakeDamage()
    {
        currentHealth--;
        Debug.Log("Corgi hit! Health: " + currentHealth + " Lives: " + currentLives);

        if (currentHealth <= 0)
        {
            currentLives--;
            Debug.Log("Corgi life lost! Lives remaining: " + currentLives);

            if (currentLives <= 0)
            {
                Debug.Log("Corgi truly dead!");
                GameManager.instance.EnemyDied();
                Destroy(gameObject);
            }
            else
            {
                currentHealth = healthPerLife;
                Debug.Log("Corgi healed! Rising to next stage");
                if (corgiCloud != null)
                    corgiCloud.RiseToNextStage();
            }
        }
    }

    void ShootSingle()
    {
        if (starPrefab == null || player == null) return;
        Vector3 spawnPos = throwPoint != null ? throwPoint.position : transform.position + transform.forward;
        Vector3 targetPos = new Vector3(player.position.x, player.position.y + 1f, player.position.z);
        Vector3 direction = (targetPos - spawnPos).normalized;
        SpawnStar(spawnPos, direction);
    }

    void ShootTriple()
    {
        if (starPrefab == null || player == null) return;
        Vector3 spawnPos = throwPoint != null ? throwPoint.position : transform.position + transform.forward;
        Vector3 targetPos = new Vector3(player.position.x, player.position.y + 1f, player.position.z);
        Vector3 baseDirection = (targetPos - spawnPos).normalized;

        SpawnStar(spawnPos, baseDirection);
        SpawnStar(spawnPos, Quaternion.Euler(0, -tripleShotSpread, 0) * baseDirection);
        SpawnStar(spawnPos, Quaternion.Euler(0, tripleShotSpread, 0) * baseDirection);
    }

    void SpawnStar(Vector3 spawnPos, Vector3 direction)
    {
        Debug.Log("Corgi shooting star, direction: " + direction);
        GameObject star = Instantiate(starPrefab, spawnPos, Quaternion.identity);
        star.GetComponent<Ball>().destroyOnTag = "Player";
        star.GetComponent<Rigidbody>().useGravity = false;
        star.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
    }
}