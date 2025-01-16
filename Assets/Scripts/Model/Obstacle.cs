using UnityEngine;
using Events;
using Interfaces;

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
            EventManager.Broadcast(new PlayerEvent.ObstacleCollision(gameObject));
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            EventManager.Broadcast(new PlayerEvent.ObstacleEvasion(gameObject));
        }

        public int DetermineScore()
        {
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
}