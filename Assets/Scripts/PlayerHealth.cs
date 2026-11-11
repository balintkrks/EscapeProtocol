using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Vector3 respawnPosition;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        respawnPosition = transform.position;
    }

    public void Die()
    {
        rb.linearVelocity = Vector2.zero;
        transform.position = respawnPosition;
    }

    public void SetCheckpoint(Vector3 newPosition)
    {
        respawnPosition = newPosition;
    }
}
