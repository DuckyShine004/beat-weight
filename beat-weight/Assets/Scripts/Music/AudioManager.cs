using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    AudioSource musicSource;

    public AudioClip goldenClip;
    public AudioClip livinOnAPrayerClip;
    public AudioClip rockThatBodyClip;
    public AudioClip believerClip;

    public VisualBPMManager gameScript;
    public BeatBlockSpawner spawner;
    public TimerScript timer;
    public EndScreenScript endScreen;
    public FollowCurveBPM followCurveBPM;
    public StartDelayTimer startDelayTimer;

    public void PlaySoundEffect(AudioClip soundEffect)
    {
        musicSource.PlayOneShot(soundEffect);
    }

    public void SelectGolden()
    {
        musicSource.clip = goldenClip;
        gameScript.bpm = 123f;
        gameScript.offset = -2.5f;
        spawner.bpm = 123.5f;
        spawner.delay = 0.5f;
        timer.timerDuration = 80f;
        endScreen.songName = "Golden - HUNTR/X";
        followCurveBPM.bpm = 123.5f;
        followCurveBPM.delay = 2.5f;
        startDelayTimer.SetDelay(2.5f);
        musicSource.Play();
    }

    public void SelectLivinOnAPrayer()
    {
        musicSource.clip = livinOnAPrayerClip;
        gameScript.bpm = 122.5f;
        gameScript.offset = -2.8f;
        spawner.bpm = 123f;
        spawner.delay = 0.2f;
        timer.timerDuration = 73f;
        endScreen.songName = "Livin' on a Prayer - Bon Jovi";
        followCurveBPM.bpm = 122.5f;
        followCurveBPM.delay = 2.8f;
        startDelayTimer.SetDelay(2.8f);
        musicSource.Play();
    }

    public void SelectRockThatBody()
    {
        musicSource.clip = rockThatBodyClip;
        gameScript.bpm = 124.5f;
        gameScript.offset = -4.0f;
        spawner.bpm = 125f;
        spawner.delay = -1.0f;
        timer.timerDuration = 72f;
        endScreen.songName = "Rock That Body - Black Eyed Peas";
        followCurveBPM.bpm = 125f;
        followCurveBPM.delay = 4.0f;
        startDelayTimer.SetDelay(4.0f);
        musicSource.Play();
    }

    public void SelectBeliever()
    {
        musicSource.clip = believerClip;
        gameScript.bpm = 124.5f;
        gameScript.offset = -4.4f;
        spawner.bpm = 125f;
        spawner.delay = -1.4f;
        timer.timerDuration = 69f;
        endScreen.songName = "Believer - Imagine Dragons";
        followCurveBPM.bpm = 125f;
        followCurveBPM.delay = 4.4f;
        startDelayTimer.SetDelay(4.4f);
        musicSource.Play();
    }

    public void NSSelectGolden()
    {
        musicSource.clip = goldenClip;
        gameScript.bpm = 115f;
        gameScript.offset = -4.0f;
        spawner.bpm = 115.5f;
        spawner.delay = 0.5f;
        timer.timerDuration = 80f;
        endScreen.songName = "Golden - HUNTR/X (NS)";
        followCurveBPM.bpm = 115.5f;
        followCurveBPM.delay = 4.0f;
        startDelayTimer.SetDelay(4.0f);
        musicSource.Play();
    }

    public void NSSelectLivinOnAPrayer()
    {
        musicSource.clip = livinOnAPrayerClip;
        gameScript.bpm = 115.5f;
        gameScript.offset = -2.1f;
        spawner.bpm = 115f;
        spawner.delay = 0.2f;
        timer.timerDuration = 73f;
        endScreen.songName = "Livin' on a Prayer - Bon Jovi (NS)";
        followCurveBPM.bpm = 115.5f;
        followCurveBPM.delay = 2.1f;
        startDelayTimer.SetDelay(2.1f);
        musicSource.Play();
    }

    public void NSSelectRockThatBody()
    {
        musicSource.clip = rockThatBodyClip;
        gameScript.bpm = 115.5f;
        gameScript.offset = -2.0f;
        spawner.bpm = 115f;
        spawner.delay = -1.0f;
        timer.timerDuration = 72f;
        endScreen.songName = "Rock That Body - Black Eyed Peas (NS)";
        followCurveBPM.bpm = 115f;
        followCurveBPM.delay = 2.0f;
        startDelayTimer.SetDelay(2.0f);
        musicSource.Play();
    }

    public void NSSelectBeliever()
    {
        musicSource.clip = believerClip;
        gameScript.bpm = 115.5f;
        gameScript.offset = -1.0f;
        spawner.bpm = 115f;
        spawner.delay = -1.4f;
        timer.timerDuration = 69f;
        endScreen.songName = "Believer - Imagine Dragons (NS)";
        followCurveBPM.bpm = 115f;
        followCurveBPM.delay = 1.0f;
        startDelayTimer.SetDelay(1.0f);
        musicSource.Play();
    }
}
