using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    Queue<TurnBasedBehavior> queue;
    TurnBasedBehavior currentAgent;

    void Awake() {
        queue = new Queue<TurnBasedBehavior>();
    }

    void StartNextTurn() {
        queue.Enqueue(currentAgent);
        currentAgent = queue.Dequeue();
        if(currentAgent != null) {
            currentAgent.StartTurn();
        }
        else {
            StartNextTurn();
        }
    }

    // call this function to tell the turn system that your turn is done
    // pass self as input to ensure you are the correct agent
    public void AlertTurnEnded(TurnBasedBehavior you) {
        if(currentAgent == you) {
            StartNextTurn();
        }
        else {
            Debug.LogError("Attempted to end turn while it was not your turn!");
        }
    }

    public void AddAgent(TurnBasedBehavior you) {
        queue.Enqueue(you);
        if (currentAgent == null) {
            currentAgent = queue.Dequeue();
        currentAgent.StartTurn();
        }
    }
}
