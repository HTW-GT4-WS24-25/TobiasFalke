using UnityEngine;
using Events;
using Utility;

namespace Model
{
    public enum ObstacleType
    {
        Wall,
        BigObstacle,
        SmallObstacle,
        Rail,
        Hole
    }

    public class Obstacle : FallingObject
    {
        [SerializeField] internal ObstacleType Type;
        
        public bool canJumpOver => Type != ObstacleType.Wall;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            EventManager.Trigger(new PlayerEvent.ObstacleCollision(gameObject));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            EventManager.Trigger(new PlayerEvent.ObstacleEvasion(gameObject));
        }

        public int DetermineScore()
        {
            return Type switch
            {
                ObstacleType.BigObstacle => 200,
                ObstacleType.SmallObstacle => 100,
                ObstacleType.Hole => 100,
                _ => 100
            };
        }

        public int DetermineDamageAmount()
        {
            return Type switch
            {
                ObstacleType.Wall => 50,
                ObstacleType.BigObstacle => 50,
                ObstacleType.SmallObstacle => 25,
                ObstacleType.Hole => 25,
                ObstacleType.Rail => 25,
                _ => -25
            };
        }
    }
}