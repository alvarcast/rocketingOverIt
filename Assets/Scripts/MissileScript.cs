using Unity.VisualScripting;
using UnityEngine;


public class MissileScript : MonoBehaviour
{
    [Header("Missile Settings")]
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float missileSpeed = 10f;

    [Header("Missile objects")]
    [SerializeField] private GameObject explosion;
    [SerializeField] private Transform tip;

    [Header("Audio")]
    [SerializeField] private AudioClip explosionSound;
    private AudioSource audioSource;

    private Vector2 direction;
    private float angle;
    private bool exploded = false;

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Start()
    {
        GameObject o = GameObject.Find("AudioPlayer/SFX");
        audioSource = o.GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.Translate(direction * missileSpeed * Time.deltaTime, Space.World);

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Blowable") || collision.CompareTag("Ice")) && !exploded)
        {
            exploded = true;
            Explode();
        }
    }

    private void Explode()
    {
        audioSource.PlayOneShot(explosionSound);

        GameObject exp = Instantiate(explosion, tip.position, Quaternion.identity);
        ExplosionScript explosionScript = exp.GetComponent<ExplosionScript>();

        explosionScript.Init(direction);

        Destroy(gameObject);
    }
}
