using Assets.Scripts.Enums;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class Window : MonoBehaviour
    {
        [SerializeField]
        private WindowType _windowType;

        public WindowType WindowType
        {
            get
            {
                return _windowType;
            }
        }
    }
}
