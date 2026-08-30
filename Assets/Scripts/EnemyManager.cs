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

    void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrEnemyIdx = 0;
    }

    public void HurtEnemy()
    {
        currentEnemy.hpComponent.Damage(1);
    }


    // Debe spawn/despawn de enemylist
    // trackear current enemy
    public void Spawn()
    {
        if(CurrEnemyIdx > enemyList.Count - 1) {
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
        
        int enemigosRestantes = enemyList.Count - 1 - CurrEnemyIdx;
        Debug.Log($"Spawning enemigo. Enemigos restantes en lista: {enemigosRestantes}. Max bullets: {currentEnemy.maxBullletAmount}");

        CurrEnemyIdx++;
    }

    public void Despawn(){

        if(!currentEnemy) { return; }

        currentEnemy.OnDespawn();
        StartCoroutine(NextEnemy());
    }

    
    public IEnumerator NextEnemy()
    {
        AnimatorStateInfo stateInfo = currentEnemy.anim.GetCurrentAnimatorStateInfo(0);
        yield return new WaitForSeconds(stateInfo.length * stateInfo.speed * 1.25f);

        currentEnemy.anim.SetBool("Dead", false);
        
        Destroy(currentEnemy.gameObject);

        if(CurrEnemyIdx > enemyList.Count - 1) { GameManager.Instance.Win(); }
        else { Spawn(); }
    }

}
