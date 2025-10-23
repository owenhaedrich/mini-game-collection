using UnityEngine;

namespace MiniGameCollection.Games2025.Team00
{
    public class Enemy : MonoBehaviour
    {
        [field: SerializeField] public EnemyType EnemyType { get; private set; }
        // These should probably be in a lookup table
        [field: SerializeField] public int PointsAwarded { get; private set; } = 100;
        [field: SerializeField] public int PointPenalty { get; private set; } = -100;

        private void Update()
        {
            switch (EnemyType)
            {
                case EnemyType.Level1_Green: LogicLevel1(); break;
                case EnemyType.Level2_Orange: LogicLevel2(); break;
                case EnemyType.Level3_Black: LogicLevel3(); break;
                default: throw new System.NotImplementedException();
            }
        }

        private void LogicLevel1()
        {
            // Nothing :)
        }

        private void LogicLevel2()
        {
            // hive mind random shooting ...
        }

        private void LogicLevel3()
        {
            // shoot on sight
        }

        public void DestroyEnemy()
        {
            Destroy(this.gameObject);
        }

    }
}