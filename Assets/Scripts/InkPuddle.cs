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

        gameObject.transform.position += new Vector3(0, 0.75f, 0);
    }

    void Update()
    {
        if (player.GetComponent<GridItemBehavior>().gridPosition == gridItemBehavior.gridPosition)
        {
            player.GetComponent<inkBar>().gainInk(inkGain);
            Destroy(this.gameObject);
        }
    }
}
