using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    Rigidbody   rb;

    List<Vector3> testedPositions;
    List<float>   testedScale;
    bool          processing = false;
    GameObject    objectCopy;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
    }

    public bool ThrowObjectConstantSize(Vector3 dir, Camera camera, System.Action<bool> onDone)
    {
        if (processing) return false;

        processing = true;

        // Copy this object
        objectCopy = Instantiate(gameObject);
        objectCopy.transform.position = transform.position;
        objectCopy.transform.rotation = transform.rotation;
        objectCopy.transform.localScale = transform.localScale;

        // Remove all components except transform, rigidbody and colliders,
        // and convert all colliders to triggers (and delete all trigger colliders)
        var components = objectCopy.GetComponentsInChildren<Component>();
        foreach (var component in components)
        {
            if ((component is Rigidbody) ||
                (component is Transform)) continue;

            if (component is Collider)
            {
                Collider collider = component as Collider;
                if (!collider.isTrigger)
                {
                    collider.isTrigger = true;
                    continue;
                }
            }

            Destroy(component);
        }

        StartCoroutine(CheckCollisionFixedSize(dir, camera, onDone));

        return false;
    }

    IEnumerator CheckCollisionFixedSize(Vector3 dir, Camera camera, System.Action<bool> onDone)
    {
        testedPositions = new List<Vector3>();
        testedScale = new List<float>();

        Rigidbody   rb = objectCopy.GetComponent<Rigidbody>();
        var         matrix = camera.projectionMatrix;
        Vector3     startPos = objectCopy.transform.position;
        float       startScale = objectCopy.transform.localScale.x;
        Vector3     pos = startPos;
        int         nIterations = 0;
        float       dist = 0.0f;
        float       inc = 1.0f;
        Vector3     prevPos, prevScale;
        RaycastHit  hitInfo;

        while (true)
        {
            dist += inc;

            prevPos = pos;
            prevScale = objectCopy.transform.localScale;
            pos = startPos + dir * dist;

            float objDist = Vector3.Distance(camera.transform.position, pos);
            float scale = startScale * matrix.m00 * objDist;

            objectCopy.transform.localScale = new Vector3(scale, scale, scale);

            yield return null;

            testedPositions.Add(pos);
            testedScale.Add(scale);

            if (rb.SweepTest(dir, out hitInfo, Vector3.Distance(transform.position, pos)))
            {
                transform.position = prevPos;
                transform.localScale = prevScale;
                transform.SetParent(null);
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                onDone(true);

                break;
            }

            nIterations++;
            if (nIterations > 200)
            {
                onDone(false);

                break;
            }
        }

        Destroy(objectCopy);
        objectCopy = null;
        processing = false;
    }

    private void OnDrawGizmos()
    {
        float baseSize = 0.1f;

        if (testedPositions != null)
        {
            for (int i = 0; i < testedPositions.Count; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(testedPositions[i], testedScale[i] * baseSize);
            }
        }
    }
}
