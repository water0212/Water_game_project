using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
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
    private float FadeCount_E =0;
    private float FadeTime_E = 5;
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
    private float FadeCount_Q;
    private float FadeTime_Q = 5;
    [Header("計算冷卻與施放準備")]
    public bool[] ReadySkill_E = new bool[3];
    public bool[] ReadySkill_Q = new bool[3];

    [Header("技能數據與UI物件")]
    public SkillDatabase skillDatabase;
    public SkillUIManager skillUIManager;
    public static bool isUsingSkill;
    private bool skillFinish;
    [Header("廣播")]
    public SkillEventSO CastSkill;
    public VoidEventSO FinishSkill;
    [Header("監聽")]
    public FloatEventSO GainSkillEvent;
    [Header("Instance")]
    private static SkillManager instance;
    public static SkillManager GetSkillManager(){
        return instance;
    }
    private void Awake() {
        if(instance){
            Debug.LogError("Found more than one SkillManager in same Scene");
            Destroy(this);
        }
        instance = this;
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
            
            if(FadeCount_E < 0){
                SwitchToNextSkill(ref Skill_E,9,minindexSlots_Skill_E,RightControlButton);
            }else if(minindexSlots_Skill_E != indexSlots_Skill_E){
                skillUIManager.skill_E_CD_GameObj[indexSlots_Skill_E-1].GetComponent<Animator>().SetBool("Fading",true);
                FadeCount_E -= Time.deltaTime;
            }
            for(int i =0; i<Skill_E.Length; i++){
                if(Skill_E[i] == null) continue;
                //skillUIManager.ChangeEquipSkillIcon_E(i);
            if(Skill_E[i].useCount <Skill_E[i].MaxUseCount)
            skillUIManager.UpdateSkillIcon_E(Skill_E[i].cooldownCount/Skill_E[i].cooldown,i);
            skillUIManager.ChangeTimeOfUse_E(Skill_E[i].useCount,i);
            }
        }

        if(currentSkill_Q != null ){
            currentSkill_Q.Update();
            if(FadeCount_Q < 0){
                
                SwitchToNextSkill(ref Skill_Q,9,minindexSlots_Skill_Q,LeftControlButton);
            }else if(minindexSlots_Skill_Q != indexSlots_Skill_Q){
                skillUIManager.skill_Q_CD_GameObj[indexSlots_Skill_Q-1].GetComponent<Animator>().SetBool("Fading",true);
                FadeCount_Q -= Time.deltaTime;
            }
            for(int i =0; i<Skill_Q.Length; i++){
                if(Skill_Q[i] == null) continue;
                //skillUIManager.ChangeEquipSkillIcon_Q(i);
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
        if(context.started){
        AddSkill(2);
        AddSkill(1);
        AddSkill(0);
        }
        
    }//測試用
    public void LoadSkill_Q(Skill LoadSkill, int skillindex){//將技能放入技能槽位
        Skill_Q[skillindex-1] = LoadSkill;
        if(currentSkill_Q == null){
            currentSkill_Q = LoadSkill;
        }
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
    if(nextSkill == null){}if(indexSlots_Skill_E != 9)
        skillUIManager.skill_E_CD_GameObj[indexSlots_Skill_E-1].GetComponent<Animator>().SetBool("Fading",false);
        currentSkill_E.UnEquip();
        currentSkill_E = nextSkill;
        currentSkill_E.OnEquip();
        indexSlots_Skill_E = index;
        skillUIManager.ChangeEquipSkillIcon_E(index);
        FadeCount_E = FadeTime_E; 
    }
    /// <summary>
    /// 加載技能準備施放
    /// </summary>
    /// <param name="nextSkill">下一個技能</param>
    /// <param name="index">下一個技能的位置(!=0)</param>
    public void EquipSkill_Q(Skill nextSkill, int index){
        if(indexSlots_Skill_Q != 9){
        skillUIManager.skill_Q_CD_GameObj[indexSlots_Skill_Q-1].GetComponent<Animator>().SetBool("Fading",false);
        }
        currentSkill_Q.UnEquip();
        currentSkill_Q = nextSkill;
        currentSkill_Q.OnEquip();
        indexSlots_Skill_Q = index;
        skillUIManager.ChangeEquipSkillIcon_Q(index-1);
        FadeCount_Q = FadeTime_Q;
    }
    public void ActiveSkill(InputAction.CallbackContext context){
        var controlName = context.control.name;
        //Debug.Log(controlName + LeftControlButton + RightControlButton);
        if(context.started&&!isUsingSkill){
            
            if(controlName == LeftControlButton){
                if(!currentSkill_Q || currentSkill_Q.useCount ==0) return;
                CastSkill.RaiseEvent(currentSkill_Q);
                currentSkill_Q.Activate(Player);
                StartCoroutine(WaitSkill_Q_Finish());
                //ReadySkill_Q[indexSlots_Skill_Q] = false;
                
            }else if(controlName == RightControlButton){
                if(!currentSkill_E || currentSkill_Q.useCount ==0) return;
                CastSkill.RaiseEvent(currentSkill_E);
                currentSkill_E.Activate(Player);
                StartCoroutine(WaitSkill_E_Finish());
                //ReadySkill_Q[indexSlots_Skill_E] = false;
                
            }else{
                Debug.Log("無此技能按鍵" + controlName);
            }
        }
    }
    public IEnumerator WaitSkill_E_Finish(){
        isUsingSkill = true;
        yield return new WaitUntil(() => skillFinish);
        SwitchToNextSkill(ref Skill_E,  indexSlots_Skill_E,minindexSlots_Skill_E, "e");
        skillFinish = false;
        isUsingSkill = false;
    }
    public IEnumerator WaitSkill_Q_Finish(){
        isUsingSkill = true;
        yield return new WaitUntil(() => skillFinish);
        SwitchToNextSkill(ref Skill_Q, indexSlots_Skill_Q,minindexSlots_Skill_Q, "q");
        skillFinish = false;
        isUsingSkill = false;
    }
    public void SkillFinish(){

        skillFinish = true;
        FinishSkill.RaiseEvent();
    }
    public void SwitchToNextSkill(ref Skill[] skillList, int indexSolt, int minIndexSlot, string BTN){
        if(minIndexSlot >= indexSolt){ // 當使用完技能後發現前幾個技能未準備好
            if(BTN == LeftControlButton){
                for (int i = indexSolt; i < skillList.Length; i++) {//indexSolt是index的+1，而我正好想要比當前技能索引大的技能
                    Debug.Log("ReadySkill_Q" + " " + ReadySkill_Q[i].ToString());
                    if(ReadySkill_Q[i]){
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
                if(isReady){//找最小的isReady值
                    if(minindexSlots_Skill_E > index){
                        minindexSlots_Skill_E = index;
                    } 
                }else{
                    // if(minindexSlots_Skill_E == index){
                    //     if(index != Skill_E.Length)minindexSlots_Skill_E+=1;
                    // }
                }
                ReadySkill_E[i] = isReady;
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
                    
                    if(minindexSlots_Skill_Q > index){
                        minindexSlots_Skill_Q = index;
                    } 
                }else{
                    //若當前技能使用，且當前技能還可以再使用，
                    // if(minindexSlots_Skill_Q == index){
                    //     if(index != Skill_Q.Length)minindexSlots_Skill_Q+=1;
                    // }
                }
                ReadySkill_Q[i] = isReady;
            }
        }
    }
}
