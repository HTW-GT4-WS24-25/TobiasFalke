using UnityEngine;

// TODO: overhaul class

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 10.0f;
    public float backgroundHeight = 10.5f;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, backgroundHeight);
        transform.position = _startPosition + Vector3.down * newPosition;
    }
}