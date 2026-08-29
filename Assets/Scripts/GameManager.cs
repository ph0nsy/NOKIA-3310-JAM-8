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
    }

    void Start()
    {
        hpComponent = GetComponent<HealthComponent>();
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
        if (earlyParryFlag || lateParryFlag) 
        {
            Debug.Log("Parried!");

            EnemyManager.Instance.currentEnemy.hpComponent.Damage(1);
            GameObject parriedBullet = EnemyManager.Instance.currentEnemy.transform.GetChild(0).gameObject;
            Destroy(parriedBullet);

            // heal on perfect parry
            if(earlyParryFlag && lateParryFlag)
            {
                Debug.Log("Perfect parry!");
                hpComponent.Heal(1);
            }
        }
        Debug.Log("Missed!");
        return;
    }

    private void OnHealthChanged(int HpChange, bool isHealing)
    {
        if (isHealing){ OnHeal(HpChange); }
        else { OnHurt(HpChange); }
    }

    //remove Hat
    private void OnHurt(int HpChange)
    {

    }

    //add Hat
    private void OnHeal(int HpChange)
    {

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
    }
}
