using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletScript : MonoBehaviour

{

    [SerializeField] public float speed = 5f;
    [SerializeField] public int size = 4;

    public bool isParried = false;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isGameOver) 
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.velocity = Vector2.left * speed;
    }

} 