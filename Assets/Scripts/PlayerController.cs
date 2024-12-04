using UnityEngine;

// TODO: implement SP bar logic

public class PlayerController : MonoBehaviour
{
    private PlayerStats stats;
    public int _maxHealth = 100;
    public int _maxSpecial = 100;
    
    private void Start()
    {
        stats = new PlayerStats
        {
            _health = _maxHealth,
            _special = _maxSpecial,
            _score = 0f
        };
    }

    private void Update()
    {
        stats._score += Time.deltaTime * 5;
    }

    private void LateUpdate()
    {
        UIManager.Instance.SetScore(stats._score);
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        CheckItemCollision(other);
        CheckObstacleCollision(other);
        
        if (stats._health == 0)
        {
            Destroy(gameObject);
            // TODO: connect to GameManager and trigger Game Over
        }
    }

    private void CheckItemCollision(Collision2D other)
    {
        ItemManager item = other.gameObject.GetComponent<ItemManager>();
        if (item != null)
        {
            stats = item.TriggerItemEffect(stats, item.itemType);
        }
    }

    private void CheckObstacleCollision(Collision2D other)
    {
        // TODO: implement obstacle collision triggering
    }
}