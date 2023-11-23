using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RanglerBehavior : MonoBehaviour
{
    TurnBasedBehavior turnBasedBehavior;
    EnemyBehavior enemyBehavior;

    private void Awake()
    {
        turnBasedBehavior = GetComponent<TurnBasedBehavior>();
        enemyBehavior = GetComponent<EnemyBehavior>();
    }

    void Update() {
        if(turnBasedBehavior.TurnStarted()) {
            if(enemyBehavior.PlayerInRange(1)) {
                // attack player
            }
            else {
                enemyBehavior.ChasePlayer(1);
            }
            turnBasedBehavior.EndTurn();
        }
    }
}
