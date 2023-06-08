using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController3D : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;

    [SerializeField]
    private float playerSpeed = 2.0f;

    [SerializeField]
    private HealthObject health;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(playerSpeed * Time.deltaTime * move);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
            if (animator != null) animator.SetInteger("Walking", 1);
        }
        else
        {
            if (animator != null) animator.SetInteger("Walking", 0);
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
