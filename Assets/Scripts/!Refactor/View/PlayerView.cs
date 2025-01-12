using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (animator == null) Debug.LogError("Animator component missing from PlayerView.");
        if (spriteRenderer == null) Debug.LogError("SpriteRenderer component missing from PlayerView.");
    }
    
    // sprite transformation
    
    public void UpdateDirection(float direction)
    {
        if (direction != 0) spriteRenderer.flipX = direction < 0;
    }
    
    // animation

    public void SetRunning(bool isRunning)
    {
        // animator.SetBool("isRunning", isRunning);
    }
    
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
    
    // visual effects
    
    public void OnHealthChanged()
    {
        // Visual feedback for health changes
    }


}