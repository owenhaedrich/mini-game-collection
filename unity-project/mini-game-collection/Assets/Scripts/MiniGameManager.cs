using System;
using System.Collections;
using UnityEngine;

namespace MiniGameCollection
{
    public sealed class MiniGameManager : MonoBehaviour
    {
        // Delegates
        public delegate void TimerEvent();
        public delegate void TimerMessage(string message);
        public delegate void TimerUpdateInt(int seconds);
        public delegate void TimerUpdateFloat(float seconds);
        public delegate void DisplayWinner(MiniGameWinner winner);

        // Events, ordered by occurence
        public event TimerUpdateInt OnTimerInit;
        public event TimerMessage OnCountDown;
        public event TimerEvent OnGameStart;
        public event TimerUpdateFloat OnTimerUpdateFloat;
        public event TimerUpdateInt OnTimerUpdateInt;
        public event TimerEvent OnGameEnd;
        public event DisplayWinner OnGameWinner;
        public event TimerEvent OnGameClose;

        // Private state
        private int PreviousIntTime { get; set; } = -1;
        private WaitForSeconds Delay1Second { get; set; }
        private IEnumerator StateCoroutine { get; set; }

        // Inspector fields
        [field: Header("Internal State")]
        [field: SerializeField, ReadOnlyGUI]
        public MiniGameManagerState State { get; private set; } = MiniGameManagerState.WaitingForStart;

        [field: SerializeField, ReadOnlyGUI]
        public string CurrentCountDownMessage { get; private set; } = string.Empty;

        [field: SerializeField, ReadOnlyGUI]
        public float TimeFloat { get; private set; }

        [field: Header("Debug")]
        [field: SerializeField]
        public bool DebugMode { get; private set; } = false;

        [field: SerializeField, Min(0.25f)]
        public float DebugGameTime { get; private set; } = 5;

        [field: Header("Parameters")]
        [field: SerializeField]
        public string CountDownStartMessage { get; private set; } = "GO!";

        [field: SerializeField]
        public MiniGameMaxTime MaxGameTime { get; private set; } = MiniGameMaxTime.Seconds30;

        [field: SerializeField]
        public MiniGameWinner Winner { get; set; } = MiniGameWinner.Unset;

        [field: Header("Metadata")]
        [field: SerializeField]
        [field: Tooltip("Student names that contributed to making this mini game.")]
        public string[] AuthorCredits { get; private set; } = { "Firstname Lastname" };


        // Properties
        public int TimeInt => (int)Math.Ceiling(TimeFloat);
        public bool IsCountingDown => State == MiniGameManagerState.InCountDown;
        public bool IsTimerRunning => State == MiniGameManagerState.TimerRunning;
        public bool IsTimerExpired => State == MiniGameManagerState.TimerExpired;


        private void Start()
        {
            Initialize();
        }

        public void Update()
        {
            switch (State)
            {
                case MiniGameManagerState.WaitingForStart:
                case MiniGameManagerState.InCountDown:
                case MiniGameManagerState.TimerExpired:
                case MiniGameManagerState.GameOver:
                    break;

                case MiniGameManagerState.TimerRunning:
                    StateRunning();
                    break;

                default:
                    throw new NotImplementedException($"{State}");
            }
        }

        public void StartGame()
        {
            if (IsTimerRunning)
            {
                string msg = $"Attempt to invoke {nameof(MiniGameManager)}.{nameof(StartGame)} when timer already running.";
                Debug.LogWarning(msg);
                return;
            }

            // Begin countdown
            State = MiniGameManagerState.InCountDown;
            StateCoroutine = CoroutineCountDown();
            StartCoroutine(StateCoroutine);
        }

        public void StopGame()
        {
            if (!IsTimerRunning)
            {
                string msg = $"Attempt to invoke {nameof(MiniGameManager)}.{nameof(StopGame)} when timer not running.";
                Debug.LogWarning(msg);
                return;
            }

            State = MiniGameManagerState.TimerExpired;
            StateCoroutine = CoroutineTimerExpired();
            StartCoroutine(StateCoroutine);
        }


        private void Initialize()
        {
            if (DebugMode)
            {
                // Use short intevals and define game time
                Delay1Second = new WaitForSeconds(0.25f);
                TimeFloat = DebugGameTime;
            }
            else
            {
                // Typical delay between ticks
                Delay1Second = new WaitForSeconds(1);

                // Set time based on enum
                TimeFloat = MaxGameTime switch
                {
                    MiniGameMaxTime.Seconds30 => 30f,
                    MiniGameMaxTime.Seconds60 => 60f,
                    _ => throw new NotImplementedException($"{MaxGameTime}")
                };
            }

            // Pass in initial time
            OnTimerInit?.Invoke(TimeInt);
            PreviousIntTime = TimeInt;

            State = MiniGameManagerState.WaitingForStart;
        }


        private void StateRunning()
        {
            // Update time
            TimeFloat = Mathf.Clamp(TimeFloat - Time.deltaTime, 0f, float.MaxValue);
            OnTimerUpdateFloat?.Invoke(TimeFloat);

            // Update whole number time if changed
            if (TimeInt != PreviousIntTime)
            {
                PreviousIntTime = TimeInt;
                OnTimerUpdateInt?.Invoke(TimeInt);
            }

            // End timer running state if timer expired
            if (TimeInt == 0)
            {
                State = MiniGameManagerState.TimerExpired;
                StateCoroutine = CoroutineTimerExpired();
                StartCoroutine(StateCoroutine);
            }
        }

        private IEnumerator CoroutineCountDown()
        {
            // Do 3-2-1 countdown
            for (int i = 3; i > 0; i--)
            {
                CurrentCountDownMessage = $"{i}";
                OnCountDown?.Invoke(CurrentCountDownMessage);
                yield return Delay1Second;
            }

            // Print "GO!" or otherwise defined message
            CurrentCountDownMessage = CountDownStartMessage;
            OnCountDown?.Invoke(CurrentCountDownMessage);
            yield return Delay1Second;

            // Clear message
            CurrentCountDownMessage = string.Empty;
            OnCountDown?.Invoke(CurrentCountDownMessage);

            // Kick off timer
            State = MiniGameManagerState.TimerRunning;
            OnGameStart?.Invoke();

            // Clear reference to this coroutine
            StateCoroutine = null;
        }

        private IEnumerator CoroutineTimerExpired()
        {
            // Call game end
            OnGameEnd?.Invoke();
            yield return Delay1Second;

            // Call game winner
            OnGameWinner?.Invoke(Winner);
            yield return Delay1Second;

            // Kill state machine with terminal state
            State = MiniGameManagerState.GameOver;
            OnGameClose?.Invoke();

            // Clear reference to this coroutine
            StateCoroutine = null;
        }

    }
}
