using UnityEngine;

namespace MiniGameCollection.Games2024.Team00
{
    public class ScoreKeeper : MiniGameBehaviour
    {
        [field: SerializeField] public MiniGameScoreUI MiniGameScoreUI { get; private set; }
        [field: SerializeField] public ScoreTrigger P1ScoreTrigger { get; private set; }
        [field: SerializeField] public ScoreTrigger P2ScoreTrigger { get; private set; }

        protected override void OnEnable()
        {
            base.OnEnable();

            P1ScoreTrigger.OnScorePoint += UpdateScores;
            P2ScoreTrigger.OnScorePoint += UpdateScores;
        }

        protected override void OnGameEnd()
        {
            int scoreP1 = P1ScoreTrigger.Score;
            int scoreP2 = P2ScoreTrigger.Score;

            if (scoreP1 == scoreP2)
                MiniGameManager.Winner = MiniGameWinner.Draw;
            else if (scoreP1 > scoreP2)
                MiniGameManager.Winner = MiniGameWinner.Player1;
            else if (scoreP1 < scoreP2)
                MiniGameManager.Winner = MiniGameWinner.Player2;
            else
                throw new System.NotImplementedException();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            P1ScoreTrigger.OnScorePoint -= UpdateScores;
            P2ScoreTrigger.OnScorePoint -= UpdateScores;
        }

        private void UpdateScores()
        {
            MiniGameScoreUI.SetPlayerScore(1, P1ScoreTrigger.Score);
            MiniGameScoreUI.SetPlayerScore(2, P2ScoreTrigger.Score);
        }
    }
}
