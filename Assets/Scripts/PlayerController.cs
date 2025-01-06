using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{ 
    private PlayerStats stats; // Handles player's current health, special & score.
    private PlayerMovement movement; // Handles player's movement input & animation.
    private Animator animator; // Reference to Animator component
    
    private void Start()
    {
        AudioManager.Instance.PlayTrack("mainSceneMusic");
        
        stats = new PlayerStats(); // Is created with default values for each stat.
        movement = GetComponent<PlayerMovement>(); // Values specified in component attached within the player prefab.
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
    }

    private void LateUpdate()
    {
        // Update currently shown score in UI.
        UIManager.Instance.UpdateScoreCounter(stats._score);
        // Show special action button when special bar is full.
        if (stats._special >= 100) UIManager.Instance.ToggleSpecialActionButton(true);
    }
    
    private void TriggerSpecialAction()
    {
        // Trigger effect of special action.
        stats.SetHealth(100);
        stats.UpdateScoreMultiplier(100);
        stats.UpdateSpeedMultiplier(50);
        stats.UpdateJumpDuration(50);
        AudioManager.Instance.PlaySound("specialAction"); 
        
        // Reset special points to 0.
        stats._special = 0;
        UIManager.Instance.UpdateSpecialBar(0);
        
        // Hide button for triggering special action.
        UIManager.Instance.ToggleSpecialActionButton(false); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision is triggered.");
        // Check whether collided object is power up or obstacle.
        var interactable = other.gameObject.GetComponent<IObject>();
        // Change player stats & movement according to collision effect.
        interactable?.Collide(other.gameObject, stats, movement);
        // Trigger game over screen if collision causes health to drop to 0.
        if (stats._health <= 0) TriggerGameOver(); 
    }
    
    private void TriggerGameOver()
    {     
        // Disable movement in PlayerMovement script
        movement.disableMovement = true;

        // Stop the game from playing
        GameManager.Instance.isPlaying = false;

        // Trigger death animation
        animator.SetBool("isDead", true);
        AudioManager.Instance.PlaySound("gameOver"); 
        // Start coroutine to delay scene change
        StartCoroutine(DelayedGameOver());
    }

    private IEnumerator DelayedGameOver()
    {
        // Wait for the length of the death animation (e.g., 2 seconds)
        yield return new WaitForSeconds(2f); 

        // Change the scene after delay
        SceneLoader.Instance.LoadScene(SceneLoader.gameOver);
        AudioManager.Instance.PlayTrack("gameOverMusic");
    }
}
