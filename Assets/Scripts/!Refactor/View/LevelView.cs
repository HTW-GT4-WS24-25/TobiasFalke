using System.Collections.Generic;
using UnityEngine;

public class LevelView : MonoBehaviour
{
    public List<Sprite> levelBackgrounds;
    private List<SpriteRenderer> activeBackgrounds;
    private Vector3 startPosition;
    private SpriteRenderer originalBackground;
    private float backgroundHeight = 10f;
    private float currentScrollSpeed = 2f;

    private void Awake()
    {
        originalBackground = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InitializeBackgrounds();
        EventManagerR.AddListener<LevelEvents.StageChangedEventR>(OnLevelChanged);
        EventManagerR.AddListener<LevelEvents.StageSpeedChangedEventR>(OnLevelSpeedChanged);
    }

    private void OnDestroy()
    {
        EventManagerR.RemoveListener<LevelEvents.StageSpeedChangedEventR>(OnLevelSpeedChanged);
        EventManagerR.RemoveListener<LevelEvents.StageChangedEventR>(OnLevelChanged);
    }

    private void Update()
    {
        ScrollBackgrounds();
    }

    private void InitializeBackgrounds()
    {
        startPosition = transform.position;
        activeBackgrounds = new List<SpriteRenderer>();

        for (int i = 0; i < 2; i++)
        {
            GameObject backgroundObj = new GameObject("Background" + i);
            SpriteRenderer spriteRenderer = backgroundObj.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = originalBackground.sprite;
            spriteRenderer.sortingLayerID = originalBackground.sortingLayerID;
            spriteRenderer.sortingOrder = originalBackground.sortingOrder;
            backgroundObj.transform.SetParent(transform);

            ScaleBackgroundToCamera(spriteRenderer);

            float yPos = startPosition.y + i * backgroundHeight;
            backgroundObj.transform.position = new Vector3(startPosition.x, yPos, startPosition.z);

            activeBackgrounds.Add(spriteRenderer);
        }
    }

    private void ScrollBackgrounds()
    {
        foreach (var bg in activeBackgrounds)
        {
            Vector3 pos = bg.transform.position;
            pos.y -= currentScrollSpeed * Time.deltaTime;
            bg.transform.position = pos;

            if (bg.transform.position.y < startPosition.y - backgroundHeight)
            {
                bg.transform.position += new Vector3(0, 2 * backgroundHeight, 0);
            }
        }
    }

    private void OnLevelSpeedChanged(LevelEvents.StageSpeedChangedEventR evt)
    {
        currentScrollSpeed = evt.StageSpeed;
    }

    private void OnLevelChanged(LevelEvents.StageChangedEventR evt)
    {
        UpdateLevelBackground(evt.NewStage);
    }

    private void UpdateLevelBackground(int level)
    {
        int levelId = level % levelBackgrounds.Count;
        foreach (var bg in activeBackgrounds)
        {
            bg.sprite = levelBackgrounds[levelId];
        }
    }

    private void ScaleBackgroundToCamera(SpriteRenderer spriteRenderer)
    {
        Camera cam = Camera.main;
        float cameraHeight = cam.orthographicSize * 2;
        float cameraWidth = cameraHeight * cam.aspect;
        float spriteHeight = spriteRenderer.bounds.size.y;
        float spriteWidth = spriteRenderer.bounds.size.x;
        Vector3 newScale = spriteRenderer.transform.localScale;
        float heightScaleFactor = cameraHeight / spriteHeight;
        float widthScaleFactor = cameraWidth / spriteWidth;
        newScale.x *= widthScaleFactor;
        newScale.y *= heightScaleFactor;
        spriteRenderer.transform.localScale = newScale;
    }
}