using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team04
{
    public class Player : MiniGameBehaviour
    {
        // Player specific variables
        public bool player2;
        Vector2 down = Vector2.left;
        Vector2 right = Vector2.down;

        // Input variables
        bool tryJump = false;
        bool tryInteract = false;
        bool releaseInteract = false;
        bool readyToPlace = false;
        float placeTime = 0.3f;
        float placeTimer = 0f;

        // Movement variables
        public float gravity = 9.8f;
        public float moveForce = 7f;
        public float jumpForce = 200f;
        public float jumpCooldown = 0.2f;
        float jumpTimer = 0f;
        public float landingCooldown = 0.01f;
        float landingTimer = 0f;
        bool canJump = true;
        int faceDirection = 1;
        public float directionChangeForce = 18f; // Boost when changing direction
        Vector2 nextMove;

        // Rocket handling variables
        Rocket heldRocket;
        Rocket pilotedRocket;
        Vector3 holdOffset;
        Vector3 placeOffset;

        // Physics and colliders
        Rigidbody2D rigidbody;
        CapsuleCollider2D physicsCollider;
        CircleCollider2D pickupCollider;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            physicsCollider = GetComponent<CapsuleCollider2D>();
            pickupCollider = GetComponent<CircleCollider2D>();
            if (player2)
            {
                down = Vector2.right;
                right = Vector2.up;
            }

            holdOffset = -down * physicsCollider.size.y;
            placeOffset = Vector2.up * physicsCollider.size.x;
        }

        // Update is called once per frame
        void Update()
        {
            // Handle input
            if (player2)
            {
                float moveInput = ArcadeInput.Player2.AxisX;
                nextMove = right * moveInput;
                if (ArcadeInput.Player2.Action1.Down) tryJump = true;
                if (ArcadeInput.Player2.Action2.Down) tryInteract = true;
                if (ArcadeInput.Player2.Action2.Released) releaseInteract = true;
            }
            else
            {
                float moveInput = ArcadeInput.Player1.AxisX;
                nextMove = right * moveInput;
                if (ArcadeInput.Player1.Action1.Down) tryJump = true;
                if (ArcadeInput.Player1.Action2.Down) tryInteract = true;
                if (ArcadeInput.Player1.Action2.Released) releaseInteract = true;
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
            Vector2 nextMoveForce = Vector2.zero;

            if (pilotedRocket != null)
            {
                pilotedRocket.rotationInput = nextMove.y;
            }
            else
            {
                nextMoveForce = moveForce * nextMove;
                faceDirection = GetFaceDirection();

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
            }

            rigidbody.AddForce(gravityForce + nextMoveForce);

            // Handle held rocket
            if (releaseInteract && heldRocket != null)
            {
                // Fire rocket if ready, otherwise reset the timer to prepare to place
                if (placeTimer <= 0)
                {
                    heldRocket.fired = true;
                    pilotedRocket = heldRocket;
                    heldRocket = null;
                    readyToPlace = false;
                    placeTimer = placeTime;
                }
                else
                {
                    readyToPlace = true;
                    placeTimer = placeTime;
                }
            }

            Vector2 heldRocketPosition = transform.position + holdOffset;

            // Handle interaction
            if (tryInteract)
            {
                if (heldRocket == null)
                {
                    TryPickup();
                    placeTimer = placeTime;
                }
                else if (readyToPlace)
                {
                    heldRocketPosition = transform.position + placeOffset * faceDirection;
                    placeTimer -= Time.fixedDeltaTime;
                }
            }

            if (heldRocket != null)
            {
                heldRocket.moveToPosition = heldRocketPosition;
            }

            // Reset input flags
            tryJump = false;
            tryInteract = false;
            releaseInteract = false;
        }

        bool Grounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, down, physicsCollider.size.y + 0.01f, LayerMask.GetMask("Ground"));
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

        void TryPickup()
        {
            List<Collider2D> nearbyColliders = new List<Collider2D>();
            pickupCollider.OverlapCollider(new ContactFilter2D().NoFilter(), nearbyColliders);
            foreach (Collider2D collider in nearbyColliders)
            {
                //Debug.Log("Found collider: " + collider.name);
                Rocket rocket = collider.GetComponent<Rocket>();
                if (rocket != null)
                {
                    heldRocket = rocket;
                    rocket.moveToPosition = transform.position + holdOffset;
                    break;
                }
            }
        }

        int GetFaceDirection()
        {
            int newfaceDirection = (int) Mathf.Sign(rigidbody.velocity.y);
            if (Mathf.Abs(rigidbody.velocity.y) < 1f)
            {
                newfaceDirection = faceDirection;
            }
            return newfaceDirection;
        }
    }
}
