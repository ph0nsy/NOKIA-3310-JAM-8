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
        GameManager.Instance.UpdateParryTrigger(tipo, true);
    }
    void OnTriggerExit2D(Collider2D other){
        GameManager.Instance.UpdateParryTrigger(tipo, false);
    }

    
}
