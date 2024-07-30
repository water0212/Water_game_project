using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "SkillSystem/Skills/test")]
public class testSkill : Skill
{
    public float damage;
    public GameObject summonPrefab;
    public override void Activate(GameObject user)
    {
        GameObject summonedObject = Instantiate(summonPrefab, user.transform.position + new Vector3(user.transform.localScale.x,0,0), Quaternion.identity);
    }

    public override void OnLoad()
    {
        Debug.Log("技能加載完畢");
    }

    public override void OnExit()
    {
        
    }
}