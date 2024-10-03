    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
public class AttackAndTakeOnDamage : MonoBehaviour
    {
        protected Collider2D _coll;

        public virtual void OnTakeDamage(Transform transform,float attack,Vector2 attackDisplaces,int AttackStrength,float TenacityDamage,float TenacityDamageRate){

        }
        public virtual void TakeTenacityDamage(float TenacityDamage,float TenacityDamageRateBoost){
           
        }
        public virtual void  OnTriggerEnter2D(Collider2D other) {
        }
    }