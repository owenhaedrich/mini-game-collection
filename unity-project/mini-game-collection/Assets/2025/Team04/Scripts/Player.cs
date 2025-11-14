using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team04
{
    public class Player : MiniGameBehaviour
    {
        // Player specific variables
        public bool Player2;
        public CircleCollider2D PlanetCenter;
        Vector2 Down = Vector2.left;
        Vector2 Right = Vector2.down;
        bool Started = false;

        // Input variables
        bool TryJump = false;
        bool TryInteract = false;
        bool ReleaseInteract = false;
        bool ReadyToPlace = false;
        float PlaceTime = 0.3f;
        float PlaceTimer = 0f;

        // Movement variables
        public float Gravity = 9.8f;
        public float MoveForce = 7f;
        public float JumpForce = 200f;
        public float JumpCooldown = 0.2f;
        float JumpTimer = 0f;
        public float LandingCooldown = 0.01f;
        float LandingTimer = 0f;
        bool CanJump = true;
        int FaceDirection = 1;
        public float DirectionChangeForce = 18f; // Boost when changing direction
        Vector2 NextMove;

        // Rocket handling variables
        Rocket HeldRocket;
        Rocket PilotedRocket;
        Vector3 HoldOffset;
        Vector3 PlaceOffset;

        // Physics and colliders
        Rigidbody2D Rigidbody;
        CapsuleCollider2D PhysicsCollider;
        CircleCollider2D PickupCollider;

        // Start is called before the first frame update
        void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            PhysicsCollider = GetComponent<CapsuleCollider2D>();
            PickupCollider = GetComponent<CircleCollider2D>();
            if (Player2)
            {
                Down = Vector2.right;
                Right = Vector2.up;
            }

            HoldOffset = -Down * PhysicsCollider.size.y;
            PlaceOffset = Vector2.up * PhysicsCollider.size.x;
        }

        protected override void OnGameStart()
        {
            Started = true;
        }

        protected override void OnGameEnd()
        {
            Started = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (!Started)
            {
                Rigidbody.velocity = Vector2.zero;
            }

            // Handle input
            if (Player2)
            {
                float moveInput = ArcadeInput.Player2.AxisX;
                NextMove = Right * moveInput;
                if (ArcadeInput.Player2.Action1.Down) TryJump = true;
                if (ArcadeInput.Player2.Action2.Down) TryInteract = true;
                if (ArcadeInput.Player2.Action2.Released) ReleaseInteract = true;
            }
            else
            {
                float moveInput = ArcadeInput.Player1.AxisX;
                NextMove = Right * moveInput;
                if (ArcadeInput.Player1.Action1.Down) TryJump = true;
                if (ArcadeInput.Player1.Action2.Down) TryInteract = true;
                if (ArcadeInput.Player1.Action2.Released) ReleaseInteract = true;
            }

            // Handle jump cooldown
            if (!CanJump)
            {
                JumpTimer -= Time.deltaTime;
                if (JumpTimer <= 0f)
                {
                    CanJump = true;
                }
            }
        }

        // FixedUpdate is called at a fixed interval - consistent physics
        void FixedUpdate()
        {
            Down = (PlanetCenter.bounds.center - transform.position).normalized;
            transform.rotation = Quaternion.FromToRotation(Vector2.left, Down) * Quaternion.Euler(0, 0, -90);

            Vector2 gravityForce = Gravity * Down;
            Vector2 nextMoveForce = Vector2.zero;

            if (PilotedRocket != null)
            {
                PilotedRocket.RotationInput = NextMove.y;
            }
            else
            {
                nextMoveForce = MoveForce * NextMove;
                FaceDirection = GetFaceDirection();

                // Apply extra force when changing direction
                if (Mathf.Sign(Rigidbody.velocity.y) != Mathf.Sign(NextMove.y))
                {
                    nextMoveForce += DirectionChangeForce * NextMove;
                }

                if (TryJump && CanJump && Grounded())
                {
                    nextMoveForce += JumpForce * -Down;
                    CanJump = false;
                    JumpTimer = JumpCooldown;
                }
            }

            Rigidbody.AddForce(gravityForce + nextMoveForce);

            // Handle held rocket
            if (ReleaseInteract && HeldRocket != null)
            {
                HeldRocket.transform.rotation = transform.rotation;

                // Fire rocket if ready, otherwise reset the timer to prepare to place
                if (PlaceTimer <= 0)
                {
                    HeldRocket.Fired = true;
                    PilotedRocket = HeldRocket;
                    HeldRocket = null;
                    ReadyToPlace = false;
                    PlaceTimer = PlaceTime;
                }
                else
                {
                    ReadyToPlace = true;
                    PlaceTimer = PlaceTime;
                }
            }

            Vector2 heldRocketPosition = transform.position + HoldOffset;

            // Handle interaction
            if (TryInteract)
            {
                if (HeldRocket == null)
                {
                    TryPickup();
                    PlaceTimer = PlaceTime;
                }
                else if (ReadyToPlace)
                {
                    heldRocketPosition = transform.position + PlaceOffset * FaceDirection;
                    PlaceTimer -= Time.fixedDeltaTime;
                }
            }

            if (HeldRocket != null)
            {
                HeldRocket.MoveToPosition = heldRocketPosition;
            }

            // Reset input flags
            TryJump = false;
            TryInteract = false;
            ReleaseInteract = false;
        }

        bool Grounded()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Down, PhysicsCollider.size.y + 0.01f, LayerMask.GetMask("Ground"));
            if (hit.collider != null)
            {
                LandingTimer -= Time.fixedDeltaTime;
            }
            bool landed = LandingTimer <= 0f && hit.collider != null;
            if (landed)
            {
                LandingTimer = LandingCooldown;
                Rigidbody.velocity = new Vector2(0f, Rigidbody.velocity.y);
            }
            return landed;
        }

        void TryPickup()
        {
            List<Collider2D> nearbyColliders = new List<Collider2D>();
            PickupCollider.OverlapCollider(new ContactFilter2D().NoFilter(), nearbyColliders);
            foreach (Collider2D collider in nearbyColliders)
            {
                //Debug.Log("Found collider: " + collider.name);
                Rocket rocket = collider.GetComponent<Rocket>();
                if (rocket != null)
                {
                    HeldRocket = rocket;
                    rocket.MoveToPosition = transform.position + HoldOffset;
                    break;
                }
            }
        }

        int GetFaceDirection()
        {
            int newfaceDirection = (int) Mathf.Sign(Rigidbody.velocity.y);
            if (Mathf.Abs(Rigidbody.velocity.y) < 1f)
            {
                newfaceDirection = FaceDirection;
            }
            return newfaceDirection;
        }
    }
}
