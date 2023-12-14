using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinnowBehavior : MonoBehaviour
{
    TurnBasedBehavior turnBasedBehavior;
    EnemyBehavior enemyBehavior;

    private void Awake()
    {
        turnBasedBehavior = GetComponent<TurnBasedBehavior>();
        enemyBehavior = GetComponent<EnemyBehavior>();
    }

    void Update()
    {
        if(turnBasedBehavior.TurnStarted()) {
            enemyBehavior.MoveRandomly();
            turnBasedBehavior.EndTurn();
        }
    }
}
