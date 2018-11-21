using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaunchArcRendrer : MonoBehaviour
{
    //https://en.wikipedia.org/wiki/Projectile_motion

    public float velocity;
    public float angle;
    public int resolution = 10; // to make it smooth or sharp

    float gravity; // the force of gravity on y axis
    float radianAngle; // we use anle in radians not degress

    LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        gravity = Mathf.Abs(Physics2D.gravity.y);
    }

    // when we change in inspector
    private void OnValidate()
    {
        if (lr != null && Application.isPlaying)
        {
            RenderArc();
        }
    }

    private void Start()
    {
        RenderArc();
    }

    // make line renderer with appropriate settings
    void RenderArc()
    {
        lr.positionCount = resolution + 1;
        lr.SetPositions(CalculateArcArray());
    }

    // create an aarray of vector3 position for arc
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / gravity;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution; // progress
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    // calculate height and distance of each vertex
    Vector3 CalculateArcPoint(float t,float maxDistance)
    {
        float x = t * maxDistance; // percent from max position
        float y = x * Mathf.Tan(radianAngle) - ((gravity * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }
}
