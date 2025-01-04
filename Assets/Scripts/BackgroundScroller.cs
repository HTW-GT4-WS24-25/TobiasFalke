using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 10.0f;
    public float backgroundHeight = 10.5f;
    public SpriteRenderer backGroundImage;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        backgroundHeight = backGroundImage.bounds.size.y;
    }

    private void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, backgroundHeight);
        transform.position = _startPosition + Vector3.down * newPosition;
    }
}