using UnityEngine;

public class ObstacleView : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (animator == null)
        {
            Debug.LogWarning("Animator not found on ObstacleView.");
        }
        
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer not found on ObstacleView.");
        }
    }

    /// <summary>
    /// Plays a hit animation or effect when the obstacle collides with the player.
    /// </summary>
    public void PlayHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
    }

    /// <summary>
    /// Changes the color or effect when an obstacle is activated or deactivated.
    /// </summary>
    public void SetActiveVisual(bool isActive)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isActive ? Color.white : Color.gray;
        }
    }

    /// <summary>
    /// Call this when the obstacle is destroyed or removed.
    /// </summary>
    public void OnDestroyEffect()
    {
        // Implement any destruction effects, such as particle effects or sounds
    }
}