using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed = .1f;

    public Vector2Int gridPosition;
    public GridBehavior gridGenerator;
    public int movementTime = 1; // time in seconds between each input read

    bool canMove = true;

    enum Direction {
        UP,
        LEFT,
        DOWN,
        RIGHT
    }

    void Start()
    {
        GetStartPosition();
        Debug.Log(gridPosition);
        transform.position = gridGenerator.GetWorldPosition(gridPosition.x, gridPosition.y);
        transform.position += new Vector3(0, 2, 0); // temp line...

        StartCoroutine(movementCountdown());
    }

    void GetStartPosition() {
        int startX = 0;
        int startY = 0;
        for(startX = 0; startX < gridGenerator.rows; startX++) {
            for (startY = 0; startY < gridGenerator.columns; startY++) {
                if(gridGenerator.IsPositionValid(startX, startY)) {
                    gridPosition = new Vector2Int(startX,startY);
                    return;
                }
            }
        }

        
        gridPosition = new Vector2Int(startX, startY);
    }

    void Update()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        if(canMove) {
            if(xDirection != 0 || yDirection != 0) {
                Direction selectedDirection = GetCardinalDirection(xDirection, yDirection);
                Debug.Log(selectedDirection);
                AttemptMovement(selectedDirection);
            }
            canMove = false;
        }
    }

    Direction GetCardinalDirection(float xInput, float yInput) {
        Vector2 moveDirection = new Vector2(xInput, yInput).normalized;
        float shortestDistance = 100;
        int outIndex = 0;
        Vector2[] directionArray = {Vector2.up, Vector2.left, Vector2.down, Vector2.right};

        for(int i = 0; i < 4; i++) {
            float thisDist = Vector2.Distance(moveDirection, directionArray[i]);
            if(thisDist < shortestDistance) {
                shortestDistance = thisDist;
                outIndex = i;
            }
        }

        return (Direction) outIndex;
    }

    void AttemptMovement(Direction direction) {
        Vector2Int targetPosition = gridPosition;
        // select target grid position depending on inputted direction
        switch(direction) {
            case Direction.LEFT: {targetPosition.x -= 1; break;}
            case Direction.RIGHT: {targetPosition.x += 1; break;}
            case Direction.UP: {targetPosition.y += 1; break;}
            case Direction.DOWN: {targetPosition.y -= 1; break;}
        }
        
        if(gridGenerator.IsPositionValid(targetPosition.x, targetPosition.y)) {
            // successful movement code here
            gridPosition = targetPosition;
            transform.position = gridGenerator.GetWorldPosition(gridPosition.x, gridPosition.y);
            transform.position += new Vector3(0, 2, 0); // temp line...
        }
    }

// this is a really mcgwyver solution to get user input from axes
// i think ideally this would instead work via button down
// but god fucking damn
    IEnumerator movementCountdown() {
        while(true) {
            yield return new WaitForSeconds(movementTime);
            canMove = true;
        }
    }
}
