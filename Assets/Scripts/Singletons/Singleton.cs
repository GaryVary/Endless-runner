using UnityEngine;

namespace Assets.Scripts.Singletons
{
    public abstract class Singleton<T> : MonoBehaviour where T: Singleton<T>
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
#if UNITY_EDITOR
            else
            {
                Debug.LogError("Multiple Instances of " + ToString());
            }
#endif
        }
    }
}
