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
    
    void Start() 
    {
        transform.localScale = new Vector3(size, 1, 1);
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.inPlay) 
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.velocity = Vector2.left * speed;
    }

} 