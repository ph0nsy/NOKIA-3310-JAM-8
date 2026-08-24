using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class BulletScript : MonoBehaviour

{

    [SerializeField] private float speed = 200f;

    private RectTransform rectTransform;

    private bool isInitialized = true; // false when Enemy class is implemented
    private bool isInsideCollider = false;


    private void Awake()

    {
        rectTransform = GetComponent<RectTransform>();
    }


    // public void Initialize(float enemySpeed)
    // Enemy has to indicate the speed and initialize the class
    // Also Enemy has to delete the oldest bullet on press Enter

    // {

    //     speed = enemySpeed;

    //     isInitialized = true;

    // }


    void Update()
    {
        if (isInitialized)
        {
            Debug.Log("moving");
            rectTransform.anchoredPosition += Vector2.left * speed * Time.deltaTime;
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
        }
    }

} 