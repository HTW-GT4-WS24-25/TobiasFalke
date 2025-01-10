using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Utilities;

public class PlayerController : MonoBehaviour
{ 
    private PlayerStats stats; // Handles player's current health, special & score.
    private PlayerMovement movement; // Handles player's movement input & animation.
    private Animator _animator;
    private static Dictionary<ItemType, IItemEffect> itemEffects;
    
    private void Start()
    {
        stats = new PlayerStats(); // Is created with default values for each stat.
        movement = GetComponent<PlayerMovement>(); // Values specified in component attached within the player prefab.
        _animator = GetComponent<Animator>(); // Get the Animator component
        AudioManager.Instance.PlayTrack("mainSceneMusic");

        itemEffects = new Dictionary<ItemType, IItemEffect>
        {
            { ItemType.HealthBoost, new HealthBoostEffect() },
            { ItemType.HealthBoom, new HealthBoomEffect() },
            { ItemType.SpecialBoost, new SpecialBoostEffect() },
            { ItemType.SpecialBoom, new SpecialBoomEffect() },
            { ItemType.SpeedBoost, new SpeedBoostEffect() },
            { ItemType.SpeedBoom, new SpeedBoomEffect() },
            { ItemType.JumpBoost, new JumpBoostEffect() },
            { ItemType.JumpBoom, new JumpBoomEffect() },
            { ItemType.ScoreBoost, new ScoreBoostEffect() },
            { ItemType.ScoreBoom, new ScoreBoomEffect() },
            { ItemType.ScoreMultiplierBoost, new ScoreMultiplierBoostEffect() }
        };

        GameView.Instance.InitializeStatusBars(stats);
        EventManager.AddListener<ObstacleCollisionEvent>(OnCollision);
        EventManager.AddListener<ObstacleCollisionExitEvent>(OnExitCollision);
        EventManager.AddListener<TrickEvent>(OnTrick);
        EventManager.AddListener<PickupEvent>(OnItemPickup);
    }

    private void OnDestroy()
    {
        // Remove all event listeners when the player is destroyed.
        EventManager.RemoveListener<ObstacleCollisionEvent>(OnCollision);
        EventManager.RemoveListener<ObstacleCollisionExitEvent>(OnExitCollision);
        EventManager.RemoveListener<TrickEvent>(OnTrick);
        EventManager.RemoveListener<PickupEvent>(OnItemPickup);
    }

    private void OnTrick(TrickEvent evt)
    {
        stats.ChangeScore(evt.Points);
    }
    private void OnItemPickup(PickupEvent evt){

        AudioManager.Instance.PlaySound("item");
        TriggerItemEffect(stats,evt.ItemType);
    }

    private static void TriggerItemEffect(PlayerStats playerStats, ItemType type)
    {
        if (itemEffects.TryGetValue(type, out IItemEffect effect))
        {
            effect.ApplyEffect(playerStats);
        }
    }

    private void FixedUpdate()
    {
        // Increase score continuously while game is active.
        UpdateScore();
    }

    private void Update()
    {
        // Trigger special action if conditions are met.
        if (Input.GetKeyDown("x") && stats._special == 100) TriggerSpecialAction();
        // Update animation.
        _animator.SetBool("isInvincible", stats.isInvincible);
    }

    private void LateUpdate()
    {
        // Update currently shown score in UI.
        GameView.Instance.UpdateScoreCounter(stats._score);
        // Show special action button when special bar is full.
        if (stats._special >= 100) GameView.Instance.ToggleSpecialActionButton(true);
    }

    private void UpdateScore()
    {
        // Increase score continuously while game is active.
       stats._score += Time.deltaTime * 5 * stats._scoreMultiplier * (GameModel.Instance.GameSpeed / 10);
        
        // Grinding bonus.
        if (movement.IsGrinding)
        {
            stats.ChangeScore(50);
        }
    }

    // TODO: Fix collision logic (refactoring broke it).
    private void OnCollision(ObstacleCollisionEvent evt){
        Debug.Log("Collision is triggered.");
        var obstacle = evt.Obstacle;
        if (movement.IsJumping && obstacle.IsJumpable)
        {
            var score = obstacle.DetermineScore();
            stats.ChangeScore(score);
        }
        if (obstacle.Type == ObstacleType.Rail)
        {
            movement.SetIsOverRail = true;
            Debug.Log("Player is over rail!");
        }
        if (!obstacle.IsJumpable || !movement.IsJumping)
        {
            TriggerCollisionEffect(obstacle);
        }
    }
    
    // TODO: Fix rail grinding bug where player keeps grinding (Happened after refactoring).
    private void OnExitCollision(ObstacleCollisionExitEvent evt)
    {
        var obstacle = evt.Obstacle;
        if (obstacle.Type == ObstacleType.Rail)
        {
            movement.SetIsOverRail = false;
            Debug.Log("Player is no longer over rail!");
        }
    }
    
    private void TriggerCollisionEffect(Obstacle obstacle)
    {
        if(stats.isInvincible) return;
        StartCoroutine(SetInvincibility());
        Debug.Log("Collision!");
        AudioManager.Instance.PlaySound("crash");
        var damage = obstacle.DetermineDamageAmount();
        stats.ChangeHealth(damage);
        if(stats._health <= 0)
        {
            TriggerGameOver();
        }
    }

    // TODO: Broken game over after refactoring.
    private void TriggerGameOver()
    {
        GameOverEvent evt = Events.GameOverEvent;
        evt.Score = stats._score;
        EventManager.Broadcast(evt);
        // Trigger death animation.
        _animator.SetBool("isDead", true);
    }
    
    private void TriggerSpecialAction()
    {
        // Trigger status changes of special action.
        stats.TriggerSpecialAction(); // TODO: make special action stat raise temporary (6 seconds (?))
        // Hide button for triggering special action.
        GameView.Instance.ToggleSpecialActionButton(false); 
        AudioManager.Instance.PlaySound("specialAction");
        StartCoroutine(FlashBlue(6.0f));
        GameView.Instance.PlayScreenFlash(6.0f);
    }
    
    // TODO: Animations broke after refactoring.
    
    // TODO: move this method to separate player animation
    private IEnumerator FlashBlue(float time)
    {
        SpriteRenderer playerSprite = movement._playerSprite; // remove once method was moved
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
    
    // TODO: Move to separate animation controller.
    private IEnumerator SetInvincibility()
    {
        stats.isInvincible = true;
        _animator.SetBool("isInvincible", true);
        Debug.Log("Player is invincible for " + stats.invincibilityDuration + " seconds.");
        yield return new WaitForSeconds(stats.invincibilityDuration);
        stats.isInvincible = false;
        _animator.SetBool("isInvincible", false);
        Debug.Log("Player is no longer invincible.");
    }
}