using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillManager : MonoBehaviour
{
    // Start is called before the first frame updatezz
    public GameObject Player;
    public Skill testSkillLoad;
    public string LeftControlButton = "q";
    public string RightControlButton = "e";

    [Header("技能E")]
    public Skill[] Skill_E = new Skill[3];
    public Skill currentSkill_E;
    private int maxSlots_Skill_E = 3;
    public int indexSlots_Skill_E = 1;
    public int minindexSlots_Skill_E = 1;

    [Header("技能Q")]
    public Skill[] Skill_Q = new Skill[3];
    public Skill currentSkill_Q;
    private int maxSlots_Skill_Q = 3;
    public int indexSlots_Skill_Q = 1;
    public int minindexSlots_Skill_Q = 1;
    [Header("計算冷卻與施放準備")]
    public bool[] ReadySkill_E = new bool[3];
    public bool[] ReadySkill_Q = new bool[3];

    [Header("技能數據與UI物件")]
    public SkillDatabase skillDatabase;
    public SkillUIManager skillUIManager;
    [Header("監聽")]
    public FloatEventSO GainSkillEvent;
    private void Awake() {
         ReadySkill_E = new bool[3];
        ReadySkill_Q = new bool[3];
    }
    private void OnEnable() {
        Debug.Log($"ReadySkill_Q length: {ReadySkill_Q.Length}");
        GainSkillEvent.OnEventRaised += AddSkill;
    }
    private void OnDisable() {
        GainSkillEvent.OnEventRaised -= AddSkill;
    }
    private void Update() {
        BackGroundUpdateSkill();
        if(currentSkill_E != null ){
            currentSkill_E.Update();
            for(int i =0; i<Skill_E.Length; i++){
                if(Skill_E[i] == null) continue;
                skillUIManager.ChangeEquipSkillIcon_E(i);
            if(Skill_E[i].useCount <Skill_E[i].MaxUseCount)
            skillUIManager.UpdateSkillIcon_E(Skill_E[i].cooldownCount/Skill_E[i].cooldown,i);
            skillUIManager.ChangeTimeOfUse_E(Skill_E[i].useCount,i);
            }
        }

        if(currentSkill_Q != null ){
            currentSkill_Q.Update();
            for(int i =0; i<Skill_Q.Length; i++){
                if(Skill_Q[i] == null) continue;
                skillUIManager.ChangeEquipSkillIcon_Q(i);
            if(Skill_Q[i].useCount <Skill_Q[i].MaxUseCount)
            skillUIManager.UpdateSkillIcon_Q(Skill_Q[i].cooldownCount/Skill_Q[i].cooldown,i);
            skillUIManager.ChangeTimeOfUse_Q(Skill_Q[i].useCount,i);
            }
        }
    }
    public void AddSkill(float num){
        skillUIManager.SkillAdd(skillDatabase.GetSkillByID((int)num));
        //LoadSkill_Q(testSkillLoad);
    }
    public void LoadSkill_TEST(InputAction.CallbackContext context)
    {
        Debug.Log("skill_Load");
        if(context.started)
        EquipSkill_E(testSkillLoad,2);
        
    }//測試用
    public void LoadSkill_Q(Skill LoadSkill, int skillindex){//將技能放入技能槽位
        if(currentSkill_Q == null){
            currentSkill_Q = LoadSkill;
        }
        Skill_Q[skillindex-1] = LoadSkill;
        if(indexSlots_Skill_Q > skillindex){
            EquipSkill_Q(LoadSkill, skillindex);
        }
        Skill_Q[skillindex-1].OnLoad(Player);
    }
    public void LoadSkill_E(Skill LoadSkill, int skillindex){//將技能放入技能槽位
        if(currentSkill_E == null){
            currentSkill_E = LoadSkill;
        }
        Skill_E[skillindex-1] = LoadSkill;
        if(indexSlots_Skill_E > skillindex){
            EquipSkill_E(LoadSkill, skillindex);
        }
        Skill_E[skillindex-1].OnLoad(Player);
    }
    public void EquipSkill_E(Skill nextSkill, int index){//替換可使用的技能
        currentSkill_E.UnEquip();
        currentSkill_E = nextSkill;
        currentSkill_E.OnEquip();
        indexSlots_Skill_E = index;
        skillUIManager.ChangeEquipSkillIcon_E(index);
    }
    /// <summary>
    /// 加載技能準備施放
    /// </summary>
    /// <param name="nextSkill">下一個技能</param>
    /// <param name="index">下一個技能的位置(!=0)</param>
    public void EquipSkill_Q(Skill nextSkill, int index){
        currentSkill_Q.UnEquip();
        currentSkill_Q = nextSkill;
        currentSkill_Q.OnEquip();
        indexSlots_Skill_Q = index;
        skillUIManager.ChangeEquipSkillIcon_Q(index);
    }
    public void ActiveSkill(InputAction.CallbackContext context){
        var controlName = context.control.name;
        //Debug.Log(controlName + LeftControlButton + RightControlButton);
        if(context.started){
            if(controlName == LeftControlButton){
                if(!currentSkill_Q.Activate(Player)) return;
                //ReadySkill_Q[indexSlots_Skill_Q] = false;
                SwitchToNextSkill(ref Skill_Q,ref currentSkill_Q, indexSlots_Skill_Q,minindexSlots_Skill_Q, controlName);
            }else if(controlName == RightControlButton){
                currentSkill_E.Activate(Player);
                //ReadySkill_Q[indexSlots_Skill_E] = false;
                SwitchToNextSkill(ref Skill_E,ref currentSkill_E,  indexSlots_Skill_E,minindexSlots_Skill_E, controlName);
            }else{
                Debug.Log("無此技能按鍵" + controlName);
            }
        }
    }
    public void SwitchToNextSkill(ref Skill[] skillList, ref Skill currentSkill, int indexSolt, int minIndexSlot, string BTN){
        if(minIndexSlot >= indexSolt){ // 當使用完技能後發現前幾個技能未準備好
            Debug.Log(BTN + LeftControlButton);
            if(BTN == LeftControlButton){
                Debug.Log(indexSolt + " "+skillList.Length);
                for (int i = indexSolt; i < skillList.Length; i++) {//indexSolt是index的+1，而我正好想要比當前技能索引大的技能
                    Debug.Log("ReadySkill_Q" + " " + ReadySkill_Q[i].ToString());
                    if(ReadySkill_Q[i] == true){
                        EquipSkill_Q(skillList[i],i+1);
                        break;
                    }
                }
            }else if(BTN == RightControlButton){
                for (int i = indexSolt; i < skillList.Length; i++) {//indexSolt是index的+1，而我正好想要比當前技能索引大的技能
                    if(ReadySkill_E[i]){
                        EquipSkill_E(skillList[i],i+1);
                        break;
                    }
                }
            }else{
                Debug.LogWarning("ERROR" + BTN);
            }
        }else{
            if(BTN == LeftControlButton){
                EquipSkill_Q(skillList[minIndexSlot-1],minIndexSlot);
            }else if(BTN == RightControlButton){
                EquipSkill_E(skillList[minIndexSlot-1],minIndexSlot);
            }
        }
        
    }
    private void BackGroundUpdateSkill(){
        if(currentSkill_E != null){
            for (int i = 0; i < Skill_E.Length; i++) {
                Skill skill = Skill_E[i];
                if(skill == null) continue;
                skill.isUpdate = false;
            }
            for (int i = 0; i < Skill_E.Length; i++) {
                Skill skill = Skill_E[i];
                if(skill == null) continue;
                int index = i+1;
                bool isReady = skill.BackGroundUpdate();
                if(isReady){
                    ReadySkill_E[i] = true;
                    if(minindexSlots_Skill_E > index){
                        minindexSlots_Skill_E = index;
                    } 
                }
            }
        }
        if(currentSkill_Q != null){
            for (int i = 0; i < Skill_Q.Length; i++) {
                Skill skill = Skill_Q[i];
                if(skill == null) continue;
                skill.isUpdate = false;
            }
            for (int i = 0; i < Skill_Q.Length; i++) {
                Skill skill = Skill_Q[i];
                if(skill == null) continue;
                int index = i+1;
                bool isReady = skill.BackGroundUpdate();
                if(isReady){
                    //Debug.Log("bug" + i);
                    ReadySkill_Q[i] = true;
                    if(minindexSlots_Skill_Q > index){
                        minindexSlots_Skill_Q = index;
                    } 
                }
            }
        }
    }
}
