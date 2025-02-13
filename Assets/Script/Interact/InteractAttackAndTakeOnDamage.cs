using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class InteractAttackAndTakeOnDamage : AttackAndTakeOnDamage,IInteractable
{
    public InteractCustomAttackAndTakeOnDamage AATD;
    private Animator animator;
    public void OnEnable() {
      //  AATD.DamageTextSpawner = GetComponent<DamageTextSpawner>();
        animator = GetComponent<Animator>();
    }

    public void TriggerAction()
    {
        
    }
    public override void OnTakeDamage(Transform transform, float attack, Vector2 attackDisplaces, int AttackStrength, float TenacityDamage, float TenacityDamageRate)
    {
        if(!AATD.TakeOnDamage) return;
        base.OnTakeDamage(transform, attack, attackDisplaces, AttackStrength, TenacityDamage, TenacityDamageRate);
        if(AATD.Health-1>0){
            animator.Play("Hurt");
            AATD.Health-=1;
            AttackScene.GetInstance().HitPause(AttackStrength);
            CamaeraControl.GetInstance().CameraShake(attackDisplaces);
        }else{
            animator.Play("ignite");
            this.tag = "unknown";
            AATD.TakeOnDamage = false;
        }
    }
}
[System.Serializable]
    public class InteractCustomAttackAndTakeOnDamage{
        public bool TakeOnDamage = false;
        [HideInInspector]public int Health;
        [HideInInspector] public DamageTextSpawner DamageTextSpawner;

    }
    [CustomEditor(typeof(InteractAttackAndTakeOnDamage))]
public class InteractAttackAndTakeOnDamageEditor : Editor{
        InteractAttackAndTakeOnDamage Interact;
        private void OnEnable(){
            Interact = (InteractAttackAndTakeOnDamage)target;
        }
        public override void OnInspectorGUI(){
            DrawDefaultInspector();
            if(Interact.AATD.TakeOnDamage){
                Interact.AATD.DamageTextSpawner = (DamageTextSpawner)EditorGUILayout.ObjectField("生成傷害數字",Interact.AATD.DamageTextSpawner,typeof(DamageTextSpawner), true);
                Interact.AATD.Health = EditorGUILayout.IntField("可受到傷害的次數",Interact.AATD.Health);
            }
        }
    }
