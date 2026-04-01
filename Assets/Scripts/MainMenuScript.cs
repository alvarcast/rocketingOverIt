using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject mainButtons;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject credits;

    [Header("Settings")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Audio")]
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    [SerializeField] private AudioClip explosion;

    private void Start()
    {
        setMusicVolume();
        setSfxVolume();
    }

    public void play()
    {
        SceneManager.LoadScene("Game");
    }

    public void openSettings()
    {
        mainButtons.SetActive(false);
        settings.SetActive(true);
    }

    public void closeSettings()
    {
        mainButtons.SetActive(true);
        settings.SetActive(false);
    }

    public void openCredits()
    {
        mainButtons.SetActive(false);
        credits.SetActive(true);
    }

    public void closeCredits()
    {
        mainButtons.SetActive(true);
        credits.SetActive(false);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void playTestSfx()
    {
        sfxAudioSource.PlayOneShot(explosion);
    }

    public void setMusicVolume()
    {
        mixer.SetFloat("Music", Mathf.Log10(musicSlider.value) * 20);
    }

    public void setSfxVolume()
    {
        mixer.SetFloat("SFX", Mathf.Log10(sfxSlider.value) * 20);
    }
}
