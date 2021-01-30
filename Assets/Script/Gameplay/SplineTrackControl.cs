using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineTrackControl : MonoBehaviour
{
    [Tooltip("Object that moves along the track")]
    public GameObject ObjectToMove;

    [Tooltip("How fast to go from 0 to 1, in seconds")]
    public float MoveSpeed = 5.0f;
    [Range(0.0f, 1.0f), Tooltip("Starting Position on the whole track")]
    public float StartingPosition = 0.0f;
    [Tooltip("True if the position gets bigger over time")]
    public bool GoForwardAtStart = true;
    [Tooltip("If we go back and forth")]
    public bool RepeatRoute = true;
    [Tooltip("Moving at Start")]
    public bool MovingAtStart = true;

    float currentRatio;
    float TotalDistance = 0.0f;
    float[] ChildrenDistance;
    bool bGoingForward;
    bool bMoving;
    void Start()
    {
        TotalDistance = 0.0f;
        ChildrenDistance = new float[transform.childCount - 1];
        for (int i = 0; i < transform.childCount - 1; ++i)
        {
            ChildrenDistance[i] = (transform.GetChild(i + 1).position - transform.GetChild(i).position).magnitude;
            TotalDistance += ChildrenDistance[i];
        }
        bGoingForward = GoForwardAtStart;
        currentRatio = StartingPosition;
        bMoving = MovingAtStart;
    }

    // Update is called once per frame
    void Update()
    {
        if(bMoving)
        {
            if (bGoingForward)
            {
                currentRatio += Time.deltaTime / MoveSpeed;
                if (currentRatio > 1.0f)
                {
                    bGoingForward = !RepeatRoute;
                }
            }
            else
            {
                currentRatio -= Time.deltaTime / MoveSpeed;
                if (currentRatio < 0.0f)
                {
                    bGoingForward = RepeatRoute;
                }
            }

            ObjectToMove.transform.position = GetPosition(currentRatio);
        }
    }

    Vector3 GetPosition(float PositionRatio)
    {
        if (transform.childCount == 0)
            return transform.position;

        if (transform.childCount == 1)
            return transform.GetChild(0).position;


        for (int i = 0; i < transform.childCount - 1; ++i)
        {
            float currentRationInDistance = ChildrenDistance[i] / TotalDistance;
            if(PositionRatio < currentRationInDistance)
            {
                Vector3 StartPosition = transform.GetChild(i).position;
                Vector3 EndPosition = transform.GetChild(i+1).position;
                return Vector3.Lerp(StartPosition, EndPosition, PositionRatio / currentRationInDistance);
            }
            else
            {
                PositionRatio -= currentRationInDistance;
            }
        }

        // we shouldn't reach here.
        return transform.position;
    }

    public void StartMoving()
    {
        bMoving = true;
    }
    public void StopMoving()
    {
        bMoving = false;
    }

}
