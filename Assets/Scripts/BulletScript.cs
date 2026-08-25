using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletScript : MonoBehaviour

{

    [SerializeField] private float speed = 5f;

    private bool isInitialized = true; // false when Enemy class is implemented
    private bool isInsideCollider = false;


    // public void Initialize(float enemySpeed)

    // {

    //     speed = enemySpeed;

    //     isInitialized = true;

    // }


    void Update()
    {
        if (isInitialized)
        {
            Debug.Log("moving");
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BarSensor"))
        {
            isInsideCollider = true;
            Debug.Log("Bullet IN collider");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BarSensor"))
        {
            isInsideCollider = false;
            Debug.Log("Bullet OUT collider");
            Destroy(gameObject);
        }
    }

} 