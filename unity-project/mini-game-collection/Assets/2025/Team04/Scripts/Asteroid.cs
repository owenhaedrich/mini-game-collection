using UnityEngine;

namespace MiniGameCollection.Games2025.Team04
{
    public class Asteroid : MiniGameBehaviour
    {
        public Vector2 Direction = Vector2.down;// default direction
        public float Speed = 3f;// default speed
        public float RotationSpeed = 0f;// rotation speed in degrees per second
        public float Lifetime = 12f;// default lifetime in seconds

        float Timer;

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
            transform.position += (Vector3)(Direction.normalized * Speed * Time.deltaTime); // move asteroid

            if (RotationSpeed != 0f)// if rotation speed set
                transform.Rotate(0f, 0f, RotationSpeed * Time.deltaTime);// rotate asteroid

            Timer += Time.deltaTime;// update lifetime timer
            if (Timer >= Lifetime) Destroy(gameObject);// destroy after lifetime
        }
    }
}
