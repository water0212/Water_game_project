// SkillManager.cs (重構版)
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillManager : MonoBehaviour
{
    public GameObject Player;
    public SkillDatabase skillDatabase;
    public SkillUIManager skillUIManager;
    public Skill testSkillLoad;

    public string LeftControlButton = "q";
    public string RightControlButton = "e";

    public SkillSlotGroup Skill_Q = new SkillSlotGroup();
    public SkillSlotGroup Skill_E = new SkillSlotGroup();

    public static bool isUsingSkill = false;
    private bool skillFinish = false;

    public SkillEventSO CastSkill;
    public VoidEventSO FinishSkill;
    public FloatEventSO GainSkillEvent;

    private static SkillManager instance;
    public static SkillManager GetSkillManager() => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one SkillManager in the scene.");
            Destroy(this);
        }
        instance = this;

        Skill_Q.Initialize(Player, 5f, OnEquip_Q, UpdateFadeVisual_Q);
        Skill_E.Initialize(Player, 5f, OnEquip_E, UpdateFadeVisual_E);
    }

    private void OnEnable() => GainSkillEvent.OnEventRaised += AddSkill;
    private void OnDisable() => GainSkillEvent.OnEventRaised -= AddSkill;

    private void Update()
    {
        Skill_Q.Update();
        Skill_E.Update();

        UpdateSkillUI(Skill_Q, true);
        UpdateSkillUI(Skill_E, false);
    }

    public void AddSkill(float id)
    {
        Skill skill = skillDatabase.GetSkillByID((int)id);
        if (skill == null)
        {
            Debug.LogError($"無法找到 ID 為 {id} 的技能");
            return;
        }
        skillUIManager.SkillAdd(skill);
    }

    public void LoadSkill_Q(Skill skill, int index) => Skill_Q.LoadSkill(skill, index);
    public void LoadSkill_E(Skill skill, int index) => Skill_E.LoadSkill(skill, index);

    public void ActiveSkill(InputAction.CallbackContext context)
    {
        if (!context.started || isUsingSkill) return;
        string controlName = context.control.name;

        if (controlName == LeftControlButton)
        {
            Skill skill = Skill_Q.GetCurrentSkill();
            if (skill == null || skill.useCount <= 0) return;
            CastSkill.RaiseEvent(skill);
            Skill_Q.ActivateSkill();
            StartCoroutine(WaitSkillFinish(Skill_Q));
        }
        else if (controlName == RightControlButton)
        {
            Skill skill = Skill_E.GetCurrentSkill();
            if (skill == null || skill.useCount <= 0) return;
            CastSkill.RaiseEvent(skill);
            Skill_E.ActivateSkill();
            StartCoroutine(WaitSkillFinish(Skill_E));
        }
        else
        {
            Debug.Log("未知技能按鍵: " + controlName);
        }
    }

    private IEnumerator WaitSkillFinish(SkillSlotGroup group)
    {
        isUsingSkill = true;
        yield return new WaitUntil(() => skillFinish);
        group.SkillFinish();
        skillFinish = false;
        isUsingSkill = false;
    }

    public void SkillFinish()
    {
        skillFinish = true;
        FinishSkill.RaiseEvent();
    }

    private void UpdateSkillUI(SkillSlotGroup group, bool isQ)
    {
        Skill[] list = group.skills;
        for (int i = 0; i < list.Length; i++)
        {
            Skill skill = list[i];
            if (skill == null) continue;
            if (skill.useCount < skill.MaxUseCount)
            {
                if (isQ)
                    skillUIManager.UpdateSkillIcon_Q(skill.GetCooldown() / skill.Maxcooldown, i);
                else
                    skillUIManager.UpdateSkillIcon_E(skill.GetCooldown() / skill.Maxcooldown, i);
            }

            if (isQ)
                skillUIManager.ChangeTimeOfUse_Q(skill.useCount, i);
            else
                skillUIManager.ChangeTimeOfUse_E(skill.useCount, i);
        }
    }

    private void OnEquip_Q(Skill skill, int index) => skillUIManager.ChangeEquipSkillIcon_Q(index - 1);
    private void OnEquip_E(Skill skill, int index) => skillUIManager.ChangeEquipSkillIcon_E(index - 1);

    private void UpdateFadeVisual_Q(int index, bool fading)
    {
        if (index < skillUIManager.skill_Q_CD_GameObj.Length && skillUIManager.skill_Q_CD_GameObj[index] != null)
        {
            skillUIManager.skill_Q_CD_GameObj[index].GetComponent<Animator>().SetBool("Fading", fading);
        }
    }

    private void UpdateFadeVisual_E(int index, bool fading)
    {
        if (index < skillUIManager.skill_E_CD_GameObj.Length && skillUIManager.skill_E_CD_GameObj[index] != null)
        {
            skillUIManager.skill_E_CD_GameObj[index].GetComponent<Animator>().SetBool("Fading", fading);
        }
    }
}
