using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SharkBehavior : MonoBehaviour
{
    //Navmesh removed just in case 
    //public NavMeshAgent agent;

    public GridBehavior shark;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float timeBtwAttacks, attackRange, sightRange, health;

    public bool alrdyAttacked, playerInAttackRange, playerInSightRange;

    float damageDealt = 1f;

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
        //Whatever the player's name is, replace the string in the delimiter
        player = GameObject.Find("Player").transform;
        //agent = GetComponent<NavMeshAgent>(); 
    }

    // Update is called once per frame
    public void UpdateState()
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
            //agent.SetDestination(walkPoint);
            shark.setEndX((int)walkPoint.x);
            shark.setEndY((int)walkPoint.y);
            shark.SetPath();
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
        //float randomY = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        //agent.SetDestination(player.position);
        shark.setEndX((int)player.position.x);
        shark.setEndY((int)player.position.y);
        shark.SetPath();
    }

    private void AttackPlayer()
    {
        //agent.SetDestination(transform.position);
        transform.LookAt(player);
        if (!alrdyAttacked)
        {
            //attack code here like (player.health -= damageDealt, need player object?)

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
