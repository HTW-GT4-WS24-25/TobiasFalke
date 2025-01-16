using System;
using System.Collections;
using Events;
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
        private float jumpTime;
        private float initialJumpY;
        private bool isAboveRail;

        public void Initialize(PlayerModel model)
        {
            playerModel = model;
            rb2d = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            HandleInput();
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void HandleInput()
        {
            // if (Input.GetKeyDown("f")) EventManager.Broadcast(new PlayerEvent.TrickActionTriggered(1f));
        }

        private void UpdateMovement()
        {
            ProcessMovement();
            ProcessJump();
            HandleGrinding();
        }
        
        private void ProcessMovement()
        {
            float moveSpeed = playerModel.Speed * playerModel.SpeedMultiplier;
            playerModel.CurrentVelocity = new Vector2(movementInput.x * moveSpeed, movementInput.y * moveSpeed / 2);
            rb2d.linearVelocity = playerModel.CurrentVelocity;
        }

        private void ProcessJump()
        {
            if (!playerModel.IsDoingJumpAction) return;

            jumpTime += Time.fixedDeltaTime;
            var progress = jumpTime / playerModel.JumpDuration;
            var verticalOffset = playerModel.JumpHeight * Mathf.Sin(Mathf.PI * progress);
            transform.position = new Vector3(transform.position.x, initialJumpY + verticalOffset, transform.position.z);
            if (progress >= 1) ProcessLanding();
        }

        private void ProcessLanding()
        {
            AudioManager.Instance.PlaySound("land");
            if (isAboveRail) StartGrinding();
            playerModel.IsDoingJumpAction = false;
        }

        private void StartGrinding()
        {
            playerModel.IsDoingGrindAction = true;
            AudioManager.Instance.PlaySound("grinding");
        }

        private void HandleGrinding()
        {
            if (!playerModel.IsDoingGrindAction) return;
            if (playerModel.IsDoingJumpAction || !isAboveRail) FinishGrinding();
        }

        private void FinishGrinding()
        {
            playerModel.IsDoingGrindAction = false;
        }

        private void OnMove(InputValue inputValue)
        {
            if (!playerModel.IsDoingGrindAction) movementInput = inputValue.Get<Vector2>();
        }

        private void OnJump()
        {
            if (!playerModel.IsDoingJumpAction)
            {
                AudioManager.Instance.StopBackgroundTrack();
                AudioManager.Instance.PlaySound("jump");
                playerModel.IsDoingJumpAction = true;
                jumpTime = 0;
                initialJumpY = transform.position.y;
                float shadowSpriteHeight = initialJumpY;
                //EventManager.Broadcast(new PlayerEvent.JumpActionTriggered(shadowSpriteHeight));
            }
        }
    }
}