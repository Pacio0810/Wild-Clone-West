using UnityEngine;

public class EnemyShooter : EnemyBase
{
    // Temporary
    public GameObject projectile;
    public Transform muzzle;
    public float timeBetweenAttacks;

    bool canAttack;

    private void Start()
    {
        viewAngle = 120;
        viewRadius = 20;
        attackRange = 10;
        walkPointRange = 20;

        patrolSpeed = 5;
        chaseSpeed = 10;
        attackSpeed = chaseSpeed;
        maxAcceleration = 12;
        baseAcceleration = 8;

        canAttack = true;

        stateMachine = new StateMachine();
        stateMachine.AddState(StateType.PATROL, new PatrolState(this));
        stateMachine.AddState(StateType.CHASE, new ChaseState(this));
        stateMachine.AddState(StateType.ATTACK, new AttackState(this));

        stateMachine.GoTo(StateType.PATROL);
    }

    private void Update()
    {
        if (!isDead)
        {
            stateMachine.Update();
        }
    }

    public override void Attack()
    {
        if (canAttack)
        {
            //Attack
            GameObject bang = Instantiate(projectile, muzzle.position, Quaternion.identity);

            bang.GetComponent<Rigidbody>().AddForce(transform.forward * 32f, ForceMode.Impulse);
            //bang.GetComponent<Rigidbody>().AddForce(transform.up * 8, ForceMode.Impulse);

            canAttack = false;

            Invoke("ResetAttack", timeBetweenAttacks);
        }
    }

    public override void SpawnItem()
    {
        // Spawn random ammo, gun , health
    }

    private void ResetAttack()
    {
        canAttack = true;
    }

    public override void SetSpeed(StateType state)
    {
        switch (state)
        {
            case StateType.PATROL:

                agent.speed = patrolSpeed;
                agent.acceleration = baseAcceleration;

                break;

            case StateType.CHASE:

                agent.speed = chaseSpeed;
                agent.acceleration = maxAcceleration;

                break;

            case StateType.ATTACK:

                agent.speed = attackSpeed;

                break;
        }
    }
}

