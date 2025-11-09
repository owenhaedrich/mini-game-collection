using UnityEngine;

namespace MiniGameCollection.Games2025.Team04
{
    public class Asteroid : MiniGameBehaviour
    {
        public Vector2 direction = Vector2.down;// default direction
        public float speed = 3f;// default speed
        public float rotationSpeed = 0f;// rotation speed in degrees per second
        public float lifetime = 12f;// default lifetime in seconds

        float t;

        void Awake()
        {
            
            if (TryGetComponent<Rigidbody2D>(out var rb))// if RB exists
            {
                rb.bodyType = RigidbodyType2D.Kinematic; // set to kinematic
                rb.gravityScale = 0f; // no gravity
                rb.velocity = Vector2.zero; // no velocity  
                rb.angularVelocity = 0f; // no angular velocity
            }
        }

        void Update()
        {
            transform.position += (Vector3)(direction.normalized * speed * Time.deltaTime); // move asteroid

            if (rotationSpeed != 0f)// if rotation speed set
                transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);// rotate asteroid

            t += Time.deltaTime;// update lifetime timer
            if (t >= lifetime) Destroy(gameObject);// destroy after lifetime
        }
    }
}
