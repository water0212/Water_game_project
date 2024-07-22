using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class knightEnemy : Enemy
{
    protected override void Awake() {
        base.Awake();
        patrolState = new KnightPatrolState();
        chaseState = new knightChaseState();
    }
    #region 受擊轉向
    public override void EnemyOnTakeDamage(Transform Enemytransform){
        if(Enemytransform.position.x>transform.position.x){
            transform.localScale= new Vector3(1, 1,1);
        }else{
            transform.localScale= new Vector3(-1, 1,1);
        }
    }    
    #endregion
    
}
