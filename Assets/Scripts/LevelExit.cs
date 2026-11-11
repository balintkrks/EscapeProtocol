using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    public Sprite[] doorFrames;
    public float frameDuration = 0.18f;

    private SpriteRenderer doorRenderer;
    private GameObject enteringPlayer;
    private SpriteRenderer playerRenderer;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private bool isOpening;
    private float timer;

    private void Start()
    {
        doorRenderer = GetComponent<SpriteRenderer>();
        if (doorFrames != null && doorFrames.Length > 0)
        {
            doorRenderer.sprite = doorFrames[0];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpening == false && other.CompareTag("Player"))
        {
            isOpening = true;
            timer = 0f;
            enteringPlayer = other.gameObject;

            PlayerMovement movement = enteringPlayer.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.enabled = false;
            }

            playerRb = enteringPlayer.GetComponent<Rigidbody2D>();
            playerRenderer = enteringPlayer.GetComponent<SpriteRenderer>();
            playerAnimator = enteringPlayer.GetComponent<Animator>();

            if (playerRenderer != null)
            {
                playerRenderer.sortingOrder = 4;
            }
        }
    }

    private void Update()
    {
        if (isOpening == false)
        {
            return;
        }

        timer += Time.deltaTime;

        if (doorFrames != null && doorFrames.Length > 0)
        {
            int frameIndex = (int)(timer / frameDuration);
            if (frameIndex >= doorFrames.Length)
            {
                frameIndex = doorFrames.Length - 1;
            }
            doorRenderer.sprite = doorFrames[frameIndex];
        }

        if (enteringPlayer != null)
        {
            Vector3 targetPos = new Vector3(transform.position.x, enteringPlayer.transform.position.y, enteringPlayer.transform.position.z);
            enteringPlayer.transform.position = Vector3.MoveTowards(enteringPlayer.transform.position, targetPos, 2.5f * Time.deltaTime);

            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector2(0f, playerRb.linearVelocity.y);
            }

            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("Speed", 3f);
            }

            if (timer > 0.45f && playerRenderer != null)
            {
                float alpha = 1f - ((timer - 0.45f) / 0.55f);
                if (alpha < 0f)
                {
                    alpha = 0f;
                }
                playerRenderer.color = new Color(1f, 1f, 1f, alpha);
                enteringPlayer.transform.localScale = Vector3.one * (0.75f + 0.25f * alpha);
            }
        }

        if (timer >= 1.15f)
        {
            LoadNextLevel();
        }
    }

    private void LoadNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextIndex);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
