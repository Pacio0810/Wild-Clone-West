using UnityEngine;

public class EnemyStalker : EnemyBase
{
    [Header("Explosion Settings")]
    [SerializeField] private GameObject explosion;
    [SerializeField] private float explosionRange;
    [SerializeField] private float explosionForce;
    [SerializeField] private float explosionDamage;


    private void Start()
    {
        viewAngle = 120;
        viewRadius = 30;
        attackRange = 0.5f;
        walkPointRange = 20;

        patrolSpeed = 10;
        chaseSpeed = 15;
        attackSpeed = 20;
        maxAcceleration = 16;
        baseAcceleration = 10;

        explosionRange = 5;
        explosionForce = 5;
        explosionDamage = 50;

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
        Explode();
    }

    private void Explode()
    {
        //Instantiate explosion
        Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies 
        if (Physics.CheckSphere(transform.position, explosionRange, LayerTarget))
        {
            // Get component of player and add force
            //targetPlayer.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);

            // Get component of player and call Take Damage
            //targetPlayer.GetComponent<PlayerController>().TakeDamage(explosionDamage);
        }

        DestroyEnemy();
    }

    public override void SpawnItem()
    {
        // Spawn random ammo, gun , health
    }

    public override void DestroyEnemy()
    {
        SpawnItem();

        Destroy(gameObject);

        Debug.Log("PIPPO");
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

