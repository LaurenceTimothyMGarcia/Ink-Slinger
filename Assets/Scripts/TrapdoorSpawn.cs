using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapdoorSpawn : MonoBehaviour
{
    GridBehavior gridGenerator;
    GridItemBehavior gridItemBehavior;
    Collider collider;

    public bool goLevel1;
    public bool goLevel2;
    public bool goLevel3;

    // Start is called before the first frame update
    void Start()
    {
        gridGenerator = GameObject.Find("GridGenerator").GetComponent<GridBehavior>();
        gridItemBehavior = GetComponent<GridItemBehavior>();
        GetTrapPosition();
    }

    void GetTrapPosition()
    {
        int x = Random.Range(0, gridGenerator.gridArray.GetLength(0) - 1), y = Random.Range(0, gridGenerator.gridArray.GetLength(1) - 1);
        while(gridGenerator.gridArray[x,y] == null) {
            x = Random.Range(0, gridGenerator.gridArray.GetLength(0) - 1);
            y = Random.Range(0, gridGenerator.gridArray.GetLength(1) - 1);
        }

        gridItemBehavior.moveToPosition(x, y, 0);
    }

    void Update()
    {
        OnTriggerEnter(collider);
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     Debug.Log("somethign collide");

    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         Debug.Log("Tag be tagging");
    //         NextLevel();
    //     }
    // }

    void OnTriggerEnter(Collider collider)
    {
        // Debug.Log("Player entered thingy");

        if (collider.CompareTag("Player"))
        {
            Debug.Log("Tag be tagging");
            NextLevel();
        }
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
