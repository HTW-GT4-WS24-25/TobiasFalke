using Events;
using UnityEngine;
using Utility;

namespace Model
{
    public abstract class FallingObject : MonoBehaviour
    {
        public float fallSpeed;

        protected virtual void Start()
        {
            EventManager.Add<LevelEvent.StageSpeedChanged>(OnStageSpeedChanged);
        }

        protected virtual void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            transform.Translate(Vector3.down * (fallSpeed * Time.deltaTime));
            if (transform.position.y <= -10) Destroy(gameObject);
        }

        private void OnStageSpeedChanged(LevelEvent.StageSpeedChanged evt)
        {
            fallSpeed = evt.StageSpeed;
        }
        
        protected virtual void OnDestroy()
        {
            EventManager.Remove<LevelEvent.StageSpeedChanged>(OnStageSpeedChanged);
        }
    }
}