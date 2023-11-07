using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBasedBehavior : MonoBehaviour
{
    TurnSystem turnSystem;

    bool turnStarted;

    void Awake() {
        turnSystem = GameObject.Find("TurnSystem").GetComponent<TurnSystem>();
        turnSystem.AddAgent(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnDisable is called when this object is destroyed
    void OnDisable() {

    }

    // only call this if you are TurnSystem.cs, please...
    public void StartTurn() {
        turnStarted = true;
    }

    public bool TurnStarted() {
        return turnStarted;
    }

    public void EndTurn() {
        if(turnStarted) {
            turnSystem.AlertTurnEnded(this);
            turnStarted = false;
        }
    }
}
