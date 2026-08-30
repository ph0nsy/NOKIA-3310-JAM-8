using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
 
    public bool inPlay = false;
    public bool ignoreInput = false;
    // public bool isGameFinished =false;
 
    public HealthComponent hpComponent;

    public bool earlyParryFlag = false;
    public bool lateParryFlag = false;
    public float Cooldown = 0.25f;
    public bool bIsOnCooldown = false;
    public float m_cooldownTimer = 0.0f;
    public float perfectParryWindow = 0.2f;

    private Animator anim;

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
        hpComponent.Init(6,1);
        hpComponent.OnHealthChanged+=Hurt;
        hpComponent.OnDeath+=Lose;
        CinematicSequence.Instance.OnCinematicEnd += ResetGame;
        anim = GetComponent<Animator>();
        AudioManager.Instance.PlayBGM(ESourceBGM.Intro);
    }

    void Hurt(int _hp, bool isHeal)
    {   
        if(isHeal) { return; }
        AudioManager.Instance.PlaySFX(ESourceSFX.Hurt);
    }

    void ResetGame(int _animIdx)
    {
        if(_animIdx > 0)  
        {
            Debug.Log("GAME START");
            // inPlay = true;
            ignoreInput = false;
            hpComponent.Init(6,1);
            EnemyManager.Instance.CurrEnemyIdx = 0;
            anim.SetBool("Dead", false);
            anim.SetBool("Parrying", false);
        }
        if(_animIdx == 1) 
        { 
            inPlay = true;
            EnemyManager.Instance.Spawn(); 
            AudioManager.Instance.PlayBGM(ESourceBGM.Level);
        }
    }

    void Update()
    {
        if(ignoreInput) { return; }
        if (Input.GetKeyDown("space")) 
        { 
            // Debug.Log("CLICK");
            if (!inPlay) 
            {
                if(CinematicSequence.Instance.activeCinematic.Count>0){
                    CinematicSequence.Instance.StopCinematic();
                    return;
                }
                CinematicSequence.Instance.PlayCinematic(1);
                AudioManager.Instance.PlayBGM(ESourceBGM.Intro);
                AudioManager.Instance.PlaySFX(ESourceSFX.Button);
                // ignoreInput = true;
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

        // Debug.Log($"Trigger {type} actualizado a: {status}");
    }

    public void ResetParryTrigger(){
        lateParryFlag = false;
        earlyParryFlag = false;
    }

    private void Parry()
    {
        if (bIsOnCooldown) {
            // Debug.Log("Parry not ready");
            return;
        }
        m_cooldownTimer = Cooldown;
        bIsOnCooldown = true;

        anim.SetBool("Parrying", true);
        //
        if (earlyParryFlag || lateParryFlag) 
        {
            AudioManager.Instance.PlaySFX(ESourceSFX.Parry);
            // Debug.Log($"Early: {earlyParryFlag} | Late: {lateParryFlag} | HP Actual: {hpComponent.CurrentHP}");
            
            // heal on perfect parry
            if(earlyParryFlag && lateParryFlag)
            {
                // Debug.Log("Perfect parry!");
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
        else 
        {
            AudioManager.Instance.PlaySFX(ESourceSFX.Slash);
        }


        StartCoroutine(ResetSprite());
        Debug.Log("Missed!");
        return;
    }

    public IEnumerator ResetSprite()
    {
        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(Cooldown);
        anim.SetBool("Parrying", false);
    }

    public void Win()
    {
        CinematicSequence.Instance.PlayCinematic(2);
        AudioManager.Instance.PlayBGM(ESourceBGM.Win);
        inPlay = false;
        ignoreInput = false;
        // isGameFinished = true;
        Debug.Log("game won");
    }

    public void Lose()
    {
        ignoreInput = true;
        anim.SetBool("Dead", true);
        StartCoroutine(LoseSequence());
    }

    public IEnumerator LoseSequence() 
    {
        yield return new WaitForSeconds(1);
        ignoreInput = false;
        inPlay = false;
        // isGameFinished = true;
        EnemyManager.Instance.Despawn();
        
        CinematicSequence.Instance.PlayCinematic(3);
        AudioManager.Instance.PlayBGM(ESourceBGM.Lose);
        
        
        Debug.Log("game lost");
    }
}
