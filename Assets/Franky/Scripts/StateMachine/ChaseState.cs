using UnityEngine;

public class ChaseState : State
{
    private EnemyBase owner;

    public ChaseState(EnemyBase enemy)
    {
        owner = enemy;
    }

    public override void OnEnter()
    {
        Debug.Log("CHASE");

        owner.SetViewState(StateType.CHASE);
    }

    public override void OnExit()
    {

    }

    public override void Update()
    {
        if (!owner.IsInViewRange())
        {
            fsm.GoTo(StateType.PATROL);
        }
        else if (owner.IsInAttackRange())
        {
            fsm.GoTo(StateType.ATTACK);
        }
        else
        {
            owner.agent.SetDestination(owner.targetPlayer.position);
        }
    }
}
