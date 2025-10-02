using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip goldenClip;
    public AudioClip livinOnAPrayerClip;
    public AudioClip rockThatBodyClip;
    public AudioClip believerClip;

    public BeatHandSyncController gameScript;
    public BeatBlockSpawner spawner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void selectGolden()
    {
        musicSource.clip = goldenClip;
        gameScript.bpm = 123f;
        gameScript.offset = -2.5f;
        spawner.bpm = 123.5f;
        spawner.delay = 0.5f;
        musicSource.Play();
    }

    public void selectLivinOnAPrayer()
    {
        musicSource.clip = livinOnAPrayerClip;
        gameScript.bpm = 122.5f;
        gameScript.offset = -2.8f;
        spawner.bpm = 123f;
        spawner.delay = 0.2f;
        musicSource.Play();
    }

    public void selectRockThatBody()
    {
        musicSource.clip = rockThatBodyClip;
        gameScript.bpm = 124.5f;
        gameScript.offset = -4.0f;
        spawner.bpm = 125f;
        spawner.delay = -1.0f;
        musicSource.Play();
    }

    public void selectBeliever()
    {
        musicSource.clip = believerClip;
        gameScript.bpm = 124.5f;
        gameScript.offset = -4.4f;
        spawner.bpm = 125f;
        spawner.delay = -1.4f;
        musicSource.Play();
    }
}
