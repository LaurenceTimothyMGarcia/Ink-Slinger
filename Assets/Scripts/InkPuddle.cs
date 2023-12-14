using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkPuddle : MonoBehaviour
{
    public float inkGain = 5f;

    GridBehavior gridGenerator;
    GridItemBehavior gridItemBehavior;

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        gridGenerator = GameObject.Find("GridGenerator").GetComponent<GridBehavior>();
        gridItemBehavior = GetComponent<GridItemBehavior>();
        SpawnInk();
    }

    void Update()
    {
        if (player.GetComponent<GridItemBehavior>().gridPosition == gridItemBehavior.gridPosition)
        {
            player.GetComponent<inkBar>().gainInk(inkGain);
        }
    }

    void SpawnInk()
    {
        int x = Random.Range(0, gridGenerator.gridArray.GetLength(0) - 1), y = Random.Range(0, gridGenerator.gridArray.GetLength(1) - 1);
        while(gridGenerator.gridArray[x,y] == null) {
            x = Random.Range(0, gridGenerator.gridArray.GetLength(0) - 1);
            y = Random.Range(0, gridGenerator.gridArray.GetLength(1) - 1);
        }

        gridItemBehavior.moveToPosition(x, y, 0);
    }
}
