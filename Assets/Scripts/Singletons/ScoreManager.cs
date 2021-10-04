using Assets.Scripts.Delegates;
using Assets.Scripts.DTO;
using Assets.Scripts.Enums;
using Assets.Scripts.Web;
using System.Collections.Generic;

namespace Assets.Scripts.Singletons
{
    public class ScoreManager : Singleton<ScoreManager>
    {
        public event GenericDelegate<int> ScoreUpdatedEvent;
        public event GenericDelegate<List<Entry>> ScoreDownloadedEvent;

        private int _currentScore = 0;
        private string _playerNickname = "Guest";

        private void HandleGameCondition(GameCondition gameCondition)
        {
            switch (gameCondition)
            {
                case GameCondition.InProgress:
                    {
                        IncreaseScore(-_currentScore);
                        break;
                    }
                case GameCondition.End:
                    {
                        StartCoroutine(LeaderboardClient.GetTopFiveScore());

                        if (_currentScore != 0)
                        {
                            StartCoroutine(LeaderboardClient.UploadScore(_playerNickname, _currentScore.ToString()));
                        }
                        break;
                    }
            }
        }

        private void IncreaseScore(int value)
        {
            _currentScore += value;
            ScoreUpdatedEvent.Invoke(_currentScore);
        }

        private void ChangeNickname(string value)
        {
            _playerNickname = value;
        }

        private void OnScoreDowloaded(List<Entry> entries)
        {
            ScoreDownloadedEvent.Invoke(entries);
        }

        private void Start()
        {
            GameManager.Instance.GameConditionChanged += HandleGameCondition;
            PlayerControl.Instance.PlayerGainedScoreEvent += IncreaseScore;
            UIManager.Instance.NickNameChangedEvent += ChangeNickname;
            LeaderboardClient.ScoreDownloadedEvent += OnScoreDowloaded;
        }
    }
}
