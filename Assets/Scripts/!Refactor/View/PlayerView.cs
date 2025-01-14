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
        EventManagerR.AddListener<PlayerEvents.JumpEvent>(OnJumpAction);
        EventManagerR.AddListener<PlayerEvents.TrickActionEvent>(OnTrickAction);
        EventManagerR.AddListener<PlayerEvents.SpecialActionEvent>(OnSpecialAction);
        EventManagerR.AddListener<PlayerEvents.ObstacleCollisionEvent>(OnObstacleCollision);
        EventManagerR.AddListener<PlayerEvents.ObstacleCollisionExitEvent>(OnObstacleExit);
        EventManagerR.AddListener<PlayerEvents.PickupCollisionEvent>(OnPickupCollision);
    }
    
    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void UnregisterEvents()
    {
        EventManagerR.RemoveListener<PlayerEvents.JumpEvent>(OnJumpAction);
        EventManagerR.RemoveListener<PlayerEvents.TrickActionEvent>(OnTrickAction);
        EventManagerR.RemoveListener<PlayerEvents.SpecialActionEvent>(OnSpecialAction);
        EventManagerR.RemoveListener<PlayerEvents.ObstacleCollisionEvent>(OnObstacleCollision);
        EventManagerR.RemoveListener<PlayerEvents.ObstacleCollisionExitEvent>(OnObstacleExit);
        EventManagerR.RemoveListener<PlayerEvents.PickupCollisionEvent>(OnPickupCollision);
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
    
    private void OnJumpAction(PlayerEvents.JumpEvent obj)
    {
        float newHeight = obj.initialJumpHeight - playerSprite.bounds.extents.y;
        playerShadowSprite.enabled = !playerShadowSprite.enabled;
    }

    private void OnPickupCollision(PlayerEvents.PickupCollisionEvent obj)
    {
        throw new NotImplementedException();
    }

    private void OnObstacleExit(PlayerEvents.ObstacleCollisionExitEvent obj)
    {
        throw new NotImplementedException();
    }

    private void OnObstacleCollision(PlayerEvents.ObstacleCollisionEvent obj)
    {
        playerAnimator.SetBool("isInvincible", true);
    }

    private void OnTrickAction(PlayerEvents.TrickActionEvent obj)
    {
        AudioManager.Instance.PlaySound("flip");
        playerAnimator.SetTrigger("Flip");
        playerShadowAnimator.SetTrigger("Flip");
    }

    
    private void OnSpecialAction(PlayerEvents.SpecialActionEvent obj)
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