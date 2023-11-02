using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySystem : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float timeBtwAttacks, attackRange, sightRange, health;

    bool alrdyAttacked, playerInAttackRange, playerInSightRange;

    //Shark only
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Possibly will use this for puddles, could do either maybe?
    //public static event Action<EnemySystem> OnEnemyKilled;

    // Start is called before the first frame update
    void Start()
    {
        //Max health = 3f for now lol
        health = 3f;   
    }

    private void Awake()
    {
        //Whatever the player's name is, replace the string
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>(); 
    }

    // Update is called once per frame
    void Update()
    {
        //Check if you can attack
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if(playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 dToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if(dToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }

    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alrdyAttacked)
        {
            //attack code here for grid stuff

            alrdyAttacked = true;
            Invoke(nameof(ResetAttack), timeBtwAttacks);
        }
    }

    private void ResetAttack()
    {
        alrdyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
        //possibly create ink puddle here as well
    }
}
