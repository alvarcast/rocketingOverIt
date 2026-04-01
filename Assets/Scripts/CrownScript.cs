using UnityEngine;

public class CrownScript : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip fanfare;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioSource musicAudioSource;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        GameObject o = GameObject.Find("AudioPlayer/SFX");
        sfxAudioSource = o.GetComponent<AudioSource>();

        o = GameObject.Find("AudioPlayer/Music");
        musicAudioSource = o.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);

            winGame();
        }
    }

    private void winGame()
    {
        sfxAudioSource.PlayOneShot(fanfare);
        musicAudioSource.Stop();

        playerController.stopGame();
    }
}
