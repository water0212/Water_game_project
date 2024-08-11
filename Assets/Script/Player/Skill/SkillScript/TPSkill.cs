using System.Configuration;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillSystem/Skills/TP")]
public class TPSkill : Skill
{
    public bool count_storage_capability;
    public float cooldownCount;
    public float TpMarkCount;
    public float MaxTpMarkCount;
    public float speed;
    public float distanceBehide;
    public float verticaloffset;
    public Enemy enemy = null;
    public GameObject summonPrefab;
    private Rigidbody2D rb;
   [HideInInspector] public GameObject summonedObject;
   [HideInInspector] public SkillSummonAndEffect skillSummon;
    public Vector2 attackDisplaces;
    public  bool canUse;
    public bool canTp;

    public override void OnLoad(GameObject user)
    {
        TpMarkCount = MaxTpMarkCount;
        cooldownCount = cooldown;
        summonedObject = Instantiate(summonPrefab,Constants.SkillObjectPoolPosition, Quaternion.identity);
        animator = summonedObject.GetComponent<Animator>();
        skillSummon = summonedObject.GetComponent<SkillSummonAndEffect>();
        rb = summonedObject.GetComponent<Rigidbody2D>();
        InitializeSkillData(user);
        Debug.Log( skillName + "技能加載完畢");
    }

    public override void Activate(GameObject user)
    {
        if(enemy == null|| skillSummon.isSummoned == false&&canUse){
            InitializeSkillData(user);
            canUse = false;
            skillSummon.isSummoned = true;
            summonedObject.transform.position =user.transform.position+ new Vector3(user.transform.localScale.x,1,0);
            summonedObject.transform.localScale = -user.transform.localScale;
            rb.AddForce(new Vector2(speed * -summonedObject.transform.localScale.x,0),ForceMode2D.Impulse);
        }else if (enemy !=null&&canTp){
            //TODO:傳送片段
            SpriteRenderer targetRenderer = enemy.GetComponent<SpriteRenderer>();
            Vector3 targetOffset = new Vector3(-enemy.transform.localScale.x* distanceBehide,0,0);
            user.gameObject.transform.position = enemy.transform.position + targetOffset + new Vector3 ( 0, verticaloffset, 0);
            canTp = false;
        }
    }
    public override void Update()
    {
        if(!canUse){
            cooldownCount -= Time.deltaTime;
            if(cooldownCount <= 0){
                canUse = true;
                cooldownCount = cooldown;
            }
        }
        if(skillSummon.Target != null&&enemy == null){
            enemy = skillSummon.Target.GetComponent<Enemy>();
            canTp = true;
            TpMarkCount = MaxTpMarkCount;
        }
        if(enemy){
            TpMarkCount -= Time.deltaTime;
            if(TpMarkCount <= 0 ){
                skillSummon.Target = null;
                enemy = null;
                TpMarkCount = MaxTpMarkCount;
            }
        }
    }
    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }
    public void InitializeSkillData(GameObject user){
        ISummonedAndEffectObject summonComponent = summonedObject.GetComponent<ISummonedAndEffectObject>();
        summonComponent.Initialize(user,duration);
    }
    
}
