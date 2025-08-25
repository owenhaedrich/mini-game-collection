using UnityEngine;

namespace MiniGameCollection
{
    public abstract class MiniGameBehaviour : MonoBehaviour
    {
        [field: SerializeField] protected MiniGameManager MiniGameManager { get; set; }

        protected virtual void OnEnable()
        {
            GetMiniGameManagerIfNull();

            if (MiniGameManager == null)
                return;

            MiniGameManager.OnTimerInit += OnTimerInitialized;
            MiniGameManager.OnCountDown += OnCountDown;
            MiniGameManager.OnGameStart += OnGameStart;
            MiniGameManager.OnTimerUpdateFloat += OnTimerUpdate;
            MiniGameManager.OnTimerUpdateInt += OnTimerUpdateInt;
            MiniGameManager.OnGameEnd += OnGameEnd;
            MiniGameManager.OnGameWinner += OnGameWinner;
            MiniGameManager.OnGameClose += OnGameClose;
        }

        protected virtual void OnDisable()
        {
            if (MiniGameManager == null)
                return;

            MiniGameManager.OnTimerInit -= OnTimerInitialized;
            MiniGameManager.OnCountDown -= OnCountDown;
            MiniGameManager.OnGameStart -= OnGameStart;
            MiniGameManager.OnTimerUpdateFloat -= OnTimerUpdate;
            MiniGameManager.OnTimerUpdateInt -= OnTimerUpdateInt;
            MiniGameManager.OnGameEnd -= OnGameEnd;
            MiniGameManager.OnGameWinner -= OnGameWinner;
            MiniGameManager.OnGameClose -= OnGameClose;
        }

        protected virtual void Reset()
        {
            GetMiniGameManagerIfNull();
        }

        /// <summary>
        ///     Mini game time initiallized to value <paramref name="maxGameTime"/>.
        /// </summary>
        /// <param name="maxGameTime">The max game time.</param>
        protected virtual void OnTimerInitialized(int maxGameTime) { }

        /// <summary>
        ///     The 3-2-1-<message> countdown events.
        /// </summary>
        /// <param name="message">The UI message to display.</param>
        protected virtual void OnCountDown(string message) { }
        
        /// <summary>
        ///     Event signalling mini game start.
        /// </summary>
        protected virtual void OnGameStart() { }

        /// <summary>
        ///     Event singalling <paramref name="timeRemaining"/>.
        /// </summary>
        /// <remarks>
        ///     Occurs every MonoBehaviour.Update.
        /// </remarks>
        /// <param name="timeRemaining">The remaining mini game time.</param>
        protected virtual void OnTimerUpdate(float timeRemaining) { }

        /// <summary>
        ///     Event singalling <paramref name="timeRemaining"/>.
        /// </summary>
        /// <param name="timeRemaining"></param>
        /// <remarks>
        ///     Occurs once per second on whole number change.
        /// </remarks>
        protected virtual void OnTimerUpdateInt(int timeRemaining) { }

        /// <summary>
        ///     Event signalling mini game end.
        /// </summary>
        protected virtual void OnGameEnd() { }

        /// <summary>
        ///     Event signalling mini game <paramref name="winner"/>.
        /// </summary>
        /// <param name="winner">Who won the mini game.</param>
        protected virtual void OnGameWinner(MiniGameWinner winner) { }

        /// <summary>
        ///     Event signalling mini game terminated.
        /// </summary>
        protected virtual void OnGameClose() { }




        private void GetMiniGameManagerIfNull()
        {
            if (MiniGameManager == null)
            {
                MiniGameManager = FindAnyObjectByType<MiniGameManager>();
            }
        }
    }
}
