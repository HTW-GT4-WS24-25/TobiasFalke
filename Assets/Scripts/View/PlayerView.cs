using System;
using System.Collections;
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
        EventManager.AddListener<PlayerEvents.JumpEventR>(OnJumpAction);
        EventManager.AddListener<PlayerEvents.TrickActionEventR>(OnTrickAction);
        EventManager.AddListener<PlayerEvents.SpecialActionEventR>(OnSpecialAction);
        EventManager.AddListener<PlayerEvents.ObstacleCollisionEventR>(OnObstacleCollision);
        EventManager.AddListener<PlayerEvents.ObstacleCollisionExitEventR>(OnObstacleExit);
        EventManager.AddListener<PlayerEvents.PickupCollisionEventR>(OnPickupCollision);
    }
    
    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void UnregisterEvents()
    {
        EventManager.RemoveListener<PlayerEvents.JumpEventR>(OnJumpAction);
        EventManager.RemoveListener<PlayerEvents.TrickActionEventR>(OnTrickAction);
        EventManager.RemoveListener<PlayerEvents.SpecialActionEventR>(OnSpecialAction);
        EventManager.RemoveListener<PlayerEvents.ObstacleCollisionEventR>(OnObstacleCollision);
        EventManager.RemoveListener<PlayerEvents.ObstacleCollisionExitEventR>(OnObstacleExit);
        EventManager.RemoveListener<PlayerEvents.PickupCollisionEventR>(OnPickupCollision);
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
    
    private void OnJumpAction(PlayerEvents.JumpEventR obj)
    {
        float newHeight = obj.initialJumpHeight - playerSprite.bounds.extents.y;
        playerShadowSprite.enabled = !playerShadowSprite.enabled;
    }

    private void OnPickupCollision(PlayerEvents.PickupCollisionEventR obj)
    {
        throw new NotImplementedException();
    }

    private void OnObstacleExit(PlayerEvents.ObstacleCollisionExitEventR obj)
    {
        //
    }

    private void OnObstacleCollision(PlayerEvents.ObstacleCollisionEventR obj)
    {
        playerAnimator.SetBool("isInvincible", true);
    }

    private void OnTrickAction(PlayerEvents.TrickActionEventR obj)
    {
        AudioManager.Instance.PlaySound("flip");
        playerAnimator.SetTrigger("Flip");
        playerShadowAnimator.SetTrigger("Flip");
    }

    
    private void OnSpecialAction(PlayerEvents.SpecialActionEventR obj)
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