using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinnowBehavior : MonoBehaviour
{
    //public NavMeshAgent agent;

    public GridBehavior minnow;

    public GameObject Player;

    public float EnemyDistanceRun = 4.0f;

    public float health;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        //agent = GetComponent<NavMeshAgent>();
        //can change this value
        health = 2f;
    }

    // Update is called once per frame
    public void UpdateState()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        //Run from player
        if(distance < EnemyDistanceRun)
        {
            //in respect to the player

            Vector3 dirToPlayer = transform.position - Player.transform.position;

            Vector3 newPos = transform.position + dirToPlayer;

            //agent.SetDestination(newPos);

            minnow.setEndX((int)newPos.x);
            minnow.setEndY((int)newPos.y);
            minnow.SetPath();
        } else {
            //The minnow moves randomly unless the player gets too close
            Patroling();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            //agent.SetDestination(walkPoint);
            minnow.setEndX((int)walkPoint.x);
            minnow.setEndY((int)walkPoint.y);
            minnow.SetPath();
        }

        Vector3 dToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if (dToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }

    }

    private void SearchWalkPoint()
    {
        //float randomY = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
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
