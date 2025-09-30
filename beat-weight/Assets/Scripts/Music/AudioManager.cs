using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip goldenClip;
    public AudioClip livinOnAPrayerClip;
    public AudioClip rockThatBodyClip;
    public AudioClip believerClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void selectGolden()
    {
        musicSource.clip = goldenClip;
        musicSource.Play();
    }

    public void selectLivinOnAPrayer()
    {
        musicSource.clip = livinOnAPrayerClip;
        musicSource.Play();
    }

    public void selectRockThatBody()
    {
        musicSource.clip = rockThatBodyClip;
        musicSource.Play();
    }

    public void selectBeliever()
    {
        musicSource.clip = believerClip;
        musicSource.Play();
    }
}
