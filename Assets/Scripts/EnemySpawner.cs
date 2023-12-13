using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public enum EnemyType {
        RANGLER,
        MINNOW,
        SHARK,
    }

    [System.Serializable]
    public class EnemyPrefab {
        public EnemyType type;
        public GameObject prefab;
    }

    public List<EnemyPrefab> prefabs;
    GridBehavior gridGenerator;

    // Start is called before the first frame update
    void Start()
    {
        gridGenerator = GameObject.Find("GridGenerator").GetComponent<GridBehavior>();

        SpawnEnemyRandomly(EnemyType.MINNOW);
        SpawnEnemyRandomly(EnemyType.SHARK);
        SpawnEnemyRandomly(EnemyType.RANGLER);
    }

    void SpawnEnemyRandomly(EnemyType type) {
        // this should be kinda naive since enemies can spawn in places where there are already enemies or the player but whatever
        // find a set of xy coordinates that are valid
        int x = Random.Range(0, gridGenerator.gridArray.GetLength(0) - 1), y = Random.Range(0, gridGenerator.gridArray.GetLength(1) - 1);
        while(gridGenerator.gridArray[x,y] == null) {
            x = Random.Range(0, gridGenerator.gridArray.GetLength(0) - 1);
            y = Random.Range(0, gridGenerator.gridArray.GetLength(1) - 1);
        }

        SpawnEnemy(type, x, y);
    }

    // spawns an enemy at the grid position (x, y)
    void SpawnEnemy(EnemyType type, int x, int y) {
        EnemyPrefab iPrefab = prefabs.Find(ePrefab => ePrefab.type == type);
        if(iPrefab == null || iPrefab.prefab == null) {
            Debug.LogWarning("prefab not found; check the prefabs list and your spelling");
            return;
        }

        GameObject newEnemy = Instantiate(iPrefab.prefab, new Vector3(0,0,0), Quaternion.identity);
        newEnemy.GetComponent<GridItemBehavior>().moveToPosition(x, y);

    }
}
