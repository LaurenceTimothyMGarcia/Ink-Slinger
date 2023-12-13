using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaircaseSpawner : MonoBehaviour
{
    public GameObject stairCase;
    public Collider collider;
    GridBehavior gridGenerator;

    private void Start()
    {
        SpawnStairs();
    }

    private void Update()
    {
        OnTriggerEnter(collider);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.name == "Player")
        {
            NextLevel();
        }
    }

    public void SpawnStairs()
    {
        int x = Random.Range(0, gridGenerator.gridArray.GetLength(0) - 1), y = Random.Range(0, gridGenerator.gridArray.GetLength(1) - 1);
        while (gridGenerator.gridArray[x, y] == null)
        {
            x = Random.Range(0, gridGenerator.gridArray.GetLength(0) - 1);
            y = Random.Range(0, gridGenerator.gridArray.GetLength(1) - 1);
        }

        stairCase = Instantiate(stairCase, new Vector3(0, 0, 0), Quaternion.identity);
        stairCase.GetComponent<GridItemBehavior>().moveToPosition(x, y);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
