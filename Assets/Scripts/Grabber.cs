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

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        Interactive currentObject = null;
        
        Ray ray = new Ray(castStartTransform.position, castStartTransform.forward);

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            currentObject = hitInfo.collider.GetComponent<Interactive>();
            if (currentObject == null)
            {
                currentObject = hitInfo.collider.GetComponentInParent<Interactive>();
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
/*                float   scale;
                    Vector3 pos = grabbedObject.GetTargetPosition(castStartTransform.forward, grabCamera, out scale);*/

                grabbedObject.transform.SetParent(null);

                grabbedObject.ProjectObject(castStartTransform.forward, grabCamera);

//                grabbedObject.transform.position = pos;
//                grabbedObject.transform.localScale = new Vector3(scale, scale, scale);
                grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
                grabbedObject = null;
                cursor.enabled = true;
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
                    grabbedObject.transform.localPosition = new Vector3(0, 0, 2.0f);
                    grabbedObject.transform.localScale = s * 2.0f * m00;
                    grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
                    cursor.enabled = false;
                }                
            }
        }
    }

    /*private void OnDrawGizmos()
    {
        if (castStartTransform)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + castStartTransform.forward * 100);
        }
    }//*/
}
