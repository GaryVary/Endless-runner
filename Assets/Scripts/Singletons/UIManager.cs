using Assets.Scripts.Delegates;
using Assets.Scripts.DTO;
using Assets.Scripts.Enums;
using Assets.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Singletons
{
    public class UIManager : Singleton<UIManager>
    {
        public event BasicDelegate PlayButtonHitEvent;
        public event GenericDelegate<string> NickNameChangedEvent;

        public Vector2 CanvasSize { get; private set; }

        [SerializeField]
        private GameObject _scoreRowPrefab;

        private void HandleGameCondition(GameCondition gameCondition)
        {
            switch (gameCondition)
            {
                case GameCondition.Start:
                case GameCondition.InProgress:
                case GameCondition.End:
                    {
                        WindowsManager.Instance.ShowNextWindow();
                        SetLoadingIconState(true);
                        break;
                    }
            }
        }

        private void UpdateNicknameText(string nickname)
        {
            WindowsManager.Instance.SetInfoText(InfoTextType.Nickname, nickname);
        }

        private void UpdateScoreText(int value)
        {
            WindowsManager.Instance.SetInfoText(InfoTextType.Score, value.ToString());
        }

        private void UpdateTimeText(int value)
        {
            WindowsManager.Instance.SetInfoText(InfoTextType.Time, value.ToString());
        }

        private void OnPlayButtonClick()
        {
            PlayButtonHitEvent.Invoke();
        }

        private void OnNicknameChanged(string value)
        {
            if(string.IsNullOrEmpty(value))
            {
                return;
            }
            NickNameChangedEvent.Invoke(value);
            UpdateNicknameText(value);
        }

        private void OnScoreDowloaded(List<Entry> entries)
        {
            SetLoadingIconState(false);
            SetScoreTable(entries);
        }

        private void SetLoadingIconState(bool state)
        {
            WindowsManager.Instance.GetLoadingIcon().gameObject.SetActive(state);
        }

        private void SetScoreTable(List<Entry> entries)
        {
            var scrollRowHolder = WindowsManager.Instance.GetScoreRowHolder();

            foreach (Transform child in scrollRowHolder.transform)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < entries.Count; i++)
            {
                var instance = Instantiate(_scoreRowPrefab, scrollRowHolder.transform).GetComponent<UIScoreRow>();

                instance.PlaceOnLeaderboard = (i + 1).ToString();
                instance.Nickname = entries[i].name;
                instance.Score = entries[i].score;
            }
        }

        private void Start()
        {
            CalculateRelativeCanvasSize();
            SubscribeOnGameConditionChangeEvent();
            SubscribeOnScoreChangeEvent();
            SubscribeOnScoreDowloadedEvent();
            SubscribeOnTimeChangeEvent();
            SubscribeOnNicknameInputField();
            SubscribeOnPlayButtons();
        }

        private void SubscribeOnGameConditionChangeEvent()
        {
            GameManager.Instance.GameConditionChanged += HandleGameCondition;
        }

        private void SubscribeOnScoreChangeEvent()
        {
            ScoreManager.Instance.ScoreUpdatedEvent += UpdateScoreText;
        }

        private void SubscribeOnScoreDowloadedEvent()
        {
            ScoreManager.Instance.ScoreDownloadedEvent += OnScoreDowloaded;
        }

        private void SubscribeOnTimeChangeEvent()
        {
            TimerManager.Instance.RemainingTimeChanged += UpdateTimeText;
        }

        private void SubscribeOnPlayButtons()
        {
            var buttons = WindowsManager.Instance.GetButtonsInWindows<StartPlayButton>();
            foreach (var button in buttons)
            {
                button.onClick.AddListener(OnPlayButtonClick);
            }
        }

        private void SubscribeOnNicknameInputField()
        {
            var input = WindowsManager.Instance.GetNicknameInputField();
            input.onEndEdit.AddListener(OnNicknameChanged);
        }

        private void CalculateRelativeCanvasSize()
        {
            var canvasRect = GetComponent<RectTransform>().rect;
            CanvasSize = new Vector2(canvasRect.width * transform.localScale.x, canvasRect.height * transform.localScale.y);
        }
    }
}