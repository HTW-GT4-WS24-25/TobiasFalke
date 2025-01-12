using UnityEngine;

public class BackgroundScrollerR : MonoBehaviour
{
    public float scrollSpeed = 2f; // Speed at which the background scrolls
    public float tileSizeY = 10f;  // The height of the background tile

    private Vector3 startPosition;

    private void Start()
    {
        // Store the starting position of the background
        startPosition = transform.position;
    }

    private void Update()
    {
        // Calculate new position based on time, speed, and the tile size
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeY);
        transform.position = startPosition + Vector3.down * newPosition;
    }
}