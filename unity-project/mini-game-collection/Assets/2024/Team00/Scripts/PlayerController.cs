using UnityEngine;

namespace MiniGameCollection.Games2025.Team00
{
    public class PlayerController : MonoBehaviour
    {
        [field: SerializeField]
        public PlayerID PlayerID { get; private set; }

        [field: SerializeField]
        public GameObject BulletPrefab { get; private set; }

        [field: SerializeField]
        public Rigidbody2D Rigidbody2D { get; private set; }

        [field: SerializeField]
        public float BulletSpeed { get; private set; } = 2f; // 2 units a second

        private BulletOwner Owner => PlayerID switch
        {
            PlayerID.Player1 => BulletOwner.Player1,
            PlayerID.Player2 => BulletOwner.Player2,
            _ => throw new System.Exception(),
        };


        void Update()
        {
            float movement = ArcadeInput.Players[(int)PlayerID].AxisY * Time.deltaTime;
            Vector3 newPosition = transform.position + new Vector3(0, movement, 0);
            Rigidbody2D.MovePosition(newPosition);

            if (ArcadeInput.Players[(int)PlayerID].Action1.Pressed)
            {
                ShootBullet();
            }
        }

        void ShootBullet()
        {
            Vector3 position = transform.position + transform.up;
            GameObject prefab = Instantiate(BulletPrefab, position, transform.rotation);
            Bullet bullet = prefab.GetComponent<Bullet>();
            bullet.Shoot(Owner, transform.up, BulletSpeed);
        }

        private void OnValidate()
        {
            if (Rigidbody2D == null)
                Rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }
}
