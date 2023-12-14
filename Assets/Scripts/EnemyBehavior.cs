using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    public Animator animator;
    public GameObject player;

    public Slider healthSlider;
    public Slider easeHealthSlider;

    public GameObject inkPuddle;

    GridItemBehavior gridItemBehavior;
    InkSpawner inkSpawner;

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

    public void MoveRandomly() {
        // todo: something here
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
        // FindObjectOfType<AudioManager>().PlaySFX("EnemyDeath");
        // inkSpawner.SpawnInk(enemyName);
        GameObject iPuddle = Instantiate(inkPuddle);
        iPuddle.GetComponent<GridItemBehavior>().gridPosition = new Vector2Int(gridItemBehavior.gridPosition.x, gridItemBehavior.gridPosition.y);
        Destroy(gameObject);
    }
}
