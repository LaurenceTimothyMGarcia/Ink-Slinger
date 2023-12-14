using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    public Animator animator;
    [SerializeField] EnemySpawner spawner;

    GridBehavior gridGenerator;
    GridItemBehavior gridItemBehavior;
    TurnBasedBehavior turnBasedBehavior;
    public float movementTime = .25f; // time in seconds between each input read

    public int strength = 5;
    public Direction facing = Direction.UP;

    bool canMove = true;

    enum Direction
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
                Attack(strength);

                turnBasedBehavior.EndTurn();
                StartCoroutine(movementCountdown());
            }
            else if (Input.GetButton("Fire2"))
            {
                Attack(strength*3);

                turnBasedBehavior.EndTurn();
                StartCoroutine(movementCountdown());
            }
            else if (Input.GetButton("Fire3"))
            {
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
                for(int i = 0; i < spawner.enemyList.length; i++)
                {
                    Vector2DInt enemyLocation = spawner.enemyList[i].GetComponent<GridItemBehavior>().gridPosition;
                    if(enemyLocation == new Vector2DInt(gridItemBehavior.gridposition.x-1, gridItemBehavior.gridposition.y)){
                    spawner.enemyList[i].takeDamage(strength);
                    }
                }
                break;
            }
            case Direction.RIGHT:{
                for(int i = 0; i < spawner.enemyList.length; i++)
                {
                    Vector2DInt enemyLocation = spawner.enemyList[i].GetComponent<GridItemBehavior>().gridPosition;
                    if(enemyLocation == new Vector2DInt(gridItemBehavior.gridposition.x+1, gridItemBehavior.gridposition.y)){
                    spawner.enemyList[i].takeDamage(strength);
                    }
                }
                break;
            }
            case Direction.UP:{
                for(int i = 0; i < spawner.enemyList.length; i++)
                {
                    Vector2DInt enemyLocation = spawner.enemyList[i].GetComponent<GridItemBehavior>().gridPosition;
                    if(enemyLocation == new Vector2DInt(gridItemBehavior.gridposition.x, gridItemBehavior.gridposition.y+1)){
                    spawner.enemyList[i].takeDamage(strength);
                    }
                }
                break;
            }
            case Direction.DOWN:{
                for(int i = 0; i < spawner.enemyList.length; i++)
                {
                    Vector2DInt enemyLocation = spawner.enemyList[i].GetComponent<GridItemBehavior>().gridPosition;
                    if(enemyLocation == new Vector2DInt(gridItemBehavior.gridposition.x, gridItemBehavior.gridposition.y-1)){
                    spawner.enemyList[i].takeDamage(strength);
                    }
                }
                break;
            }
        }
    }

    void aoeAttack(int strength)
    {
        for(int i = 0; i < spawner.enemyList.length; i++)
            {
                Vector2DInt enemyLocation = spawner.enemyList[i].GetComponent<GridItemBehavior>().gridPosition;
                if(enemyLocation == new Vector2DInt(gridItemBehavior.gridposition.x+1, gridItemBehavior.gridposition.y)){
                spawner.enemyList[i].takeDamage(strength);
                }
                if(enemyLocation == new Vector2DInt(gridItemBehavior.gridposition.x-1, gridItemBehavior.gridposition.y)){
                spawner.enemyList[i].takeDamage(strength);
                }
                if(enemyLocation == new Vector2DInt(gridItemBehavior.gridposition.x, gridItemBehavior.gridposition.y+1)){
                spawner.enemyList[i].takeDamage(strength);
                }
                if(enemyLocation == new Vector2DInt(gridItemBehavior.gridposition.x, gridItemBehavior.gridposition.y-1)){
                spawner.enemyList[i].takeDamage(strength);
                }
            }
    }

}
