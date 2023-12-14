using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SharkBehavior : MonoBehaviour
{
    TurnBasedBehavior turnBasedBehavior;
    GridItemBehavior gridItemBehavior;
    EnemyBehavior enemyBehavior;

    public Animator animator;

    public int AttackRange = 1;
    public int DamageAmount = 10;
    public int MovementSpeed = 1;
    public int AggroActions = 2;
    public int AggroRange = 6;
    public float movementTime = 0.5f;

    Vector2Int PatrolTarget;

    bool aggro = false;
    
    // temp movement stuff
    bool movingRight = true;

    void Awake() {
        turnBasedBehavior = GetComponent<TurnBasedBehavior>();
        enemyBehavior = GetComponent<EnemyBehavior>();
        gridItemBehavior = GetComponent<GridItemBehavior>();

    }

    void Start() {
        
        PatrolTarget = gridItemBehavior.getRandomPosition();
    }

    void Update() {
        if(turnBasedBehavior.TurnStarted()) {
            if(aggro) {
                StartCoroutine(AggroBehavior());
            }
            else {
                PatrolBehavior();
            }
            turnBasedBehavior.EndTurn();
        }
    }

    IEnumerator AggroBehavior() {
        for(int i = 0; i < AggroActions; i++){
            if(enemyBehavior.PlayerInRange(AttackRange)) {
                enemyBehavior.HurtPlayer(DamageAmount);
            }
            else {
                enemyBehavior.ChasePlayer(MovementSpeed);
            }
            
            yield return new WaitForSeconds(.2f);
        }
    }

    void PatrolBehavior() {
        // patrol movement
        PatrolMovement();
        // check if player is in range
        if(enemyBehavior.PlayerInRange(AggroRange)) {
            FindObjectOfType<AudioManager>().PlaySFX("SharkDetect");
            aggro = true;
        }
    }

    void PatrolMovement() {
        if(PatrolTarget == null || gridItemBehavior.gridPosition == PatrolTarget) {
            PatrolTarget = gridItemBehavior.getRandomPosition();
        }
        enemyBehavior.GoTo(PatrolTarget, 1);
    }
}
