using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    public CharacterController controller;

    [Header("Movement Properties")]
    public float maxSpeed = 10.0f;
    public float gravity = -30.0f;
    public float jumpHeight = 3.0f;
    public Vector3 velocity;

    [Header("Ground Detection Properties")]
    public Transform groundPoint;
    public float groundRadius = 0.5f;
    public LayerMask groundMask;
    public bool isGrounded;

    [Header("Health Properties")]
    public float maxHealth = 100f;
    public float currentHealth;
    public HealthBarUI healthBar;  

    [SerializeField]
    InputActionAsset inputActions;
    InputAction movementInput;
    InputAction jumpInput;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        movementInput = inputActions.FindAction("Move");
        jumpInput = inputActions.FindAction("Jump");

        currentHealth = maxHealth;
        if (healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundPoint.position, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0.0f)
            velocity.y = -2.0f;

        float x = movementInput.ReadValue<Vector2>().x;
        float z = movementInput.ReadValue<Vector2>().y;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * maxSpeed * Time.deltaTime);

        if (jumpInput.IsPressed() && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);

        if (healthBar != null)
            healthBar.SetHealth(currentHealth);
    }

    void Die()
    {
        Debug.Log("Player died!");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ur mom"))
        {
            Debug.Log("owie");
            TakeDamage(10f); 
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundPoint.position, groundRadius);
    }
}