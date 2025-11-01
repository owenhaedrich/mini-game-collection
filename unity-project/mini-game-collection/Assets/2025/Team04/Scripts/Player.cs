using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team04
{
    public class Player : MiniGameBehaviour
    {
        public bool player2;
        Vector2 down = Vector2.right;
        Vector2 right = Vector2.up;
        bool tryJump = false;
        float gravity = 9.8f;
        float moveForce = 7f;
        float jumpForce = 200f;
        float jumpCooldown = 0.2f;
        float jumpTimer = 0f;
        float landingCooldown = 0.01f;
        float landingTimer = 0f;
        bool canJump = true;
        float directionChangeForce = 18f; // Boost when changing direction
        Vector2 nextMove;
        Rigidbody2D rigidbody;
        CircleCollider2D collider;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<CircleCollider2D>();
            if (player2)
            {
                down = Vector2.left;
                right = Vector2.down;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Handle input
            if (player2)
            {
                float moveInput = ArcadeInput.Player2.AxisX;
                nextMove = right * moveInput;
                tryJump = ArcadeInput.Player2.Action1.Down;
            }
            else
            {
                float moveInput = ArcadeInput.Player1.AxisX;
                nextMove = right * moveInput;
                tryJump = ArcadeInput.Player1.Action1.Down;
            }

            // Handle jump cooldown
            if (!canJump)
            {
                jumpTimer -= Time.deltaTime;
                if (jumpTimer <= 0f)
                {
                    canJump = true;
                }
            }
        }

        // FixedUpdate is called at a fixed interval - consistent physics
        void FixedUpdate()
        {
            Vector2 gravityForce = gravity * down;
            Vector2 nextMoveForce = moveForce * nextMove;

            // Apply extra force when changing direction
            if (Mathf.Sign(rigidbody.velocity.y) != Mathf.Sign(nextMove.y))
            {
                nextMoveForce += directionChangeForce * nextMove;
            }

            if (tryJump && canJump && Grounded())
            {
                nextMoveForce += jumpForce * -down;
                canJump = false;
                jumpTimer = jumpCooldown;
            }

            rigidbody.AddForce(gravityForce + nextMoveForce);
        }

        bool Grounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, down, collider.radius + 0.01f, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                landingTimer -= Time.fixedDeltaTime;
            }
            bool landed = landingTimer <= 0f && hit.collider != null;
            if (landed)
            {
                landingTimer = landingCooldown;
                rigidbody.velocity = new Vector2(0f, rigidbody.velocity.y);
            }
            return landed;
        }
    }
}
