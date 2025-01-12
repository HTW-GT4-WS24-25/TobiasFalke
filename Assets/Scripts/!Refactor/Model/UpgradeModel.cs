using UnityEngine;

public class UpgradeModel : MonoBehaviour, ICollectable
{
    public int scoreBoost = 10;

    // Implementing the ICollectible interface
    public void OnCollect(PlayerModel player)
    {
        // Increase the player's score by the scoreBoost value
        player.IncreaseScore(scoreBoost);
        Debug.Log("Player collected an upgrade!");
        Destroy(gameObject); // Destroy upgrade after collection
    }
}