using System.Collections;
using Events;
using UnityEngine;

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
            EventManager.AddListener<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
            EventManager.AddListener<PlayerEvent.ObstacleCollision>(OnObstacleCollision); 
        }
    
        private void OnJumpAction()
        {
            // TODO: move shadow sprite dynamically with jump height
            playerShadowSprite.enabled = !playerShadowSprite.enabled;
        }
    
        private void OnTrickAction()
        {
            AudioManager.Instance.PlaySound("flip");
            playerAnimator.SetTrigger("Flip");
            playerShadowAnimator.SetTrigger("Flip");
        }
    
        private void OnSpecialAction(PlayerEvent.SpecialActionTriggered evt)
        {
            StartCoroutine(SpecialActionEffect(evt.SpecialActionDuration));
        }
    
        private IEnumerator SpecialActionEffect(float time)
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

        private void OnObstacleCollision(PlayerEvent.ObstacleCollision obj)
        {
            playerAnimator.SetBool("isInvincible", true);
        }
    
        private void OnDestroy()
        {
            UnsubscribeEvents();
        }

        private void UnsubscribeEvents()
        {
            EventManager.RemoveListener<PlayerEvent.SpecialActionTriggered>(OnSpecialAction);
            EventManager.RemoveListener<PlayerEvent.ObstacleCollision>(OnObstacleCollision);
        }
    }
}