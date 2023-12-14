using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RanglerBehavior : MonoBehaviour
{
    TurnBasedBehavior turnBasedBehavior;
    EnemyBehavior enemyBehavior;

    public Animator animator;

    public int DamageAmount = 10;
    public int AttackRange = 1;
    public int MovementSpeed = 1;

    private void Awake()
    {
        turnBasedBehavior = GetComponent<TurnBasedBehavior>();
        enemyBehavior = GetComponent<EnemyBehavior>();
    }

    void Update() {
        if(turnBasedBehavior.TurnStarted()) {
            if(enemyBehavior.PlayerInRange(AttackRange)) {
                // attack player
                enemyBehavior.HurtPlayer(DamageAmount);
            }
            else {
                enemyBehavior.ChasePlayer(MovementSpeed);
            }
            turnBasedBehavior.EndTurn();
        }
    }
}
