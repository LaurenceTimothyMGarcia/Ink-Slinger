using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SharkBehavior : MonoBehaviour
{
    TurnBasedBehavior turnBasedBehavior;
    GridItemBehavior gridItemBehavior;
    EnemyBehavior enemyBehavior;

    public int AttackRange = 1;
    public int DamageAmount = 10;
    public int PatrolMovementSpeed = 1;
    public int AggroMovementSpeed = 2;
    public int AggroRange = 6;

    bool aggro = false;
    
    // temp movement stuff
    bool movingRight = true;

    void Awake() {
        turnBasedBehavior = GetComponent<TurnBasedBehavior>();
        enemyBehavior = GetComponent<EnemyBehavior>();
        gridItemBehavior = GetComponent<GridItemBehavior>();


    }

    void Update() {
        if(turnBasedBehavior.TurnStarted()) {
            if(aggro) {
                AggroBehavior();
            }
            else {
                PatrolBehavior();
            }
            turnBasedBehavior.EndTurn();
        }
    }

    void AggroBehavior() {
        if(enemyBehavior.PlayerInRange(AttackRange)) {
                enemyBehavior.HurtPlayer(DamageAmount);
            }
            else {
                enemyBehavior.ChasePlayer(AggroMovementSpeed);
            }
    }

    void PatrolBehavior() {
        Debug.Log("patrol behavior function called");
        // patrol movement
        PatrolMovement();
        // check if player is in range
        if(enemyBehavior.PlayerInRange(AggroRange)) {
            aggro = true;
        }
    }

    void PatrolMovement() {
        Debug.Log("patrol movement function called");
        // really sorry for this line of code
        // the ternary operator is here to change which direction is checked
        // this code determines if the cell in the direction the shark is traveling is valid
        // if it isn't, the shark turns around
        if(!GameObject.Find("GridGenerator").GetComponent<GridBehavior>().IsPositionValid(gridItemBehavior.gridPosition.x + (movingRight ? 1 : -1)  , gridItemBehavior.gridPosition.y)) {
            Debug.Log("changed directio");
            movingRight = !movingRight;
        }
        Debug.Log("direction of movement: " + (movingRight ? 1 : -1));

        gridItemBehavior.moveToPosition(gridItemBehavior.gridPosition.x + (movingRight ? 1 : -1), gridItemBehavior.gridPosition.y);
    }
}
