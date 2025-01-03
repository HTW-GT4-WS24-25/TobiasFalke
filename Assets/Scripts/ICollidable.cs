using UnityEngine;

public interface ICollidable
{
    void Collide(GameObject powerUp, PlayerStats playerStats, PlayerMovement playerMovement);
}