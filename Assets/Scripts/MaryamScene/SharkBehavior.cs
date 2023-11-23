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
        // patrol movement

        // check if player is in range
        if(enemyBehavior.PlayerInRange(AggroRange)) {
            aggro = true;
        }
    }
}
