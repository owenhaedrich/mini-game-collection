using System.Collections;
using System.Collections.Generic;
using MiniGameCollection;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team04
{
    public class Rocket : MiniGameBehaviour
    {
        public bool player2;
        Vector2 up = Vector2.right;

        public bool fired = false;
        public Vector3 moveToPosition;
        public float rotationInput = 0f;

        [Header("Use These to Tune Rocket")]
        public float fireTimer = 0.1f;
        float fuseTimer = 1f;
        float placeSpeed = 4f;
        float maxDistanceFromMoveTo = 0.1f;
        public float rocketForce = 15f;
        public float turnSpeed = 3f;
        public float rocketTurnSlowRate = 0.03f;
        public float gravityForce = 3f;


        Rigidbody2D rigidbody;
        CircleCollider2D collider;
        Transform centerLine;

        // Start is called before the first frame update
        void Start()
        {
            if (player2)
            {
                up = Vector2.left;
            }
            rigidbody = GetComponent<Rigidbody2D>();
            collider = GetComponent<CircleCollider2D>();
            centerLine = GameObject.Find("CenterLine").transform;
            moveToPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (fired)
            {
                collider.isTrigger = false;
                float distanceFromCenter = centerLine.position.x - transform.position.x;
                Vector2 appliedGravity = Vector2.left * Mathf.Sign(distanceFromCenter) * gravityForce;
                rigidbody.AddForce(appliedGravity);
                if (fireTimer > 0f)
                {
                    fireTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    //Control rocket with rotation input
                    float appliedRoatation = rotationInput * turnSpeed;
                    if (player2)
                    {
                        appliedRoatation = -appliedRoatation;
                    }
                    up = Quaternion.Euler(0f, 0f, appliedRoatation) * up;
                    rigidbody.rotation = rigidbody.rotation + appliedRoatation;

                    //Slow down rocket when turning
                    if (rotationInput != 0f)
                    {
                        rigidbody.velocity *= 1 - rocketTurnSlowRate;
                    }
                    rigidbody.AddForce(up * rocketForce);

                    //Explode on impace after fuse timer
                    fuseTimer -= Time.fixedDeltaTime;
                    Collider2D[] hitColliders = new Collider2D[1];
                    collider.OverlapCollider(new ContactFilter2D().NoFilter(), hitColliders);
                    if (hitColliders[0] != null && fuseTimer <= 0)
                    {
                        Destroy(this.gameObject);
                    }
                }
            }
            else
            {
                rigidbody.transform.position = MoveToController(moveToPosition, placeSpeed);
            }
        }

        Vector3 MoveToController(Vector3 targetPosition, float desiredSpeed)
        {
            Vector3 nextPosition = Vector3.MoveTowards(rigidbody.transform.position, targetPosition, desiredSpeed * Time.fixedDeltaTime);
            float distanceToTarget = Vector3.Distance(nextPosition, targetPosition);
            if (distanceToTarget < maxDistanceFromMoveTo)
            {
                nextPosition = Vector3.MoveTowards(rigidbody.transform.position, targetPosition, distanceToTarget);
            }

            return nextPosition;
        }
    }
}
