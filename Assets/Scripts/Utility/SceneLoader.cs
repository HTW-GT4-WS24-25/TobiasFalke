using Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class SceneLoader : MonoBehaviour, IScene
    {
        public static IScene Instance { get; private set; }

        private void Awake()
        { 
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else Destroy(gameObject);
        }
        public void LoadScene(string sceneName)
        {
            AudioManager.Instance.StopTrack();
            if (SceneManager.GetActiveScene().name != sceneName)
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}