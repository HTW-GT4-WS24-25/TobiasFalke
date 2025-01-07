using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private float scrollSpeed = 5.0f;
    public float backgroundHeight = 10f;

    [SerializeField] public List<Sprite> levelBackgrounds;
    public SpriteRenderer lowerBackground;
    public SpriteRenderer upperBackground;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        UpdateLevelBackground(0);
    }

    public void UpdateLevelBackground(int level)
    {
        int levelId = level % levelBackgrounds.Count;
        lowerBackground.sprite = levelBackgrounds[levelId];
        upperBackground.sprite = levelBackgrounds[levelId];
        ScaleBackgroundToCamera(lowerBackground);
        ScaleBackgroundToCamera(upperBackground);
        backgroundHeight = lowerBackground.bounds.size.y;
    }

    private void Update()
    {
        scrollSpeed = GameManager.Instance.gameSpeed / 2;
        transform.Translate(new Vector3(0f, -scrollSpeed * Time.deltaTime, 0f));

        if (transform.position.y <= _startPosition.y - backgroundHeight)
        {
            ScrollBack();
        }
    }

    private void ScrollBack()
    {
        // Reset to the start position
        transform.position = _startPosition;
    }

    private void ScaleBackgroundToCamera(SpriteRenderer spriteRenderer)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is not assigned.");
            return;
        }

        Camera cam = Camera.main;

        // Get the camera's height and width in world units
        float cameraHeight = cam.orthographicSize * 2;
        float cameraWidth = cameraHeight * cam.aspect;

        // Get sprite's original size in world units
        float spriteHeight = spriteRenderer.bounds.size.y;
        float spriteWidth = spriteRenderer.bounds.size.x;

        // Calculate scale, aiming to fit only vertically or horizontally, but not to stretch disproportionately
        Vector3 newScale = spriteRenderer.transform.localScale;

        // Match the smallest scale needed without cropping
        float heightScaleFactor = cameraHeight / spriteHeight;
        float widthScaleFactor = cameraWidth / spriteWidth;

        // Apply only what's necessary to fill at least one dimension
        newScale.x *= widthScaleFactor;
        newScale.y *= heightScaleFactor;

        spriteRenderer.transform.localScale = newScale;
    }
}