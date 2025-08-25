using System;
using TMPro;
using UnityEngine;

namespace MiniGameCollection
{
    public sealed class MiniGameManagerFloatTimerUI : MiniGameBehaviour
    {
        [field: SerializeField] public TMP_Text MiniGameTimer { get; private set; }
        [field: SerializeField] public string Format { get; private set; } = "0.00";

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

            MiniGameTimer.text = $"{maxGameTime.ToString(Format)}";
        }


        protected override void OnTimerUpdate(float timeRemaining)
        {
            if (MiniGameTimer == null)
            {
                Debug.LogWarning(MiniGameTimerIntWarning);
                return;
            }

            MiniGameTimer.text = $"{timeRemaining.ToString(Format)}";
        }

        protected override void Reset()
        {
            base.Reset();

            if (MiniGameTimer == null)
                MiniGameTimer = GetComponent<TMP_Text>();
        }

    }
}
