using TMPro;
using UnityEngine;

namespace MiniGameCollection
{
    public class MiniGameScoreUI : MonoBehaviour
    {
        [field: SerializeField] public TMP_Text[] ScoreDisplays { get; private set; }
        [field: SerializeField] public int[] PlayerScores { get; private set; }

        private void Awake()
        {
            // Ensure array size is correct
            if (PlayerScores.Length > ScoreDisplays.Length)
            {
                PlayerScores = PlayerScores[..PlayerScores.Length];
            }
            else if (PlayerScores.Length < ScoreDisplays.Length)
            {
                int[] scores = new int[ScoreDisplays.Length];
                PlayerScores.CopyTo(scores, 0);
                PlayerScores = scores;
            }

            UpdatePlayerScoreDisplays();
        }

        public void SetPlayerScore(int playerNumber, int score)
        {
            int playerIndex = playerNumber - 1;
            PlayerScores[playerIndex] = score;
            UpdatePlayerScoreDisplay(playerIndex);
        }

        public void IncrementPlayerScore(int playerNumber)
        {
            int playerIndex = playerNumber - 1;
            PlayerScores[playerIndex]++;
            UpdatePlayerScoreDisplay(playerIndex);
        }

        public void DecrementPlayerScore(int playerNumber)
        {
            int playerIndex = playerNumber - 1;
            PlayerScores[playerIndex]--;
            UpdatePlayerScoreDisplay(playerIndex);
        }

        private void UpdatePlayerScoreDisplay(int playerIndex)
        {
            int score = PlayerScores[playerIndex];
            ScoreDisplays[playerIndex].text = score.ToString();
        }

        private void UpdatePlayerScoreDisplays()
        {
            for (int i = 0; i < ScoreDisplays.Length; i++)
            {
                UpdatePlayerScoreDisplay(i);
            }
        }

    }
}
