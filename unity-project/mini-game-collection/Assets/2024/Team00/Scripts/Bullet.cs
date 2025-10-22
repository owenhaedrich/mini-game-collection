using UnityEngine;

namespace MiniGameCollection.Games2025.Team00
{
    public class Bullet : MonoBehaviour
    {
        [field: SerializeField]
        public BulletOwner Owner { get; private set; }

        [field: SerializeField]
        public Rigidbody2D Rigidbody2D { get; private set; }

        [field: SerializeField]
        public float SelfDestructTime { get; private set; } = 10; // 10 seconds

        public void Shoot(BulletOwner owner, Vector2 direction, float speed)
        {
            this.Owner = owner;
            Rigidbody2D.velocity = direction * speed;

            Destroy(this.transform.root.gameObject, SelfDestructTime);
        }

        private void OnValidate()
        {
            if (Rigidbody2D == null)
                Rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}
