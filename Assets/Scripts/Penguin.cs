using UnityEngine;

public class Penguin : MonoBehaviour
{
    public float attackRange = 15f;
    public float throwForce = 10f;
    public float throwInterval = 2f;
    public GameObject spherePrefab;
    public Transform throwPoint;
    public Transform player;
    private float timer;
    public int health = 20;
    private bool frozen = false;
    public GameObject frozenEffect;
    private GameObject activeFrozenEffect;

    void Start()
    {
        Debug.Log("Penguin initialized with health: " + health);
    }

    void Update()
    {
        if (frozen) return; // stop throwing while frozen
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            timer += Time.deltaTime;
            if (timer >= throwInterval)
            {
                ThrowAtPlayer();
                timer = 0f;
            }
        }
        else
        {
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
        health--;
        Debug.Log("Penguin hit! Health remaining: " + health);
        if (health <= 0)
        {
            Debug.Log("Penguin dead");
            GameManager.instance.EnemyDied();
            Destroy(gameObject);
        }
    }

    void ThrowAtPlayer()
    {
        if (spherePrefab == null || player == null)
        {
            Debug.Log("Enemy ThrowAtPlayer failed - prefab: " + spherePrefab + " player: " + player);
            return;
        }

        Vector3 spawnPos = throwPoint != null ? throwPoint.position : transform.position + transform.forward;
        Vector3 targetPos = new Vector3(player.position.x, player.position.y + 1f, player.position.z);
        Vector3 direction = (targetPos - spawnPos).normalized;

        Debug.Log("Enemy throwing sphere, direction: " + direction);

        GameObject sphere = Instantiate(spherePrefab, spawnPos, Quaternion.identity);
        sphere.GetComponent<Ball>().destroyOnTag = "Player";
        sphere.GetComponent<Rigidbody>().useGravity = false;
        sphere.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
    }
}