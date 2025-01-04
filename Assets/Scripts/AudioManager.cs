using UnityEngine;

public class AudioManager : MonoBehaviour, IAudio
{
    public static IAudio Instance { get; private set; } // singleton instance for easy access
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource soundSource;
    private float musicVolume = 1.0f;
    private float soundVolume = 1.0f;
    
    private void Awake()
    { 
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Validate audio sources
        Debug.Assert(musicSource != null, "AudioManager: musicSource not assigned.");
        Debug.Assert(soundSource != null, "AudioManager: soundSource not assigned.");
    }

    public void PlayMusic(string fileName)
    {
        if (musicSource == null) return;
        // load audio by file name
        AudioClip musicClip = Resources.Load<AudioClip>(fileName);
        if (musicClip == null) return;
        // do nothing if the track is already playing
        if (musicSource.clip == musicClip && musicSource.isPlaying) return;
        // play music with desired properties
        musicSource.clip = musicClip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource == null) return;
        musicSource.Stop();
    }

    public void PlaySound(string fileName)
    {
        if (soundSource == null) return;
        // load audio by file name
        AudioClip soundClip = Resources.Load<AudioClip>(fileName);
        if (soundClip == null) return;
        // play sound effect with desired properties
        soundSource.volume = soundVolume;
        soundSource.PlayOneShot(soundClip);
    }
}