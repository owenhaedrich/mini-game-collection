using System;
using TMPro;
using UnityEngine;

namespace MiniGameCollection
{
    public sealed class MiniGameManagerIntTimerUI : MiniGameBehaviour
    {
        [field: SerializeField] public TMP_Text MiniGameTimer { get; private set; }

        private string MiniGameTimerIntWarning => $"Reference {nameof(MiniGameTimer)} is not set in {nameof(MiniGameManagerIntTimerUI)} on {name}.";


        protected override void OnEnable()
        {
            base.OnEnable();

            if (MiniGameTimer == null)
                throw new NullReferenceException(MiniGameTimerIntWarning);
        }

        protected override void OnTimerInitialized(int maxGameTime)
        {
            if (MiniGameTimer == null)
            {
                Debug.LogWarning(MiniGameTimerIntWarning);
                return;
            }

            MiniGameTimer.text = $"{maxGameTime}";
        }


        protected override void OnTimerUpdateInt(int timeRemaining)
        {
            if (MiniGameTimer == null)
            {
                Debug.LogWarning(MiniGameTimerIntWarning);
                return;
            }

            MiniGameTimer.text = $"{timeRemaining}";
        }

        protected override void Reset()
        {
            base.Reset();

            if (MiniGameTimer == null)
                MiniGameTimer = GetComponent<TMP_Text>();
        }

    }
}
