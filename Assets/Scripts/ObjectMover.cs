using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    public Spline spline;          // Reference to the spline
    public float speed = 2f;       // Speed of movement along the spline
    float SetSpeed;
    private float t = 0f;          // Spline interpolation value (0 to 1)

    public int SlowDownDistance = 10;

    bool SlowedDown = false;

    private Quaternion initialRotation;

    GameObject AttachedGO;

    private void Start()
    {
        SetSpeed = speed;
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        if (spline == null || spline.controlPoints.Count < 2)
            return;

        // Move the object along the spline
        t += speed * Time.deltaTime / spline.curveResolution;
        t = Mathf.Clamp01(t); // Clamp between 0 and 1

        transform.position = spline.GetSplinePoint(t);


        if (GetDistanceToGoal() < SlowDownDistance)
        {
            if (!SlowedDown)
            {
                SlowedDown = true;
                speed = speed / 2;
            }
        }
        else if(GetDistanceFromStart() < SlowDownDistance)
        {
            if (!SlowedDown)
            {
                SlowedDown = true;
                speed = speed / 2;
            }
        }
        else
        {

            SlowedDown = false;
            speed = SetSpeed;
        }

        // Optional: Face the direction of travel
        if (t < 1f)
        {
            Vector3 nextPosition = spline.GetSplinePoint(Mathf.Min(t + 0.01f, 1f));
            transform.forward = (nextPosition - transform.position).normalized;
        }
        else
        {
            ResetPosition();
        }
        transform.rotation = initialRotation * Quaternion.Euler(0, 10 * Mathf.Sin(t * Mathf.PI), 0);
    }

    private void ResetPosition()
    {
        if(AttachedGO != null)
        {
            AttachedGO.GetComponent<PlayerStateMachine>().enabled = true;
            AttachedGO.GetComponent<Animator>().enabled = true;
            AttachedGO.GetComponent<Animation>().enabled = false;
            AttachedGO.transform.SetParent(null);
        }   

        t = 0f; // Reset interpolation value
        transform.position = spline.GetSplinePoint(t); // Update position

        transform.rotation = initialRotation * Quaternion.Euler(0, 10 * Mathf.Sin(t * Mathf.PI), 0);
    }

    public float GetDistanceToGoal()
    {
        if (spline == null || spline.controlPoints.Count < 2)
            return 0f;

        // Get the position of the last control point
        Vector3 goalPosition = spline.GetSplinePoint(1f); // 1f represents the end of the spline

        // Calculate and return the distance to the goal
        return Vector3.Distance(transform.position, goalPosition);
    }

    public float GetDistanceFromStart()
    {
        if (spline == null || spline.controlPoints.Count < 2)
            return 0f;

        // Calculate distance along the spline from t = 0 to the current t
        float distance = 0f;
        Vector3 previousPoint = spline.GetSplinePoint(0f); // Start of the spline

        // Sample the spline at intervals up to the current t
        int samples = Mathf.CeilToInt(t * spline.curveResolution); // Number of samples
        for (int i = 1; i <= samples; i++)
        {
            float sampleT = i / (float)spline.curveResolution;
            sampleT = Mathf.Min(sampleT, t); // Clamp to the current t
            Vector3 currentPoint = spline.GetSplinePoint(sampleT);
            distance += Vector3.Distance(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }

        return distance;
    }

    private void OnTriggerEnter(Collider collision)
    {
        collision.gameObject.GetComponent<PlayerStateMachine>().enabled = false;

        collision.gameObject.GetComponent<Animator>().enabled = false;
        collision.gameObject.GetComponent<Animation>().enabled = true;

        collision.gameObject.transform.SetParent(transform);

        collision.gameObject.transform.localPosition = Vector3.zero - new Vector3(0, 0.5f, 0);

        AttachedGO = collision.gameObject;
    }
    private void OnTriggerExit(Collider collision)
    {
        collision.gameObject.GetComponent<PlayerStateMachine>().enabled = true;

        collision.gameObject.GetComponent<Animator>().enabled = true;
        collision.gameObject.GetComponent<Animation>().enabled = false;
        collision.gameObject.transform.SetParent(null);
    }

}
