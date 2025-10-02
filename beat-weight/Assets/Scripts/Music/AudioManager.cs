using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    public AudioClip mainSong;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSource.clip = mainSong;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
