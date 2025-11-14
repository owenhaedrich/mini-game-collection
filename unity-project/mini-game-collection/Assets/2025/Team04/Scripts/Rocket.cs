using System.Collections;
using System.Collections.Generic;
using MiniGameCollection;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team04
{
    public class Rocket : MiniGameBehaviour
    {
        public bool Player2;
        Vector2 Up = Vector2.right;

        public bool Fired = false;
        public Vector3 MoveToPosition;
        public float RotationInput = 0f;

        [Header("Use These to Tune Rocket")]
        public float FireTimer = 0.1f;
        float FuseTimer = 1f;
        float PlaceSpeed = 4f;
        float MaxDistanceFromMoveTo = 0.1f;
        public float RocketForce = 15f;
        public float TurnSpeed = 3f;
        public float RocketTurnSlowRate = 0.03f;
        public float GravityForce = 3f;


        Rigidbody2D Rigidbody;
        CircleCollider2D Collider;
        Transform CenterLine;

        // Start is called before the first frame update
        void Start()
        {
            if (Player2)
            {
                Up = Vector2.left;
            }
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<CircleCollider2D>();
            CenterLine = GameObject.Find("CenterLine").transform;
            MoveToPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if (Fired)
            {
                Collider.isTrigger = false;
                float distanceFromCenter = CenterLine.position.x - transform.position.x;
                Vector2 appliedGravity = Vector2.left * Mathf.Sign(distanceFromCenter) * GravityForce;
                Rigidbody.AddForce(appliedGravity);
                if (FireTimer > 0f)
                {
                    FireTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    //Control rocket with rotation input
                    float appliedRoatation = RotationInput * TurnSpeed;
                    if (Player2)
                    {
                        appliedRoatation = -appliedRoatation;
                    }
                    Up = Quaternion.Euler(0f, 0f, appliedRoatation) * Up;
                    Rigidbody.rotation = Rigidbody.rotation + appliedRoatation;

                    //Slow down rocket when turning
                    if (RotationInput != 0f)
                    {
                        Rigidbody.velocity *= 1 - RocketTurnSlowRate;
                    }
                    Rigidbody.AddForce(Up * RocketForce);

                    //Explode on impace after fuse timer
                    FuseTimer -= Time.fixedDeltaTime;
                    Collider2D[] hitColliders = new Collider2D[1];
                    ContactFilter2D noTriggers = new ContactFilter2D();
                    noTriggers.useTriggers = false;
                    Collider.OverlapCollider(noTriggers, hitColliders);
                    if (hitColliders[0] != null && FuseTimer <= 0)
                    {
                        Destroy(this.gameObject);

                        //Add score if you hit a player
                        Player potentialPlayer = hitColliders[0].gameObject.GetComponent<Player>();
                        if (potentialPlayer != null)
                        {
                            ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();
                            if (scoreManager == null) Debug.LogError("No score manager found.");
                                if (potentialPlayer.Player2)
                                {
                                    scoreManager.AddScore(2, 1);
                                }
                                else
                                {
                                    scoreManager.AddScore(1, 1);
                                }
                        }
                    }
                }
            }
            else
            {
                Rigidbody.transform.position = MoveToController(MoveToPosition, PlaceSpeed);
            }
        }

        Vector3 MoveToController(Vector3 targetPosition, float desiredSpeed)
        {
            Vector3 nextPosition = Vector3.MoveTowards(Rigidbody.transform.position, targetPosition, desiredSpeed * Time.fixedDeltaTime);
            float distanceToTarget = Vector3.Distance(nextPosition, targetPosition);
            if (distanceToTarget < MaxDistanceFromMoveTo)
            {
                nextPosition = Vector3.MoveTowards(Rigidbody.transform.position, targetPosition, distanceToTarget);
            }

            return nextPosition;
        }
    }
}
