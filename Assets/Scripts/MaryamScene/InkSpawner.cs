using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkSpawner : MonoBehaviour
{
    public GameObject inkPuddle;
    public Collider collider;

    private void Update()
    {
        OnTriggerEnter(collider);
    }

    private void OnTriggerEnter(Collider collider)
    {
        //change if necessary to whatever the Player object is called
        if (collider.gameObject.name == "Player")
        {
            InkAbsorbed();
        }
    }

    public void SpawnInk(string enemyName){
        Vector3 spawnPosition = GameObject.Find(enemyName).transform.position;
        Instantiate(inkPuddle, spawnPosition, Quaternion.identity);
        //inkPuddle = (GameObject) Instantiate(inkPuddle, spawnPosition, Quaternion.identity);
    }

    public void InkAbsorbed()
    {
        //Debug.Log("Destroyed.");
        Destroy(gameObject);
        //DestroyImmediate(inkPuddle, true);
        //gameObject.SetActive(false);
        
        //increase ink gauge code goes here
    }
}
