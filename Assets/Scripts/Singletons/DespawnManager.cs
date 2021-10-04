using Assets.Scripts.Pooling;
using Assets.Scripts.Singletons;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Collider2D))]
    public class DespawnManager : Singleton<DespawnManager>
    {
        private const float DistanceBetweenDespawnerAndCanvas = 4;

        private void Start()
        {
            SetPositionAccordingCanvas();
        }

        private void SetPositionAccordingCanvas()
        {
            var position = transform.position;
            position.x = -UIManager.Instance.CanvasSize.x / 2 - DistanceBetweenDespawnerAndCanvas;
            transform.position = position;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var poolObject = collision.gameObject.GetComponent<PoolObject>();
            poolObject.ReturnToPool();
        }
    }
}