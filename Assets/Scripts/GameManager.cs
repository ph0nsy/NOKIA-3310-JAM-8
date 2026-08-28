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
    public bool isGameOver = false;
    public float perfectParryWindow = 0.2f;


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
            Debug.Log("CLICK");
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

        Debug.Log($"Trigger {type} actualizado a: {status}");
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
            Debug.Log($"Early: {earlyParryFlag} | Late: {lateParryFlag} | HP Actual: {hpComponent.CurrentHP}");

            // 1. heal on perfect parry
            if(earlyParryFlag && lateParryFlag){
                Debug.Log("Perfect parry!");

                hpComponent.Heal(1);
            }
            
            BulletScript[] activeBullets = FindObjectsByType<BulletScript>(FindObjectsSortMode.None);
        
            if (activeBullets.Length > 0)
            {
                // 2. Find and destroy the oldest bullet
                BulletScript oldestBullet = activeBullets[0];
                
                foreach (BulletScript b in activeBullets)
                {
                    if (b.transform.position.x < oldestBullet.transform.position.x)
                    {
                        oldestBullet = b;
                    }
                }

                oldestBullet.isParried = true;
                Destroy(oldestBullet.gameObject);
            }
            

            // 3. Reset parry flags and hurt enemy
            ResetParryTrigger();

            EnemyManager.Instance.HurtEnemy();

            Debug.Log($"[POST-COMBATE] Enemigo derrotado. Vida restante del jugador: {hpComponent.CurrentHP}");

        }
        // Debug.Log("Missed!");
        // return;

    }


    private void OnHealthChanged(int currentHP, bool isHealing){
        if (isHealing){
            
            OnHeal(currentHP);
            
        }
        else {
            OnHurt(currentHP);
        }
        Debug.Log($"Vida actualizada. HP Actual: {currentHP}");
    }

    //remove Hat
    private void OnHurt(int currentHP){
        Debug.Log("OURCH HP: "+ hpComponent.CurrentHP);
    }

    //add Hat
    private void OnHeal(int currentHP){
        Debug.Log("HEAL HP: "+ hpComponent.CurrentHP);
    }


    public void Win(){
        Debug.Log("game won");
    }

    public void Lose(){
        Debug.Log("game lost");
        isGameOver = true;
    }
}
