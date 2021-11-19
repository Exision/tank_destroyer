using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMove: MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private GameObject _target;

        private float _moveSpeed;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetTarget(GameObject target) => 
            _target = target;

        public void SetMoveSpeed(float speed)
        {
            _moveSpeed = speed;
            _rigidbody.simulated = true;
        }

        private void FixedUpdate()
        {
            if (NeedMove())
            {
                Rotate();
                Move();
            }
            else
            {
                Stop();
            }
        }

        private bool NeedMove() => 
            _target != null;

        private void Rotate()
        {
            var direction = (Vector2)(_target.transform.position - transform.position);
            var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            var angleAxis = Quaternion.Euler(0,0,-angle);

            transform.rotation = angleAxis;
        }

        private void Move() => 
            _rigidbody.velocity = transform.up * _moveSpeed * Time.fixedDeltaTime;

        private void Stop()
        {
            _rigidbody.velocity = Vector2.zero;
            _rigidbody.simulated = false;
        }
    }
}