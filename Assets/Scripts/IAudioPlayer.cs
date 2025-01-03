using UnityEngine;

// Define an interface for sound playing

public interface IAudioPlayer
{
        void PlayMusic(string fileName);
        void StopMusic();
        void PlaySound(string fileName);
}