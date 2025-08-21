using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float speed = 5f; 
    public float sensitivity = 2f; 

    private float rotationX = 0f;
    private float rotationY = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mausbewegung für Rotation
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f); // Begrenze vertikale Rotation

        transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);

        // Tastenbewegung (WASD oder Pfeiltasten)
        float horizontal = Input.GetAxis("Horizontal"); // A/D oder Pfeiltasten links/rechts
        float vertical = Input.GetAxis("Vertical");     // W/S oder Pfeiltasten hoch/runter
        Vector3 movement = transform.forward * vertical + transform.right * horizontal;

        transform.position += movement * speed * Time.deltaTime;

    }
}
