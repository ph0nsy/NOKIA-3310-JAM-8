using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public HealthComponent hpComponent;


    public bool earlyParryFlag = false;
    public bool lateParryFlag = false;
    public float Cooldown = 0.1f;
    public bool bIsOnCooldown = false;
    public float m_cooldownTimer = 0.0f;


    void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);

        hpComponent = GetComponent<HealthComponent>();
        hpComponent.Init(3,1);
        hpComponent.OnHealthChanged+=OnHealthChanged;
        hpComponent.OnDeath+=Lose;
        
    }

    void Start(){
    }

    void Update(){
        if (Input.GetKeyDown("space")) {
            // Debug.Log("PArry Attempt");
            Parry();
        }
        UpdateParryCooldownTimer();

    }

    private void UpdateParryCooldownTimer(){
        if (!bIsOnCooldown) { 
            // Debug.Log("Parry Ready");
            return; 
        }
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

    public void ResetParryTrigger(){
        lateParryFlag = false;
        earlyParryFlag = false;
    }


    private void Parry(){

        // Debug.Log(bIsOnCooldown);
        // Debug.Log( m_cooldownTimer);
        if (bIsOnCooldown) {
            Debug.Log("Parry not ready");
            return;
        }
        m_cooldownTimer = Cooldown;
        bIsOnCooldown = true;

        
        if (earlyParryFlag || lateParryFlag) {
            // Debug.Log("Parried!");

            // GameObject parriedBullet = EnemyManager.Instance.currentEnemy.transform.GetChild(0).gameObject;
            // Destroy(parriedBullet);
            // EnemyManager.Instance.currentEnemy.hpComponent.Damage(1);
            

            // heal on perfect parry
            if(earlyParryFlag && lateParryFlag){
                Debug.Log("Perfect parry!");

                hpComponent.Heal(1);
            }

            EnemyManager.Instance.HurtEnemy();

        }
        // Debug.Log("Missed!");
        // return;

    }


    private void OnHealthChanged(int HpChange, bool isHealing){
        if (isHealing){
            
            OnHeal(HpChange);
            
        }
        else {
            OnHurt(HpChange);
        }
    }

    //remove Hat
    private void OnHurt(int HpChange){
        Debug.Log("OURCH HP: "+ hpComponent.CurrentHP);
    }

    //add Hat
    private void OnHeal(int HpChange){
        Debug.Log("HEAL HP: "+ hpComponent.CurrentHP);
    }


    public void Win(){
        Debug.Log("game won");
    }

    public void Lose(){
        Debug.Log("game lost");
    }
}
