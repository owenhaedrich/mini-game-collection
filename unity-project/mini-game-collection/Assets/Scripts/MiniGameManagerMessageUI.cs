using System;
using TMPro;
using UnityEngine;

namespace MiniGameCollection
{
    public sealed class MiniGameManagerMessageUI : MiniGameBehaviour
    {
        [field: SerializeField] public TMP_Text MessageDisplay { get; private set; }

        private string MessageDisplayWarning => $"Reference {nameof(MessageDisplay)} is not set in {nameof(MiniGameManagerIntTimerUI)} on {name}.";


        protected override void OnEnable()
        {
            base.OnEnable();

            if (MessageDisplay == null)
                throw new NullReferenceException(MessageDisplayWarning);
        }

        protected override void OnCountDown(string message)
        {
            if (MessageDisplay == null)
            {
                Debug.LogWarning(MessageDisplayWarning);
                return;
            }

            MessageDisplay.text = message;
        }

        protected override void OnGameEnd()
        {
            if (MessageDisplay == null)
            {
                Debug.LogWarning(MessageDisplayWarning);
                return;
            }

            MessageDisplay.text = "Finish!";
        }

        protected override void OnGameWinner(MiniGameWinner miniGameWinner)
        {
            if (MessageDisplay == null)
            {
                Debug.LogWarning(MessageDisplayWarning);
                return;
            }

            string message = miniGameWinner switch
            {
                MiniGameWinner.Player1 => "Player 1 Wins!",
                MiniGameWinner.Player2 => "Player 2 Wins!",
                MiniGameWinner.Draw => "Draw",
                MiniGameWinner.Unset => $"NO WINNER SET IN {nameof(MiniGameManager)}",
                _ => throw new NotImplementedException($"Unhandled value: {miniGameWinner}")
            };

            MessageDisplay.text = message;
        }

        protected override void Reset()
        {
            base.Reset();

            if (MessageDisplay == null)
                MessageDisplay = GetComponent<TMP_Text>();
        }

    }
}
