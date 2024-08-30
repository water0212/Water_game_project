using UnityEngine;

public interface ISummonedAndDamageObject

{
    /// <summary>
    /// 召喚/傷害技能之初始化
    /// </summary>
    /// <param name="user">使用者</param>
    /// <param name="attack">傷害</param>
    /// <param name="attackDisplaces">擊退效果</param>
    /// <param name="duration">持續時間</param>
    void Initialize(GameObject user, float attack, Vector2 attackDisplaces, float duration,int AttackStrength);
}
public interface ISummonedAndEffectObject
    {

       void Initialize(GameObject user, float duration); 
    }

    public interface IInteractable
    {
    // Start is called before the first frame update
    /// <summary>
    /// 下方選入要執行的指令碼
    /// </summary>
    void TriggerAction();
}

