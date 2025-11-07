using System.Collections;
using UnityEngine;

namespace MiniGameCollection.Games2024.Team04
{
    public class AsteroidSpawner : MonoBehaviour
    {
        public enum SpawnMode { CameraBounds, BoxArea } // CameraBounds requires a main camera in the scene
        public enum EdgeStyle { EdgeLine, BandInside } // EdgeLine places exactly on edge + inset, BandInside places within a band inside the edge

        [Header("Prefab")]// Asteroid prefab to spawn
        public GameObject asteroidPrefab;

        [Header("Mode")]// Spawning mode
        public SpawnMode mode = SpawnMode.CameraBounds;
        public BoxCollider2D area;

        [Header("Edge placement")]// Edge placement style
        public EdgeStyle edgeStyle = EdgeStyle.BandInside;
        public float edgeInset = 0.5f;
        public float bandThickness = 1.0f;

        [Header("Timing")]// Spawn timing
        public float minSpawnInterval = 0.35f;
        public float maxSpawnInterval = 0.9f;

        [Header("Size")]// Asteroid size
        public float minScale = 0.7f;
        public float maxScale = 1.8f;

        [Header("Speed")]// Asteroid speed
        public float minSpeed = 2.5f;
        public float maxSpeed = 4.0f;

        [Header("Motion")]// Asteroid motion
        public float maxSideDrift = 0.0f;
        public bool useKinematic = true;
        public bool colliderIsTrigger = true;

        [Header("CameraBounds settings")]// CameraBounds specific settings
        [Range(0f, 0.5f)] public float xPaddingViewport = 0.1f;
        public float verticalSpawnOffset = 0.75f;

        [Header("Spin")]// Asteroid spin
        public float minAngularVelocity = -90f;
        public float maxAngularVelocity = 90f;

        Camera cam;
        float halfH, halfW;
        Coroutine loop;

        void Start()
        {
            if (asteroidPrefab == null || asteroidPrefab.scene.IsValid()) // Prefab must not be part of the scene
            {
                enabled = false; 
                return;
            }

            if (mode == SpawnMode.CameraBounds) // Use main camera
            {
                cam = Camera.main;
                if (cam == null) { enabled = false; return; } // No main camera found
                CacheCamDims();
            }
            else if (area == null) // BoxArea mode requires area to be assigned through trigger collider
            {
                enabled = false;
                return;
            }

            loop = StartCoroutine(SpawnLoop());// Start spawning loop
        }

        void OnDisable()
        {
            if (loop != null) StopCoroutine(loop);// Stop spawning loop
            loop = null;
        }

        void CacheCamDims()
        {
            halfH = cam.orthographicSize;// Vertical half-size
            halfW = halfH * cam.aspect;// Horizontal half-size
        }

        IEnumerator SpawnLoop()// Spawning loop coroutine
        {
            SpawnOne();// Initial spawn
            while (true)// Repeat indefinitely
            {
                float wait = Mathf.Max(0.01f, Random.Range(minSpawnInterval, maxSpawnInterval));// Random wait time
                yield return new WaitForSeconds(wait);// Wait
                SpawnOne();// Spawn an asteroid
            }
        }

        void SpawnOne()
        {
            Vector3 spawnPos;// Position to spawn at
            bool fromTop = Random.value > 0.5f;// Randomly choose top or bottom edge

            if (mode == SpawnMode.CameraBounds)// CameraBounds mode
            {
                CacheCamDims();// Update camera dimensions in case of resize
                Vector3 c = cam.transform.position;// Camera center
                float pad = Mathf.Clamp01(xPaddingViewport) * halfW;// Horizontal padding
                float x = Random.Range(c.x - halfW + pad, c.x + halfW - pad);// Random x within padded horizontal bounds
                float yEdge = fromTop ? c.y + halfH : c.y - halfH;// Y position of the chosen edge

                if (edgeStyle == EdgeStyle.EdgeLine)// EdgeLine style
                    spawnPos = new Vector3(x, yEdge + (fromTop ? -edgeInset : edgeInset), 0f);// Spawn exactly on edge + inset
                else
                {
                    float yMin = fromTop ? yEdge - bandThickness : yEdge;// Minimum y for band
                    float yMax = fromTop ? yEdge : yEdge + bandThickness;// Maximum y for band
                    spawnPos = new Vector3(x, Random.Range(yMin, yMax), 0f);// Spawn randomly within band
                }

                spawnPos.y += fromTop ? verticalSpawnOffset : -verticalSpawnOffset;// Apply vertical offset
            }
            else
            {
                Bounds b = area.bounds;// Get bounds of the box area
                float x = Random.Range(b.min.x, b.max.x);// Random x within box bounds
                float yEdge = fromTop ? b.max.y : b.min.y;// Y position of the chosen edge

                if (edgeStyle == EdgeStyle.EdgeLine)
                    spawnPos = new Vector3(x, yEdge + (fromTop ? -edgeInset : edgeInset), 0f);// Spawn exactly on edge + inset
                else
                {
                    float yMin = fromTop ? yEdge - bandThickness : yEdge;// Minimum y for band
                    float yMax = fromTop ? yEdge : yEdge + bandThickness;// Maximum y for band
                    spawnPos = new Vector3(x, Random.Range(yMin, yMax), 0f);// Spawn randomly within band
                }
            }

            GameObject go = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);// Instantiate asteroid prefab at spawn position
            float scale = Random.Range(minScale, maxScale);// Random scale
            go.transform.localScale = Vector3.one * scale;// Apply scale

            var rb = go.GetComponent<Rigidbody2D>();// Get or add Rigidbody2D component
            if (rb == null) rb = go.AddComponent<Rigidbody2D>();// Add Rigidbody2D if not present

            if (useKinematic)// Set Rigidbody2D type
            {
                rb.bodyType = RigidbodyType2D.Kinematic;// Kinematic body
            }
            else
            {
                rb.bodyType = RigidbodyType2D.Dynamic;// Dynamic body
                rb.gravityScale = 0f;// No gravity
                rb.drag = 0f;// No linear drag
                rb.angularDrag = 0f;// No angular drag
            }

            var col = go.GetComponent<Collider2D>();// Get Collider2D component
            if (col != null) col.isTrigger = colliderIsTrigger;// Set trigger state

            float speed = Random.Range(minSpeed, maxSpeed);// Random speed
            float side = Mathf.Approximately(maxSideDrift, 0f) ? 0f : Random.Range(-maxSideDrift, maxSideDrift);// Random side drift
            Vector2 dir = fromTop ? Vector2.down : Vector2.up;// Direction towards center
            Vector2 vel = dir * speed + new Vector2(side, 0f);// Calculate velocity

            rb.velocity = vel;// Apply velocity
            rb.angularVelocity = Random.Range(minAngularVelocity, maxAngularVelocity);// Random angular velocity

            var a = go.GetComponent<Asteroid>();// Get or add Asteroid component
            if (a == null) a = go.AddComponent<Asteroid>();// Add Asteroid if not present
            a.Setup(true, dir, speed, cam, mode == SpawnMode.BoxArea ? area : null, 1.0f, 20f);// Setup asteroid parameters
        }
    }
}
