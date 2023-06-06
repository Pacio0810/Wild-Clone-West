using UnityEngine;

public class AttackState : State
{
    private EnemyBase owner;

    public AttackState(EnemyBase enemy)
    {
        owner = enemy;
    }

    public override void OnEnter()
    {
        Debug.Log("ATTACK");

        owner.SetViewState(StateType.ATTACK);
        owner.SetSpeed(StateType.ATTACK);

    }

    public override void OnExit()
    {

    }

    public override void Update()
    {
        if (!owner.IsInAttackRange())
        {
            if(owner.IsInViewRange())
            {
                fsm.GoTo(StateType.CHASE);
            }
            else
            {
                fsm.GoTo(StateType.CHASE);
            }
        }
        else
        {
            //Make sure enemy doesn't move
            owner.agent.SetDestination(owner.transform.position);

            owner.transform.LookAt(owner.targetPlayer);

            owner.Attack();

            Debug.Log("SHOOT");
        }
    }
}
