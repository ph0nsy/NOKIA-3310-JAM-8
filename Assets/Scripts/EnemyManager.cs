using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public const float PIXELRATIO = 175/420;


    public static EnemyManager Instance {get; private set;}
    public List<EnemySO> enemyList = new List<EnemySO>();
    public Enemy currentEnemy;
    public GameObject enemyPrefab;
    public Transform EnemySpawnPoint;

    void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Start(){
        Spawn();
    }


    // Debe spawn/despawn de enemylist
    // trackear current enemy
    public void Spawn(){
        if(enemyList.Count < 1) {
            return;
        }
        EnemySO tmpEnemySO = enemyList[0];
        currentEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
        currentEnemy.hpComponent.Init(tmpEnemySO.HP, tmpEnemySO.HP);

        currentEnemy.hpComponent.OnDeath+=Despawn;

        currentEnemy.bulletCooldown = tmpEnemySO.Handicap + PIXELRATIO*(tmpEnemySO.BulletSize + 4 )/tmpEnemySO.BulletSpeed;
        currentEnemy.maxBullletAmount = tmpEnemySO.HP;

        currentEnemy.bulletPrefab.GetComponent<BulletScript>().speed = enemyList[0].BulletSpeed;
        currentEnemy.bulletPrefab.GetComponent<BulletScript>().size = enemyList[0].BulletSize;

        enemyList.Remove(tmpEnemySO);


    }

    public void Despawn(){

        currentEnemy.OnDespawn();
        // yield WaitForSeconds(currentEnemy.Animation["Death"].length*currentEnemy.Animation["Death"].speed)
        Destroy(currentEnemy.gameObject);

        if(enemyList.Count < 1) {
            Debug.Log("GameManager.win()");
            GameManager.Instance.Win();
            return;
        }
        Spawn();
        
    }

}
