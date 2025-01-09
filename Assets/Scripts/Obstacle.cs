using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum ObstacleType
{
    Wall,
    BigObstacle,
    SmallObstacle,
    Rail,
    Hole
}

public class Obstacle : MonoBehaviour
{
    [SerializeField] public ObstacleType Type;

    private float _fallSpeed;

    public bool IsJumpable => Type != ObstacleType.Wall;
    
    private void Update()
    {
        _fallSpeed = GameModel.Instance.GameSpeed / 2;
        transform.Translate(new Vector3(0f, -_fallSpeed * Time.deltaTime, 0f));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision is triggered.");
        Debug.Log("Player Tag: " + other.gameObject.tag);
        if (!other.gameObject.CompareTag("Player")) return;
        var evt = Events.ObstacleCollisionEvent;
        evt.DamageValue = DetermineDamageAmount();
        evt.Obstacle = this;
        EventManager.Broadcast(evt);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Collision is exited.");
        if (!other.gameObject.CompareTag("Player")) return;
        var evt = Events.ObstacleCollisionEvent;
        evt.Obstacle = this;
        EventManager.Broadcast(evt);
    }
    
    public int DetermineScore()
    {
        // Determine score based on obstacle type. Default is 10.
        return Type switch
        {
            ObstacleType.BigObstacle => 50,
            ObstacleType.SmallObstacle => 10,
            ObstacleType.Hole => 60,
            _ => 10
        };
    }
    
    public int DetermineDamageAmount()
    {
        return Type switch
        {
            ObstacleType.Wall => -50,
            ObstacleType.BigObstacle => -20,
            ObstacleType.SmallObstacle => -10,
            ObstacleType.Hole => -100,
            ObstacleType.Rail => -30,
            _ => -20
        };
    }
}
