using System;
using System.Collections;
using Events;
using UnityEngine;
using Utility;
using static Utility.GameConstants;

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
            EventManager.Add<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
            EventManager.Add<PlayerEvent.TrickActionTriggered>(OnTrickActionTriggered);
            EventManager.Add<PlayerEvent.ObstacleCollision>(OnObstacleCollision); 
            EventManager.Add<PlayerEvent.GameOverTriggered>(OnGameOverTriggered);
        }
    
        private void OnJumpAction()
        {
            playerShadowSprite.enabled = !playerShadowSprite.enabled;
        }
    
        private void OnTrickActionTriggered(PlayerEvent.TrickActionTriggered evt)
        {
            Debug.Log("Flip animation triggered");
            playerAnimator.SetTrigger(Animations.trickAction);
            playerShadowAnimator.SetTrigger(Animations.trickAction);
        }
    
        private void OnSpecialAction(PlayerEvent.SpecialActionTriggered evt)
        {
            StartCoroutine(FlashingEffect(evt.SpecialActionDuration, Color.blue));
        }
        
        private void OnObstacleCollision(PlayerEvent.ObstacleCollision evt)
        {
            StartCoroutine(FlashingEffect(2f, Color.yellow));
        }
        
        private IEnumerator FlashingEffect(float time, Color flashColor)
        {
            Color originalColor = playerSprite.color;
            float flashDuration = 0.1f;
            int flashCount = (int)(time * 10);

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
            EventManager.Remove<PlayerEvent.TrickActionTriggered>(OnTrickActionTriggered);
            EventManager.Remove<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
            EventManager.Remove<PlayerEvent.ObstacleCollision>(OnObstacleCollision);
            EventManager.Remove<PlayerEvent.GameOverTriggered>(OnGameOverTriggered);
        }
    }
}