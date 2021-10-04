#if UNITY_ANDROID || UNITY_EDITOR
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controls.PlatformDependentControls
{
    public class OnScreenStickHider : MonoBehaviour, IPointerUpHandler
    {
        private GameObject stickPlaceholder;

        private void Start()
        {
            stickPlaceholder = transform.parent.gameObject;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            stickPlaceholder.SetActive(false);
        }
    }
}
#endif