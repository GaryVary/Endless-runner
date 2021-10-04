using Assets.Scripts.Enums;
using Assets.Scripts.Pooling;
using Assets.Scripts.Utility;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Singletons
{
    public class SpawnManager : Singleton<SpawnManager>
    {
        private const float DistanceBetweenObjectAndCanvas = 4;
        private const float MinDistanceBetweenMaxSpawningOffsetAndCanvasBoundaries = 1;

        [SerializeField]
        private PoolManager[] ObstaclePools; 
        private float timeBetweenWaves = 1.5f;
        private float timeBetweenWavesOffset = 0f;

        private float spawningOffset;
        private Coroutine spawningCoroutine;

        private void HandleGameCondition(GameCondition gameCondition)
        {
            switch (gameCondition)
            {
                case GameCondition.InProgress:
                    {
                        StartSpawning();
                        break;
                    }
                case GameCondition.End:
                    {
                        StopSpawning();
                        break;
                    }
            }
        }

        private void HandleTimerChange(int remainingTime)
        {
            timeBetweenWavesOffset = Helper.CalculateAdditionalSpeedByRemainingTime(TimerManager.MaxGameTime, remainingTime, TimerManager.MaxGameTime * 2);
        }

        private void Start()
        {
            SubscribeGameConditionHandling();
            SubscribeOnTimerChangeHandling();
            SetPositionAccordingCanvas();
            SetSpawningYOffset();
        }

        private void SubscribeGameConditionHandling()
        {
            GameManager.Instance.GameConditionChanged += HandleGameCondition;
        }

        private void SubscribeOnTimerChangeHandling()
        {
            TimerManager.Instance.RemainingTimeChanged += HandleTimerChange;
        }
        private void StartSpawning()
        {
            spawningCoroutine = StartCoroutine(SpawnObstacles());
        }

        private void StopSpawning()
        {
            StopCoroutine(spawningCoroutine);
            
            foreach(var poolManager in ObstaclePools)
            {
                poolManager.ReturnAllObjectsToPool();
            }
        }

        private void SetPositionAccordingCanvas()
        {
            var position = transform.position;
            position.x = UIManager.Instance.CanvasSize.x / 2 + DistanceBetweenObjectAndCanvas;
            transform.position = position;
        }

        private void SetSpawningYOffset()
        {
            spawningOffset = UIManager.Instance.CanvasSize.y / 2 - MinDistanceBetweenMaxSpawningOffsetAndCanvasBoundaries;
        }

        private IEnumerator SpawnObstacles()
        {
            while (true)
            {
                var obstacleType = Random.Range(0, ObstaclePools.Length);
                var obstacleYPosition = Random.Range(-spawningOffset, spawningOffset);

                ObstaclePools[obstacleType].PlaceObjectOnScene(obstacleYPosition);

                yield return new WaitForSeconds(timeBetweenWaves - timeBetweenWavesOffset);
            }
        }
    }
}