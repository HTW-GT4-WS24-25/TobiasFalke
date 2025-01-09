using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{ 
    private PlayerStats stats; // Handles player's current health, special & score.
    private PlayerMovement movement; // Handles player's movement input & animation.
    private Animator _animator;
    
    
    private void Start()
    {
        
        stats = new PlayerStats(); // Is created with default values for each stat.
        movement = GetComponent<PlayerMovement>(); // Values specified in component attached within the player prefab.
        _animator = GetComponent<Animator>(); // Get the Animator component
        AudioManager.Instance.PlayTrack("mainSceneMusic");

        GameView.Instance.InitializeStatusBars(stats);
        EventManager.AddListener<ObstacleCollisionEvent>(OnDamageTaken);
        EventManager.AddListener<TrickEvent>(OnTrick);
    }

    private void OnTrick(TrickEvent evt)
    {
        stats.ChangeScore(evt.Points);
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

    private void OnCollision(ObstacleCollisionEvent evt){
        Debug.Log("Collision is triggered.");  
        // TODO implement: Collide from Obstacle 
        // Trigger collision Effect in Player Controller

    }

    private void OnDamageTaken(ObstacleCollisionEvent evt)
    {
        stats.ChangeHealth(evt.DamageValue);
        
        // Trigger game over screen if collision causes health to drop to 0.
        if (stats._health <= 0)
        {
            TriggerGameOver();
        }; 
    }
    
    /*
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision is triggered.");
        // Check whether collided object is power up or obstacle.
        var interactable = other.gameObject.GetComponent<IObject>();
        // Change player stats & movement according to collision effect.
        interactable?.Collide(other.gameObject, stats, movement, _animator);
        
        // Trigger game over screen if collision causes health to drop to 0.
        if (stats._health <= 0)
        {
            TriggerGameOver();
        }; 
    }*/

    private void OnTriggerExit2D(Collider2D other)
    {
        //TODO Add Rail exit code
        var interactable = other.gameObject.GetComponent<IObject>();
        if (interactable.GetType() != typeof(Obstacle)) return;
        var obstacle = interactable as Obstacle;
        obstacle?.ExitCollision(other.gameObject, movement);
        
        if (obstacle.obstacleType == ObstacleType.Rail)
        {
            movement.SetIsOverRail = false;
            Debug.Log("Player is no longer over rail!");
        }
    }

    private void TriggerGameOver()
    {
        EventManager.Broadcast(Events.PlayerDeathEvent);
        // Trigger death animation
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
}
