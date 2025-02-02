using UnityEngine;

public interface ISummonedAndDamageObject

{//召喚並傷害類技能
    /// <summary>
    /// 召喚/傷害技能之初始化
    /// </summary>
    /// <param name="user">使用者</param>
    /// <param name="attack">傷害</param>
    /// <param name="attackDisplaces">擊退效果</param>
    /// <param name="duration">持續時間</param>
    void Initialize(GameObject user, float attack, float attackMutiplier,Vector2 attackDisplaces, float duration,int AttackStrength,float TenacityBlockRate,float TenacityDamage);
}
public interface ISummonedAndEffectObject
    {//召喚並特定效果類技能

       void Initialize(GameObject user, float duration); 
    }

    public interface IInteractable
    {
    // Start is called before the first frame update
    /// <summary>
    /// 下方選入要執行的指令碼
    /// </summary>
    void TriggerAction();//這是關於可互動物品之接口
}
public interface ITransformation
    {//追憶類技能，
        void Initialize(GameObject user, float duration, float attack, float attackMultiplier,Vector2 attackDisplaces,int AttackStrength,float TenacityBlockRate,float TenacityDamage);
    }
public interface IDamageProvider{
    //只需要輸出時的接口 目前使用於技能上
    float attackDamage { get; set;}
    float attackMultiplier{ get; set;}
    int attackStrength { get; set;}
    float TenacityDamage { get; set;}
    float TenacityDamageRate { get; set;}
    Vector2 attackDisplaces { get; set;}

}

