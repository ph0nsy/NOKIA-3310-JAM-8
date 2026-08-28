using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryTrigger : MonoBehaviour
{
    public enum TriggerType { Late, Early};
    public TriggerType tipo;
    public static bool wasParried = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
    
    }

    void OnTriggerEnter2D(Collider2D other){
        // Debug.Log("Something came in");
        if (other.CompareTag("Bullet"))
        {
            GameManager.Instance.UpdateParryTrigger(tipo, true);
        }
    }
    void OnTriggerExit2D(Collider2D other){
        // Debug.Log("something came OUT");

        if (other.CompareTag("Bullet"))
        {
            GameManager.Instance.UpdateParryTrigger(tipo, false);
            BulletScript bullet = other.GetComponent<BulletScript>();
            if (tipo == TriggerType.Late && !bullet.isParried){
                // Debug.Log("DESTROY BULLET");
                Destroy(other.gameObject);
                if (!GameManager.Instance.isGameOver)
                {
                    GameManager.Instance.hpComponent.Damage(1);
                }
            }
        }
    }

    
}
