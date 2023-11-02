using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RanglerBehavior : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float timeBtwAttacks, attackRange, health;

    bool alrdyAttacked, playerInAttackRange;

    //Possibly will use this for puddles, could do either maybe?
    //public static event Action<RanglerBehavior> OnEnemyKilled;

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
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInAttackRange) ChasePlayer();
        if (playerInAttackRange) AttackPlayer();
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

        if (health <= 0)
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
