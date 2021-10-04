using Assets.Scripts.Pooling;
using Assets.Scripts.Singletons;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    [RequireComponent(typeof(Collider2D))]
    public class BaseObject : PoolObject
    {
        [SerializeField]
        private float objectSpeed;
        public float additionalSpeed;

        public void Start()
        {
            TimerManager.Instance.RemainingTimeChanged += CalculateAdditionalSpeed;
        }

        public void Update()
        {
            transform.Translate(Vector2.left * Mathf.Lerp(objectSpeed, objectSpeed + additionalSpeed, 0.5f) * Time.deltaTime);
        }

        private void CalculateAdditionalSpeed(int remainingTime)
        {
            additionalSpeed = Helper.CalculateAdditionalSpeedByRemainingTime(TimerManager.MaxGameTime, remainingTime, TimerManager.MaxGameTime / 4);
        }
    }
}
