using System.Configuration;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillSystem/Skills/TP")]
public class TPSkill : Skill
{
    private GameObject user;
    public bool count_storage_capability;
    [Header("氣力加成")]
    public float TenacityDamageRateBoost;
    public float TpMarkCount;
    public float MaxTpMarkCount;
    public float speed;
    public float distanceBehide;
    public float verticaloffset;
    public GameObject enemy;
    public GameObject summonPrefab;
    private Rigidbody2D rb;
   [HideInInspector] public GameObject summonedObject;
   [HideInInspector] public SkillSummonAndEffect skillSummon;
    public Vector2 attackDisplaces;
    private PlayerControler PC;
    public  bool canUse;
    public bool canTp;

    public override void OnLoad(GameObject user)
    {
        this.user = user;
        TpMarkCount = 0 ;
        cooldownCount = 0;
        useCount = 0;
        enemy = null;
        summonedObject = Instantiate(summonPrefab,Constants.SkillObjectPoolPosition, Quaternion.identity);
        animator = summonedObject.GetComponent<Animator>();
        skillSummon = summonedObject.GetComponent<SkillSummonAndEffect>();
        rb = summonedObject.GetComponent<Rigidbody2D>();
        PC = user.GetComponent<PlayerControler>();
        InitializeSkillData(user);
        Debug.Log( skillName + "技能加載完畢");
        SkillManager.GetSkillManager().onActivated += Teleport;
    }

    public override bool Activate(GameObject user)
    {
        base.Activate(user);
        try
        {
        if(useCount == 1 && (enemy == null || skillSummon.isSummoned == false)){
            InitializeSkillData(user);
            useCount = 0;
            skillSummon.isSummoned = true;
            summonedObject.transform.position =user.transform.position+ new Vector3(PC.faceOn,1,0);
            summonedObject.transform.localScale = -new Vector3(PC.faceOn,1,1);
            rb.AddForce(new Vector2(speed * -summonedObject.transform.localScale.x,0),ForceMode2D.Impulse);
            SkillManager.GetSkillManager().SkillFinish();
        }else{
            return false;
        }
        return true;
            
        }
        catch (System.Exception)
        {
            
            Debug.LogError("使用技能時出錯");
            return false;
        }
    }
    private void Teleport(Skill skill){
        if(skill.GetId() != 1 && canTp){
            Debug.Log("使用tp");
            SpriteRenderer targetRenderer = enemy.GetComponent<SpriteRenderer>();
            Vector3 targetOffset = new Vector3(-enemy.transform.localScale.x* distanceBehide,0,0);
            user.gameObject.transform.position = enemy.transform.position + targetOffset + new Vector3 ( 0, verticaloffset, 0);
            if(user.gameObject.transform.position.x - enemy.transform.position.x >= 0f){
                Player_Info.ChangePlayerFaceOn(-1);

            }else{
                Player_Info.ChangePlayerFaceOn(1);
            }
            canTp = false;
        }
    }
    public override void Update()
    {
    }
    public void InitializeSkillData(GameObject user){
        ISummonedAndEffectObject summonComponent = summonedObject.GetComponent<ISummonedAndEffectObject>();
        summonComponent.Initialize(user,duration);
    }
    public override bool BackGroundUpdate()
    {
        if(skillSummon != null){
        if(skillSummon.Target != null&&enemy == null){
            enemy = skillSummon.Target;
            canTp = true;
            TpMarkCount = MaxTpMarkCount;
            Debug.Log("目標命中"+enemy.gameObject.name);
        }

        if(enemy){
            TpMarkCount -= Time.deltaTime;
            if(TpMarkCount <= 0 ){
                skillSummon.Target = null;
                enemy = null;
                TpMarkCount = MaxTpMarkCount;
                canTp = false;
            }
        }
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
        SkillManager.GetSkillManager().onActivated -= Teleport;
        Destroy(summonedObject);
    }
}
