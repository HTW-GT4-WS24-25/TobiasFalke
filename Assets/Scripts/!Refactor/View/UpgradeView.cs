using UnityEngine;

public class UpgradeView : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (animator == null)
        {
            Debug.LogWarning("Animator not found on UpgradeView.");
        }
        
        if (spriteRenderer == null)
        {
            Debug.LogWarning("SpriteRenderer not found on UpgradeView.");
        }
    }

    /// <summary>
    /// Plays a collection animation or effect when the upgrade is collected by the player.
    /// </summary>
    public void PlayCollectAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Collect");
        }
    }

    /// <summary>
    /// Visual representation when the upgrade is active.
    /// </summary>
    public void SetActiveVisual(bool isActive)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isActive ? Color.white : Color.gray;
        }
    }

    /// <summary>
    /// Implement effects on removal, such as a fade-out or a sound effect.
    /// </summary>
    public void OnRemoveEffect()
    {
        // Implement removal effects or logic
    }
}