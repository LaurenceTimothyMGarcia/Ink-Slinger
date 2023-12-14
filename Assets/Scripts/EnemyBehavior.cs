using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Animator animator;

    GridItemBehavior gridItemBehavior;
    public GameObject player;
    InkSpawner inkSpawner;

    //temporary
    GameObject healthBar;

    public int MaxHealth = 3;
    int health;

    //Possibly will use this for puddles, could do either maybe?
    //public static event Action<EnemySystem> OnEnemyKilled;

    private void Awake()
    {
        //Whatever the player's name is, replace the string in the delimiter
        player = GameObject.FindGameObjectWithTag("Player");
        //agent = GetComponent<NavMeshAgent>(); 

        gridItemBehavior = GetComponent<GridItemBehavior>();

        // temp
        healthBar = GameObject.FindGameObjectWithTag("HealthBar");
    }

    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
    }

    void Update()
    {
        if (health <= 0) 
        {
            DestroyEnemy(this.gameObject.name);
        }
    }

    public void TakeDamage(int amount)
    {
        if (health > 0)
        {
            animator.SetTrigger("Hurt");
            health -= amount;
        }
        else 
        {
            health = 0;
        }
    }

    public void ChasePlayer(int TileSpeed)
    {
        gridItemBehavior.GetPathTo(player.GetComponent<GridItemBehavior>().gridPosition);
        StartCoroutine(gridItemBehavior.MoveOnPath(TileSpeed));
    }

    public bool PlayerInRange(int range)
    {
        Vector2Int distanceVector = gridItemBehavior.gridPosition - player.GetComponent<GridItemBehavior>().gridPosition;
        return (Mathf.Abs(distanceVector.x) + Mathf.Abs(distanceVector.y)) <= range;
    }

    public void HurtPlayer(int amount)
    {
        player.GetComponent<healthBar>().takeDamage(amount);
    }

    public void DestroyEnemy(string enemyName)
    {
        Destroy(gameObject);
        inkSpawner.SpawnInk(enemyName);
    }
}
