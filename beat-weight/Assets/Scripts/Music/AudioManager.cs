using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip goldenClip;
    public AudioClip livinOnAPrayerClip;
    public AudioClip rockThatBodyClip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void selectGolden()
    {
        musicSource.clip = goldenClip;
        musicSource.Play();
    }

    public void selectLivinOnAPrayer()
    {
        Debug.Log("selectLivinOnAPrayer called");
        if (musicSource == null)
        {
            Debug.LogError("musicSource is null!");
            return;
        }

        if (livinOnAPrayerClip == null)
        {
            Debug.LogError("livinOnAPrayerClip is null!");
            return;
        }

        if (!musicSource.enabled)
        {
            Debug.LogWarning("musicSource is disabled. Enabling now.");
            musicSource.enabled = true;
        }

        if (!musicSource.gameObject.activeInHierarchy)
        {
            Debug.LogWarning("musicSource GameObject is inactive. Activating now.");
            musicSource.gameObject.SetActive(true);
        }

        musicSource.clip = livinOnAPrayerClip;
        Debug.Log("Playing livinOnAPrayerClip");
        musicSource.Play();
    }

    public void selectRockThatBody()
    {
        musicSource.clip = rockThatBodyClip;
        musicSource.Play();
    }
}
