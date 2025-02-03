using System;
using System.Collections;
using Events;
using UnityEngine;
using Utility;
using static Utility.GameConstants;
using Event = UnityEngine.Event;

namespace View
{
    public class PlayerView : MonoBehaviour
    {
        private SpriteRenderer playerSprite;
        public SpriteRenderer playerShadowSprite;
        private Animator playerAnimator;
        public Animator playerShadowAnimator;
    
        private float _shadowSpriteY;
        private float _initialShadowSpriteY;

        private void Awake()
        {
            playerSprite = GetComponent<SpriteRenderer>();
            playerAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            EventManager.Add<PlayerEvent.JumpActionTriggered>(OnJumpAction);
            EventManager.Add<PlayerEvent.GrindActionTriggered>(OnGrindAction);
            EventManager.Add<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
            EventManager.Add<PlayerEvent.TrickActionTriggered>(OnTrickActionTriggered);
            EventManager.Add<PlayerEvent.ObstacleCollision>(OnObstacleCollision); 
            EventManager.Add<PlayerEvent.InvincibilityTriggered>(OnInvincibilityTriggered); 
            EventManager.Add<PlayerEvent.GameOverTriggered>(OnGameOverTriggered);
        }
    
        private void OnJumpAction(PlayerEvent.JumpActionTriggered evt)
        {
            // TODO: fix shadow position transform to stay on the ground
        }
        
        private void OnGrindAction(PlayerEvent.GrindActionTriggered evt)
        {
            // TODO: trigger special effect for grinding
        }
        
        private void OnTrickActionTriggered(PlayerEvent.TrickActionTriggered evt)
        {
            playerAnimator.SetTrigger(Animations.trickAction);
            playerShadowAnimator.SetTrigger(Animations.trickAction);
        }
    
        private void OnSpecialAction(PlayerEvent.SpecialActionTriggered evt)
        {
            StartCoroutine(FlashingEffect(evt.SpecialActionDuration, Color.blue));
        }
        
        private void OnObstacleCollision(PlayerEvent.ObstacleCollision evt)
        {
        }

        private void OnInvincibilityTriggered(PlayerEvent.InvincibilityTriggered evt)
        {
            StartCoroutine(FlashingEffect(evt.InvincibilityDuration, Color.white));
        }
        
        private IEnumerator FlashingEffect(float time, Color flashColor)
        {
            Color originalColor = playerSprite.color;
            float flashDuration = 0.2f;
            int flashCount = (int)(time * 10);
            originalColor.a = 1.0f;
            flashColor.a = 0.5f;

            for (int i = 0; i < flashCount; i++)
            {
                playerSprite.color = flashColor;
                playerShadowSprite.color = flashColor;
                yield return new WaitForSeconds(flashDuration);
        
                playerSprite.color = originalColor;
                playerShadowSprite.color = originalColor;
                yield return new WaitForSeconds(flashDuration);
            }
        }

        private void OnGameOverTriggered(PlayerEvent.GameOverTriggered evt)
        {
            playerAnimator.SetBool(Animations.gameOver, true);
        }
    
        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            EventManager.Remove<PlayerEvent.JumpActionTriggered>(OnJumpAction);
            EventManager.Remove<PlayerEvent.GrindActionTriggered>(OnGrindAction);
            EventManager.Remove<PlayerEvent.TrickActionTriggered>(OnTrickActionTriggered);
            EventManager.Remove<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
            EventManager.Remove<PlayerEvent.ObstacleCollision>(OnObstacleCollision);
            EventManager.Remove<PlayerEvent.InvincibilityTriggered>(OnInvincibilityTriggered);
            EventManager.Remove<PlayerEvent.GameOverTriggered>(OnGameOverTriggered);
        }
    }
}