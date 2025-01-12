using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        // Retrieve the components needed for visual representation
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Optionally add checks to ensure components are correctly assigned
        if (animator == null)
        {
            Debug.LogError("Animator component missing from PlayerView.");
        }
        
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component missing from PlayerView.");
        }
    }

    /// <summary>
    /// Called to update the animation state.
    /// </summary>
    /// <param name="isRunning">True if the player is running, false otherwise.</param>
    public void SetRunning(bool isRunning)
    {
        animator.SetBool("isRunning", isRunning);
    }

    /// <summary>
    /// Updates the player's visual orientation based on direction.
    /// </summary>
    /// <param name="direction">The horizontal direction of movement.</param>
    public void UpdateDirection(float direction)
    {
        if (direction != 0)
        {
            spriteRenderer.flipX = direction < 0;
        }
    }

    /// <summary>
    /// Updates the visual feedback when player's health changes.
    /// </summary>
    public void OnHealthChanged()
    {
        // Implement visual feedback for health changes, such as a flash effect or color change
    }

    /// <summary>
    /// Play a specific animation based on external triggers, such as a jump or attack.
    /// </summary>
    /// <param name="animationName">Name of the animation to play.</param>
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}