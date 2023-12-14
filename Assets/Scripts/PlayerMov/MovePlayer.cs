using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    public Animator animator;

    EnemySpawner spawner;

    GridBehavior gridGenerator;
    GridItemBehavior gridItemBehavior;
    TurnBasedBehavior turnBasedBehavior;
    PlayerParticleSystem playerPS;

    inkBar inkGauge;

    GameObject trapdoor;

    public float movementTime = .25f; // time in seconds between each input read

    public int strength = 5;
    public Direction facing = Direction.UP;

    public float inkSpell1Cost = 5f;
    public float inkSpell2Cost = 10f;

    bool canMove = true;

    public enum Direction
    {
        UP,
        LEFT,
        DOWN,
        RIGHT
    }

    void Start()
    {
        gridGenerator = GameObject.Find("GridGenerator").GetComponent<GridBehavior>();
        gridItemBehavior = GetComponent<GridItemBehavior>();
        turnBasedBehavior = GetComponent<TurnBasedBehavior>();
        playerPS = GetComponent<PlayerParticleSystem>();
        inkGauge = GetComponent<inkBar>();
        spawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();

        trapdoor = GameObject.FindGameObjectWithTag("Trapdoor");

        GetStartPosition();
    }

    // temporary function that just grabs the first valid start position it finds and uses that
    void GetStartPosition()
    {
        int startX = 0;
        int startY = 0;
        for (startX = 0; startX < gridGenerator.rows; startX++)
        {
            for (startY = 0; startY < gridGenerator.columns; startY++)
            {
                if (gridGenerator.IsPositionValid(startX, startY))
                {
                    gridItemBehavior.moveToPosition(startX, startY, movementTime);
                    return;
                }
            }
        }
    }

    void Update()
    {
        if (trapdoor.GetComponent<GridItemBehavior>().gridPosition == gridItemBehavior.gridPosition)
        {
            Debug.Log("MOVE TO NEXT STATE");

            if (trapdoor.GetComponent<TrapdoorSpawn>().goLevel1)
            {
                SceneManager.LoadScene("Titlescreen");
            }

            if (trapdoor.GetComponent<TrapdoorSpawn>().goLevel2)
            {
                SceneManager.LoadScene("Level2");
            }

            if (trapdoor.GetComponent<TrapdoorSpawn>().goLevel3)
            {
                SceneManager.LoadScene("Level3");
            }
        }

       // if(Input.GetButtonDown("Fire2")) {
       //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       // }
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        if (canMove && turnBasedBehavior.TurnStarted())
        {
            if (Input.GetButton("Down"))
            {
                AttemptMovement(Direction.DOWN);
            }
            else if (Input.GetButton("Up"))
            {
                AttemptMovement(Direction.UP);
            }
            else if (Input.GetButton("Left"))
            {
                AttemptMovement(Direction.LEFT);
            }
            else if (Input.GetButton("Right"))
            {
                AttemptMovement(Direction.RIGHT);
            }
            else if (Input.GetButton("Fire1"))
            {
                // temporary; skip your turn
                //Debug.Log("turn skipped");
                animator.SetTrigger("MeleeAttack");
                playerPS.PlayUseMeleeSlash();

                Attack(strength);

                turnBasedBehavior.EndTurn();
                StartCoroutine(movementCountdown());
            }
            else if (Input.GetButton("Fire2"))
            {
                if (inkGauge.ink < inkSpell1Cost)
                {
                    Debug.Log("No More Ink");
                    return;
                }

                animator.SetTrigger("RangedAttack");
                playerPS.PlayUseInk();
                inkGauge.useInk(inkSpell1Cost);
                Attack(strength*3);

                turnBasedBehavior.EndTurn();
                StartCoroutine(movementCountdown());
            }
            else if (Input.GetButton("Fire3"))
            {
                if (inkGauge.ink < inkSpell2Cost)
                {
                    Debug.Log("No More Ink");
                    return;
                }

                animator.SetTrigger("RangedAttack");
                playerPS.PlayUseInk();
                inkGauge.useInk(inkSpell2Cost);
                aoeAttack(strength*3);

                turnBasedBehavior.EndTurn();
                StartCoroutine(movementCountdown());
            }
        }
    }

    void AttemptMovement(Direction direction)
    {
        Vector2Int targetPosition = gridItemBehavior.gridPosition;
        Vector3 moveDirection = Vector3.zero;

        // select target grid position depending on inputted direction
        switch (direction)
        {
            case Direction.LEFT: { 
                targetPosition.x -= 1; 
                moveDirection = Vector3.left;
                break; 
            }
            case Direction.RIGHT: { 
                targetPosition.x += 1; 
                moveDirection = Vector3.right;
                break; 
            }
            case Direction.UP: { 
                targetPosition.y += 1; 
                moveDirection = Vector3.forward;
                break; 
            }
            case Direction.DOWN: { 
                targetPosition.y -= 1; 
                moveDirection = Vector3.back;
                break; 
            }
        }

        if (gridGenerator.IsPositionValid(targetPosition.x, targetPosition.y))
        {
            gridItemBehavior.moveToPosition(targetPosition.x, targetPosition.y, movementTime);
            gridItemBehavior.RotateTowards(moveDirection);
            facing = direction;
            turnBasedBehavior.EndTurn();
        }
        StartCoroutine(movementCountdown());
    }

    // this is a really mcgwyver solution to get user input from axes
    // i think ideally this would instead work via button down
    // but god fucking damn
    IEnumerator movementCountdown()
    {
        canMove = false;
        yield return new WaitForSeconds(movementTime);
        canMove = true;
    }

    void Attack(int strength)
    {
        switch (facing)
        {
            case Direction.LEFT:{
                for(int i = 0; i < spawner.enemyList.Count; i++)
                {
                    Vector2Int enemyLocation = spawner.enemyList[i].GetComponent<GridItemBehavior>().gridPosition;
                    if(enemyLocation == new Vector2Int(gridItemBehavior.gridPosition.x-1, gridItemBehavior.gridPosition.y)){
                        spawner.enemyList[i].GetComponent<EnemyBehavior>().TakeDamage(strength);
                    }
                }
                break;
            }
            case Direction.RIGHT:{
                for(int i = 0; i < spawner.enemyList.Count; i++)
                {
                    Vector2Int enemyLocation = spawner.enemyList[i].GetComponent<GridItemBehavior>().gridPosition;
                    if(enemyLocation == new Vector2Int(gridItemBehavior.gridPosition.x+1, gridItemBehavior.gridPosition.y)){
                        spawner.enemyList[i].GetComponent<EnemyBehavior>().TakeDamage(strength);
                    }
                }
                break;
            }
            case Direction.UP:{
                for(int i = 0; i < spawner.enemyList.Count; i++)
                {
                    Vector2Int enemyLocation = spawner.enemyList[i].GetComponent<GridItemBehavior>().gridPosition;
                    if(enemyLocation == new Vector2Int(gridItemBehavior.gridPosition.x, gridItemBehavior.gridPosition.y+1)){
                        spawner.enemyList[i].GetComponent<EnemyBehavior>().TakeDamage(strength);
                    }
                }
                break;
            }
            case Direction.DOWN:{
                for(int i = 0; i < spawner.enemyList.Count; i++)
                {
                    Vector2Int enemyLocation = spawner.enemyList[i].GetComponent<GridItemBehavior>().gridPosition;
                    if(enemyLocation == new Vector2Int(gridItemBehavior.gridPosition.x, gridItemBehavior.gridPosition.y-1)){
                        spawner.enemyList[i].GetComponent<EnemyBehavior>().TakeDamage(strength);
                    }
                }
                break;
            }
        }
    }

    void aoeAttack(int strength)
    {
        for(int i = 0; i < spawner.enemyList.Count; i++)
            {
                Vector2Int enemyLocation = spawner.enemyList[i].GetComponent<GridItemBehavior>().gridPosition;
                if(enemyLocation == new Vector2Int(gridItemBehavior.gridPosition.x+1, gridItemBehavior.gridPosition.y)){
                    spawner.enemyList[i].GetComponent<EnemyBehavior>().TakeDamage(strength);
                }
                if(enemyLocation == new Vector2Int(gridItemBehavior.gridPosition.x-1, gridItemBehavior.gridPosition.y)){
                    spawner.enemyList[i].GetComponent<EnemyBehavior>().TakeDamage(strength);
                }
                if(enemyLocation == new Vector2Int(gridItemBehavior.gridPosition.x, gridItemBehavior.gridPosition.y+1)){
                    spawner.enemyList[i].GetComponent<EnemyBehavior>().TakeDamage(strength);
                }
                if(enemyLocation == new Vector2Int(gridItemBehavior.gridPosition.x, gridItemBehavior.gridPosition.y-1)){
                    spawner.enemyList[i].GetComponent<EnemyBehavior>().TakeDamage(strength);
                }
            }
    }

}
