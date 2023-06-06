using UnityEngine;

public class PatrolState : State
{
    private EnemyBase owner;

    public PatrolState(EnemyBase enemy)
    {
        owner = enemy;
    }

    public override void OnEnter()
    {
        Debug.Log("PATROL");

        owner.SetViewState(StateType.PATROL);
        owner.SetSpeed(StateType.PATROL);
    }

    public override void OnExit()
    {

    }

    public override void Update()
    {
        if (owner.IsInViewRange())
        {
            fsm.GoTo(StateType.CHASE);
        }
        else
        {
            if (!owner.walkPointSet) SearchWalkPoint();

            // Calculate direction and walk to Point
            if (owner.walkPointSet)
            {
                owner.agent.SetDestination(owner.walkPoint);
            }

            // Calculates DistanceToWalkPoint
            Vector3 distanceToWalkPoint = owner.transform.position - owner.walkPoint;

            // Walkpoint reached
            if (distanceToWalkPoint.magnitude < 1f)
            {
                owner.walkPointSet = false;
            }
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-owner.walkPointRange, owner.walkPointRange);
        float randomX = Random.Range(-owner.walkPointRange, owner.walkPointRange);

        owner.walkPoint = new Vector3(owner.transform.position.x + randomX, owner.transform.position.y, owner.transform.position.z + randomZ);

        if (Physics.Raycast(owner.walkPoint, -owner.transform.up, 2, owner.LayerNav))
        {
            owner.walkPointSet = true;
        }
    }
}
