using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip goldenClip;
    public AudioClip livinOnAPrayerClip;
    public AudioClip rockThatBodyClip;
    public AudioClip believerClip;

    public BeatHandSyncController gameScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void selectGolden()
    {
        musicSource.clip = goldenClip;
        gameScript.bpm = 123f;
        gameScript.offset = -2.5f;
        musicSource.Play();
    }

    public void selectLivinOnAPrayer()
    {
        musicSource.clip = livinOnAPrayerClip;
        gameScript.bpm = 123f;
        gameScript.offset = -2.8f;
        musicSource.Play();
    }

    public void selectRockThatBody()
    {
        musicSource.clip = rockThatBodyClip;
        gameScript.bpm = 125f;
        gameScript.offset = -4.0f;
        musicSource.Play();
    }

    public void selectBeliever()
    {
        musicSource.clip = believerClip;
        gameScript.bpm = 125f;
        gameScript.offset = -4.4f;
        musicSource.Play();
    }
}
