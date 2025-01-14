using UnityEngine;

public interface IObject
{
    void InitializeFallSpeed(float initialSpeed);
    void UpdateFallSpeed(float newSpeed);
    void MoveDownwards();
}