using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovePlayer : MonoBehaviour
{
    GridBehavior gridGenerator;
    GridItemBehavior gridItemBehavior;
    TurnBasedBehavior turnBasedBehavior;
    public float movementTime = .25f; // time in seconds between each input read

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
        if(Input.GetButtonDown("Fire2")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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
                Debug.Log("turn skipped");

                turnBasedBehavior.EndTurn();
                StartCoroutine(movementCountdown());
            }
        }
    }

    void AttemptMovement(Direction direction)
    {
        Vector2Int targetPosition = gridItemBehavior.gridPosition;
        // select target grid position depending on inputted direction
        switch (direction)
        {
            case Direction.LEFT: { 
                targetPosition.x -= 1; 
                break; 
            }
            case Direction.RIGHT: { 
                targetPosition.x += 1; 
                break; 
            }
            case Direction.UP: { 
                targetPosition.y += 1; 
                break; 
            }
            case Direction.DOWN: { 
                targetPosition.y -= 1; 
                break; 
            }
        }

        if (gridGenerator.IsPositionValid(targetPosition.x, targetPosition.y))
        {
            gridItemBehavior.moveToPosition(targetPosition.x, targetPosition.y, movementTime);
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
}
