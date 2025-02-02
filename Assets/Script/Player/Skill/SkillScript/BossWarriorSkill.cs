using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillSystem/Skills/BossWarrior")]
public class BossWarriorSkill : Skill
{
    [Header("力道")]
    private float attack;
    public float attackMultiplier;
    public float attackDamage;
    public int attackStrength;
    public Vector2 attackDisplaces {get; set;}
    [Header("氣力傷害")]
    public float TenacityDamage;
    public float TenacityDamageRate;
    public GameObject summonPrefeb;
    private GameObject summonedObj;
    private SkillPlayerTransformation skillSummon;
    private PlayerControler playerControler;

    public override void OnLoad(GameObject user)
    {
        cooldownCount = 0;
        useCount = MaxUseCount -1;
        Vector2 spawnPosition = (Vector2)user.transform.position + new Vector2(0.44f, 1.41f);
        summonedObj = Instantiate(summonPrefeb, spawnPosition, Quaternion.identity, user.transform);
        summonedObj.transform.localPosition = new Vector2(0.44f, 1.41f);
        skillSummon = summonedObj.GetComponent<SkillPlayerTransformation>();
        playerControler = user.GetComponent<PlayerControler>();
        Debug.Log(skillName + "技能加載完畢");
    }
    public override bool Activate(GameObject user)
    {
        if(useCount <= 0) return false;
        useCount--;
        InitializeSkillData(user);
        //user.isUsingSkill = true;
        //user.SetUsingSkill(duration);
        return true;
    }

    public override void OnEquip()
    {
        throw new System.NotImplementedException();
    }


    public override void UnEquip()
    {
        throw new System.NotImplementedException();
    }

    public override void UnLoad()
    {
        throw new System.NotImplementedException();
    }

    public override void Update()
    {
        throw new System.NotImplementedException();
    }
    public void InitializeSkillData(GameObject user){
        Character character= user.GetComponent<Character>();
        attack = character.attackPower;
        ITransformation transformation = summonedObj.GetComponent<ITransformation>();
        transformation.Initialize(user, duration,attack, attackMultiplier, attackDisplaces, attackStrength, TenacityDamageRate, TenacityDamage);
    }
}
