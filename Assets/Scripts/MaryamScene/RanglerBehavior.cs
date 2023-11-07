using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RanglerBehavior : MonoBehaviour
{
    //Navmesh removed just in case 
    //public NavMeshAgent agent;

    GridItemBehavior gridItemBehavior;

    public GameObject player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float timeBtwAttacks, attackRange, sightRange, health;

    public bool alrdyAttacked, playerInAttackRange;

    float damageDealt = 1f;

    int tileSpeed = 3; // the number of tiles the rangler moves per action

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
        player = GameObject.Find("Player");
        //agent = GetComponent<NavMeshAgent>(); 

        gridItemBehavior = GetComponent<GridItemBehavior>();
        StartCoroutine(TestTimer());
    }

    void Update() {
        if(temp) {
            ChasePlayer();
            temp = false;
        }
    }

    bool temp = false;
    IEnumerator TestTimer() {
        while(true){
        yield return new WaitForSeconds(3);
        temp = true;}
    }

    // Update is called once per frame
    public void UpdateState()
    {
        //Check if you can attack
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInAttackRange) ChasePlayer();
        if (playerInAttackRange) AttackPlayer();
    }

    private void ChasePlayer()
    {
        gridItemBehavior.GetPathTo(player.GetComponent<GridItemBehavior>().gridPosition);
        StartCoroutine(gridItemBehavior.MoveOnPath(tileSpeed));
    }

    private void AttackPlayer()
    {
        //agent.SetDestination(transform.position);
        transform.LookAt(player.transform);
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
