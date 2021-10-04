using UnityEngine;

namespace Assets.Scripts.Pooling
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField]
        private PoolObject _poolObjectPrefab;
        private ObjectPool<PoolObject> _objectPool;

        public void PlaceObjectOnScene(float positionY)
        {
            var pooledObject = _objectPool.GetFromPool();
            var position = pooledObject.transform.localPosition;
            position.y = positionY;
            pooledObject.transform.localPosition = position;
        }

        public void ReturnAllObjectsToPool()
        {
            _objectPool.ReturnAll();
        }

        public void ReturnObjectToPool(PoolObject poolObject)
        {
            DeactivatePoolObject(poolObject);
        }

        private void Awake()
        {
            _objectPool = new ObjectPool<PoolObject>(CreatePoolObject, ActivatePoolObject, DeactivatePoolObject);
        }

        private PoolObject CreatePoolObject()
        {
            var newInstance = Instantiate(_poolObjectPrefab, transform);
            newInstance.ParentPoolManager = this;
            return newInstance;
        }

        private void ActivatePoolObject(PoolObject poolObject)
        {
            poolObject.Activate();
        }

        private void DeactivatePoolObject(PoolObject poolObject)
        {
            poolObject.Deactivate();
        }
    }
}
