using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGameCollection.Games2025.Team00
{
    public class EnemyPattern : MiniGameBehaviour
    {
        private bool isActive;
        private float delayTimeRemaining;
        private float delayDistanceToMove;
        private int[] EnemyWaypointIndex;

        [field: SerializeField]
        public Color DebugGizmosColor { get; private set; } = Color.red;

        [field: SerializeField]
        public int BeginPatternAtTime { get; private set; }

        [field: SerializeField]
        public float MovementSpeed { get; private set; } = 10f; // units per second

        [field: SerializeField]
        public float AlignmentSpacing { get; private set; } = 1f; // units

        [field: SerializeField]
        public Waypoint[] Waypoints { get; private set; }

        [field: SerializeField]
        public Rigidbody2D[] EnemiesInPattern { get; private set; }

        public bool IsPausedAtWaypoint => delayTimeRemaining > 0;


        void Update()
        {
            // Don't run unless active
            if (!isActive)
                return;

            // Don't run unless delay time is negative
            delayTimeRemaining -= Time.deltaTime;
            if (delayTimeRemaining > 0)
                return;

            // Do movement for all enemies in pattern
            for (int i = 0; i < EnemiesInPattern.Length; i++)
            {
                MoveEnemyToWaypoint(i);
            }
        }

        protected override void OnTimerUpdateInt(int timeRemaining)
        {
            bool beginPattern = BeginPatternAtTime >= timeRemaining;
            if (!beginPattern)
                return;

            if (isActive)
                return;

            isActive = true;
            EnemyWaypointIndex = new int[EnemiesInPattern.Length];

            // move and align
            Vector3 startPosition = Waypoints[0].Position;
            Vector3 startDirection = Waypoints[0].transform.up;
            for (int i = 0; i < EnemiesInPattern.Length; i++)
            {
                Rigidbody2D enemy = EnemiesInPattern[i];
                Vector3 position = startPosition + AlignmentSpacing * i * startDirection;
                enemy.transform.position = position;
                enemy.gameObject.SetActive(true);
            }
        }

        private void MoveEnemyToWaypoint(int enemyIndex)
        {
            // Get enemy
            Rigidbody2D enemy = EnemiesInPattern[enemyIndex];
            if (enemy == null)
                return;

            if (EnemyWaypointIndex[enemyIndex] == Waypoints.Length)
                return;

            // Do movement
            Vector3 position = enemy.position;
            float distanceTravelled = 0;
            // A bit janky, but basically override movement when paused at waypoint
            float distanceLeftToMove = IsPausedAtWaypoint
                ? delayDistanceToMove
                : MovementSpeed * Time.deltaTime;
            while (distanceLeftToMove > 0)
            {
                // get waypoint
                int waypointIndex = EnemyWaypointIndex[enemyIndex];
                Waypoint waypoint = Waypoints[waypointIndex];
                // calc incremental movement to waypoint
                float distanceToWaypoint = Vector2.Distance(enemy.position, waypoint.Position);
                float distanceToMove = Mathf.Clamp(distanceLeftToMove, 0, distanceToWaypoint);
                Vector3 directionOfMove = Vector3.Normalize(waypoint.Position - enemy.transform.position);
                Vector3 moveVector = distanceToMove * directionOfMove;
                position += moveVector;
                // subtract distance travelled from record, check to see if we are at waypoint
                distanceLeftToMove -= distanceToMove;
                distanceTravelled += distanceToMove;
                // Check to see if we hit checkpoint
                if (distanceToWaypoint - distanceToMove == 0)
                {
                    // Set next waypoint
                    EnemyWaypointIndex[enemyIndex]++;

                    // if at end, destory enemy
                    if (EnemyWaypointIndex[enemyIndex] == Waypoints.Length)
                    {
                        enemy.gameObject.SetActive(false);
                        break;
                    }

                    // See if we need to wait at this waypoint
                    if (waypoint.ActivateDelay())
                    {
                        delayTimeRemaining = waypoint.DelayTimeSeconds;
                        delayDistanceToMove = distanceTravelled;
                        break;
                    }
                }
            }
            enemy.MovePosition(position);
        }


        private void OnDrawGizmos()
        {
            if (Waypoints.Length < 2)
                return;

            Gizmos.color = DebugGizmosColor;
            Gizmos.DrawWireCube(Waypoints[0].Position, Vector3.one);
            for (int i = 0; i < Waypoints.Length - 1; i++)
            {
                int index0 = i + 0;
                int index1 = i + 1;
                Gizmos.DrawLine(Waypoints[index0].Position, Waypoints[index1].Position);
                Gizmos.DrawCube(Waypoints[index1].Position, Vector3.one);
            }
        }
    }
}
