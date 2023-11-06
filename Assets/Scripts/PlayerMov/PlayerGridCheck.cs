using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridCheck : MonoBehaviour
{
    // Shoot a raycast down and get the grid stat
    public Vector2 gridPosition;
    public LayerMask groundLayer;

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 10f, groundLayer))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 2f);
            gridPosition = new Vector2(gameObject.GetComponent<GridStat>().x, gameObject.GetComponent<GridStat>().y);
            // Debug.Log(gridPosition);
        }
    }
}
