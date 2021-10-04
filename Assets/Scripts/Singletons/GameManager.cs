using Assets.Scripts.Delegates;
using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.Singletons
{
    public class GameManager : Singleton<GameManager>
    {
        public event HandleGameCondition GameConditionChanged;

        private void StartGame()
        {
            GameConditionChanged.Invoke(GameCondition.Start);
        }

        private void EndGame()
        {
            GameConditionChanged.Invoke(GameCondition.End);
        }

        private void GameInProgress()
        {
            GameConditionChanged.Invoke(GameCondition.InProgress);
        }

        protected override void Awake()
        {
            base.Awake();
            QualitySettings.vSyncCount = 1;
        }

        private void Start()
        {
            SubscribeForPlayerDiedEvent();
            StartGame();
        }

        private void SubscribeForPlayerDiedEvent()
        {
            PlayerControl.Instance.PlayerDiedEvent += EndGame;
            TimerManager.Instance.TimeIsUpEvent += EndGame;
            UIManager.Instance.PlayButtonHitEvent += GameInProgress;
        }
    }
}
