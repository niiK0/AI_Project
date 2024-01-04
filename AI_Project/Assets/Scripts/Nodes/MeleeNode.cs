using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.AI;

public class MeleeNode : Node
{
    private NavMeshAgent agent;
    private BossAI ai;

    public MeleeNode(NavMeshAgent agent, BossAI ai)
    {
        this.agent = agent;
        this.ai = ai;
    }

    public override NodeState Evaluate()
    {
        agent.isStopped = true;
        ai.SetColor(Color.red);
        //Instanciar projeteis
        return NodeState.RUNNING;
    }
}
