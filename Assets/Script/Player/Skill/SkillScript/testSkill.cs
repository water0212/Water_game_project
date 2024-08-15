using System.Configuration;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillSystem/Skills/test")]
public class testSkill : Skill
{
    public bool count_storage_capability;
    public float cooldownCount;
    public int useCount;
    public int MaxUseCount;
    public float attack;
    public float attackMultiplier;
    public GameObject summonPrefab;
   [HideInInspector] public GameObject summonedObject;
   [HideInInspector] public SkillSummonAndDamage skillSummon;
    public Vector2 attackDisplaces;
    public override void OnLoad(GameObject user)
    {
        cooldownCount =0;
        useCount = MaxUseCount;
        summonedObject = Instantiate(summonPrefab,Constants.SkillObjectPoolPosition, Quaternion.identity);
        animator = summonedObject.GetComponent<Animator>();
        skillSummon = summonedObject.GetComponent<SkillSummonAndDamage>();
        attack = user.GetComponent<Character>().attackPower;
        Debug.Log(skillName + "技能加載完畢");
    }
    public override void Activate(GameObject user)
    {
        if(useCount <= 0)return;
        useCount-=1;
        var damage = attack * attackMultiplier;
        InitializeSkillData(user,damage);
        skillSummon.fade = 1;
        skillSummon.isSummoned = true;
        skillSummon.isDissolving = false;
        skillSummon.durationTimeCount = skillSummon.duration;
        summonedObject.transform.position =user.transform.position+ new Vector3(user.transform.localScale.x,0,0);
        summonedObject.transform.localScale = user.transform.localScale;
        animator.SetTrigger("Attack");
        
    }

    public override void Update()
    {
        if(useCount < MaxUseCount){
            cooldownCount -= Time.deltaTime;
            if(cooldownCount < 0){
                useCount ++;
                cooldownCount = cooldown;
            }
        }
    }

    public override void OnExit()
    {
        
    }
    public void InitializeSkillData(GameObject user,float damage){
        ISummonedAndDamageObject summonComponent = summonedObject.GetComponent<ISummonedAndDamageObject>();
        summonComponent.Initialize(user, damage,attackDisplaces,duration);
    }
}