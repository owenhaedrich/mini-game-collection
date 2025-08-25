using UnityEngine;

namespace MiniGameCollection.Games2024.Team00
{
    public class PlayerJump : MiniGameBehaviour
    {
        [field: SerializeField, Range(1, 2)]
        private int PlayerID { get; set; } = 1;

        [field: SerializeField]
        private bool ControlsActive { get; set; }

        [field: SerializeField]
        private AnimationCurve JumpCurve { get; set; }

        [field: SerializeField]
        private float MaxJumpHeight { get; set; } = 5;

        [field: SerializeField]
        private float JumpSpeed { get; set; } = 0.7f;

        public int ID => PlayerID - 1;
        public bool DoJump { get; private set; }
        public bool IsJumping { get; private set; }
        public float AnimationTime { get; private set; }
        public Vector3 InitialPosition { get; private set; }
        public float MaxGameTime { get; private set; }
        public float TimeRemaining { get; private set; }
        public float GameSpeed => (1 - TimeRemaining / MaxGameTime) + 1; // TODO: put in function
        public float JumpCurveTime => JumpCurve.keys[^1].time;

        protected override void OnTimerInitialized(int maxGameTime)
        {
            MaxGameTime = maxGameTime;
        }

        protected override void OnTimerUpdate(float timeRemaining)
        {
            TimeRemaining = timeRemaining;
        }

        protected override void OnGameStart()
        {
            InitialPosition = transform.position;
            ControlsActive = true;
        }

        protected override void OnGameEnd()
        {
            ControlsActive = false;
        }

        private void Update()
        {
            // Do jumping animation
            if (IsJumping)
            {
                // Add to animation
                AnimationTime += Time.deltaTime * GameSpeed * JumpCurveTime / JumpSpeed;
                float jumpHeight = JumpCurve.Evaluate(AnimationTime) * MaxJumpHeight;
                transform.position = InitialPosition + Vector3.up * jumpHeight;

                if (AnimationTime >= JumpCurveTime)
                {
                    AnimationTime = 0;
                    IsJumping = false;
                }
            }

            // Don't run update if controls not enabled
            if (!ControlsActive)
                return;

            // See if player should jump
            bool doJump = ArcadeInput.Players[ID].Action1.Down;
            if (doJump && !IsJumping)
            {
                IsJumping = true;
            }
        }

    }
}