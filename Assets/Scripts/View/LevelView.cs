using System.Collections.Generic;
using Events;
using UnityEngine;
using Utility;

namespace View
{
    public class LevelView : MonoBehaviour
    {
        public List<Sprite> stageBackgrounds;
        private SpriteRenderer backgroundRenderer;
        private List<SpriteRenderer> activeBackgrounds;
        private float backgroundScrollSpeed;
        private Vector3 startPosition;
        
        private void Awake()
        {
            backgroundRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            backgroundScrollSpeed = GameConfig.Instance.BaseStageSpeed;
            InitializeBackgrounds();
            EventManager.Add<LevelEvent.StageChanged>(OnStageChanged);
            EventManager.Add<LevelEvent.StageSpeedChanged>(OnStageSpeedChanged);
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
                spriteRenderer.transform.localScale = ScaleBackground(spriteRenderer);
                float yPos = startPosition.y + i * GameConfig.Instance.BaseStageHeight;
                backgroundObj.transform.position = new Vector3(startPosition.x, yPos, startPosition.z);
                activeBackgrounds.Add(spriteRenderer);
            }

            backgroundRenderer.sprite = null;
        }
        
        private void Update()
        {
            ScrollBackgrounds();
        }
        
        private void ScrollBackgrounds()
        {
            foreach (var bg in activeBackgrounds)
            {
                Vector3 pos = bg.transform.position;
                pos.y -= backgroundScrollSpeed * Time.deltaTime;
                bg.transform.position = pos;

                if (bg.transform.position.y < startPosition.y - GameConfig.Instance.BaseStageHeight)
                {
                    bg.transform.position += new Vector3(0, 2 * GameConfig.Instance.BaseStageHeight, 0);
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

        private Vector3 ScaleBackground(SpriteRenderer spriteRenderer)
        {
            float stageWidth = GameConfig.Instance.BaseStageWidth;
            float stageHeight = GameConfig.Instance.BaseStageHeight;
            float spriteWidth = spriteRenderer.sprite.bounds.size.x;
            float spriteHeight = spriteRenderer.sprite.bounds.size.y;

            float scaleX = stageWidth / spriteWidth;
            float scaleY = stageHeight / spriteHeight;

            return new Vector3(scaleX, scaleY, 1f);
        }

        
        private void OnDestroy()
        {
            EventManager.Remove<LevelEvent.StageSpeedChanged>(OnStageSpeedChanged);
            EventManager.Remove<LevelEvent.StageChanged>(OnStageChanged);
        }
    }
}