using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyBase : MonoBehaviour
{
    // Layers
    public LayerMask LayerNav, LayerTarget, LayerObstacle;
    public NavMeshAgent agent;

    // Target
    public Transform targetPlayer;

    // Var
    protected int health;
    protected bool isDead;
    protected StateMachine stateMachine;
    protected MeshRenderer meshRenderer;

    // Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    // Var
    [Header("FOV Parameter")]
    public float viewRadius;
    public float attackRange;
    [Range(0, 360)]
    public float viewAngle;

    // Special
    public Material green, red, yellow;

    private void Awake()
    {
        targetPlayer = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {

    }

    public bool IsInViewRange()
    {
        // Check if Player in sightrange
        bool isInRadius = Physics.CheckSphere(transform.position, viewRadius, LayerTarget);

        if (isInRadius)
        {
            Vector3 dirTotarget = (targetPlayer.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirTotarget) < viewAngle / 2)
            {
                float distTotarget = Vector3.Distance(transform.position, targetPlayer.position);

                if (!Physics.Raycast(transform.position, dirTotarget, distTotarget, LayerObstacle))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsInAttackRange()
    {
        //Check if Player in attackrange
        bool isInRadius = Physics.CheckSphere(transform.position, attackRange, LayerTarget);

        if (isInRadius)
        {
            Vector3 dirTotarget = (targetPlayer.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, dirTotarget) < viewAngle / 2)
            {
                float distTotarget = Vector3.Distance(transform.position, targetPlayer.position);

                if (!Physics.Raycast(transform.position, dirTotarget, distTotarget, LayerObstacle))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public abstract void Attack();

    public virtual void SetViewState(StateType state)
    {
        switch (state)
        {
            case StateType.PATROL:
                meshRenderer.material = green;
                break;
            case StateType.CHASE:
                meshRenderer.material = yellow;
                break;
            case StateType.ATTACK:
                meshRenderer.material = red;
                break;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        health -= damage;

        if (health < 0)
        {
            isDead = true;
            Destroy();
        }
    }

    protected virtual void Destroy()
    {
        Destroy(gameObject);
    }
}

