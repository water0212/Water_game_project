using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.U2D.IK;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillSystem/Skills/BossWarrior")]
public class BossWarriorSkill : Skill
{
    private GameObject user;
    [Header("關於此技能")]
    [Tooltip("施放距離")]
    public float maxDistance;
    [Tooltip("向上傳送距離")]
    public float tpDistance;
    [Tooltip("誤差值")]
    public float offest;
    [Tooltip("準備偵測的layer")]
    public LayerMask Layer;
    [SerializeField]private Vector2 spawnPosition;
    [Header("力道")]
    private float attack;
    public float attackMultiplier;
    public int attackStrength;
    public Vector2 attackDisplaces;
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
        useCount = MaxUseCount;
        Vector2 spawnPosition = (Vector2)user.transform.position + new Vector2(0.44f, 1.41f);
        summonedObj = Instantiate(summonPrefeb, spawnPosition, Quaternion.identity, user.transform);
        summonedObj.transform.localPosition = new Vector2(0.44f, 1.41f);
        skillSummon = summonedObj.GetComponent<SkillPlayerTransformation>();
        playerControler = user.GetComponent<PlayerControler>();
        this.user = user;
        Debug.Log(skillName + "技能加載完畢");
    }
    public override bool Activate(GameObject user)
    {
        base.Activate(user);
        spawnPosition = CheckSpawnSkillPosition();
        try
        {
        if(useCount <= 0) return false;
        NotifyActivated(this);
        useCount--;
        InitializeSkillData(user);
        user.transform.position = spawnPosition;
        skillSummon.ActiveSkillTransformation();
        //user.isUsingSkill = true;
        //user.SetUsingSkill(duration);
        return true;
        }
        catch (System.Exception)
        {
            Debug.LogError("使用技能時出錯");
            return false;
        }
    }
    public override bool BackGroundUpdate()
    {
        
            if(skillSummon.skillAnimationClock[0]){
                user.transform.position = user.transform.position + new Vector3(0,tpDistance,0);
                skillSummon.skillAnimationClock[0] = false;
                
            }else if(skillSummon.skillAnimationClock[1]){
                user.GetComponent<Rigidbody2D>().gravityScale = 16f;
                skillSummon.skillAnimationClock[1] = false;
            }
        return base.BackGroundUpdate();
    }

    public override void OnEquip()
    {
        
    }


    public override void UnEquip()
    {
        
    }

    public override void UnLoad()
    {
        
    }

    public override void Update()
    {
        spawnPosition = CheckSpawnSkillPosition();
    }

    private Vector2 CheckSpawnSkillPosition()
    {
        Vector2 startPosition = new Vector2(user.transform.position.x, user.transform.position.y +0.3f);
        Vector2 direction = new Vector2(Player_Info.GetPlayerFaceon(), 0);
        RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, maxDistance,Layer);
        Vector2 Position;
        if(hit.collider != null && hit.collider.tag == "Ground"){
            Position = hit.point - new Vector2(offest*Player_Info.GetPlayerFaceon(),0);
        }else if(hit.collider != null && hit.collider.tag == "Enemy"){
            Position = hit.point;
        } 
        else{
            Position = startPosition + new Vector2(Player_Info.GetPlayerFaceon() * maxDistance,0);
        }

        return Position;
    }

    public void InitializeSkillData(GameObject user){
        Character character= user.GetComponent<Character>();
        attack = character.attackPower;
        ITransformation transformation = summonedObj.GetComponent<ITransformation>();
        transformation.Initialize(user,clip, duration,attack, attackMultiplier, attackDisplaces, attackStrength, TenacityDamageRate, TenacityDamage);
    }
    public void OnDrawGizmos()
    {
        // 繪製 Raycast 來偵測最大距離
        Gizmos.color = Color.red;
        Vector2 startPosition = new Vector2(user.transform.position.x, user.transform.position.y + 0.1f);
        Gizmos.DrawLine(startPosition, startPosition + new Vector2(Player_Info.GetPlayerFaceon(), 0) * maxDistance);
    }
}
