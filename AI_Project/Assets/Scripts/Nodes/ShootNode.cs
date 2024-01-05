using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShootNode : Node
{
    private BossAI ai;
    private float cw;

    public ShootNode(float cw, BossAI ai)
    {
        this.ai = ai;
        this.cw = cw;
    }

    public override NodeState Evaluate()
    {
        if (cw <= 0)
        {
            ai.Shoot();
            cw = 1;
            return NodeState.SUCCESS;
        }
        else
        {
            return NodeState.RUNNING;
        }
    }
}
