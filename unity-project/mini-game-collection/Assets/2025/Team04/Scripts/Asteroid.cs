using UnityEngine;

namespace MiniGameCollection.Games2024.Team04
{
    public class Asteroid : MonoBehaviour
    {
        bool clampSpeed;// Whether to clamp speed
        Vector2 fixedDir = Vector2.up;// Fixed movement direction
        float targetSpeed = 3f;// Target speed

        Camera cam;
        BoxCollider2D box;// Optional box area for despawning
        float margin = 1f;// Margin for despawning
        float lifetime = 20f;// Lifetime in seconds
        float startTime;// Time when enabled
        Rigidbody2D rb;// Rigidbody2D reference

        public void Setup(
            bool clampSpeed,
            Vector2 fixedDirection,
            float targetSpeed,
            Camera despawnWithCamera,
            BoxCollider2D boxArea,
            float margin,
            float lifetimeSeconds)// Setup parameters
        {
            this.clampSpeed = clampSpeed;// Set clamp speed
            this.fixedDir = fixedDirection.normalized;// Set fixed direction
            this.targetSpeed = targetSpeed;// Set target speed
            this.cam = despawnWithCamera;// Set camera for despawning
            this.box = boxArea;// Set box area
            this.margin = margin;// Set margin
            this.lifetime = lifetimeSeconds;// Set lifetime
        }

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();// Get Rigidbody2D component
        }

        void OnEnable()// Called when enabled
        {
            startTime = Time.time;// Record start time
            if (rb == null) rb = GetComponent<Rigidbody2D>();// Get Rigidbody2D component
        }

        void FixedUpdate()// FixedUpdate for physics
        {
            if (clampSpeed && rb != null)// Clamp speed if enabled
                rb.velocity = fixedDir * targetSpeed;// Set velocity
        }

        void Update()
        {
            if (Time.time - startTime > lifetime)// Check lifetime
            {
                Destroy(gameObject);// Destroy if exceeded
                return; // Exit
            }

            if (box != null)// Despawn with box area
            {
                Bounds b = box.bounds;// Get bounds
                Vector3 p = transform.position;// Get position
                if (p.y > b.max.y + margin || p.y < b.min.y - margin || p.x < b.min.x - margin || p.x > b.max.x + margin)// Check bounds
                    Destroy(gameObject);// Destroy if outside
                return;// Exit
            }

            if (cam != null && cam.orthographic)// Despawn with camera bounds
            {
                float halfH = cam.orthographicSize;// Vertical half-size
                float halfW = halfH * cam.aspect;// Horizontal half-size
                Vector3 c = cam.transform.position;// Camera center
                Vector3 p = transform.position;// Get position

                if (p.y > c.y + halfH + margin || p.y < c.y - halfH - margin || p.x < c.x - halfW - margin || p.x > c.x + halfW + margin)// Check bounds
                    Destroy(gameObject);// Destroy if outside
            }
        }
    }
}
