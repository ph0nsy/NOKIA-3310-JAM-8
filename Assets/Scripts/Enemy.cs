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


    void Awake(){
        hpComponent = GetComponent<HealthComponent>();
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
        if (GameManager.Instance.isGameOver) return;
        UpdateBulletCooldownTimer();
        Shoot();
    }

    // estos metodos nos valen, pero no para proyectos grandes
    void OnSpawn()
    {
        bIsOnCooldown = true;
    }

    public void OnDespawn(){
        
        Debug.Log("ENEMY DEAD");
        
    }

    void OnHurt(int HPchange, bool isHealing){
        // Debug.Log("I am hurt by the samurai!");
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
    void Shoot(){
        
        if (bIsOnCooldown) {
            // Debug.Log("Bullet not ready");
            return;
        }
        b_cooldownTimer = bulletCooldown;
        bIsOnCooldown = true;

        if(transform.childCount<maxBullletAmount){
            // Debug.Log("BANG!");
            Instantiate(bulletPrefab, transform);
        }
    }
}
