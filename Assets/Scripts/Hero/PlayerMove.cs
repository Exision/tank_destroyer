using Services;
using UnityEngine;
using Zenject;

namespace Hero
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMove: MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private IInputService _inputService;

        private float _moveSpeed;
        private float _rotationSpeed;

        [Inject]
        private void Construct(IInputService inputService)
        {
            _inputService = inputService;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnEnable() => 
            _inputService.Axis += UpdateMovement;

        private void OnDisable() => 
            _inputService.Axis -= UpdateMovement;

        public void Initialize(float moveSpeed, float rotationSpeed)
        {
            _moveSpeed = moveSpeed;
            _rotationSpeed = rotationSpeed;
        }

        private void UpdateMovement(Vector2 input)
        {
            Move(input);
            Rotate(input);
        }

        private void Move(Vector2 input) => 
            _rigidbody.velocity = transform.up * input.y * _moveSpeed * Time.fixedDeltaTime;

        private void Rotate(Vector2 input) => 
            _rigidbody.MoveRotation(transform.rotation * Quaternion.Euler(0, 0, -input.x * _rotationSpeed * Time.fixedDeltaTime));
    }
}