using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public HealthComponent hpComponent;


    public bool earlyParryFlag;
    public bool lateParryFlag;
    [HideInInspector] public float Cooldown { get; set; }
    [HideInInspector] public bool bIsOnCooldown = false;
    float m_cooldownTimer = 0.0f;


    void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);

        hpComponent = new HealthComponent();

        
    }

    void Update(){
        if (Input.GetKeyDown("space")) {
           Parry();
        }
        UpdateParryCooldownTimer();

    }

    private void UpdateParryCooldownTimer(){
        if (!bIsOnCooldown) { return; }
        m_cooldownTimer -= Time.deltaTime;
        if (m_cooldownTimer <= 0) 
        {
            bIsOnCooldown = false;
        }
    }

    public void UpdateParryTrigger(ParryTrigger.TriggerType type, bool status){
        if(type == ParryTrigger.TriggerType.Late){
            lateParryFlag = status;
        }
        if(type == ParryTrigger.TriggerType.Early){
            earlyParryFlag = status;
        }
    }
    
    private void Parry(){

        if (bIsOnCooldown) {
            Debug.Log("Parry not ready");
            return;
        }
        m_cooldownTimer = Cooldown;
        bIsOnCooldown = true;

        if(earlyParryFlag && lateParryFlag){
            Debug.Log("Perfect parry!");
            return;
        }
        if (earlyParryFlag || lateParryFlag) {
            Debug.Log("Parried!");
            return;
        }
        Debug.Log("Missed!");
        return;

    }

    public void Win(){

    }

    public void Lose(){

    }
}
