using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 1;
    public Transform playerBody;

    private float XRotation = 0.0f;

    [SerializeField]
    InputActionAsset inputActions;
    InputAction mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        mousePosition = inputActions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = mousePosition.ReadValue<Vector2>().x * mouseSensitivity;
        float mouseY = mousePosition.ReadValue<Vector2>().y * mouseSensitivity;

        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90.0f, 90.0f);

        transform.localRotation = Quaternion.Euler(XRotation, 0.0f, 0.0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
