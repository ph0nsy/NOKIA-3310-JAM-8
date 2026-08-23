using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
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
        //currentEnemy.bulletPrefab.GetComponent<Bullet>().speed = enemyList[0].BulletSpeed;

        enemyList.Remove(tmpEnemySO);


    }

    public void Despawn(){
        currentEnemy.OnDespawn();

        // yield WaitForSeconds(currentEnemy.Animation["Death"].length*currentEnemy.Animation["Death"].speed)
        if(enemyList.Count < 1) {
            Debug.Log("GameManager.win()");
        }
        
    }

}
