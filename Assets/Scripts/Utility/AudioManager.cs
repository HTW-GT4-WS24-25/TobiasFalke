using Interfaces;
using UnityEngine;

namespace Utility
{
    public class AudioManager : MonoBehaviour, IAudio
    {
        public static IAudio Instance { get; private set; }
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource backgroundSource;
        [SerializeField] private AudioSource soundSource;
    
        private float musicVolume = 1.0f;
        private float backgroundVolume = 1.0f;
        private float soundVolume = 1.0f;
    
        private void Awake()
        { 
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }

        public void PlayTrack(string fileName)
        {
            if (musicSource == null) return;
            AudioClip musicClip = Resources.Load<AudioClip>(fileName);
            if (musicClip == null) return;
            if (musicSource.clip == musicClip && musicSource.isPlaying) return;
            musicSource.clip = musicClip;
            musicSource.volume = musicVolume;
            musicSource.loop = true;
            musicSource.Play();
            Debug.Assert(musicSource != null, "AudioManager: musicSource not assigned.");
        }
    
        public void StopTrack()
        {
            if (musicSource == null) return;
            musicSource.Stop();
        }
    
        public void PlayBackgroundTrack(string fileName)
        {
            if (backgroundSource == null) return;
            AudioClip musicClip = Resources.Load<AudioClip>(fileName);
            if (musicClip == null) return;
            if (backgroundSource.clip == musicClip && backgroundSource.isPlaying) return;
            backgroundSource.clip = musicClip;
            backgroundSource.volume = musicVolume;
            backgroundSource.loop = true;
            backgroundSource.Play();
            Debug.Assert(backgroundSource != null, "AudioManager: musicSource not assigned.");
        }
    
        public void StopBackgroundTrack()
        {
            if (backgroundSource == null) return;
            backgroundSource.Stop();
        }

        public void PlaySound(string fileName)
        {
            if (!soundSource) return;
            AudioClip soundClip = Resources.Load<AudioClip>(fileName);
            if (!soundClip) return;
            soundSource.volume = soundVolume;
            soundSource.PlayOneShot(soundClip);
            Debug.Assert(soundSource != null, "AudioManager: soundSource not assigned.");
        }
    }
}