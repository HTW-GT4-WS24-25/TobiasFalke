using System.Collections.Generic;
using Config;
using Events;
using UnityEngine;

namespace View
{
    public class LevelView : MonoBehaviour
    {
        public List<Sprite> stageBackgrounds;
        private SpriteRenderer backgroundRenderer;
        private List<SpriteRenderer> activeBackgrounds;
        private float backgroundScrollSpeed;
        private Vector3 startPosition;
        private const float backgroundHeight = 10f;
        
        private void Awake()
        {
            backgroundRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            backgroundScrollSpeed = GameConfig.BaseStageSpeed;
            InitializeBackgrounds();
            EventManager.AddListener<LevelEvent.StageChanged>(OnStageChanged);
            EventManager.AddListener<LevelEvent.StageSpeedChanged>(OnStageSpeedChanged);
        }
        
        private void InitializeBackgrounds()
        {
            startPosition = transform.position;
            activeBackgrounds = new List<SpriteRenderer>();

            for (int i = 0; i < 2; i++)
            {
                GameObject backgroundObj = new GameObject("Background" + i);
                SpriteRenderer spriteRenderer = backgroundObj.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = backgroundRenderer.sprite;
                spriteRenderer.sortingLayerID = backgroundRenderer.sortingLayerID;
                spriteRenderer.sortingOrder = backgroundRenderer.sortingOrder;
                backgroundObj.transform.SetParent(transform);
                spriteRenderer.transform.localScale = ScaleBackgroundToCamera(spriteRenderer);
                float yPos = startPosition.y + i * backgroundHeight;
                backgroundObj.transform.position = new Vector3(startPosition.x, yPos, startPosition.z);
                activeBackgrounds.Add(spriteRenderer);
            }
        }
        
        private void Update()
        {
            ScrollBackgrounds();
        }
        
        // TODO: refactor to only work with one sprite instead of 2
        private void ScrollBackgrounds()
        {
            foreach (var bg in activeBackgrounds)
            {
                Vector3 pos = bg.transform.position;
                pos.y -= backgroundScrollSpeed * Time.deltaTime;
                bg.transform.position = pos;

                if (bg.transform.position.y < startPosition.y - backgroundHeight)
                {
                    bg.transform.position += new Vector3(0, 2 * backgroundHeight, 0);
                }
            }
        }
        
        private void OnStageSpeedChanged(LevelEvent.StageSpeedChanged evt)
        {
            backgroundScrollSpeed = evt.StageSpeed;
        }

        private void OnStageChanged(LevelEvent.StageChanged evt)
        {
            int stageId = evt.NewStage % stageBackgrounds.Count;
            foreach (var background in activeBackgrounds) background.sprite = stageBackgrounds[stageId];
        }

        private static Vector2 ScaleBackgroundToCamera(SpriteRenderer spriteRenderer)
        {
            Camera cam = Camera.main;
            float cameraHeight = cam.orthographicSize * 2;
            float cameraWidth = cameraHeight * cam.aspect;
            float spriteHeight = spriteRenderer.bounds.size.y;
            float spriteWidth = spriteRenderer.bounds.size.x;
            Vector3 newScale = spriteRenderer.transform.localScale;
            newScale.x *= (cameraWidth / spriteWidth);
            newScale.y *= (cameraHeight / spriteHeight);
            return newScale;
        }
        
        private void OnDestroy()
        {
            EventManager.RemoveListener<LevelEvent.StageSpeedChanged>(OnStageSpeedChanged);
            EventManager.RemoveListener<LevelEvent.StageChanged>(OnStageChanged);
        }
    }
}