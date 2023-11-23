using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    GridItemBehavior gridItemBehavior;
    public GameObject player;

    //temporary
    GameObject healthBar;

    public int MaxHealth = 3;
    int health;

    //Possibly will use this for puddles, could do either maybe?
    //public static event Action<EnemySystem> OnEnemyKilled;

    private void Awake()
    {
        //Whatever the player's name is, replace the string in the delimiter
        player = GameObject.Find("Player");
        //agent = GetComponent<NavMeshAgent>(); 

        gridItemBehavior = GetComponent<GridItemBehavior>();

        // temp
        healthBar = GameObject.Find("Health Bar");
    }

    // Start is called before the first frame update
    void Start()
    {
        health = MaxHealth;
    }

    public void ChasePlayer(int TileSpeed)
    {
        gridItemBehavior.GetPathTo(player.GetComponent<GridItemBehavior>().gridPosition);
        StartCoroutine(gridItemBehavior.MoveOnPath(TileSpeed));
    }

    public bool PlayerInRange(int range) {
        Vector2Int distanceVector = gridItemBehavior.gridPosition - player.GetComponent<GridItemBehavior>().gridPosition;
        return (Mathf.Abs(distanceVector.x) + Mathf.Abs(distanceVector.y)) <= range;
    }

    public void HurtPlayer(int amount) {
        healthBar.GetComponent<healthBar>().takeDamage(amount);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
        //possibly create ink puddle here as well
    }
}
