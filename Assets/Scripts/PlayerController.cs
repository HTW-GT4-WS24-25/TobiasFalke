using UnityEngine;

public class PlayerController : MonoBehaviour
{ 
    private PlayerStats stats; // handles player's current health, special & score
    private PlayerMovement movement; // handles player's movement input & animation

    private void Start()
    {
        stats = new PlayerStats(); // is created with default values for each stat
        movement = GetComponent<PlayerMovement>(); // values specified in component attached within the player prefab
        // initialize player's max health & special bars on UI
        UIManager.Instance.SetMaxHealth(stats._maxHealth);
        UIManager.Instance.SetMaxSpecial(stats._maxSpecial);
        UIManager.Instance.UpdateHealthBar(stats._health);
        UIManager.Instance.UpdateSpecialBar(stats._special);
    }

    private void FixedUpdate()
    {
        stats._score += Time.deltaTime * 5 * stats._scoreMultiplier; // raise score continuously while game is active
    }

    private void Update()
    {
        // trigger special action if conditions are met
        if (Input.GetKeyDown("x") && stats._special == 100) TriggerSpecialAction(); 
    }

    private void LateUpdate()
    {
        // update currently shown score in UI
        UIManager.Instance.UpdateScoreCounter(stats._score);
        // show special action button when special bar is full
        if (stats._special >= 100) UIManager.Instance.ToggleSpecialActionButton(true);
    }
    
    private void TriggerSpecialAction()
    {
        // trigger effect of special action
        stats.SetHealth(100);
        stats.UpdateScoreMultiplier(100);
        stats.UpdateSpeedMultiplier(50);
        stats.UpdateJumpDuration(50);
        AudioManager.Instance.PlaySound("specialAction"); 
        // reset special points to 0
        stats._special = 0;
        UIManager.Instance.UpdateSpecialBar(0);
        // hide button for triggering special action
        UIManager.Instance.ToggleSpecialActionButton(false); 
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // check if collided object is a power up or an obstacle
        var interactable = other.gameObject.GetComponent<IObject>();
        // change player stats & movement according to collision effect
        interactable?.Collide(other.gameObject, stats, movement);
        // trigger game over screen if collision had health reach 0
        if (stats._health <= 0) TriggerGameOver(); 
    }
    
    private void TriggerGameOver()
    {       
        // TODO: connect to GameManager and trigger Game Over
        // TODO: trigger death animation (e.g. explosion, whatever)
        AudioManager.Instance.PlaySound("gameOver"); 
    }
}
