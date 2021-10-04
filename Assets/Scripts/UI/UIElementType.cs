using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UIElementType<T> : MonoBehaviour
    {
        [SerializeField]
        private T _elementType;

        public T ElementType
        {
            get
            {
                return _elementType;
            }
        }
    }
}
