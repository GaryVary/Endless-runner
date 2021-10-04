using Assets.Scripts.Enums;
using Assets.Scripts.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Singletons
{
    public class WindowsManager : Singleton<WindowsManager>
    {
        public WindowType CurrentWindow { get; private set; }
        private Dictionary<WindowType, GameObject> _allWindows = new Dictionary<WindowType, GameObject>();
        private Dictionary<InfoTextType, List<Text>> _allInfoTexts = new Dictionary<InfoTextType, List<Text>>();
        private Button[] _buttons;
        private InputField _nicknameInputField;
        private GameObject _scrollRowHolder;
        private LoadingIcon _loadingIcon;

        public void ShowNextWindow()
        {
            switch (CurrentWindow)
            {
                case WindowType.None:
                    {
                        ActivateWindow(WindowType.NewGameWindow);
                        break;
                    }
                case WindowType.NewGameWindow:
                    {
                        DeactivateWindow(CurrentWindow);
                        ActivateWindow(WindowType.InfoWindow);
                        break;
                    }
                case WindowType.InfoWindow:
                    {
                        DeactivateWindow(CurrentWindow);
                        ActivateWindow(WindowType.GameOverWindow);
                        break;
                    }
                case WindowType.GameOverWindow:
                    {
                        DeactivateWindow(CurrentWindow);
                        ActivateWindow(WindowType.InfoWindow);
                        break;
                    }
            }
        }

        public LoadingIcon GetLoadingIcon()
        {
            return _loadingIcon;
        }

        public InputField GetNicknameInputField()
        {
            return _nicknameInputField;
        }

        public GameObject GetScoreRowHolder()
        {
            return _scrollRowHolder;
        }

        public T[] GetButtonsInWindows<T>() where T : Button
        {
            return _buttons.Select(button => button as T)
                           .Where(button => button != null)
                           .ToArray();
        }

        public void SetInfoText(InfoTextType infoTextType, string value)
        {
            foreach (var infoText in _allInfoTexts[infoTextType])
            {
                infoText.text = value;
            }
        }

        private void Start()
        {
            RegisterWindows();
            RegisterAllInfoText();
            RegisterButtons();
            RegisterNicknameInputField();
            RegisterScrollRowHolder();
            RegisterLoadingIcon();
            HideAllActiveWindows();
            CurrentWindow = WindowType.None;
        }

        private void RegisterLoadingIcon()
        {
            _loadingIcon = GetComponentInChildren<LoadingIcon>(true);
        }

        private void RegisterScrollRowHolder()
        {
            _scrollRowHolder = GetComponentInChildren<ContentSizeFitter>(true).gameObject;
        }

        private void ActivateWindow(WindowType windowType)
        {
            var window = _allWindows[windowType];
            window.SetActive(true);
            CurrentWindow = windowType;
        }

        private void DeactivateWindow(WindowType windowType)
        {
            var window = _allWindows[windowType];
            window.SetActive(false);
        }

        private void RegisterAllInfoText()
        {
            var textInfos = GetComponentsInChildren<TextInfo>(true);

            foreach (var textInfo in textInfos)
            {
                if(!_allInfoTexts.ContainsKey(textInfo.ElementType))
                {
                    _allInfoTexts.Add(textInfo.ElementType , new List<Text>());
                }

                _allInfoTexts[textInfo.ElementType].Add(textInfo.GetComponent<Text>());
            }
        }

        private void RegisterWindows()
        {
            var windows = GetComponentsInChildren<Window>(true);

            foreach (var window in windows)
            {
                _allWindows.Add(window.WindowType, window.gameObject);
            }
        }

        private void RegisterButtons()
        {
            _buttons = _allWindows.Select(keyValuePair => keyValuePair.Value.GetComponentInChildren<Button>(true))
                                  .Where(button => button != null)
                                  .ToArray();
        }

        private void RegisterNicknameInputField()
        {
            _nicknameInputField = _allWindows[WindowType.NewGameWindow].GetComponentInChildren<InputField>(true);
        }

        private void HideAllActiveWindows()
        {
            foreach (var window in _allWindows)
            {
                if (window.Value.activeInHierarchy)
                {
                    DeactivateWindow(window.Key);
                }
            }
        }
    }
}
