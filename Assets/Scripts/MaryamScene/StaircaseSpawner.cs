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
    public float checkX, checkZ;
    public float setDifficulty;

    private void Start()
    {
        gridGenerator = GameObject.Find("GridGenerator").GetComponent<GridBehavior>();
        SpawnStairs();
    }

    private void Update()
    {
        OnTriggerEnter(collider);
        //Debug.Log(checkX + " " + checkZ);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            NextLevel();
        }
    }

    public void SpawnStairs()
    {
        //Vector3 playerPos = gridGenerator.GetWorldPosition((int)GameObject.Find("Player").transform.position.x, (int)GameObject.Find("Player").transform.position.y);
        float playerX = GameObject.FindGameObjectWithTag("Player").transform.position.x;
        float playerZ = GameObject.FindGameObjectWithTag("Player").transform.position.z;

        int x = Random.Range(0, gridGenerator.gridArray.GetLength(0) - 1), z = Random.Range(0, gridGenerator.gridArray.GetLength(1) - 1);

        float distance_X = x - playerX;
        float distance_Z = z - playerZ;
        bool isValid = gridGenerator.IsPositionValid(x, z);
        Debug.Log("Player stuff: " + playerX + " " + playerZ);
        Debug.Log("x/z: " + x + " " + z);
        Debug.Log("Distance: " + distance_X + " " + distance_Z);

        if (goLevel1)
        {
            setDifficulty = 8.0f;
        }
        if (goLevel2)
        {
            setDifficulty = 15.0f;
        }
        if (goLevel3)
        {
            setDifficulty = 21.0f;
        }

        while (distance_X < setDifficulty && distance_Z < setDifficulty && isValid == false)
        {
            x = Random.Range(0, gridGenerator.gridArray.GetLength(0) - 1);
            z = Random.Range(0, gridGenerator.gridArray.GetLength(1) - 1);

            distance_X = (float) x - playerX;
            distance_Z = (float) z - playerZ;
            Debug.Log("While: " + distance_X + " " + distance_Z);
            checkX = distance_X;
            checkZ = distance_Z;
            isValid = gridGenerator.IsPositionValid(x, z);
        }

        stairCase = (GameObject) Instantiate(stairCase, gridGenerator.GetWorldPosition(x, z), Quaternion.identity);
        Vector3 pos = stairCase.transform.position;
        pos.y = 0.5f;
        stairCase.transform.position = pos;
        //stairCase.GetComponent<GridItemBehavior>().moveToPosition(x, y);
    }

    public void NextLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneManager.LoadScene("ProtoTypeBuild");
        if (goLevel1)
        {
            SceneManager.LoadScene("Titlescreen");
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
