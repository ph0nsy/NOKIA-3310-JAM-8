using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [HideInInspector] public HealthComponent hpComponent;
    public GameObject bulletPrefab;
    public int maxBullletAmount;
    public float bulletCooldown;
    [HideInInspector] public bool bIsOnCooldown = false;
    float b_cooldownTimer = 0.0f;

    public Animator anim;

    void Awake(){
        hpComponent = GetComponent<HealthComponent>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        hpComponent.OnHealthChanged+=OnHurt;
        OnSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.inPlay) { return; }
        if(hpComponent.CurrentHP>0){
            UpdateBulletCooldownTimer();
            Shoot();
        }
    }

    // estos metodos nos valen, pero no para proyectos grandes
    void OnSpawn()
    {
        bIsOnCooldown = true;
        b_cooldownTimer = bulletCooldown;
    }

    public void OnDespawn()
    {
        // Remove bullets to wait for animation
        for(int i = 0; i<transform.childCount; i++) 
        {
           GameObject.Destroy(transform.GetChild(i).gameObject);
        }
        
        anim.SetBool("Dead", true);
    }

    void OnHurt(int HPchange, bool isHealing)
    {    
        anim.SetBool("Hit", true);
        AudioManager.Instance.PlaySFX(ESourceSFX.Surprised);
        StartCoroutine(ResetSpriteFromHit());
    }

    public IEnumerator ResetSpriteFromHit()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Hit", false);
    }

    private void UpdateBulletCooldownTimer(){
        if (!bIsOnCooldown) { return; }
        b_cooldownTimer -= Time.deltaTime;
        if (b_cooldownTimer <= 0) 
        {
            bIsOnCooldown = false;
        }
    }


    // Spawnea bala
    void Shoot()
    {
        if (bIsOnCooldown) {
            // Debug.Log("Bullet not ready");
            return;
        }
        
        b_cooldownTimer = bulletCooldown;
        bIsOnCooldown = true;

        if(transform.childCount < maxBullletAmount)
        {        
            AudioManager.Instance.PlaySFX(ESourceSFX.Gun);
            anim.SetBool("Shooting", true);
            Instantiate(bulletPrefab, transform);
            StartCoroutine(ResetSpriteFromShoot());
        }
        
    }
    
    public IEnumerator ResetSpriteFromShoot()
    {
        yield return new WaitForSeconds(bulletCooldown/4f);
        anim.SetBool("Shooting", false);
    }
}
