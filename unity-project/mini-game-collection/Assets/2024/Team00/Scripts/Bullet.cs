using UnityEngine;

namespace MiniGameCollection.Games2025.Team00
{
    public class Bullet : MonoBehaviour
    {
        [field: SerializeField] public BulletOwner Owner { get; private set; }
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
        [field: SerializeField] public float SelfDestructTime { get; private set; } = 10; // 10 seconds
        [field: SerializeField] public int PointPenalty { get; private set; } = -100;

        [field: ReadOnlyGUI]
        [field: SerializeField] public ScoreKeeper ScoreKeeper { get; private set; }

        public void Shoot(BulletOwner owner, Vector2 direction, float speed, ScoreKeeper scoreKeeper)
        {
            ScoreKeeper = scoreKeeper;

            this.Owner = owner;
            Rigidbody2D.velocity = direction * speed;

            // Self-destruct this bullet after time
            Destroy(this.transform.root.gameObject, SelfDestructTime);
        }

        private PlayerID OwnerToPlayerID(BulletOwner owner)
        {
            return (owner) switch
            {
                BulletOwner.Player1 => PlayerID.Player1,
                BulletOwner.Player2 => PlayerID.Player2,
                _ => throw new System.NotImplementedException(),
            };
        }

        private void DestroyBullet()
        {
            Destroy(this.gameObject);
        }


        private void OnTriggerEnter2D(Collider2D collider2d)
        {
            var enemy = collider2d.GetComponent<Enemy>();
            var player = collider2d.GetComponent<PlayerController>();
            PlayerID bulletOwnerPlayerID = OwnerToPlayerID(Owner);

            switch (Owner)
            {
                case BulletOwner.Player1:
                case BulletOwner.Player2:
                    if (enemy != null)
                    {
                        // Add to bullet owner's score
                        ScoreKeeper.AddScore(bulletOwnerPlayerID, enemy.PointsAwarded);
                        // Destroy enemy and this bullet
                        enemy.DestroyEnemy();
                        DestroyBullet();
                    }
                    else if (player != null)
                    {
                        // Check if hit another player
                        if (bulletOwnerPlayerID != player.PlayerID)
                        {
                            // Reduce other player's score
                            ScoreKeeper.AddScore(player.PlayerID, this.PointPenalty);
                            // Destroy this bullet
                            DestroyBullet();
                        }
                    }
                    break;
                case BulletOwner.Enemy:
                    if (player != null)
                    {
                        // Reduce player's score
                        ScoreKeeper.AddScore(player.PlayerID, enemy.PointPenalty);
                    }
                    break;
            }
        }

        private void OnValidate()
        {
            if (Rigidbody2D == null)
                Rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}
