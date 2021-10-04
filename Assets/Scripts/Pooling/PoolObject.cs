using UnityEngine;

namespace Assets.Scripts.Pooling
{
    public class PoolObject : MonoBehaviour
    {
        public PoolManager ParentPoolManager
        {
            set;
            private get;
        }

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
            transform.localPosition = new Vector3(0, 0, 0);
        }

        public void ReturnToPool()
        {
            ParentPoolManager.ReturnObjectToPool(this);
        }
    }
}
