using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSweepTest : MonoBehaviour
{
    public Transform phantom;
    public bool      debugHitPoint = false;
    public bool      debugMidFrameScale = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        RaycastHit hitInfo;
        if (rb.SweepTest(transform.forward, out hitInfo))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * hitInfo.distance);
            if (debugHitPoint)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(hitInfo.point, 0.1f);
            }

            if (debugMidFrameScale)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(transform.position + transform.forward * hitInfo.distance, 0.1f);
                
                Vector3 scale = transform.localScale;
                transform.localScale = scale * 2.0f;

                RaycastHit hitInfo2;
                if (rb.SweepTest(transform.forward, out hitInfo2))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(transform.position + transform.forward * hitInfo2.distance, 0.1f);
                }

                transform.localScale = scale;
            }

            if (phantom)
            {
                phantom.transform.position = transform.position + transform.forward * hitInfo.distance;
                phantom.transform.rotation = transform.rotation;
                phantom.transform.localScale = transform.localScale;
            }
        }
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1000);
        }
    }
}
