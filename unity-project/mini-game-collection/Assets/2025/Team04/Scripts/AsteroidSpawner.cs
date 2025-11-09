using UnityEngine;

namespace MiniGameCollection.Games2025.Team04
{
    public class AsteroidSpawnerBasic : MiniGameBehaviour
    {
        public GameObject asteroidPrefab;

        // spawn areas
        public Transform topLine;// upper spawn line
        public Transform bottomLine;// lower spawn line

        public float halfWidth = 8f;// horizontal spawn range from center

        // spawn timing
        public float minInterval = 0.6f;
        public float maxInterval = 1.4f;
        float timer = 0f;
        float nextSpawnTime = 1f;

        // asteroid properties
        public float minSpeed = 2f;
        public float maxSpeed = 5f;
        public float minScale = 0.8f;
        public float maxScale = 1.6f;

        // asteroid lifetime
        public float lifetime = 12f;

        void Start()
        {
            nextSpawnTime = Random.Range(minInterval, maxInterval);// initial spawn time
        }

        void Update()
        {
            timer += Time.deltaTime;// count up timer
            if (timer >= nextSpawnTime)// time to spawn
            {
                SpawnOne();// spawn an asteroid
                timer = 0f;// reset timer
                nextSpawnTime = Random.Range(minInterval, maxInterval);// new random spawn time
            }
        }

        void SpawnOne()// spawn a single asteroid
        {
            if (asteroidPrefab == null || topLine == null || bottomLine == null) return;

            float x = Random.Range(transform.position.x - halfWidth, transform.position.x + halfWidth);// random x position within range
            bool fromTop = Random.value > 0.5f;// randomly decide spawn from top or bottom
            float y = fromTop ? topLine.position.y : bottomLine.position.y;//  y position based on spawn side

            GameObject go = Instantiate(asteroidPrefab, new Vector3(x, y, 0f), Quaternion.identity);// create asteroid

            // size
            go.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);// random scale

            // stop any Rigidbody2D gravity
            var rb = go.GetComponent<Rigidbody2D>();// if exists
            if (rb != null)// stop gravity
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

               
            var a = go.GetComponent<Asteroid>();// get Asteroid component
            if (a == null) a = go.AddComponent<Asteroid>();// add if missing

            a.direction = fromTop ? Vector2.down : Vector2.up; // bottom spawns move upward
            a.speed = Random.Range(minSpeed, maxSpeed);// random speed
            a.lifetime = lifetime;// set lifetime
        }
    }
}
