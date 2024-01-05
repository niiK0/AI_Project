using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : Node
{
    private Transform target;
    private Vector3 currentPoint;
    private BossAI ai;

    public PatrolNode(Transform target, Vector3 currentPoint, BossAI ai)
    {
        this.target = target;
        this.currentPoint = currentPoint;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(target.position, currentPoint);
        if(distance > 0.1f)
        {
            ai.Patrol();
            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.SUCCESS;
        }
    }
}
