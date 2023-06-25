using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerController3D : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 2.0f;

    [SerializeField]
    private HealthObject health;

    private CharacterController _controller;

    private Animator _animator;

    private Vector3 _moveDirection;

    private float _gravity = 9.81f;

    private float _fallSpeed = 0f;

    private bool _isFalling;

    private void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        _moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        _moveDirection.Normalize();

        if (_controller.isGrounded)
        {
            _isFalling = false;
            _fallSpeed = 0f;

            _controller.Move(playerSpeed * Time.deltaTime * _moveDirection);

            if (_moveDirection.magnitude > 0)
            {
                _animator.SetInteger("Walking", 1);
                transform.rotation = Quaternion.LookRotation(_moveDirection);
            }
            else
            {
                _animator.SetInteger("Walking", 0);
            }
        }
        else if (!_isFalling)
        {
            _isFalling = true;
            _fallSpeed = 0f;
        }
        else
        {
            _fallSpeed -= _gravity * Time.deltaTime;
            _controller.Move(new Vector3(_moveDirection.x, _fallSpeed, _moveDirection.z) * Time.deltaTime);
        }
    }

    public void Heal(float value)
    {
        if (health.currentHealth + value > health.MaxHealth)
        {
            health.currentHealth = health.MaxHealth;
            return;
        }
        health.currentHealth += value;
    }
}
