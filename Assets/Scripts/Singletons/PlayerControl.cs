using Assets.Scripts.Delegates;
using Assets.Scripts.Enums;
using Assets.Scripts.Environment;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Singletons
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerControl : Singleton<PlayerControl>
    {
        private const float MinDistanceBetweenPlayerAndBoundaries = 1;

        public event BasicDelegate PlayerDiedEvent;
        public event GenericDelegate<int> PlayerGainedScoreEvent;

        [SerializeField]
        private float _speed;
        private Vector2 _boundary;
        private Vector2 _moveDirection = Vector2.zero;
        private PlayerInput _objectInput;

        private void HandleGameCondition(GameCondition gameCondition)
        {
            switch (gameCondition)
            {
                case GameCondition.InProgress:
                    {
                        ActivateInput();
                        break;
                    }
                case GameCondition.End:
                    {
                        DeactivateInput();
                        ResetPlayerPosition();
                        break;
                    }
            }
        }

        private void ActivateInput()
        {
            _objectInput.ActivateInput();
        }

        private void DeactivateInput()
        {
            _objectInput.DeactivateInput();
            OnDeviceLost();
        }

        private void OnMove(InputValue inputValue)
        {
            _moveDirection = inputValue.Get<Vector2>();
        }

        private void OnDeviceLost()
        {
            _moveDirection = Vector2.zero;
        }

        private void Start()
        {
            _objectInput = GetComponent<PlayerInput>();
            SubscribeGameConditionHandling();
            SetBoundary();
            DeactivateInput();
            ResetPlayerPosition();
        }

        private void SubscribeGameConditionHandling()
        {
            GameManager.Instance.GameConditionChanged += HandleGameCondition;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var collidedObjectType = collision.gameObject.GetComponent<BaseObject>();

            var asObastacle = collidedObjectType as Obstacle;
            if (asObastacle != null)
            {
                PlayerDiedEvent.Invoke();
                return;
            }

            var asScoreObject = collidedObjectType as ScoreObject;
            if (asScoreObject != null)
            {
                PlayerGainedScoreEvent.Invoke(asScoreObject.ScoreValue);
                asScoreObject.ReturnToPool();
                return;
            }
#if UNITY_EDITOR
            Debug.LogError("Player hit unintended object");
#endif
        }

        private void Update()
        {
            if (_moveDirection != Vector2.zero)
            {
                transform.position = CalculateClampedPosition();
            }
        }

        private void SetBoundary()
        {
            var canvasSize = UIManager.Instance.CanvasSize;
            _boundary = new Vector2(canvasSize.x / 2 - MinDistanceBetweenPlayerAndBoundaries, canvasSize.y / 2 - MinDistanceBetweenPlayerAndBoundaries);
        }

        private Vector2 CalculateClampedPosition()
        {
            var deltaSpeed = _speed * Time.deltaTime;

            return new Vector2(Mathf.Clamp(transform.position.x + _moveDirection.x * deltaSpeed, -_boundary.x, _boundary.x),
                               Mathf.Clamp(transform.position.y + _moveDirection.y * deltaSpeed, -_boundary.y, _boundary.y));
        }

        private void ResetPlayerPosition()
        {
            transform.position = Vector2.zero;
        }
    }
}