using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float speed = .1f;

    public Vector2 playerPosition;

    void Start()
    {
        playerPosition = new Vector2(10, 10);
    }

    void Update()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float zDirection = Input.GetAxis("Vertical");

        Vector3 moveDirection = new
        Vector3(xDirection, 0.0f, zDirection);

        transform.position += moveDirection * speed;
    }
}
