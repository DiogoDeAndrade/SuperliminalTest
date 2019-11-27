using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestScaler : MonoBehaviour
{
    public Camera baseCamera;

    float startDistance;

    private void Start()
    {
        startDistance = float.MaxValue;
    }

    void Update()
    {
        if (baseCamera == null) return;

        if (startDistance == float.MaxValue)
        {
            startDistance = Vector3.Distance(baseCamera.transform.position, transform.position);
            transform.localScale = Vector3.one;
        }

        // Scale object
        float distance = Vector3.Distance(baseCamera.transform.position, transform.position);

        float s = distance * baseCamera.projectionMatrix.m00;
        transform.localScale = new Vector3(s, s, s);
    }
}
