using System.Configuration;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillSystem/Skills/test")]
public class testSkill : Skill
{
    public float damage;
    public float attackMultiplier;
    public GameObject summonPrefab;
   [HideInInspector] public GameObject summonedObject;
   [HideInInspector] public SkillSummon skillSummon;
    public Vector2 attackDisplaces;
    public override void OnLoad(GameObject user)
    {
        summonedObject = Instantiate(summonPrefab,new Vector3 (9,-3,0), Quaternion.identity);
        animator = summonedObject.GetComponent<Animator>();
        damage = user.GetComponent<Character>().attackPower;
        damage*=attackMultiplier;
        InitializeSkillData(user);
        Debug.Log("技能加載完畢");
    }
    public override void Activate(GameObject user)
    {
        summonedObject.transform.position =user.transform.position+ new Vector3(user.transform.localScale.x,0,0);
        summonedObject.transform.localScale = user.transform.localScale;
        animator.SetTrigger("Attack");
    }



    public override void OnExit()
    {
        
    }
    public void InitializeSkillData(GameObject user){
        ISummonedObject summonComponent = summonedObject.GetComponent<ISummonedObject>();
        summonComponent.Initialize(user, damage,attackDisplaces,duration);
    }
}