using System.Configuration;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillSystem/Skills/TP")]
public class TPSkill : Skill
{
    public bool count_storage_capability;
    public float cooldownCount;
    public int useCount;
    public int MaxUseCount;
    public float TpTimeCount;
    public float MaxTpTimeCount;
    public float speed;
    public Enemy enemy = null;
    public GameObject summonPrefab;
    public Rigidbody2D rb;
   [HideInInspector] public GameObject summonedObject;
   [HideInInspector] public SkillSummon skillSummon;
    public Vector2 attackDisplaces;
    public bool isSummoned;

    public override void OnLoad(GameObject user)
    {
        cooldownCount = cooldown;
        useCount = MaxUseCount;
        summonedObject = Instantiate(summonPrefab,Constants.SkillObjectPoolPosition, Quaternion.identity);
        animator = summonedObject.GetComponent<Animator>();
        skillSummon = summonedObject.GetComponent<SkillSummon>();
        rb = summonedObject.GetComponent<Rigidbody2D>();
        Debug.Log( skillName + "技能加載完畢");
    }

    public override void Activate(GameObject user)
    {
        if(enemy == null|| isSummoned == false){
            isSummoned = true;
            summonedObject.transform.position =user.transform.position+ new Vector3(user.transform.localScale.x,0,0);
            rb.AddForce(new Vector2(speed,0),ForceMode2D.Impulse);
        }else{

        }
    }
    public override void Update()
    {
        throw new System.NotImplementedException();
    }
    public override void OnExit()
    {
        throw new System.NotImplementedException();
    }

    

    
}
