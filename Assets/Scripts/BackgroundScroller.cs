using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    // TODO: Script verbessern
    public float scrollSpeed = 10.0f; // TODO: Scrollgeschwindigkeit von Game speed vom Game Manager holen
    public float backgroundHeight = 10.5f; // TODO: automatisch Höhe ermitteln oder ganz anders machen
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, backgroundHeight);
        transform.position = _startPosition + Vector3.down * newPosition;
    }
}