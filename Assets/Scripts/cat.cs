using UnityEngine;

public class cat : MonoBehaviour
{
    public float speed = 8f;
    public float rotateSpeed = 10f;
    public float jumpForce = 5f;
    public float throwForce = 15f;
    public GameObject spherePrefab;
    public GameObject starPrefab;
    public Transform throwPoint;
    public AudioClip jump;
    public HealthBar healthBar;
    public AudioClip hit;

    [Header("Ground Check")]
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.3f;
    public float groundCheckOffset = 0.1f;

    private AudioSource audioSource;
    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isDead = false;
    public int health = 3;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            Debug.Log("Cat fell in water, game over");
            if (!isDead)
            {
                isDead = true;
                Invoke("CallGameOver", 0.3f);
            }
        }
    }

    AnimationStateChanger animationStateChanger;

    void Awake()
    {
        animationStateChanger = GetComponent<AnimationStateChanger>();
    }

    void CallGameOver()
    {
        GameManager.instance.GameOver();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        if (PlayerAbilities.instance != null)
        {
            health += PlayerAbilities.instance.bonusHealth;
            if (PlayerAbilities.instance.HasAbility(AbilityType.JumpBoost))
                jumpForce += PlayerAbilities.instance.jumpBoostAmount;
        }

        if (healthBar != null)
            healthBar.SetMaxHealth(health);

        // apply saved skin
        if (CatCustomization.instance != null &&
            CatCustomization.instance.selectedMaterials != null &&
            CatCustomization.instance.selectedMaterials.Length == 4)
        {
            Material[] mats = CatCustomization.instance.selectedMaterials;
            // mats[0] = arms, mats[1] = body, mats[2] = eyes, mats[3] = legs

            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                switch (r.gameObject.name)
                {
                    case "Cube.001": r.material = mats[1]; break; // body
                    case "Cube.002": r.material = mats[3]; break; // leg
                    case "Cube.003": r.material = mats[3]; break; // leg
                    case "Cube.004": r.material = mats[0]; break; // arm
                    case "Cube.005": r.material = mats[0]; break; // arm
                    case "Cube.008": r.material = mats[2]; break; // eye
                    case "Cube.009": r.material = mats[2]; break; // eye
                    
                }
            }
        }
        else
        {
            Debug.LogWarning("CatCustomization not found or materials not set");
        }

        Debug.Log("Cat initialized with health: " + health);
    }

    void Update()
    {
        if (transform.position.y < -15f)
        {
            if (!isDead)
            {
                isDead = true;
                Invoke("CallGameOver", 0.3f);
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 checkPos = transform.position - new Vector3(0f, groundCheckOffset, 0f);
        isGrounded = Physics.CheckSphere(checkPos, groundCheckRadius);
    }

    public void TakeDamage()
    {
        if (isDead) return;

        health--;
        Debug.Log("Player hit! Health remaining: " + health);

        if (audioSource != null && hit != null) // add this
            audioSource.PlayOneShot(hit);

        if (healthBar != null)
            healthBar.SetHealth(health);

        if (health <= 0)
        {
            isDead = true;
            Debug.Log("Player dead, triggering GameOver");
            GameManager.instance.GameOver();
        }
    }

    public void Move(Vector3 direction)
    {
        Vector3 move = direction * speed * Time.deltaTime;
        rb.MovePosition(rb.position + new Vector3(move.x, 0f, move.z));

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
            animationStateChanger.ChangeAnimationState("Walking");
        }
        else
        {
            animationStateChanger.ChangeAnimationState("Idle");
        }
    }

    public void Jump()
    {
        if (!isGrounded)
        {
            Debug.Log("Cat no jumped");
            return;
        }

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        Debug.Log("Cat jumped");

        if (audioSource != null && jump != null)
            audioSource.PlayOneShot(jump);
    }

    public void ThrowRegularBall(Transform target)
    {
        if (spherePrefab == null || target == null)
        {
            Debug.Log("ThrowRegularBall failed - prefab: " + spherePrefab + " target: " + target);
            return;
        }

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        Vector3 targetPos = new Vector3(target.position.x, target.position.y + 1f, target.position.z);
        Vector3 direction = (targetPos - spawnPos).normalized;
        direction = new Vector3(direction.x, 0f, direction.z).normalized;

        GameObject sphere = Instantiate(spherePrefab, spawnPos, Quaternion.identity);
        sphere.GetComponent<Ball>().destroyOnTag = "Enemy";
        sphere.GetComponent<Rigidbody>().useGravity = false;
        sphere.GetComponent<Rigidbody>().linearVelocity = direction * throwForce;
    }

    public void ThrowStars(Transform target)
    {
        if (starPrefab == null || target == null)
        {
            Debug.Log("ThrowStars failed - starPrefab: " + starPrefab + " target: " + target);
            return;
        }

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
        Vector3 targetPos = new Vector3(target.position.x, target.position.y + 1f, target.position.z);
        Vector3 direction = (targetPos - spawnPos).normalized;
        direction = new Vector3(direction.x, 0f, direction.z).normalized;

        GameObject star = Instantiate(starPrefab, spawnPos, Quaternion.identity);
        star.GetComponent<Ball>().destroyOnTag = "Enemy";
        star.GetComponent<Rigidbody>().useGravity = false;
        star.GetComponent<Rigidbody>().linearVelocity = direction * throwForce;
    }

    public void ThrowSphere(Transform target)
    {
        ThrowRegularBall(target);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 checkPos = transform.position - new Vector3(0f, groundCheckOffset, 0f);
        Gizmos.DrawWireSphere(checkPos, groundCheckRadius);
    }
}