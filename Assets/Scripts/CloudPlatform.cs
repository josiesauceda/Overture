using UnityEngine;

public class CloudPlatform : MonoBehaviour
{
    public float bottomY;
    public float topY;
    public float moveSpeed = 2f;
    private bool movingUp = true;
    private Rigidbody passengerRb;

    void Update()
    {
        float delta = moveSpeed * Time.deltaTime;
        Vector3 movement;

        if (movingUp)
        {
            transform.position += Vector3.up * delta;
            movement = Vector3.up * delta;
            if (transform.position.y >= topY)
                movingUp = false;
        }
        else
        {
            transform.position -= Vector3.up * delta;
            movement = Vector3.down * delta;
            if (transform.position.y <= bottomY)
                movingUp = true;
        }

        // move cat with cloud via rigidbody
        if (passengerRb != null)
            passengerRb.MovePosition(passengerRb.position + movement);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            passengerRb = other.gameObject.GetComponent<Rigidbody>();
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            passengerRb = null;
    }
}