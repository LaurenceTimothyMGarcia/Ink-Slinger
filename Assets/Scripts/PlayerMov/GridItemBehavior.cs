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

    public void moveToPosition(int x, int y, float movementTime) {
        gridPosition.x = x;
        gridPosition.y = y;

        // Make the player lerp here
        Vector3 targetPosition = gridGenerator.GetWorldPosition(gridPosition.x, gridPosition.y) + new Vector3(0, groundLvl, 0);
        StartCoroutine(SmoothMovement(targetPosition, movementTime));
        // transform.position = Vector3.Lerp(transform.position, gridWorldPos, 0.5f);
        // transform.position += new Vector3(0, groundLvl, 0); // temp line to position objects above grid
    }

    private IEnumerator SmoothMovement(Vector3 targetPosition, float movementTime) {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;
        float journeyLength = Vector3.Distance(startPosition, targetPosition);
        // float journeyTime = 0.5f; // Adjust this time as needed for the movement speed

        Vector3 direction = (targetPosition - transform.position).normalized;
        while (Time.time < startTime + movementTime) {
            float distCovered = (Time.time - startTime) * journeyLength / movementTime;
            float fracJourney = distCovered / journeyLength;
            transform.position = Vector3.Lerp(startPosition, targetPosition, fracJourney) + new Vector3(0, groundLvl, 0);

            RotateTowards(direction);

            yield return null;
        }

        transform.position = targetPosition; // Ensure reaching the exact target position
    }

    public void RotateTowards(Vector3 dir)
    {
        if (dir != Vector3.zero)
        {
            Quaternion newRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        }
    }

    public void GetPathTo(Vector2Int position) {
        GetPathTo(position.x, position.y);
    }
    public void GetPathTo(int x, int y) {
        gridGenerator.setStartX(gridPosition.x);
        gridGenerator.setStartY(gridPosition.y);
        gridGenerator.setEndX(x);
        gridGenerator.setEndY(y);
        //gridGenerator.SetDistance();
        //gridGenerator.SetPath();

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

                    moveToPosition(targetStats.x, targetStats.y, 0.5f);

                    yield return new WaitForSeconds(.5f);
                }
            }
        }
    }
}
