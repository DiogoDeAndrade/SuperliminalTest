using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grabber : MonoBehaviour
{
    public Image    cursor;
    public Color    normalColor = Color.yellow;
    public Color    highlightColor = Color.red;

    Transform   castStartTransform;
    Interactive grabbedObject;
    Camera      grabCamera;

    void Start()
    {
        cursor.color = normalColor;

        castStartTransform = transform;

        var fps = GetComponent<FPSCamera>();
        if (fps)
        {
            castStartTransform = fps.rotationTarget;
            grabCamera = fps.GetComponentInChildren<Camera>();
        }
    }

    void Update()
    {
        Interactive currentObject = null;
        
        Ray ray = new Ray(castStartTransform.position, castStartTransform.forward);

        RaycastHit[] hitInfos = Physics.SphereCastAll(ray, 0.5f, 200.0f, LayerMask.GetMask("Interactive"));
        foreach (var hitInfo in hitInfos)
        {
            currentObject = hitInfo.collider.GetComponent<Interactive>();
            if (currentObject == null)
            {
                currentObject = hitInfo.collider.GetComponentInParent<Interactive>();
                break;
            }            
        }

        if (currentObject)
        {
            cursor.color = highlightColor;
        }
        else
        {
            cursor.color = normalColor;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedObject)
            {
                grabbedObject.ThrowObjectConstantSize(castStartTransform.forward, grabCamera,
                    (b) =>
                    {
                        grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                        grabbedObject = null;
                        cursor.enabled = true;
                    });
            }
            else
            {
                if (currentObject)
                {
                    grabbedObject = currentObject;

                    float   m00 = grabCamera.projectionMatrix.m00;
                    float   distance = Vector3.Distance(grabbedObject.transform.position, castStartTransform.position);
                    float   ds = distance * m00;
                    Vector3 s = grabbedObject.transform.localScale / ds;

                    grabbedObject.transform.SetParent(castStartTransform);
                    grabbedObject.transform.localPosition = new Vector3(0, 0, 1.0f);
                    grabbedObject.transform.localScale = s * 1.0f * m00;
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    cursor.enabled = false;
                }                
            }
        }
    }
}
