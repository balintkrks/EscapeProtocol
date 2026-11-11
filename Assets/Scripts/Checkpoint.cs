using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SpriteRenderer sr;
    private BoxCollider2D col;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.SetCheckpoint(transform.position);
                sr.color = Color.green;
                col.enabled = false;
            }
        }
    }
}
