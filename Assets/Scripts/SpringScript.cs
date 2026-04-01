using UnityEngine;

public class SpringScript : MonoBehaviour
{
    public enum pushDirection { Left, Right }

    [Header("Configuración de Fuerza")]
    [SerializeField] private float force = 15f;
    [SerializeField] private pushDirection selectedDir;

    [Header("Audio")]
    [SerializeField] private AudioClip boing;
    private AudioSource audioSource;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        GameObject o = GameObject.Find("AudioPlayer/SFX");
        if (o != null) audioSource = o.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();

            if (playerRb != null)
            {
                Vector2 direccionFinal = (selectedDir == pushDirection.Right) ? Vector2.right : Vector2.left;

                audioSource.PlayOneShot(boing);

                animator.SetTrigger("Spring");

                playerRb.linearVelocity = new Vector2(0f, playerRb.linearVelocity.y);
                playerRb.AddForce(direccionFinal * force, ForceMode2D.Impulse);
            }
        }
    }
}