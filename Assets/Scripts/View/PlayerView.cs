using System;
using System.Collections;
using Events;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    private SpriteRenderer playerSprite;
    public SpriteRenderer playerShadowSprite;
    private Animator playerAnimator;
    public Animator playerShadowAnimator;
    
    private float _shadowSpriteY;
    private float _initialShadowSpriteY;

    void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        RegisterEvents();
    }
    
    private void RegisterEvents()
    {
        EventManager.AddListener<PlayerEvent.JumpTriggered>(OnJumpAction);
        EventManager.AddListener<PlayerEvent.TrickActionTriggered>(OnTrickAction);
        EventManager.AddListener<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
        EventManager.AddListener<PlayerEvent.ObstacleCollision>(OnObstacleCollision);
        EventManager.AddListener<PlayerEvent.ObstacleEvasion>(OnObstacleExit);
        EventManager.AddListener<PlayerEvent.PickupCollision>(OnPickupCollision);
    }
    
    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void UnregisterEvents()
    {
        EventManager.RemoveListener<PlayerEvent.JumpTriggered>(OnJumpAction);
        EventManager.RemoveListener<PlayerEvent.TrickActionTriggered>(OnTrickAction);
        EventManager.RemoveListener<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
        EventManager.RemoveListener<PlayerEvent.ObstacleCollision>(OnObstacleCollision);
        EventManager.RemoveListener<PlayerEvent.ObstacleEvasion>(OnObstacleExit);
        EventManager.RemoveListener<PlayerEvent.PickupCollision>(OnPickupCollision);
    }
    
    // sprite transformation
    
    public void UpdateDirection(float direction)
    {
        if (direction != 0) playerSprite.flipX = direction < 0;
    }
    
    // animation
    public void SetRunning(bool isRunning)
    {
        playerAnimator.SetBool("isRunning", isRunning);
    }
    
    public void PlayAnimation(string animationName)
    {
        playerAnimator.Play(animationName);
    }
    
    private void OnJumpAction(PlayerEvent.JumpTriggered obj)
    {
        float newHeight = obj.initialJumpHeight - playerSprite.bounds.extents.y;
        playerShadowSprite.enabled = !playerShadowSprite.enabled;
    }

    private void OnPickupCollision(PlayerEvent.PickupCollision obj)
    {
        // TODO: make evasion sound
        // TODO: make visual effect on player
    }

    private void OnObstacleExit(PlayerEvent.ObstacleEvasion obj)
    {
        // TODO: make evasion sound
    }

    private void OnObstacleCollision(PlayerEvent.ObstacleCollision obj)
    {
        playerAnimator.SetBool("isInvincible", true);
    }

    private void OnTrickAction(PlayerEvent.TrickActionTriggered obj)
    {
        AudioManager.Instance.PlaySound("flip");
        playerAnimator.SetTrigger("Flip");
        playerShadowAnimator.SetTrigger("Flip");
    }

    
    private void OnSpecialAction(PlayerEvent.SpecialActionTriggered obj)
    {
        StartCoroutine(FlashBlue(6.0f));
    }
    
    private IEnumerator FlashBlue(float time)
    {
        Color originalColor = playerSprite.color;
        Color flashColor = Color.cyan;
        
        float flashDuration = 0.1f;
        int flashCount = (int)(time * 10);

        for (int i = 0; i < flashCount; i++)
        {
            playerSprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            playerSprite.color = originalColor;
            yield return new WaitForSeconds(flashDuration);
        }
    }
}