using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    public float MoveSpeed = 10f;
    Vector3 moveVector = new Vector3(0, 0, 0);
    public Vector3 direction = new Vector3(0,0,0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Move()
    {
        moveVector.x = direction.x * MoveSpeed * Time.deltaTime;
        moveVector.y = direction.y * MoveSpeed * Time.deltaTime;
        transform.position += moveVector;
    }

    public void Move(Vector3 directionInput)
    {
        moveVector.x = directionInput.x * MoveSpeed * Time.deltaTime;
        moveVector.y = directionInput.y * MoveSpeed * Time.deltaTime;
        transform.position += moveVector;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
