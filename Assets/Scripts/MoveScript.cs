using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
  [Range(1f, 50f)]
  public float Speed = 10; // m/s
  [SerializeField]
  [Range(0,1)]
  private Vector2 Direction; // espacio de coordenadas

  private Transform tfm;
  
  private void Awake()
  {
    Direction = new Vector2(0.5f, 0.5f);
  }

  void Start()
  {
    tfm = GetComponent<Transform>();
  }

  private void FixedUpdate()
  {
    // frame = 1/120 s 
    // Time.deltaTime en segs > Tiempo en fixedupdate(x) - Tiempo en fixedupdate(x-1)
    Vector3 pos = tfm.position; // p0
    pos.x += Speed * Direction.x * Time.deltaTime; // p1.x
    pos.y += Speed * Direction.y * Time.deltaTime; // p1.y
    tfm.position = pos;

    // 10 m/s = 10/x m/x(s)

    // 10 m/s = x m/h
  }
}
