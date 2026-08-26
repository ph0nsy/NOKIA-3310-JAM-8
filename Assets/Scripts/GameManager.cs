using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    public HealthComponent hpComponent;


    public bool earlyParryFlag = false;
    public bool lateParryFlag = false;
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
        hpComponent.OnHealthChanged+=OnHealthChanged;
        hpComponent.OnDeath+=Lose;
        
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

    public void ResetParryTrigger(){
        lateParryFlag = false;
        earlyParryFlag = false;
    }


    private void Parry(){

        if (bIsOnCooldown) {
            Debug.Log("Parry not ready");
            return;
        }
        m_cooldownTimer = Cooldown;
        bIsOnCooldown = true;

        //
        if (earlyParryFlag || lateParryFlag) {
            Debug.Log("Parried!");

            EnemyManager.Instance.currentEnemy.hpComponent.Damage(1);
            GameObject parriedBullet = EnemyManager.Instance.currentEnemy.transform.GetChild(0).gameObject;
            Destroy(parriedBullet);

            // heal on perfect parry
            if(earlyParryFlag && lateParryFlag){
                Debug.Log("Perfect parry!");

                hpComponent.Heal(1);
            }
        }
        Debug.Log("Missed!");
        return;

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

    }

    //add Hat
    private void OnHeal(int HpChange){

    }


    public void Win(){
        Debug.log("game won")
    }

    public void Lose(){
        Debug.log("game lost")
    }
}
