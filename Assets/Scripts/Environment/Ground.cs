using Assets.Scripts.Singletons;
using Assets.Scripts.Utility;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class Ground : MonoBehaviour
    {
        private const float GroundSpeed = 1;

        private SpriteRenderer groundSprite;
        private float additionalSpeed;

        private void Start()
        {
            groundSprite = GetComponent<SpriteRenderer>();
            TimerManager.Instance.RemainingTimeChanged += CalculateAdditionalSpeed;
        }

        private void Update()
        {
            var textureOffset = groundSprite.material.mainTextureOffset;
            textureOffset.x += Time.deltaTime * (GroundSpeed + additionalSpeed) / 20;
            groundSprite.material.mainTextureOffset = textureOffset;
        }

        private void CalculateAdditionalSpeed(int remainingTime)
        {
            additionalSpeed = Helper.CalculateAdditionalSpeedByRemainingTime(TimerManager.MaxGameTime, remainingTime, TimerManager.MaxGameTime);
        }
    }
}