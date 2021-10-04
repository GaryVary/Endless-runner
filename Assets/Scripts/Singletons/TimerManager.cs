using Assets.Scripts.Delegates;
using Assets.Scripts.Enums;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Singletons
{
    public class TimerManager : Singleton<TimerManager>
    {
        public const int MaxGameTime = 60;

        public event GenericDelegate<int> RemainingTimeChanged;
        public event BasicDelegate TimeIsUpEvent;
        
        private int remainingTime;
        private Coroutine countdownCoroutine;

        private void Start()
        {
            GameManager.Instance.GameConditionChanged += HandleGameCondition;
        }

        private void HandleGameCondition(GameCondition gameCondition)
        {
            switch(gameCondition)
            {
                case GameCondition.InProgress:
                    {
                        countdownCoroutine = StartCoroutine(StartCountdown());
                        break;
                    }
                case GameCondition.End:
                    {
                        if (countdownCoroutine != null)
                        {
                            StopCoroutine(countdownCoroutine);
                        }
                        break;
                    }
            }
        }

        private IEnumerator StartCountdown()
        {
            remainingTime = MaxGameTime;

            while (remainingTime > -1)
            {
                RemainingTimeChanged.Invoke(remainingTime);
                yield return new WaitForSecondsRealtime(1);
                remainingTime -= 1;  
            }
            TimeIsUpEvent.Invoke();
        }
    }
}
