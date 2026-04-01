using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    [SerializeField] private float lifeTime = 0.5f;
    [SerializeField] private float force = 30f;

    private Vector2 forceDirection;

    private Rigidbody2D rb;

    public void Init(Vector2 missileDir)
    {
        forceDirection = -missileDir.normalized;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            rb = collision.GetComponent<Rigidbody2D>();

            rb.AddForce(forceDirection * force, ForceMode2D.Impulse);
        }
    }
}
