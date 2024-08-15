using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUISlots : MonoBehaviour
{
    public Skill skill= null;
    public UnityEngine.UI.Image skillImage = null;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public Button button;
    public StringEventSO stringEventSO;
    public SkillUIManager skillUIManager;
    private void Awake() {
        skillName = GetComponentInChildren<TextMeshProUGUI>();
        skillImage = GetComponentInChildren<UnityEngine.UI.Image>();
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(OnButtonClick);
        skillDescription = new TextMeshProUGUI();
        skillUIManager = GetComponentInParent<SkillUIManager>();
    }
    private void Start() {
        
    }
    public void UpdateSkillUI(Skill ski){
        skill = ski;
        skillImage.sprite = skill.skillImage;
        skillName.text = skill.skillName;
        skillDescription.text = skill.skillDescription;
    }
    private void OnButtonClick(){
        Debug.Log("按下去瞜");
        stringEventSO.RaiseEvent(skillDescription.text);
        skillUIManager.OpenSkillChoose(skill);
    }
    
    
}
