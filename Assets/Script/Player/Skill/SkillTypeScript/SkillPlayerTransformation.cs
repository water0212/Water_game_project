using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPlayerTransformation : MonoBehaviour, ITransformation, IDamageProvider
{
    // 初始化 1.將技能物件放入player層級下。
    // 觸發時 2.將角色隱藏(過度動畫)，技能物件顯示(過度動畫)。 3.播放動畫技能動畫
    // 完成後 3.將角色顯示(過度動畫)，將技能物件隱藏(過度動畫)
    //技能層級 需給予此物件 玩家 預計攻擊力(或未定)，
    private GameObject user;//玩家
    private SpriteRenderer userSR;
    private SpriteRenderer SkillObjSR;

    public float SkillDuration;
    private bool isUsingSkill; 
    public float attackDamage {get; set;}
    public Vector2 attackDisplaces {get; set;}
    public int attackStrength {get; set;}
    public float attackMultiplier {get; set;}
    public float TenacityDamage {get; set;}
    public float TenacityDamageRate {get; set;}
    [SerializeField]private float attack;
    // Start is called before the first frame update
    private void OnEnable() {
        userSR = user.GetComponent<SpriteRenderer>();
        SkillObjSR = GetComponent<SpriteRenderer>();
        SkillObjSR.enabled = false;
    }
    public void Initialize(GameObject user, float duration, float attack, float attackMultiplier,Vector2 attackDisplaces,int AttackStrength,float TenacityBlockRate,float TenacityDamage)
    {
        this.user = user;
        this.SkillDuration = duration;
        this.SkillDuration = duration;
        this.attackDamage = attack;
        this.attackMultiplier = attackMultiplier;
        this.attackDisplaces = attackDisplaces;
        this.attackStrength = attackStrength;
        this.TenacityDamageRate = TenacityDamageRate;
        this.TenacityDamage = TenacityDamage;
    }
    public void ActiveSkillTransformation(){
        isUsingSkill = true;
        StartCoroutine(UsingSkill());
    }
        private IEnumerator UsingSkill(){//當isUsingSkill為true時角色消失 施放技能
            
            userSR.enabled = false;
            SkillObjSR.enabled = true;

            Animator animator = GetComponent<Animator>();
            if(animator != null){
                animator.Play("SkillAnimation");
            }

            yield return new WaitUntil(()=> isUsingSkill);
            userSR.enabled = true;
            SkillObjSR.enabled = false;
        }
    public void SetIsUsingSkill(){
        isUsingSkill = true;
    }

}
