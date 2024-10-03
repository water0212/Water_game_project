using System.Configuration;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D.IK;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillSystem/Skills/test")]
public class testSkill : Skill
{
    public bool count_storage_capability;
    public float attack;
    public float attackMultiplier;
    [Header("力道")]
    public int AttackStrength;
    
    [Header("氣力傷害")]
    public float TenacityDamage;
    public GameObject summonPrefab;
   [HideInInspector] public GameObject summonedObject;
   [HideInInspector] public SkillSummonAndDamage skillSummon;
    public Vector2 attackDisplaces;
    private PlayerControler PC;
    public override void OnLoad(GameObject user)
    {
        cooldownCount =  0;
        useCount = MaxUseCount -1;
        summonedObject = Instantiate(summonPrefab,Constants.SkillObjectPoolPosition, Quaternion.identity);
        animator = summonedObject.GetComponent<Animator>();
        skillSummon = summonedObject.GetComponent<SkillSummonAndDamage>();
        attack = user.GetComponent<Character>().attackPower;
        PC = user.GetComponent<PlayerControler>();
        Debug.Log(skillName + "技能加載完畢");
    }
    public override void Activate(GameObject user)
    {
        if(useCount <= 0)return;
        useCount--;
        InitializeSkillData(user);
        skillSummon.fade = 1;
        skillSummon.isSummoned = true;
        skillSummon.isDissolving = false;
        skillSummon.durationTimeCount = skillSummon.duration;
        summonedObject.transform.position =user.transform.position+ new Vector3(PC.faceOn,0.5f,0);
        summonedObject.transform.localScale = new Vector3(PC.faceOn,1,1);
        animator.SetTrigger("Attack");
        
    }

    public override void Update()
    {
        if(useCount < MaxUseCount){
            cooldownCount += Time.deltaTime;
            if(cooldownCount > cooldown){
                useCount ++;
                cooldownCount = 0;
            }
        }
    }

    public override void OnExit()
    {
        
    }
    public void InitializeSkillData(GameObject user){
        Character cc = user.GetComponent<Character>();
        var damage = cc.attackPower*attackMultiplier;
        ISummonedAndDamageObject summonComponent = summonedObject.GetComponent<ISummonedAndDamageObject>();
        summonComponent.Initialize(user, damage,attackDisplaces,duration,AttackStrength,cc.TenacityDamageRate,TenacityDamage);
    }
}