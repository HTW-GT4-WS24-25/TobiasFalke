using UnityEngine;

// TODO: Health-Logik und SP-Bar-Logik einbauen

public class PlayerController : MonoBehaviour
{
    private int _playerHealth = 3;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.CompareTag("Obstacle"))
            {
                _playerHealth--;
                if (_playerHealth == 0)
                {
                    Destroy(gameObject);
                    // TODO: connect to GameManager and trigger Game Over
                }
            }
        }
    }