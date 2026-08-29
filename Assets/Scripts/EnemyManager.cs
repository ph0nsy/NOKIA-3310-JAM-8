using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public const float PIXELRATIO = 175/420;

    public static EnemyManager Instance { get; private set; }
    
    public List<EnemySO> enemyList = new List<EnemySO>();
    public int CurrEnemyIdx { get; set; }

    public Enemy currentEnemy;
    public GameObject enemyPrefab;
    public Transform EnemySpawnPoint;
    [SerializeField] private float timeBetweenEnemies = 3f;

    void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrEnemyIdx = 0;
    }

    public void Start(){
        StartCoroutine(SpawnRoutine());
    }

    public void HurtEnemy(){
        currentEnemy.hpComponent.Damage(1);
    }


    // Debe spawn/despawn de enemylist
    // trackear current enemy
    public void Spawn(){
        if(enemyList.Count <= CurrEnemyIdx) {
            return;
        }
        EnemySO tmpEnemySO = enemyList[CurrEnemyIdx];

        Debug.Log("Spawning enemy");
        
        currentEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
        currentEnemy.hpComponent.Init(tmpEnemySO.HP, tmpEnemySO.HP);

        currentEnemy.hpComponent.OnDeath+=Despawn;

        currentEnemy.bulletCooldown = tmpEnemySO.Handicap + PIXELRATIO*(tmpEnemySO.BulletSize + 4 )/tmpEnemySO.BulletSpeed;
        currentEnemy.maxBullletAmount = tmpEnemySO.HP;

        currentEnemy.bulletPrefab.GetComponent<BulletScript>().speed = tmpEnemySO.BulletSpeed;
        currentEnemy.bulletPrefab.GetComponent<BulletScript>().size = tmpEnemySO.BulletSize;
        
        CurrEnemyIdx++;

        int enemigosRestantes = enemyList.Count - CurrEnemyIdx;
        Debug.Log($"Spawning enemigo. Enemigos restantes en lista: {enemigosRestantes}. Max bullets: {currentEnemy.maxBullletAmount}");


    }

    public void Despawn(){

        currentEnemy.OnDespawn();
        // yield WaitForSeconds(currentEnemy.Animation["Death"].length*currentEnemy.Animation["Death"].speed)
        Destroy(currentEnemy.gameObject);

        if(CurrEnemyIdx >= enemyList.Count - 1) {
            Debug.Log("GameManager.win()");
            GameManager.Instance.Win();
            return;
        }
        
        Spawn();
    }

}
