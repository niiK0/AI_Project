using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolPointNode : Node
{
    private Vector3[] points;
    private Vector3 currentPoint;
    private BossAI ai;

    public PatrolPointNode(Vector3[] points, Vector3 currentPoint, BossAI ai)
    {
        this.points = points;
        this.currentPoint = currentPoint;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        Vector3 nextPoint = FindNextPoint();
        if(nextPoint != Vector3.zero)
            ai.SetCurrentPoint(nextPoint);

        return nextPoint == Vector3.zero ? NodeState.FAILURE : NodeState.SUCCESS;
    }

    Vector3 FindNextPoint()
    {
        foreach(Vector3 point in points)
        {
            if(point != currentPoint)
            {
                return point;
            }
        }
        return Vector3.zero;
    }
}
