using UnityEngine;

namespace MiniGameCollection.Games2025.Team04
{
    public class AsteroidSpawnerBasic : MiniGameBehaviour
    {
        public GameObject asteroidPrefab;

        // spawn areas
        public Transform TopLine;// upper spawn line
        public Transform BottomLine;// lower spawn line

        public float HalfWidth = 4f;// horizontal spawn range from center

        // spawn timing
        [Header("Spawn Timing")]
        public float MinInterval = 0.6f;
        public float MaxInterval = 1.4f;
        float Timer = 0f;
        float NextSpawnTime = 1f;

        // asteroid properties
        [Header("Asteroid Properties")]
        public float MinSpeed = 2f;
        public float MaxSpeed = 5f;
        public float MinScale = 0.8f;
        public float MaxScale = 1.6f;

        // asteroid lifetime
        [Header("Asteroid Lifetime")]
        public float Lifetime = 12f;

        void Start()
        {
            NextSpawnTime = Random.Range(MinInterval, MaxInterval);// initial spawn time
        }

        void Update()
        {
            Timer += Time.deltaTime;// count up timer
            if (Timer >= NextSpawnTime)// time to spawn
            {
                SpawnOne();// spawn an asteroid
                Timer = 0f;// reset timer
                NextSpawnTime = Random.Range(MinInterval, MaxInterval);// new random spawn time
            }
        }

        void SpawnOne()// spawn a single asteroid
        {
            if (asteroidPrefab == null || TopLine == null || BottomLine == null) return;

            float x = Random.Range(transform.position.x - HalfWidth, transform.position.x + HalfWidth);// random x position within range
            bool fromTop = Random.value > 0.5f;// randomly decide spawn from top or bottom
            float y = fromTop ? TopLine.position.y : BottomLine.position.y;//  y position based on spawn side

            GameObject go = Instantiate(asteroidPrefab, new Vector3(x, y, 0f), Quaternion.identity);// create asteroid

            // size
            go.transform.localScale = Vector3.one * Random.Range(MinScale, MaxScale);// random scale

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

            a.Direction = fromTop ? Vector2.down : Vector2.up; // bottom spawns move upward
            a.Speed = Random.Range(MinSpeed, MaxSpeed);// random speed
            a.Lifetime = Lifetime;// set lifetime
        }
    }
}
