using Events;
using UnityEngine;

namespace Interfaces
{
    public abstract class FallingObject : MonoBehaviour
    {
        public float fallSpeed;

        protected virtual void Start()
        {
            EventManager.AddListener<LevelEvent.StageSpeedChanged>(OnStageSpeedChanged);
        }

        protected virtual void FixedUpdate()
        {
            HandleMovement();
        }
        
        protected void HandleMovement()
        {
            transform.Translate(Vector3.down * (fallSpeed * Time.deltaTime));
            if (transform.position.y <= -10) Destroy(gameObject);
        }
        
        protected virtual void OnStageSpeedChanged(LevelEvent.StageSpeedChanged evt)
        {
            fallSpeed = evt.StageSpeed;
        }
        
        protected virtual void OnDestroy()
        {
            EventManager.RemoveListener<LevelEvent.StageSpeedChanged>(OnStageSpeedChanged);
        }
    }
}