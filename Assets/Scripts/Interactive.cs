using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    Rigidbody   rb;

    List<Vector3> testedPositions;
    List<float>   testedScale;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ProjectObject(Vector3 dir, Camera camera)
    {
        testedPositions = new List<Vector3>();
        testedScale = new List<float>();

        var         matrix = camera.projectionMatrix;
        Vector3     startPos = transform.position;
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
            prevScale = transform.localScale;
            pos = startPos + dir * dist;

            float objDist = Vector3.Distance(camera.transform.position, pos);
            float scale = matrix.m00 * objDist;

//            transform.position = pos;
            transform.localScale = new Vector3(scale, scale, scale);

            testedPositions.Add(pos);
            testedScale.Add(scale);

            if (rb.SweepTest(dir, out hitInfo, Vector3.Distance(transform.position, pos)))
            {
                transform.position = prevPos;
                transform.localScale = prevScale;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                return true;
            }

            nIterations++;
            if (nIterations > 200)
            {
                Debug.LogWarning("Max iterations exceeded!");
                return false;
            }
        }

        return true;
    }

    public Vector3 GetTargetPosition(Vector3 dir, Camera camera, out float scale)
    {
        RaycastHit hitInfo;
        scale = 1.0f;
        if (rb.SweepTest(dir, out hitInfo))
        {
            var matrix = camera.projectionMatrix;

            scale = matrix.m00 * hitInfo.distance;

            return transform.position + dir * hitInfo.distance;
        }

        return transform.position;
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
