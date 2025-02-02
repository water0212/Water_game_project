using System.Configuration;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D.IK;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillSystem/Skills/test")]
public class testSkill : Skill
{
    [Header("力道")]
    private float attack;
    public float attackMultiplier;
    public int AttackStrength;
    public Vector2 attackDisplaces;
    
    [Header("氣力傷害")]
    public float TenacityDamage;
    public GameObject summonPrefab;
   [HideInInspector] public GameObject summonedObject;
   [HideInInspector] public SkillSummonAndDamage skillSummon;
    private PlayerControler PC;
    public override void OnLoad(GameObject user)
    {
        cooldownCount =  0;
        useCount = MaxUseCount -1;
        summonedObject = Instantiate(summonPrefab,Constants.SkillObjectPoolPosition, Quaternion.identity);
        animator = summonedObject.GetComponent<Animator>();
        skillSummon = summonedObject.GetComponent<SkillSummonAndDamage>();
        PC = user.GetComponent<PlayerControler>();
        Debug.Log(skillName + "技能加載完畢");
    }
    public override bool Activate(GameObject user)
    {
        if(useCount <= 0)return false;
        useCount--;
        InitializeSkillData(user);
        skillSummon.fade = 1;
        skillSummon.isSummoned = true;
        skillSummon.isDissolving = false;
        skillSummon.durationTimeCount = skillSummon.duration;
        summonedObject.transform.position =user.transform.position+ new Vector3(PC.faceOn,0.5f,0);
        summonedObject.transform.localScale = new Vector3(PC.faceOn,1,1);
        animator.SetTrigger("Attack");
        return true;
    }

    public override void Update()
    {
    }
    public void InitializeSkillData(GameObject user){
        Character cc = user.GetComponent<Character>();
        attack = cc.attackPower;
        ISummonedAndDamageObject summonComponent = summonedObject.GetComponent<ISummonedAndDamageObject>();
        summonComponent.Initialize(user, attack, attackMultiplier,attackDisplaces,duration,AttackStrength,cc.TenacityDamageRate,TenacityDamage);
    }
    public override bool BackGroundUpdate()
    {
        return base.BackGroundUpdate();
    }

    public override void OnEquip()
    {
        //throw new System.NotImplementedException();
    }

    public override void UnEquip()
    {
        //throw new System.NotImplementedException();
    }

    public override void UnLoad()
    {
        Destroy(summonedObject);
    }
}