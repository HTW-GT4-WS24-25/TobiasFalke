using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{ 
    private PlayerStats stats; // Handles player's current health, special & score.
    private PlayerMovement movement; // Handles player's movement input & animation.
    private Animator animator; // Reference to Animator component
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    private void Start()
    {
        stats = new PlayerStats(); // Is created with default values for each stat.
        movement = GetComponent<PlayerMovement>(); // Values specified in component attached within the player prefab.
        UIManager.Instance.InitializeStatusBars(stats);
        AudioManager.Instance.PlayTrack("mainSceneMusic");

        animator = GetComponent<Animator>(); // Get the Animator component
        
        // Initialize player's max health & special bars on UI.
        UIManager.Instance.SetMaxHealth(stats._maxHealth);
        UIManager.Instance.SetMaxSpecial(stats._maxSpecial);
        UIManager.Instance.UpdateHealthBar(stats._health);
        UIManager.Instance.UpdateSpecialBar(stats._special);
    }

    private void FixedUpdate()
    {
        // Increase score continuously while game is active.
        stats._score += Time.deltaTime * 5 * stats._scoreMultiplier;
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
        UIManager.Instance.UpdateScoreCounter(stats._score);
        // Show special action button when special bar is full.
        if (stats._special >= 100) UIManager.Instance.ToggleSpecialActionButton(true);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision is triggered.");
        // Check whether collided object is power up or obstacle.
        var interactable = other.gameObject.GetComponent<IObject>();
        // Change player stats & movement according to collision effect.
        interactable?.Collide(other.gameObject, stats, movement, _animator);
        // Trigger game over screen if collision causes health to drop to 0.
        if (stats._health <= 0) TriggerGameOver(); 
    }
    
    private void TriggerSpecialAction()
    {
        // Trigger status changes of special action.
        stats.TriggerSpecialAction(); // TODO: make special action stat raise temporary (6 seconds (?))
        // Hide button for triggering special action.
        UIManager.Instance.ToggleSpecialActionButton(false); 
        AudioManager.Instance.PlaySound("specialAction");
        StartCoroutine(FlashBlue(6.0f));
        UIManager.Instance.PlayScreenFlash(6.0f);
    }
    
    // TODO: move this method to separate player animation
    private IEnumerator FlashBlue(float time)
    {
        SpriteRenderer playerSprite = movement._playerSprite; // remove once method was moved
        Color originalColor = playerSprite.color;
        Color flashColor = Color.blue;
        
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
    
    private void TriggerGameOver()
    {     
        // Stop the game from playing
        GameManager.Instance.isPlaying = false;

        // Trigger death animation
        animator.SetBool("isDead", true);
        AudioManager.Instance.PlaySound("gameOver"); 
        // Start coroutine to delay scene change
        Time.timeScale = 0f;
        StartCoroutine(DelayedGameOver());
    }

    private IEnumerator DelayedGameOver()
    {
        // Wait for the length of the death animation (e.g., 2 seconds)
        yield return new WaitForSecondsRealtime(2f); 

        // Change the scene after delay
        SceneLoader.Instance.LoadScene(SceneLoader.gameOver);
        AudioManager.Instance.PlayTrack("gameOverMusic");
    }
}
