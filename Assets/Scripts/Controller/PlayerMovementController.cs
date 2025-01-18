using System.Collections;
using Model;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using static Utility.GameConstants;

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
            if (!playerModel.IsDoingGrindAction) ProcessMovement();
            if (playerModel.IsDoingJumpAction) ProcessJumpMovement();
            if (playerModel.IsDoingGrindAction) ProcessGrindMovement();
        }
        
        private void OnMove(InputValue inputValue)
        {
            movementInput = inputValue.Get<Vector2>();
        }
        
        private void ProcessMovement()
        {
            ClampMovementInputWithinLevelBounds();
            Vector2 movement = movementInput * (playerModel.Speed * playerModel.SpeedMultiplier * Time.fixedDeltaTime);
            rb2d.MovePosition(rb2d.position + movement);
        }

        private void ClampMovementInputWithinLevelBounds()
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
            playerModel.IsDoingJumpAction = true;
            AudioManager.Instance.StopBackgroundTrack();
            AudioManager.Instance.PlaySound(Audio.JumpActionSFX);
            timeSinceJump = 0;
            origJumpPos = transform.position.y;
        }
        
        private void ProcessJumpMovement()
        {
            timeSinceJump += Time.fixedDeltaTime;
            var progress = timeSinceJump / playerModel.JumpDuration;
            var verticalOffset = playerModel.JumpHeight * Mathf.Sin(Mathf.PI * progress);
            transform.position = new Vector3(transform.position.x, origJumpPos + verticalOffset, transform.position.z);
            if (!(progress >= 1)) return;
            ProcessLanding();
        }

        private void ProcessLanding()
        {
            transform.position = new Vector3(transform.position.x, origJumpPos, transform.position.z);
            playerModel.IsDoingJumpAction = false;
            playerModel.IsDoingTrickAction = false;
            if (playerModel.IsAboveRail)
            {
                playerModel.IsDoingGrindAction = true;
                AudioManager.Instance.PlaySound(Audio.RailLandingSFX);
                AudioManager.Instance.PlayBackgroundTrack(Audio.GrindBGS);
            }
            else
            {
                AudioManager.Instance.PlaySound(Audio.GroundLandingSFX);
                AudioManager.Instance.PlaySound(Audio.DriveBGS);
            }
        }

        private void ProcessGrindMovement()
        {
            // TODO: implement grind logic
            if (!playerModel.IsDoingJumpAction && playerModel.IsAboveRail) return;
            playerModel.IsDoingGrindAction = false;
            AudioManager.Instance.PlaySound(Audio.EndGrindSFX);
        }
        
        private void OnTrickAction()
        {
            if (!playerModel.IsDoingJumpAction) return;
            playerModel.IsDoingTrickAction = true;
            AudioManager.Instance.PlaySound(Audio.TrickActionSFX);
            playerModel.ScorePoints += playerModel.TrickActionScore;
            StartCoroutine(TrickActionActive(playerModel.TrickActionDuration));
        }
        
        private IEnumerator TrickActionActive(float duration)
        {
            yield return new WaitForSeconds(duration);
            playerModel.IsDoingTrickAction = false;
        }
    }
}