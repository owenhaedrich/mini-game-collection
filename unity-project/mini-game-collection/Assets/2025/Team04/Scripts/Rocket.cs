using System.Collections;
using System.Collections.Generic;
using MiniGameCollection;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team04
{
    public class Rocket : MiniGameBehaviour
    {
        public bool player2;
        Vector2 up = Vector2.left;

        public bool fired = false;
        float fireTimer = 0.3f;
        float fuseTimer = 1f;
        float moveToSpeed = 4f;
        float maxDistanceFromMoveTo = 0.1f;
        float rocketForce = 10f;
        float rocketTurnSlowRate = 0.03f;
        float gravityForce = 3f;
        public Vector3 moveToPosition;

        public float rotationInput = 0f;
        float rotationSpeed = 3f;


        Rigidbody2D rigidbody;
        CircleCollider2D collider;
        Transform centerLine;

        // Start is called before the first frame update
        void Start()
        {
            if (player2)
            {
                up = Vector2.right;
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
                    up = Quaternion.Euler(0f, 0f, rotationInput * rotationSpeed) * up;
                    rigidbody.rotation = rigidbody.rotation + rotationInput * rotationSpeed;

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
                rigidbody.transform.position = MoveToController(moveToPosition, moveToSpeed);
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
