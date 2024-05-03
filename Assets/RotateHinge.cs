using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHinge : MonoBehaviour
{
    public float rotationSpeed = 20f;
    private Transform hingeTransform;
    private float targetAngle = -35f;
    private bool isRotating = false;

    void Start()
    {
        hingeTransform = transform.Find("hinge");

        if (hingeTransform == null)
        {
            Debug.LogError("Hinge object not found!");
            return;
        }

        isRotating = true;
    }

    void Update()
    {
        if (isRotating && hingeTransform != null)
        {
            float currentAngle = hingeTransform.localEulerAngles.x;

            float nextAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

            hingeTransform.localRotation = Quaternion.Euler(nextAngle, -90f, -90f);

            if (Mathf.Approximately(nextAngle, targetAngle))
            {
                isRotating = false;
            }
        }
    }
}