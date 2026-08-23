using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryComponent : MonoBehaviour
{
    [HideInInspector] public float Cooldown { get; set; }
    [HideInInspector] public bool bIsOnCooldown = false;
    [HideInInspector] public string Mask { get; set; }
    [HideInInspector] public Vector3 centerLateParryBox {get; set;}
    [HideInInspector] public Vector3 centerEarlyParryBox {get; set;}
    [HideInInspector] public Vector2 size {get; set;}
    float m_cooldownTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if (!bIsOnCooldown) { return; }
        m_cooldownTimer -= Time.deltaTime;
        if (m_cooldownTimer <= 0) 
        {
            bIsOnCooldown = false;
        }

    }

    public Collider2D[] getCollidersInBox(Vector3 center, string mask){
        return Physics2D.OverlapBoxAll(
            center, size, 0, LayerMask.NameToLayer(mask)
        );
        
    }

    public void Parry(){

        if (bIsOnCooldown) {
            Debug.Log("Parry not ready");
            return;
        }
        m_cooldownTimer = Cooldown;
        bIsOnCooldown = true;

        // optimize
        bool earlyCheck = getCollidersInBox(centerEarlyParryBox, Mask).Length>0;
        bool lateCheck = getCollidersInBox(centerLateParryBox, Mask).Length>0;

        if (earlyCheck && lateCheck) {
            Debug.Log("Perfect parry!");
            return;
        }
        if (earlyCheck || lateCheck) {
            Debug.Log("Parried!");
            return;
        }

        Debug.Log("Missed!");
        return;
        


    }
}
