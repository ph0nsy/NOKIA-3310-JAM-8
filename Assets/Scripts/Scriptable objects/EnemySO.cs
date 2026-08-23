using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "enemyData", menuName = "ScriptableObj/SpawnEnemyData", order = 1)]
public class EnemySO : ScriptableObject
{
    public int HP = 1;
    //Enemies should start with the same number of bullets as HP
    //public int Bullets = 1;
    public float BulletSpeed = 10f;

}
