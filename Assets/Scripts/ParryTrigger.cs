using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryTrigger : MonoBehaviour
{
    public enum TriggerType { Late, Early};
    public TriggerType tipo;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
    
    }

    void OnTriggerEnter2D(Collider2D other){
        // Debug.Log("Something came in");
        GameManager.Instance.UpdateParryTrigger(tipo, true);
    }
    void OnTriggerExit2D(Collider2D other){
        // Debug.Log("something came OUT");
        GameManager.Instance.UpdateParryTrigger(tipo, false);
        if (tipo == TriggerType.Late){
            // Debug.Log("DESTROY BULLET");
            Destroy(other.gameObject);
            GameManager.Instance.hpComponent.Damage(1);
        }
    }

    
}
