using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public HealthComponent hpComponent;
    public GameObject bulletPrefab;

    void Awake(){
        hpComponent = new HealthComponent();
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
        
    }

    // estos metodos nos valen, pero no para proyectos grandes
    void OnSpawn(){

    }

    public void OnDespawn(){
        //spawn the next enemy
        EnemyManager.Instance.Spawn();
        
    }

    void OnHurt(int HPchange, bool isHealing){

    }

    // Spawnea bala
    void Shoot(){

    }
}
