using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public enum BattleState { START, PLAYERTURN, ENEMYTURN}

    public BattleState state;

    public SharkBehavior shark;
    public MinnowBehavior minnow;
    public RanglerBehavior rangler;
    public LayerMask whatIsEnemy;

    public float attackRange;

    bool enemyInAttackRange;

    public int damageDealt;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    void Update()
    {
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
    }

    IEnumerator SetupBattle()
    {
        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        //Damage the enemy
        //damageDealt needs to be changed to .Attack function from the player object, 
        //this is just a placeholder
        if (enemyInAttackRange)
        {
            damageDealt = 1;
            if (shark.playerInAttackRange)
            {
                shark.TakeDamage(damageDealt);
            } else if (rangler.playerInAttackRange) {
                rangler.TakeDamage(damageDealt);
            } else if (minnow.playerInAttackRange) {
                minnow.TakeDamage(damageDealt);
            }
        }


        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());

        //Check if the enemy is dead
        //Change state based on what happened
    }

    IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(1f);

        shark.UpdateState();
        minnow.UpdateState();
        rangler.UpdateState();

        //change this as well according to the player object
        //can call the game over screen here as well
        bool isntDead = true;
        if(isntDead)
        {
            state = BattleState.PLAYERTURN;
        }
    }

    void PlayerTurn()
    {
        //player can move or attack, call Mo's stuff?
        //i.e. read button inputs here then call OnAttackButton
        //if they attack and Mo's movement if they move
        //if they move, change state to ENEMYTURN
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerAttack());
    }

}
