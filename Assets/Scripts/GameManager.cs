using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
 
    public bool inPlay = false;
    public bool ignoreInput = false;
 
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
    }

    void Start()
    {
        hpComponent = GetComponent<HealthComponent>();
        hpComponent.Init(3,1);
        hpComponent.OnHealthChanged+=OnHealthChanged;
        hpComponent.OnDeath+=Lose;
        CinematicSequence.Instance.OnCinematicEnd += ResetGame;
    }

    void ResetGame(int _animIdx)
    {
        if(_animIdx > 0)  
        {
            ignoreInput = false;
            hpComponent.Init(6,1);
            EnemyManager.Instance.CurrEnemyIdx = 0;
        }
        
    }


    void Update(){
        if(ignoreInput) { return; }
        if (Input.GetKeyDown("space")) 
        { 
            Debug.Log("CLICK");
            if (!inPlay) 
            {
                CinematicSequence.Instance.PlayCinematic(1);
                inPlay = true;
                ignoreInput = true;
                return;
            }
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

        //
        if (earlyParryFlag || lateParryFlag) 
        {
            Debug.Log($"Early: {earlyParryFlag} | Late: {lateParryFlag} | HP Actual: {hpComponent.CurrentHP}");
            
            EnemyManager.Instance.currentEnemy.hpComponent.Damage(1);
            GameObject parriedBullet = EnemyManager.Instance.currentEnemy.transform.GetChild(0).gameObject;
            Destroy(parriedBullet);

            // heal on perfect parry
            if(earlyParryFlag && lateParryFlag)
            {
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

        Debug.Log("Missed!");
        return;
    }

    private void OnHealthChanged(int HpChange, bool isHealing)
    {
        if (isHealing){ OnHeal(HpChange); }
        else { OnHurt(HpChange); }
        Debug.Log($"Vida actualizada. HP Actual: {hpComponent.CurrentHP}");
    }

    //remove Hat
    private void OnHurt(int HPchange){
        Debug.Log("OURCH HP: "+ HPchange);
    }

    //add Hat
    private void OnHeal(int HPchange){
        Debug.Log("HEAL HP: "+ HPchange);
    }

    public void Win(){
        CinematicSequence.Instance.PlayCinematic(2);
        inPlay = false;
        ignoreInput = false;
        Debug.Log("game won");
    }

    public void Lose(){
        CinematicSequence.Instance.PlayCinematic(3);
        inPlay = true;
        ignoreInput = false;
        Debug.Log("game lost");
        isGameOver = true;
    }
}
