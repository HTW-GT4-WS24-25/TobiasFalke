using System.Collections;
using Config;
using Model;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    public class PlayerMovementController : MonoBehaviour
    {
        private PlayerModel playerModel;
        private Rigidbody2D rb2d;
        private Vector2 movementInput;
        private float timeSinceJump;
        private float origJumpPos;

        public void Initialize(PlayerModel model)
        {
            playerModel = model;
            rb2d = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            ProcessMovement();
            ProcessJumpMovement();
            ProcessGrindMovement();
            ProcessTrickAction();
        }
        
        private void OnMove(InputValue inputValue)
        {
            if (!playerModel.IsDoingGrindAction) movementInput = inputValue.Get<Vector2>();
        }
        
        private void ProcessMovement()
        {
            ClampMovementInputWithinBounds();
            Vector2 movement = movementInput * (playerModel.Speed * playerModel.SpeedMultiplier * Time.fixedDeltaTime);
            rb2d.MovePosition(rb2d.position + movement);
        }

        private void ClampMovementInputWithinBounds()
        {
            float halfWidth = GameConfig.BaseStageWidth / 2;
            float halfHeight = GameConfig.BaseStageHeight / 2;
            float playerX = transform.position.x;
            float playerY = transform.position.y;
            const float buffer = 0.5f;
            
            if (playerX <= -halfWidth + buffer) movementInput.x = Mathf.Max(0, movementInput.x);
            else if (playerX >= halfWidth - buffer) movementInput.x = Mathf.Min(0, movementInput.x);
            if (playerY <= -halfHeight + buffer) movementInput.y = Mathf.Max(0, movementInput.y);
            else if (playerY >= halfHeight - buffer) movementInput.y = Mathf.Min(0, movementInput.y);
        }
        private void OnJumpAction()
        {
            if (playerModel.IsDoingJumpAction) return;
            playerModel.IsDoingJumpAction = true;
            AudioManager.Instance.StopBackgroundTrack();
            AudioManager.Instance.PlaySound("jump");
            timeSinceJump = 0;
            origJumpPos = transform.position.y;
        }
        
        private void ProcessJumpMovement()
        {
            if (!playerModel.IsDoingJumpAction) return;
            timeSinceJump += Time.fixedDeltaTime;
            var progress = timeSinceJump / playerModel.JumpDuration;
            var verticalOffset = playerModel.JumpHeight * Mathf.Sin(Mathf.PI * progress);
            transform.position = new Vector3(transform.position.x, origJumpPos + verticalOffset, transform.position.z);
            if (!(progress >= 1)) return;
            ProcessLanding();
        }

        private void ProcessLanding()
        {
            playerModel.IsDoingJumpAction = false;
            playerModel.IsDoingTrickAction = false;
            transform.position = new Vector3(transform.position.x, origJumpPos, transform.position.z);
            if (playerModel.IsAboveRail)
            {
                Debug.Log("Grinding now!");
                playerModel.IsDoingGrindAction = true;
                AudioManager.Instance.PlaySound("grind");
            }
            else
            {
                AudioManager.Instance.PlaySound("land");
            }
        }

        private void ProcessGrindMovement()
        {
            if (!playerModel.IsDoingGrindAction) return;
            // TODO: implement grind logic
            if (playerModel.IsDoingJumpAction || !playerModel.IsAboveRail) playerModel.IsDoingGrindAction = false;
        }
        
        private void OnTrickAction()
        {
            if (playerModel.IsDoingTrickAction) return;
            AudioManager.Instance.StopBackgroundTrack();
            playerModel.ScorePoints += playerModel.TrickActionScore;
            playerModel.IsDoingTrickAction = true;
            StartCoroutine(TrickActionActive(playerModel.TrickActionDuration));
        }
        
        private IEnumerator TrickActionActive(float duration){
            yield return new WaitForSeconds(duration);
            playerModel.IsDoingTrickAction = false;
        }
        
        private void ProcessTrickAction()
        {
            if (!playerModel.IsDoingTrickAction || !playerModel.IsDoingJumpAction) return;
            // TODO: implement trick action logic
        }
    }
}