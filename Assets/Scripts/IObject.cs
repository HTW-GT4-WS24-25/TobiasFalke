using UnityEngine;

public interface IObject
{
    void Collide(GameObject collidingObject, PlayerStats playerStats, PlayerMovement playerMovement);
}