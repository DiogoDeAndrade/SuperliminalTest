using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    public float        moveSpeed = 1.0f;
    public float        rotateSpeed = 90.0f;
    public Transform    rotationTarget;

    [Range(0.0f, 1.0f)]
    public float drag = 0.0f;

    Rigidbody rb;
    Vector3   lastMousePos;
    Vector3   currentRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        lastMousePos = Input.mousePosition;
        currentRotation = rotationTarget.rotation.eulerAngles;
    }

    void FixedUpdate()
    {
        Vector3 newVelocity = rb.velocity;

        newVelocity *= (1.0f - drag);

        Vector3 f = rotationTarget.forward; f.y = 0.0f; f.Normalize();
        Vector3 r = rotationTarget.right; r.y = 0.0f; r.Normalize();

        newVelocity += f * Input.GetAxis("Vertical") * moveSpeed * Time.fixedDeltaTime;
        newVelocity += r * Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;

        rb.velocity = newVelocity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            currentRotation = Vector3.zero;
        }

        Vector2 mouseDelta = Input.mousePosition - lastMousePos;
        lastMousePos = Input.mousePosition;

        currentRotation.y += mouseDelta.x * rotateSpeed * Time.deltaTime;
        currentRotation.x = Mathf.Clamp(currentRotation.x - mouseDelta.y * rotateSpeed * Time.deltaTime, -70.0f, 70.0f);

        rotationTarget.rotation = Quaternion.Euler(currentRotation);
    }
}
