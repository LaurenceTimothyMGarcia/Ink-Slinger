using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaircaseSpawner : MonoBehaviour
{
    public GameObject stairCase;
    public Collider collider;
    GridBehavior gridGenerator;
    public bool goLevel1;
    public bool goLevel2;
    public bool goLevel3;

    private void Start()
    {
        gridGenerator = GameObject.Find("GridGenerator").GetComponent<GridBehavior>();
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

        stairCase = (GameObject) Instantiate(stairCase, new Vector3(x, 2, y), Quaternion.identity);
        //stairCase.GetComponent<GridItemBehavior>().moveToPosition(x, y);
    }

    public void NextLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene("ProtoTypeBuild");
        if (goLevel1)
        {
            SceneManager.LoadScene("Level1");
        }

        if (goLevel2)
        {
            SceneManager.LoadScene("Level2");
        }

        if (goLevel3)
        {
            SceneManager.LoadScene("Level3");
        }
    }
}
