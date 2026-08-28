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
    [SerializeField] private float timeBetweenEnemies = 3f;

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
        StartCoroutine(SpawnRoutine());
    }

    public void HurtEnemy(){
        currentEnemy.hpComponent.Damage(1);
    }


    // Debe spawn/despawn de enemylist
    // trackear current enemy
    public IEnumerator SpawnRoutine(){

        yield return new WaitForSeconds(timeBetweenEnemies);

        if(GameManager.Instance.isGameOver || enemyList.Count < 1) {
            yield break;
        }

        EnemySO tmpEnemySO = enemyList[0];
        Debug.Log("Spawning enemy");
        currentEnemy = Instantiate(enemyPrefab).GetComponent<Enemy>();
        currentEnemy.hpComponent.Init(tmpEnemySO.HP, tmpEnemySO.HP);

        currentEnemy.hpComponent.OnDeath+=Despawn;

        currentEnemy.bulletCooldown = tmpEnemySO.Handicap + PIXELRATIO*(tmpEnemySO.BulletSize + 4 )/tmpEnemySO.BulletSpeed;
        currentEnemy.maxBullletAmount = tmpEnemySO.HP;

        currentEnemy.bulletPrefab.GetComponent<BulletScript>().speed = enemyList[0].BulletSpeed;
        currentEnemy.bulletPrefab.GetComponent<BulletScript>().size = enemyList[0].BulletSize;

        int enemigosRestantes = enemyList.Count;
        Debug.Log($"Spawning enemigo. Enemigos restantes en lista: {enemigosRestantes}. Max bullets: {currentEnemy.maxBullletAmount}");

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
        StartCoroutine(SpawnRoutine());
        
    }

}
