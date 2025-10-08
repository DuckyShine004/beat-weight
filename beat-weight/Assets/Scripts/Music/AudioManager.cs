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
    public FollowCurveBPM followCurveBPM;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void SelectGolden()
    {
        musicSource.clip = goldenClip;
        gameScript.bpm = 123f;
        gameScript.offset = -2.5f;
        spawner.bpm = 123.5f;
        spawner.delay = 0.5f;
        followCurveBPM.bpm = 123.5f;
        followCurveBPM.delay = 2.5f;
        musicSource.Play();
    }

    public void SelectLivinOnAPrayer()
    {
        musicSource.clip = livinOnAPrayerClip;
        gameScript.bpm = 122.5f;
        gameScript.offset = -2.8f;
        spawner.bpm = 123f;
        spawner.delay = 0.2f;
        followCurveBPM.bpm = 123f;
        followCurveBPM.delay = 2.8f;
        musicSource.Play();
    }

    public void SelectRockThatBody()
    {
        musicSource.clip = rockThatBodyClip;
        gameScript.bpm = 124.5f;
        gameScript.offset = -4.0f;
        spawner.bpm = 125f;
        spawner.delay = -1.0f;
        followCurveBPM.bpm = 125f;
        followCurveBPM.delay = 4.0f;
        musicSource.Play();
    }

    public void SelectBeliever()
    {
        musicSource.clip = believerClip;
        gameScript.bpm = 124.5f;
        gameScript.offset = -4.4f;
        spawner.bpm = 125f;
        spawner.delay = -1.4f;
        followCurveBPM.bpm = 125f;
        musicSource.Play();
    }
}