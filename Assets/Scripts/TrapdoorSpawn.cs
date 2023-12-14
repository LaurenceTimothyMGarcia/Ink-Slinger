using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapdoorSpawn : MonoBehaviour
{
    GridBehavior gridGenerator;
    GridItemBehavior gridItemBehavior;

    public bool goLevel2;
    public bool goLevel3;
    public bool finishGame;

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
}
