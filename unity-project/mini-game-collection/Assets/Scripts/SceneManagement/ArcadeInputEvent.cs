using System;
using UnityEngine;
using UnityEngine.Events;

namespace MiniGameCollection
{
    public class ArcadeInputEvent : MonoBehaviour
    {
        [field: SerializeField] public string SceneName { get; private set; } = string.Empty;
        [field: SerializeField] public string[] TriggerInputs { get; private set; } = Array.Empty<string>();
        [field: SerializeField] public float HoldTime { get; private set; } = 1f;
        [field: SerializeField] public UnityEvent OnTrigger { get; private set; }

        private float activeHoldTime = 0f;

        private void Update()
        {
            int countTriggersActive = 0;
            foreach (var trigger in TriggerInputs)
            {
                bool isPositive = Input.GetAxis(trigger) > +0.5f;
                bool isNegative = Input.GetAxis(trigger) < -0.5f;
                if (isPositive || isNegative)
                    countTriggersActive++;
            }

            bool allTriggersActive = countTriggersActive >= TriggerInputs.Length;
            if (allTriggersActive)
            {
                activeHoldTime += Time.deltaTime;
            }
            else
            {
                activeHoldTime = 0;
            }

            bool doTriggerEvent = activeHoldTime > HoldTime;
            if (doTriggerEvent)
            {
                OnTrigger?.Invoke();
            }
        }
    }
}