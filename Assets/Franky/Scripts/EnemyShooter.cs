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
        stateMachine = new StateMachine();
        stateMachine.AddState(StateType.PATROL, new PatrolState(this));
        stateMachine.AddState(StateType.CHASE, new ChaseState(this));
        stateMachine.AddState(StateType.ATTACK, new AttackState(this));

        stateMachine.GoTo(StateType.PATROL);

        viewAngle = 160;
        canAttack = true;
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

    private void ResetAttack()
    {
        canAttack = true;
    }
}

