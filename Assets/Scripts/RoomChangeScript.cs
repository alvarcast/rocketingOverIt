using UnityEngine;

public class RoomChangeScript : MonoBehaviour
{
    [Header("Mods")]
    [SerializeField] private bool changeGravity = false;
    [SerializeField] private float newGravity = 5;

    private float yAdd;
    private float direction;
    private Camera cam;

    private PlayerController player;
    private Rigidbody2D playerRb;
    private float previousGravity = 5f;

    private void Start()
    {
        player = FindFirstObjectByType<PlayerController>();
        playerRb = player.GetComponent<Rigidbody2D>();

        cam = Camera.main;
        yAdd = cam.orthographicSize * 2f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            direction = collision.transform.position.y < transform.position.y ? -1f : 1f;

            if (direction == -1f)
            {
                if (cam.transform.position.y == 0f || transform.position.y > cam.transform.position.y) { return; }
            }

            cam.transform.position += Vector3.up * yAdd * direction;

            if (changeGravity)
            {
                float gravity = previousGravity;
                
                if (direction != -1)
                {
                    previousGravity = playerRb.gravityScale;
                    gravity = newGravity;
                }

                playerRb.gravityScale = gravity;
            }
        }
    }
}
