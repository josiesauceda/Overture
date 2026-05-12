using UnityEngine;

public class Ball : MonoBehaviour
{
    public float lifetime = 5f;
    public string destroyOnTag;

    void Start()
    {
        Debug.Log("Ball spawned, will destroy on tag: " + destroyOnTag);
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ball hit: " + other.name + " with tag: " + other.tag);

        if (other.CompareTag(destroyOnTag))
        {
            Debug.Log("Ball hit target: " + destroyOnTag);
            if (destroyOnTag == "Player")
            {
                cat c = other.GetComponent<cat>() ?? other.GetComponentInParent<cat>();
                if (c != null) c.TakeDamage();
                else Debug.LogWarning("Ball hit Player tag but no cat component found on: " + other.name);
            }
            else if (destroyOnTag == "Enemy")
            {
                pig p = other.GetComponent<pig>() ?? other.GetComponentInParent<pig>();
                if (p != null) p.TakeDamage();

                Penguin penguin = other.GetComponent<Penguin>() ?? other.GetComponentInParent<Penguin>();
                if (penguin != null) penguin.TakeDamage();

                Corgi corgi = other.GetComponent<Corgi>() ?? other.GetComponentInParent<Corgi>();
                if (corgi != null) corgi.TakeDamage();

                Destroy(gameObject);
            }

            Destroy(gameObject);
        }

        if (other.CompareTag("Ground"))
        {
            Debug.Log("Ball hit ground, destroying");
            Destroy(gameObject);
        }
    }
}