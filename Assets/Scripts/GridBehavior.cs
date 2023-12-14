using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [RequireComponent(typeof(CellularAutomata))]
public class GridBehavior : MonoBehaviour
{
    public int rows = 10;
    public int columns = 10;
    public int scale = 1;
    public GameObject gridPrefab;
    public Vector3 leftBottomLocation = new Vector3(0, 0, 0);
    public GameObject[,] gridArray;

    public List<GameObject> path = new List<GameObject>();

    // Start is called before the first frame update
    void Awake()
    {
        gridArray = new GameObject[columns, rows];

        if (gridPrefab)
        {
            //GenerateGrid();
        }
        else
        {
            print("Missing grid prefab.");
        }

    }

    void Start()
    {
        TestPCGGenerateGrid();
    }

    void TestPCGGenerateGrid()
    {
        bool[,] cellGrid = GetComponent<ProceduralGeneration>().GetGrid();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if (cellGrid[i, j])
                {
                    GameObject obj = Instantiate(
                                        gridPrefab, new Vector3(leftBottomLocation.x + scale * i,
                                        leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
                    obj.transform.SetParent(gameObject.transform);
                    obj.GetComponent<GridStat>().x = i;
                    obj.GetComponent<GridStat>().y = j;

                    gridArray[i, j] = obj;
                }
            }
        }
    }

    public bool IsPositionValid(int x, int y)
    {
        // this if statement checks if the position is in bounds for the array
        // just because of a couple annoying errors spamming the console
        if (x < 0 || y < 0 || x >= gridArray.Length || y >= gridArray.GetLength(0))
        {
            return false;
        }
        if (gridArray[x, y])
        {
            return true;
        }
        return false;
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        if (gridArray[x, y])
        {
            return gridArray[x, y].transform.position;
        }
        else
        {
            return Vector3.zero;
        }
    }

    int GetDistance(int x1, int y1, int x2, int y2)
    {
        return (Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2));
    }

    public List<GameObject> GetPath(int x1, int y1, int x2, int y2)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                if(gridArray[i,j] != null) {
                gridArray[i,j].GetComponent<GridStat>().currentCost = -1;
                gridArray[i,j].GetComponent<GridStat>().sumValue = -1;
                gridArray[i,j].GetComponent<GridStat>().distanceFromDestination = -1;
                gridArray[i,j].GetComponent<GridStat>().parent = null;}
            }
        }

        List<GameObject> outPath = new List<GameObject>();

        List<GameObject> weirdFuckingPriorityQueue = new List<GameObject>();

        GameObject startLocation = gridArray[x1, y1];
        startLocation.GetComponent<GridStat>().currentCost = 0;
        startLocation.GetComponent<GridStat>().distanceFromDestination = GetDistance(x1, y1, x2, y2);
        startLocation.GetComponent<GridStat>().sumValue = GetDistance(x1, y1, x2, y2);

        weirdFuckingPriorityQueue.Add(startLocation);

        List<GameObject> visited = new List<GameObject>();

        while(true)
        {
            // get the best node
            GameObject best = weirdFuckingPriorityQueue[0];
            int lowestHeuristic = best.GetComponent<GridStat>().sumValue;
            int bestIndex = 0;

            for (int i = 0; i < weirdFuckingPriorityQueue.Count; i++)
            {
                if (weirdFuckingPriorityQueue[i].GetComponent<GridStat>().sumValue < lowestHeuristic)
                {
                    lowestHeuristic = weirdFuckingPriorityQueue[i].GetComponent<GridStat>().sumValue;
                    bestIndex = i;
                }
            }

            best = weirdFuckingPriorityQueue[bestIndex];

            weirdFuckingPriorityQueue.Remove(best);
            visited.Add(best);


            if (best.GetComponent<GridStat>().x == x2 && best.GetComponent<GridStat>().y == y2)
            {
                // we are done
                GameObject currentlyRetracedNode = best;
                while (currentlyRetracedNode.GetComponent<GridStat>().parent != null)
                {
                    outPath.Add(currentlyRetracedNode);
                    currentlyRetracedNode = currentlyRetracedNode.GetComponent<GridStat>().parent;
                }
                break;
            }

            Vector2Int bestPosition = new Vector2Int(best.GetComponent<GridStat>().x, best.GetComponent<GridStat>().y);
            // best has (up to) four neighbors: above, below, right and left
            Vector2Int[] neighbors = { bestPosition + Vector2Int.down, bestPosition + Vector2Int.up, bestPosition + Vector2Int.left, bestPosition + Vector2Int.right };
            foreach (Vector2Int neighborPosition in neighbors)
            {
                GameObject neighbor = gridArray[neighborPosition.x, neighborPosition.y];
                if (neighbor != null && !visited.Contains(neighbor))
                {
                    neighbor.GetComponent<GridStat>().currentCost = best.GetComponent<GridStat>().currentCost + 1;
                    neighbor.GetComponent<GridStat>().distanceFromDestination = GetDistance(neighborPosition.x, neighborPosition.y, x2, y2);
                    neighbor.GetComponent<GridStat>().sumValue = neighbor.GetComponent<GridStat>().distanceFromDestination + neighbor.GetComponent<GridStat>().currentCost;
                    neighbor.GetComponent<GridStat>().parent = best;

                    if (!weirdFuckingPriorityQueue.Contains(neighbor)) weirdFuckingPriorityQueue.Add(neighbor);
                }
            }
        }

        return outPath;
    }

    // void GenerateGrid()
    // {
    //     for(int i = 0; i < columns; i++)
    //     {
    //         for(int j = 0; j < rows; j++)
    //         {
    //             GameObject obj = Instantiate(
    //                                 gridPrefab, new Vector3(leftBottomLocation.x + scale * i, 
    //                                 leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
    //             obj.transform.SetParent(gameObject.transform);
    //             obj.GetComponent<GridStat>().x = i;
    //             obj.GetComponent<GridStat>().y = j;

    //             gridArray[i, j] = obj;
    //         }
    //     }
    // }

    // void TestCAGenerateGrid() {
    //     bool[,] cellGrid = GetComponent<CellularAutomata>().GetGrid();

    //     for(int i = 0; i < rows; i++)
    //     {
    //         for(int j = 0; j < columns; j++)
    //         {
    //             if(cellGrid[i,j]) {
    //                 GameObject obj = Instantiate(
    //                                     gridPrefab, new Vector3(leftBottomLocation.x + scale * i, 
    //                                     leftBottomLocation.y, leftBottomLocation.z + scale * j), Quaternion.identity);
    //                 obj.transform.SetParent(gameObject.transform);
    //                 obj.GetComponent<GridStat>().x = i;
    //                 obj.GetComponent<GridStat>().y = j;

    //                 gridArray[i, j] = obj;
    //             }
    //         }
    //     }
    // }
}
