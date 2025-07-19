
using System;
using UnityEngine.UI;
using TMPro;
class SkillUIBinder
{
    public readonly int index;
    public readonly bool isE;

    public readonly Image skill_CD_Icon;

    public readonly TextMeshProUGUI canUse_Times;
    // 當前綁定的 Skill
    private Skill _boundSkill;

    // 存放在 Skill 事件上的 delegate，好做 -= 用
    private Action<float>   _onCooldownChanged;
    private Action<int>     _onUseCountChanged;

    public SkillUIBinder(Image skill_CD_Icon, TextMeshProUGUI canUse_Times)
    {
        this.canUse_Times = canUse_Times;
        this.skill_CD_Icon =  skill_CD_Icon;
    }

    // 綁定新的 Skill，會自動幫你把舊的 Unsubscribe
    public void BindTo(Skill newSkill)
    {
        // 如果已經綁過，就先解綁
        if (_boundSkill != null)
        {
            _boundSkill.onCooldownChanged -= _onCooldownChanged;
            _boundSkill.onUseCountChanged  -= _onUseCountChanged;
        }

        // 準備新的 handler
        _onCooldownChanged = percent =>
        {
            skill_CD_Icon.fillAmount = percent;
        };
        _onUseCountChanged = count =>
        {
            canUse_Times.text = count.ToString();
        };

        // 與 newSkill 綁定
        newSkill.onCooldownChanged += _onCooldownChanged;
        newSkill.onUseCountChanged  += _onUseCountChanged;

        // 記住現在綁定的是誰
        _boundSkill = newSkill;
    }

    // 如果你想手動解綁
    public void Unbind()
    {
        if (_boundSkill != null)
        {
            _boundSkill.onCooldownChanged -= _onCooldownChanged;
            _boundSkill.onUseCountChanged  -= _onUseCountChanged;
            _boundSkill = null;
        }
    }
}
