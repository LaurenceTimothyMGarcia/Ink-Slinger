using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// generic script for items that are on the grid
public class GridItemBehavior : MonoBehaviour
{
    public float groundLvl;
    public Vector2Int gridPosition = new Vector2Int(0,0);
    GridBehavior gridGenerator;

    Stack<GameObject> path;

    void Awake() {
        gridGenerator = GameObject.Find("GridGenerator").GetComponent<GridBehavior>();
    }

    public void moveToPosition(int x, int y) {
        gridPosition.x = x;
        gridPosition.y = y;
        transform.position = gridGenerator.GetWorldPosition(gridPosition.x, gridPosition.y);
        transform.position += new Vector3(0, groundLvl, 0); // temp line to position objects above grid
    }

    public void GetPathTo(Vector2Int position) {
        GetPathTo(position.x, position.y);
    }
    public void GetPathTo(int x, int y) {
        gridGenerator.setStartX(gridPosition.x);
        gridGenerator.setStartY(gridPosition.y);
        gridGenerator.setEndX(x);
        gridGenerator.setEndY(y);
        gridGenerator.SetDistance();
        gridGenerator.SetPath();

        // get a shallow clone of the gridgenerator's path
        this.path = new Stack<GameObject>(gridGenerator.path);
        
    }

    // move on the current path however many steps
    public IEnumerator MoveOnPath(int steps) {
        if(path != null) {
            if(path.Count > 0) path.Pop(); // removes the first step on the path, which is the position you are already at
            for(int i = 0; i < steps; i++) {
                if(path.Count > 0) {
                    GameObject target = path.Pop();

                    GridStat targetStats = target.GetComponent<GridStat>();

                    moveToPosition(targetStats.x, targetStats.y);

                    yield return new WaitForSeconds(.5f);
                }
            }
        }
    }
}
