#if UNITY_ANDROID || UNITY_EDITOR
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Controls.PlatformDependentControls
{
    public class OnScreenStickPlacer : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField]
        private GameObject virtualJoystickPrefab;
        private GameObject virtualJoystickInstance;
        private RectTransform virtualJoystickTransform;

        private void Start()
        {
            virtualJoystickInstance = Instantiate(virtualJoystickPrefab, transform.parent);
            virtualJoystickTransform = virtualJoystickInstance.GetComponent<RectTransform>();
            virtualJoystickInstance.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            virtualJoystickTransform.anchoredPosition = eventData.pressPosition;
            virtualJoystickInstance.SetActive(true);
        }
    }
}
#endif
