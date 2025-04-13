using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Diagnostics.Eventing.Reader;
using Cinemachine;
public class InteractAttackAndTakeOnDamage : AttackAndTakeOnDamage,IInteractable
{
    public InteractCustomAttackAndTakeOnDamage AATD;
    private Animator animator;
    private bool Obtained;
    private SpriteRenderer renderer;
    private Rigidbody2D rigidbody2D;
    public float FadeDuration;
    private float FadeDurationCount;
    public float floatForce;
    public CinemachineVirtualCamera Boss;
    public CinemachineVirtualCamera Open;
    public void OnEnable() {
      //  AATD.DamageTextSpawner = GetComponent<DamageTextSpawner>();
        animator = GetComponent<Animator>();
        FadeDurationCount = 0;
        this._coll = GetComponent<Collider2D>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        
    }

    public void TriggerAction()
    {
        SkillManager.GetSkillManager().AddSkill(2);
        Obtained = true;
        this.tag = "Untagged";
        _coll.enabled = false;
        rigidbody2D.gravityScale = 0;
        CameraManager.instence.SwapCamera(Boss,Open,new Vector2(1f,0f));
    }

    public override void OnTakeDamage(Transform transform, float attack, Vector2 attackDisplaces, int AttackStrength, float TenacityDamage, float TenacityDamageRate)
    {
        if(!AATD.TakeOnDamage) return;
        base.OnTakeDamage(transform, attack, attackDisplaces, AttackStrength, TenacityDamage, TenacityDamageRate);
        if(AATD.Health-1>0){
            animator.Play("Hurt");
            AATD.Health-=1;
        }else{
            animator.Play("ignite");
            this.tag = "unknown";
            AATD.TakeOnDamage = false;
        }
            AttackScene.GetInstance().HitPause(AttackStrength);
            CamaeraControl.GetInstance().CameraShake(attackDisplaces);
    }
    private void Update() {
        if(Obtained){

            
            FadeDurationCount += Time.deltaTime;
            if(FadeDurationCount < FadeDuration){
                float alpha = Mathf.Lerp(1f,0f, FadeDurationCount/FadeDuration);
                Color color = renderer.color;
                color.a = alpha;
                renderer.color = color; 
                transform.position += Vector3.up * floatForce * alpha* Time.deltaTime;
            }else{
                Destroy(this.gameObject);
            }
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
