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

    public float MaxHealth = 3;
    float health;
    float uiHealth;

    private float lerpSpeed = 0.05f;

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
        uiHealth = health / MaxHealth * 100;
    }

    void Update()
    {
        uiHealth = health / MaxHealth * 100;

        if (health <= 0) 
        {
            DestroyEnemy(this.gameObject.name);
        }

        if (healthSlider.value != uiHealth)
        {
            healthSlider.value = uiHealth;
        }

        if (healthSlider.value != easeHealthSlider.value)
        {
            easeHealthSlider.value = Mathf.Lerp(easeHealthSlider.value, uiHealth, lerpSpeed);
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

    public void GoTo(Vector2Int targetPos, int TileSpeed) {
        gridItemBehavior.GetPathTo(targetPos);
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
        FindObjectOfType<AudioManager>().PlaySFX("EnemyDeath");
        GameObject iPuddle = Instantiate(inkPuddle);
        iPuddle.GetComponent<GridItemBehavior>().moveToPosition(gridItemBehavior.gridPosition.x, gridItemBehavior.gridPosition.y, 0);
        Destroy(gameObject);
    }
}
